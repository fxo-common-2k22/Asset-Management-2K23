ALTER PROC [dbo].[p__Attendance__Insert] @UserId INT, @AttendanceDateTime DATETIME
AS
BEGIN
	SET NOCOUNT ON

	SET DATEFIRST 1;--do later
	DECLARE @RFID VARCHAR(24), @CheckTime DATETIME, @CheckDate DATE;

	SET @CheckTime = @AttendanceDateTime;
	SET @CheckDate = CAST(@AttendanceDateTime AS DATE);

	SELECT @RFID = @UserId;



	DECLARE @ModifiedAttendanceDateTime DATETIME, @ModifiedCheckTime TIME(0), @ModifiedCheckDate DATE
	DECLARE @StartTime TIME(0), @EndTime TIME(0), @ShiftId INT, @IsSingleDay bit

	SELECT @IsSingleDay= Isnull(IsSingleDay,1), @StartTime = StartTime, @EndTime = EndTime, @ShiftId = s.ShiftId
	FROM [HR].[Employees] AS e
	INNER JOIN HR.Shifts s ON s.ShiftId = e.ShiftId
		AND s.BranchId = e.BranchId
	WHERE EmployeeId = @UserId

	DECLARE @DIFF INT = 0

	IF @EndTime < @StartTime -- night shift
	BEGIN
		SET @DIFF = DATEDIFF(MINUTE, '00:00', @EndTime) * - 1 --why 1
		SET @ModifiedAttendanceDateTime = DATEADD(MINUTE, @DIFF, @AttendanceDateTime)
		SET @ModifiedCheckTime = CAST(@ModifiedAttendanceDateTime AS TIME(0));
		SET @ModifiedCheckDate = CAST(@ModifiedAttendanceDateTime AS DATE);
	END
	ELSE
	BEGIN
		SET @DIFF = 0
		SET @ModifiedAttendanceDateTime = DATEADD(MINUTE, @DIFF, @AttendanceDateTime)
		SET @ModifiedCheckTime = CAST(@ModifiedAttendanceDateTime AS TIME(0));
		SET @ModifiedCheckDate = CAST(@ModifiedAttendanceDateTime AS DATE);		
	END

	IF(@IsSingleDay=0)--IF MULTIPLE DAY
	BEGIN
		SET @DIFF = 0
		SET @ModifiedAttendanceDateTime = DATEADD(MINUTE, @DIFF, @AttendanceDateTime)
		SET @ModifiedCheckTime = CAST(@ModifiedAttendanceDateTime AS TIME(0));
		SET @ModifiedCheckDate = CAST(@ModifiedAttendanceDateTime AS DATE);
		IF NOT EXISTS (SELECT EAID FROM [HR].[EmployeeAttendances] WHERE [AttendanceDate] = @ModifiedCheckDate)
			BEGIN
				SELECT TOP 1 @ModifiedCheckDate=CAST([AttendanceDate] AS DATE) FROM [HR].[EmployeeAttendances] ORDER BY [AttendanceDate] DESC
			END;
	END;

	IF NOT EXISTS (
				SELECT *
				FROM [HR].[EmployeeAttendances]
				WHERE [AttendanceDate] = @ModifiedCheckDate
				)
		BEGIN
			--INSERT
			INSERT INTO [HR].[EmployeeAttendances] (EAID, EmployeeId, [AttendanceDate], Arrival, BreakIn, BreakOut, Departure, DepartmentId, DesignationId, BranchId, Present, Leave, Holiday, LeaveTypeId, AttendanceStatus, ShiftTimeIn, ShiftTimeOut, ShiftId,DailyHours,GracePeriod,IsAuthorized)
			SELECT NEWID(),EmployeeId, @ModifiedCheckDate, NULL, NULL, NULL, NULL, DepartmentId, DesignationId, e.BranchId, 'False', 'False', 'False', NULL, 'Absent', StartTime, EndTime, s.ShiftId,ISNULL(s.Duration,0),0,0
			FROM [HR].[Employees] AS e
			INNER JOIN HR.Shifts s ON s.ShiftId = e.ShiftId
				AND s.BranchId = e.BranchId
			WHERE e.Active = 1;

			--SELECT * FROM 
			UPDATE [HR].[EmployeeAttendances]
			SET Leave = 1, LeaveTypeId = la.LeaveTypeId, HalfDay = 0, AttendanceStatus = 'Leave', Present = 0
			FROM [HR].[EmployeeAttendances]
			INNER JOIN [HR].[Leaves] AS la ON la.EmployeeId = [EmployeeAttendances].EmployeeId
			WHERE [AttendanceDate] = @ModifiedCheckDate
				AND @CheckDate BETWEEN la.FromDate
					AND la.ToDate
				AND la.IsApproved = 1;

			UPDATE HR.EmployeeAttendances
			SET AttendanceStatus = 'Holiday'
			FROM HR.EmployeeAttendances
			INNER JOIN HR.EmployeeWeeklyHolidays ON HR.EmployeeAttendances.EmployeeId = HR.EmployeeWeeklyHolidays.EmployeeId
			WHERE (HR.EmployeeAttendances.AttendanceDate = @ModifiedCheckDate)
				AND DATEPART(WEEKDAY, @ModifiedCheckDate) IN (
					SELECT *
					FROM dbo.fn_ParseText2Table(HolidayCSV, ',')
					)
		END;
	DECLARE @HalfDay BIT = 'False';
	DECLARE @rows int;

	UPDATE [HR].[EmployeeAttendances]
	SET Arrival = ISNULL(Arrival, @CheckTime), Departure = CASE 
			WHEN Arrival IS NOT NULL
				THEN @CheckTime
			END, Present = 'True', Leave = 'False', LeaveTypeId = CASE @HalfDay
			WHEN 'True'
				THEN LeaveTypeId
			ELSE NULL
			END, HalfDay = @HalfDay, Holiday = 'False', AttendanceStatus = 'Present'
	WHERE EmployeeId = @UserId
		AND [AttendanceDate] = @ModifiedCheckDate
		

	--EmployeeAttendanceDetails
	select @rows = COUNT(EmployeeId)+1 from [HR].[EmployeeAttendanceDetails] where EmployeeId=@UserId and AttendanceDate=@ModifiedCheckDate
	Insert into [HR].[EmployeeAttendanceDetails] (EmployeeId, AttendanceDate, Direction, CheckInOutTime,SkipedTime)
	Values (@UserId,@ModifiedCheckDate,
	case @rows%2 when 1 then 'IN' else 'OUT' end
	,@CheckTime,cast('00:00:00' as time))


	IF @@ROWCOUNT = 0
	BEGIN
		SELECT '0 ROWS'

		INSERT INTO [HR].[EmployeeAttendances] (EmployeeId, [AttendanceDate], Arrival, BreakIn, BreakOut, Departure, DepartmentId, DesignationId, BranchId, Present, Leave, Holiday, LeaveTypeId, AttendanceStatus, ShiftTimeIn, ShiftTimeOut, ShiftId,IsAuthorized)
		SELECT EmployeeId, @CheckDate, @CheckTime, NULL, NULL, NULL, DepartmentId, DesignationId, e.BranchId, 'True', 'False', 'False', NULL, 'Present', StartTime, EndTime, s.ShiftId,0
		FROM [HR].[Employees] AS e
		INNER JOIN HR.Shifts s ON s.ShiftId = e.ShiftId
			AND s.BranchId = e.BranchId
		WHERE EmployeeId = @UserId
	END

	UPDATE dbo.CHECKINOUT
	SET Processed = 1
	WHERE CHECKTIME = @AttendanceDateTime
		AND USERID = @UserId
END;
GO
ALTER TRIGGER [dbo].[t__ATTENDANCE_INSERT] ON [dbo].[CHECKINOUT]
AFTER INSERT
	, UPDATE
AS
DECLARE @UserId INT
	, @AttendanceDateTime DATETIME;

SELECT @UserId = i.USERID
	, @AttendanceDateTime = i.CHECKTIME
FROM INSERTED i;

DECLARE @EmployeeId INT
	, @StudentId INT

SELECT @EmployeeId = EmployeeId
FROM HR.Employees
WHERE EmployeeId = @UserId

SELECT @StudentId = StudentId
FROM Academics.StudentSessions
WHERE StudentId = @UserId
	AND Active = 1

IF @EmployeeId IS NOT NULL
	EXEC dbo.p__Attendance__Insert @UserId
		, @AttendanceDateTime;

--IF @StudentId IS NOT NULL
--	EXEC Academics.[p__Attendance__Insert] @UserId
--		, @AttendanceDateTime
GO
ALTER FUNCTION [dbo].[fn_ParseText2Table] (
	@p_SourceText VARCHAR(8000), @p_Delimeter VARCHAR(10) = ',' --default comma
	)
RETURNS @retTable TABLE (Value BIGINT)
AS
BEGIN
	DECLARE @w_Continue INT, @w_StartPos INT, @w_Length INT, @w_Delimeter_pos INT, @w_tmp_txt VARCHAR(48), @w_Delimeter_Len TINYINT

	IF LEN(@p_SourceText) = 0
	BEGIN
		SET @w_Continue = 0 -- force early exit
	END
	ELSE
	BEGIN
		-- parse the original @p_SourceText array into a temp table
		SET @w_Continue = 1
		SET @w_StartPos = 1
		SET @p_SourceText = RTRIM(LTRIM(@p_SourceText))
		SET @w_Length = DATALENGTH(RTRIM(LTRIM(@p_SourceText)))
		SET @w_Delimeter_Len = LEN(@p_Delimeter)
	END

	WHILE @w_Continue = 1
	BEGIN
		SET @w_Delimeter_pos = CHARINDEX(@p_Delimeter, SUBSTRING(@p_SourceText, @w_StartPos, @w_Length - @w_StartPos + @w_Delimeter_Len))

		IF @w_Delimeter_pos > 0 -- delimeter(s) found, get the value
		BEGIN
			SET @w_tmp_txt = LTRIM(RTRIM(SUBSTRING(@p_SourceText, @w_StartPos, @w_Delimeter_pos - 1)))
			SET @w_StartPos = @w_Delimeter_pos + @w_StartPos + @w_Delimeter_Len - 1
		END
		ELSE -- No more delimeters, get last value
		BEGIN
			SET @w_tmp_txt = LTRIM(RTRIM(SUBSTRING(@p_SourceText, @w_StartPos, @w_Length - @w_StartPos + @w_Delimeter_Len)))

			SELECT @w_Continue = 0
		END

		INSERT INTO @retTable
		VALUES (@w_tmp_txt)
	END

	RETURN
END
GO
ALTER TABLE FrontDesk.PurchaseReturnProducts ADD BranchId smallint NOT NULL CONSTRAINT DF_FrontDesk_PurchaseReturnProducts_BranchId DEFAULT 1001
ALTER TABLE FrontDesk.PurchaseInvoiceProducts ADD BranchId smallint NOT NULL CONSTRAINT DF_FrontDesk_PurchaseInvoiceProducts_BranchId DEFAULT 1001
ALTER TABLE FrontDesk.Brands ADD BranchId smallint NOT NULL CONSTRAINT DF_FrontDesk_Brands_BranchId DEFAULT 1001
ALTER TABLE FrontDesk.ProductGroups ADD BranchId smallint NOT NULL CONSTRAINT DF_FrontDesk_ProductGroups_BranchId DEFAULT 1001
ALTER TABLE FrontDesk.Unit ADD BranchId smallint NOT NULL CONSTRAINT DF_FrontDesk_Unit_BranchId DEFAULT 1001

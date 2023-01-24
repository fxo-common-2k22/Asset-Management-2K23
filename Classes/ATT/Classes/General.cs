using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FAPP.ViewModel.Common
{
    public static partial class ProceduresModel
    {
        public class v_mnl_CHECKINOUT_Result
        {
            public int USERID { get; set; }
            public DateTime CHECKTIME { get; set; }
            public string CHECKTYPE { get; set; }
            public int? VERIFYCODE { get; set; }
            public string SENSORID { get; set; }
            public string Memoinfo { get; set; }
            public int? WorkCode { get; set; }
            public string sn { get; set; }
            public short? UserExtFmt { get; set; }
            public int? ModifiedBy { get; set; }
            public string IP { get; set; }
            public long ID { get; set; }
            public bool? Processed { get; set; }
            public string CHECKDATETIME { get; set; }
            public string EmpName { get; set; }
            public int? EmployeeId { get; set; }
            public bool IsChecked { get; set; }
        }


       

        public static string t__Employee_Update_INTO_UserInfo(OneDbContext db, int EmployeeId)
        {
            var result = db.Database.ExecuteSqlCommand($@"DECLARE @OldVal DECIMAL(18, 2) = 0, @NewVal DECIMAL(18, 2) = 0, @EmployeeId INT, @BranchId SMALLINT
DECLARE @HasBiometricAttendance BIT = 0;
set @EmployeeId={EmployeeId}

SELECT TOP 1 @HasBiometricAttendance = HasBiometricAttendance
FROM Company.Branches b;

PRINT @HasBiometricAttendance

IF @HasBiometricAttendance = 1
BEGIN
	--CHECK IF BRANCH EXISTS
	DECLARE @DeptId INT;

	PRINT 2

	SELECT @DeptId = i.BranchId
	FROM HR.Employees  AS i where i.EmployeeId=@EmployeeId;

	IF NOT EXISTS (
			SELECT *
			FROM dbo.DEPARTMENTS
			WHERE DEPTID = @DeptId
			)
	BEGIN
		SET IDENTITY_INSERT dbo.[DEPARTMENTS] ON;

		INSERT INTO dbo.[DEPARTMENTS] ([DEPTID], [DEPTNAME], [SUPDEPTID], [InheritParentSch], [InheritDeptSch], [InheritDeptSchClass], [AutoSchPlan], [InLate], [OutEarly], [InheritDeptRule], [MinAutoSchInterval], [RegisterOT], [DefaultSchId], [ATT], [Holiday], [OverTime])
		SELECT b.BranchId, LEFT(b.NAME, 59), 1, 1, 1, 1, 1, 1, 1, 1, 24, 1, 1, 1, 1, 1
		FROM [Company].[Branches] AS b
		WHERE b.BranchId = @DeptId;

		SET IDENTITY_INSERT dbo.[DEPARTMENTS] OFF;
	END;

	--emp
	DECLARE @EmployeeDepId INT, @DEPTNAME VARCHAR(50)

	SELECT @EmployeeDepId = DEPTID, @DEPTNAME = [DEPTNAME]
	FROM dbo.DEPARTMENTS
	WHERE [DEPTNAME] = 'Employee'
		AND [SUPDEPTID] = @DeptId

	PRINT 'Here'

	IF @EmployeeDepId IS NULL
	BEGIN
		SET IDENTITY_INSERT dbo.[DEPARTMENTS] ON;
		SET @EmployeeDepId = 32767 - (@DeptId) + 1

		PRINT @EmployeeDepId

		INSERT INTO [dbo].[DEPARTMENTS] (DEPTID, [DEPTNAME], [SUPDEPTID], [InheritParentSch], [InheritDeptSch], [InheritDeptSchClass], [AutoSchPlan], [InLate], [OutEarly], [InheritDeptRule], [MinAutoSchInterval], [RegisterOT], [DefaultSchId], [ATT], [Holiday], [OverTime])
		SELECT @EmployeeDepId, 'Employee', @DeptId, 1, 1, 1, 1, 1, 1, 1, 24, 1, 1, 1, 1, 1
		FROM [Company].[Branches] AS b
		WHERE b.BranchId = @DeptId;

		SET IDENTITY_INSERT dbo.[DEPARTMENTS] OFF;
	END;

	---
	PRINT @EmployeeDepId

	DECLARE @UserId INT, @Gender CHAR;

	SELECT @UserId = i.EmployeeId, @Gender = CASE i.Gender
			WHEN 'True'
				THEN 'Male'
			ELSE 'Female'
			END
	FROM HR.Employees  AS i where i.EmployeeId=@EmployeeId;

	IF NOT EXISTS (
			SELECT *
			FROM dbo.USERINFO
			WHERE UserId = @UserId
			)
	BEGIN
		INSERT INTO dbo.USERINFO (USERID, BADGENUMBER, NAME, GENDER, BIRTHDAY, STREET, VERIFICATIONMETHOD, DEFAULTDEPTID, SSN,ATT)
		SELECT i.EmployeeId, i.EmployeeId, LEFT(i.[EmpName], 40), IIF(I.Gender = 1, 'Male', 'Female'), i.DOB, LEFT(i.[Address], 40), 1, @EmployeeDepId, i.NIC,1
		FROM HR.Employees  AS i where i.EmployeeId=@EmployeeId;
	END;
	ELSE
	BEGIN
		UPDATE ui
		SET NAME = LEFT(i.[EmpName], 40), GENDER = IIF(I.Gender = 1, 'Male', 'Female'), BIRTHDAY = i.DOB, STREET = LEFT(i.[Address], 40), SSN = i.NIC, DEFAULTDEPTID = @EmployeeDepId
		FROM dbo.USERINFO AS ui
		INNER JOIN HR.Employees  AS i  ON ui.UserId = i.EmployeeId where i.EmployeeId=@EmployeeId
	END;
END;").ToString();
            return result;
        }

        
    }
}
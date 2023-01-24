namespace FAPP.Migrations
{
    using FAPP.Model;
    using FAPP.Areas.AM.PrimaryData;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Model.OneDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Model.OneDbContext context)
        {
            //var pd = new MenuLinks(context);
            //pd.AddUpdateDefaultUserGroup();
            //pd.AddUpdateAppModule();
            //pd.AddUpdateHouseType();
            //pd.AddUpdateVoucherType();
            //pd.AddUpdateResOrderType();
            //pd.AddUpdateResOrderStatuses();
            //pd.AddUpdateResItemOrderStatuses();
            //pd.AddUpdateResAMRequestStatus();
            //pd.AddUpdateResAMConditionType();
            //pd.AddUpdateResPlacementType();
            //pd.AddUpdateRoomStatus();
            //pd.AddUpdateAllFormMenus();
            //pd.AddGroupRights(4);

            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[f__GetFirstDayOfMonth]') 
	
)
DROP FUNCTION [dbo].[f__GetFirstDayOfMonth]
");
            context.Database.ExecuteSqlCommand($@"
CREATE FUNCTION [dbo].[f__GetFirstDayOfMonth]( @Date DATE )
RETURNS DATE
AS
BEGIN
	DECLARE
	   @Month DATE;
	SELECT @Month = DATEADD( MONTH , DATEDIFF( MONTH , 0 , @Date ) , 0 );
	RETURN @Month;
END;");

            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fn_ParseText2TableDate]') 
	
)
DROP FUNCTION [dbo].[fn_ParseText2TableDate]
");

            context.Database.ExecuteSqlCommand($@"

CREATE FUNCTION [dbo].[fn_ParseText2TableDate] (
	@p_SourceText VARCHAR(8000), @p_Delimeter VARCHAR(10) = ',' --default comma
	)
RETURNS @retTable TABLE (Value DATE)
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
		VALUES (CAST(@w_tmp_txt AS DATE))
	END

	RETURN
END");


            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fn_ParseText2Table]') 
	
)
DROP FUNCTION [dbo].[fn_ParseText2Table]
");


            context.Database.ExecuteSqlCommand($@"
 

CREATE FUNCTION [dbo].[fn_ParseText2Table] (
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

 

");

            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fn_ParseText2TableString]') 
	
)
DROP FUNCTION [dbo].[fn_ParseText2TableString]
");


            context.Database.ExecuteSqlCommand($@"


CREATE FUNCTION [dbo].[fn_ParseText2TableString]
(
				@p_SourceText VARCHAR( 8000 ) ,
				@p_Delimeter  VARCHAR( 10 ) = ',' --default comma
)
RETURNS @retTable TABLE
(
Value VARCHAR( 50 )
)
AS
	 BEGIN
		 DECLARE
			@w_Continue  INT
			,@w_StartPos  INT
			,@w_Length  INT
			,@w_Delimeter_pos INT
			,@w_tmp_txt   VARCHAR( 48 )
			,@w_Delimeter_Len TINYINT
		 IF LEN( @p_SourceText ) = 0
			 BEGIN
				 SET  @w_Continue = 0 -- force early exit
			 END
		 ELSE
			 BEGIN
				 -- parse the original @p_SourceText array into a temp table
				 SET  @w_Continue = 1
				 SET @w_StartPos = 1
				 SET @p_SourceText = RTRIM( LTRIM( @p_SourceText ))
				 SET @w_Length = DATALENGTH( RTRIM( LTRIM( @p_SourceText )))
				 SET @w_Delimeter_Len = LEN( @p_Delimeter )
			 END
		 WHILE @w_Continue = 1
			 BEGIN
				 SET @w_Delimeter_pos = CHARINDEX( @p_Delimeter , SUBSTRING( @p_SourceText , @w_StartPos , @w_Length - @w_StartPos + @w_Delimeter_Len ))
				 IF @w_Delimeter_pos > 0  -- delimeter(s) found, get the value
					 BEGIN
						 SET @w_tmp_txt = LTRIM( RTRIM( SUBSTRING( @p_SourceText , @w_StartPos , @w_Delimeter_pos - 1 )))
						 SET @w_StartPos = @w_Delimeter_pos + @w_StartPos + @w_Delimeter_Len - 1
					 END
				 ELSE -- No more delimeters, get last value
					 BEGIN
						 SET @w_tmp_txt = LTRIM( RTRIM( SUBSTRING( @p_SourceText , @w_StartPos , @w_Length - @w_StartPos + @w_Delimeter_Len )))
						 SELECT @w_Continue = 0
					 END
				 INSERT INTO @retTable
				 VALUES( @w_tmp_txt )
			 END
		 RETURN
	 END
");


            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[Helper].[f__GetMonthsBetweenDates]') 
	
)
DROP FUNCTION [Helper].[f__GetMonthsBetweenDates]
");



            context.Database.ExecuteSqlCommand($@"
CREATE FUNCTION [Helper].[f__GetMonthsBetweenDates] 
(	
	@FromDate date, 
	@ToDate date
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT DISTINCT dbo.f__GetFirstDayOfMonth( d) d FROM [Helper].Calendar
	WHERE d BETWEEN @FromDate AND @ToDate-- AND DAY(d) = 1
)
 
");

            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[f__GetFirstDayOfMonth]') 
	
)
DROP FUNCTION [dbo].[f__GetFirstDayOfMonth]
");



            context.Database.ExecuteSqlCommand($@"
CREATE FUNCTION [dbo].[f__GetFirstDayOfMonth]( @Date DATE )
RETURNS DATE
AS
BEGIN
	DECLARE
	   @Month DATE;
	SELECT @Month = DATEADD( MONTH , DATEDIFF( MONTH , 0 , @Date ) , 0 );
	RETURN @Month;
END;");
            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[f_MIN2]') 
	
)
DROP FUNCTION [dbo].[f_MIN2]
");


            context.Database.ExecuteSqlCommand($@"
 CREATE FUNCTION[dbo].[f_MIN2]
		(@A INT, @B INT)
RETURNS INT
WITH EXECUTE AS CALLER
AS
BEGIN
	DECLARE @MIN INT = @A

	IF @B<@MIN

		SET @MIN = @B

	RETURN @MIN
END
			");

            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[f_MIN2Date]') 
	
)
DROP FUNCTION [dbo].[f_MIN2Date]
");

            context.Database.ExecuteSqlCommand($@"
CREATE FUNCTION [dbo].[f_MIN2Date] (
	@A DATETIME
	, @B DATETIME
	)
RETURNS DATETIME
	WITH EXECUTE AS CALLER
AS
BEGIN
	RETURN IIF(@A > @B, @B, @A)
END
");
            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fBuild2DigitMonth]') 
	
)
DROP FUNCTION [dbo].[fBuild2DigitMonth]
");


            context.Database.ExecuteSqlCommand($@"

CREATE FUNCTION [dbo].[fBuild2DigitMonth] ( @Year DATE )
RETURNS CHAR(2)
AS 
	BEGIN
		RETURN CONVERT(VARCHAR(2), @Year, 101)
	END
");
            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fBuild2DigitYear]') 
	
)
DROP FUNCTION [dbo].[fBuild2DigitYear]
");


            context.Database.ExecuteSqlCommand($@"

CREATE FUNCTION [dbo].[fBuild2DigitYear] ( @Year DATE )
RETURNS CHAR(2)
AS 
	BEGIN
		RETURN RIGHT(CONVERT(CHAR(4),YEAR(@Year)), 2) -- YEAR
	END
");
            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fnc_GetColumnsByCommas]') 
	
)
DROP FUNCTION [dbo].[fnc_GetColumnsByCommas]
");

            context.Database.ExecuteSqlCommand($@"

CREATE FUNCTION [dbo].[fnc_GetColumnsByCommas]
	(
	  -- Add the parameters for the function
	  @schemaName VARCHAR(50),
	  @tableName VARCHAR(50)
	)
RETURNS VARCHAR(4000)
AS BEGIN

	DECLARE @column VARCHAR(2000),
		@columnS VARCHAR(4000),
		@i INT
	SET @i = 0
	SET @column = ''
	SET @columnS = ''

	DECLARE Cur_Columns CURSOR STATIC
		FOR SELECT  sys.columns.name
			FROM    sys.schemas
					INNER JOIN sys.objects ON sys.schemas.schema_id = sys.objects.schema_id
					INNER JOIN sys.columns ON sys.columns.object_id = sys.objects.object_id
					INNER JOIN sys.types ON sys.columns.user_type_id = sys.types.user_type_id
			WHERE   sys.types.name <> 'timestamp'
					AND sys.objects.type = 'U'
					AND sys.objects.name = @tableName
					AND sys.schemas.name = @schemaName
			ORDER BY sys.columns.column_id
	OPEN Cur_Columns
	FETCH FIRST FROM Cur_Columns INTO @column
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @i = 0
				SET @columnS = '[' + @column + ']'
			ELSE
				SET @columnS = @columnS + ',' + '[' + @column + ']'
			SET @i = @i + 1

			FETCH NEXT FROM Cur_Columns INTO @column
		END

	CLOSE Cur_Columns
	DEALLOCATE Cur_Columns

 -- Return the result of the function
	RETURN @columns

   END
");
            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fnc_GetColumnsForValueByCommas]') 
	
)
DROP FUNCTION [dbo].[fnc_GetColumnsForValueByCommas]
");

            context.Database.ExecuteSqlCommand($@"


CREATE FUNCTION [dbo].[fnc_GetColumnsForValueByCommas]
	(
	  -- Add the parameters for the function here
	  @schemaName VARCHAR(50),
	  @tableName VARCHAR(50)
	)
RETURNS VARCHAR(4000)
AS BEGIN
 
	DECLARE @column VARCHAR(4000),
		@typeName VARCHAR(500),
		@columnS VARCHAR(2000),
		@ColStart VARCHAR(50),
		@ColEnd VARCHAR(50),
		@i INT

	SET @i = 0

	SET @column = ''
	SET @columnS = ''

	DECLARE Cur_Columns CURSOR STATIC
		FOR SELECT  sys.columns.name,
					sys.types.name
			FROM    sys.schemas
					INNER JOIN sys.objects ON sys.schemas.schema_id = sys.objects.schema_id
					INNER JOIN sys.columns ON sys.columns.object_id = sys.objects.object_id
					INNER JOIN sys.types ON sys.columns.user_type_id = sys.types.user_type_id
			WHERE   sys.types.name <> 'timestamp'
					AND sys.objects.type = 'U'
					AND sys.objects.name = @tableName
					AND sys.schemas.name = @schemaName
			ORDER BY sys.columns.column_id


	OPEN Cur_Columns
	FETCH FIRST FROM Cur_Columns INTO @column, @typeName
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @typeName = 'text'
				OR @typeName = 'uniqueidentifier'
				OR @typeName = 'varbinary'
				OR @typeName = 'smalldatetime'
				OR @typeName = 'char'
				OR @typeName = 'datetime'
				OR @typeName = 'varchar'
				OR @typeName = 'date'
				OR @typeName = 'time'
				BEGIN
					SET @ColStart = ' ISNULL(CHAR(39) + CAST ( '  
					SET @ColEnd = ' AS VARCHAR(MAX))+ CHAR(39),''NULL'') '
				END
			ELSE
				IF @typeName = 'nvarchar'
					OR @typeName = 'ntext'
					OR @typeName = 'nchar'
					BEGIN
						SET @ColStart = 'ISNULL( ''N''+CHAR(39)+ CAST ( ' 
						SET @ColEnd = ' AS NVARCHAR(MAX)) + CHAR(39),''NULL'') '
					END
				ELSE
					BEGIN 
						SET @ColStart = 'ISNULL(CAST ( ' 
						SET @ColEnd = ' AS VARCHAR(MAX)), ''NULL'') '
					END

			IF @i = 0
				SET @columnS = @ColStart + @column + @ColEnd
			ELSE
				SET @columnS = @columnS + '+'',''+' + @ColStart + @column
					+ @ColEnd
			SET @i = @i + 1

			FETCH NEXT FROM Cur_Columns INTO @column, @typeName
		END

	CLOSE Cur_Columns
	DEALLOCATE Cur_Columns


-- Return the result of the function
	RETURN @columns

   END
");
            context.Database.ExecuteSqlCommand($@"
IF EXISTS (
	SELECT * FROM sysobjects WHERE id = object_id(N'[fee].[f_GetReceiptId]') 
	
)
DROP FUNCTION [fee].[f_GetReceiptId]
");


            context.Database.ExecuteSqlCommand($@"


CREATE FUNCTION [Fee].[f_GetReceiptId] (@ReceiptDate DATE, @BranchId SMALLINT)
RETURNS BIGINT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ReceiptId BIGINT
	DECLARE @FeeReceiptId BIGINT, @VoucherPrefix VARCHAR(20), @VoucherDetailId BIGINT;
	DECLARE @Digits TINYINT = 4;

	SELECT @VoucherPrefix = CAST([BranchCode] AS VARCHAR(10)) + [dbo].[fBuild2DigitYear](@ReceiptDate) + [dbo].[fBuild2DigitMonth](@ReceiptDate)
	FROM [Company].[Branches]
	WHERE [BranchId] = @BranchId;

	SELECT @FeeReceiptId = MAX(CAST(RIGHT(CAST([fi].FeeReceiptId AS VARCHAR(20)), @Digits) AS INT))
	FROM Fee.Receipts AS [fi]
	INNER JOIN [Fee].[Vouchers] AS [fv] ON [fi].[FeeVoucherId] = [fv].[FeeVoucherId]
	WHERE [fv].[BranchId] = @BranchId
		AND YEAR(ReceivedOn) = YEAR(@ReceiptDate)
		AND MONTH(ReceivedOn) = MONTH(@ReceiptDate);

	IF @FeeReceiptId IS NULL
	BEGIN
		SET @FeeReceiptId = 0;
	END;

	SET @FeeReceiptId += 1;
	SET @FeeReceiptId = CAST(@VoucherPrefix + RIGHT('00000000' + CAST(@FeeReceiptId AS VARCHAR(20)), @digits) AS BIGINT);

	-- Return the result of the function
	RETURN @FeeReceiptId
END
");

            //Setup, Contact,  Finance, HR, AssetManagement, Inventory, Shop, Academics, Front Desk, Resturant, Academics, Fee, Library, Global DAta, etc
            //context.Database.ExecuteSqlCommand($@"");
            //context.SaveChanges();
        }
    }
}

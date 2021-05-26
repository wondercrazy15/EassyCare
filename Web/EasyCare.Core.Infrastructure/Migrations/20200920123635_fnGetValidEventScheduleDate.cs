using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class fnGetValidEventScheduleDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			string procedure = @"CREATE FUNCTION [dbo].[fnGetValidEventScheduleDate]
(
	 @PeriodOfTime NVARCHAR(MAX)
	,@StartDate DATETIME
	,@EndDate DATETIME
	,@CurrentDate DATETIME2
)

RETURNS DATETIME

AS
BEGIN

	Declare @Date DATETIME =NULL;

	SET @Date=( SELECT CONVERT(DATETIME, CONVERT(CHAR(8), Convert(DATE, GETDATE()), 112) 
	+ ' ' + CONVERT(CHAR(8), CONVERT(TIME,@StartDate), 108)))
	
	IF dbo.fnGetPeriodOfTime(@PeriodOfTime) = 1
	BEGIN
		SET @Date = @Date
	END

	IF dbo.fnGetPeriodOfTime(@PeriodOfTime) = 2
	BEGIN
		SET @Date=DATEADD(minute, -5, @Date); 
	END

	ELSE IF dbo.fnGetPeriodOfTime(@PeriodOfTime) = 3
	BEGIN
		SET @Date = DATEADD(minute, -10, getdate())
	END

	ELSE IF dbo.fnGetPeriodOfTime(@PeriodOfTime) = 4
	BEGIN
		SET @Date = DATEADD(minute, -15, getdate()) 
	END

	ELSE IF dbo.fnGetPeriodOfTime(@PeriodOfTime) = 5
	BEGIN
		SET @Date = DATEADD(minute, -30, getdate()) 
	END

	ELSE IF dbo.fnGetPeriodOfTime(@PeriodOfTime) = 6
	BEGIN
		SET @Date = DATEADD(HOUR, -1, getdate())
	END

	ELSE IF dbo.fnGetPeriodOfTime(@PeriodOfTime) = 7
	BEGIN
		SET @Date =  DATEADD(HOUR, -2, getdate()) 
	END

	ELSE IF dbo.fnGetPeriodOfTime(@PeriodOfTime) = 8
	BEGIN
		SET @Date = dateadd(DAY, -1, getdate()) 
	END

	ELSE IF dbo.fnGetPeriodOfTime(@PeriodOfTime) = 9
	BEGIN
		SET @Date = dateadd(DAY, -2, getdate()) 
	END

	ELSE IF dbo.fnGetPeriodOfTime(@PeriodOfTime) = 10
	BEGIN
		SET @Date =  DATEADD(WEEK, -1, getdate()) 
	END

	RETURN @Date;
END

GO";
			migrationBuilder.Sql(procedure);

			
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			string procedure = @"CREATE function [dbo].[fnGetValidEventScheduleDate]";
			migrationBuilder.Sql(procedure);
		}
    }
}

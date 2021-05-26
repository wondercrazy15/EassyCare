using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class fnGetPeriodOfTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			string procedure = @"CREATE FUNCTION [dbo].[fnGetPeriodOfTime]
(
	@PeriodOfTime NVARCHAR(200)
)
RETURNS INT
AS
BEGIN

		/*	

		At time to event
		5 minutes before
		10 minutes before
		15 minutes before
		30 minutes before
		1 hour before
		2 hour before
		1 day before
		2 days before
		1 week before

		*/
	DECLARE @value INT=0;


	SET @value = CASE WHEN @PeriodOfTime = 'At time of event' THEN 1
						WHEN @PeriodOfTime = '5 minutes before' THEN 2
						WHEN @PeriodOfTime = '10 minutes before' THEN 3
						WHEN @PeriodOfTime = '15 minutes before' THEN 4
						WHEN @PeriodOfTime = '30 minutes before' THEN 5
						WHEN @PeriodOfTime = '1 hour before' THEN 6
						WHEN @PeriodOfTime = '2 hour before' THEN 7
						WHEN @PeriodOfTime = '1 day before' THEN 8
						WHEN @PeriodOfTime = '2 days before' THEN 9
						WHEN @PeriodOfTime = '1 week before' THEN 10
		END

	RETURN @value;
END
GO";
			migrationBuilder.Sql(procedure);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			string procedure = @"CREATE function [dbo].[fnGetPeriodOfTime]";
			migrationBuilder.Sql(procedure);
		}
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class fnGetDrugsScheduleDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			string procedure = @"CREATE FUNCTION [dbo].[fnGetDrugsScheduleDate]
(
	-- Add the parameters for the function here
	@StartDate DATETIME2 =null,
	@EndDate DATETIME2 =null,
	@CurrentDate DATETIME =null,
	@Days int =0
)
RETURNS DATE
AS
BEGIN
	-- Declare the return variable here
	Declare @Date DATE =NULL;
	-- Add the T-SQL statements to compute the return value here
	
	declare @tmpdtartdate DATETIME=Convert(date, @StartDate)

	if(@tmpdtartdate =Convert(date, @Currentdate))
	begin
		SET @Date=@tmpdtartdate;
		RETURN @Date;
	end
	if(@Days != 0)
	BEGIN
		while(datepart(day,cast(@EndDate as date)) >= datepart(day,cast(@tmpdtartdate as date)))
		begin
		  set @tmpdtartdate=DATEADD(day, @Days, @tmpdtartdate)

		  if(@tmpdtartdate <=Convert(date, @EndDate)  and @tmpdtartdate=Convert(date, @Currentdate))
		  begin
			SET @Date=@tmpdtartdate
		  end
		end
	END
	-- Return the result of the function
	RETURN @Date

END

GO";
			migrationBuilder.Sql(procedure);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			string procedure = @"CREATE function [dbo].[fnGetDrugsScheduleDate]";
			migrationBuilder.Sql(procedure);
		}
    }
}

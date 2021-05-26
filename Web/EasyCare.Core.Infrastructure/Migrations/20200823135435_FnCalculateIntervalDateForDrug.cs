using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class FnCalculateIntervalDateForDrug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			string procedure = @"CREATE FUNCTION [dbo].[CalculateIntervalDateForDrug]
(	
	-- Add the parameters for the function here
	@startdate date,
	@enddate date,
	@interval int
)
RETURNS @intervaltbl TABLE (
        currentdate date
    )
AS
BEGIN
	 

declare @tmpdtartdate date=@startdate
while(datepart(day,cast(@enddate as date)) >= datepart(day,cast(@tmpdtartdate as date)))
begin
  
  
  set @tmpdtartdate=DATEADD(day, @interval, @tmpdtartdate)
  if(@tmpdtartdate <= @enddate)
  begin
  insert into @intervaltbl(currentdate) values(@tmpdtartdate)
  end

end


RETURN;

END
GO";
			migrationBuilder.Sql(procedure);

		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			string procedure = @"CREATE function [dbo].[CalculateIntervalDateForDrug]";
			migrationBuilder.Sql(procedure);

		}
    }
}

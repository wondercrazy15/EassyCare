using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class UpdateSpGetEventDateBySupervisorIdAndMonth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"ALTER PROCEDURE [dbo].[GetEventDateBySupervisorIdAndMonth] 
(
	 @SupervisorId uniqueidentifier=null,
	 @Month int =null,
	 @Year int =null
)
AS
BEGIN

Declare @GroupID  uniqueidentifier = NULL
Declare @GroupParticipationCount int = 0
Declare @StartDate Datetime=null
Declare @EndDate Datetime=null

SET @StartDate=(select dateadd(month, @Month - 1, dateadd(year, @Year - 1900, 0)))


SET @EndDate=(select  dateadd(month, @Month,dateadd(year, @Year - 1900, -1)))

;WITH DateRange([Date],isDateAvailable) 
AS 
(
    SELECT @StartDate as [Date], [dbo].[fnIsValidEventDate](@SupervisorId, @StartDate)
    UNION ALL
    SELECT DATEADD(d,1,[Date]), [dbo].[fnIsValidEventDate](@SupervisorId, DATEADD(d,1,[Date]))
    FROM DateRange 
    WHERE [Date] < @EndDate 
)
SELECT DISTINCT Date
FROM DateRange WHERE isDateAvailable=1
OPTION (MAXRECURSION 0)

END


GO";

            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"ALTER PROCEDURE [dbo].[GetEventDateBySupervisorIdAndMonth] ";
            migrationBuilder.Sql(procedure);
        }
    }
}

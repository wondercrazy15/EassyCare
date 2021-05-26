using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class UpdateSpGetDrugsByDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"ALTER PROCEDURE [dbo].[GetDrugsByDate] 
(
	 @Date datetime2=null,
	 @SeniorId uniqueidentifier =null,
	 @SupervisorId  uniqueidentifier =null
)
AS
BEGIN
	select * from (SELECT 
	D.Id,
	D.[Name],
	D.Description,
	D.Image,
	D.StartDate,
	D.EndDate,
    D.[Timing],
	D.Days,
	D.SeniorId,
	D.SupervisorId,
	D.IsActive,
	D.ModifiedDate,
	D.Dosis,
    CASE WHEN CAST(D.[Timing] AS time) >= '05:00:00' AND
              CAST(D.[Timing] AS time) <= '12:00:00'
         THEN 'Morning'
         WHEN CAST(D.[Timing] AS time) > '12:00:00' AND
              CAST(D.[Timing] AS time) <= '17:00:00'
         THEN 'Afternoon'
		 WHEN CAST(D.[Timing] AS time) > '17:00:00' AND
              CAST(D.[Timing] AS time) <= '20:00:00'
         THEN 'Evening'
         ELSE 'Night'       
    END AS Time
FROM [dbo].[Drugs] D
WHERE (@Date BETWEEN StartDate AND EndDate) and
((D.SeniorId = ISNULL(@SeniorId, D.SeniorId) Or D.SeniorId is null) and
(D.SupervisorId = ISNULL(@SupervisorId, D.SupervisorId) Or D.SupervisorId is null))
)S
where cast(@Date as date) in (select * from [dbo].[CalculateIntervalDateForDrug](S.StartDate,S.EndDate,1))
order by s.Time desc 
END



GO";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"ALTER PROCEDURE [dbo].[GetDrugsByDate] ";
            migrationBuilder.Sql(procedure);
        }
    }
}

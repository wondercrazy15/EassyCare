using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class SpGetDrugsByDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE [dbo].[GetDrugsByDate] 
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
	D.Days as interval,
	D.SeniorId,
	D.SupervisorId,
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
where cast(@Date as date) in (select * from [dbo].[CalculateIntervalDateForDrug](S.StartDate,S.EndDate,S.interval))
order by s.Time desc 
END

GO";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE [dbo].[GetDrugsByDate] ";
            migrationBuilder.Sql(procedure);
        }
    }
}

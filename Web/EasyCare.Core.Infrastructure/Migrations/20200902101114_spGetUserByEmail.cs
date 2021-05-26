using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class spGetUserByEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Create PROCEDURE [dbo].[GetUserByEmail] 
(
	 @Email nvarchar(250)=null 
)
AS
BEGIN
	select 
	SV.Id SupervisorId
	,D.Id DeviceId
	,S.Id Seniors
	,SV.FirstName
	,SV.SecondName
	,SV.EMail
	,S.PostCode
	,S.Street
	,S.City
 from [dbo].[Supervisor] SV 
 left join [dbo].[Device] D ON D.SupervisorId=SV.Id
 left join [dbo].[Seniors] S ON S.Id=D.SeniorId
 where SV.EMail=@Email
END

GO";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE [dbo].[GetUserByEmail] ";
            migrationBuilder.Sql(procedure);
        }
    }
}

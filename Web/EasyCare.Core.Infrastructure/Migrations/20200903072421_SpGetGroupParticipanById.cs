using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class SpGetGroupParticipanById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE [dbo].[GetGroupParticipanById]
(
	 @ParticipationId uniqueidentifier=null 
)
AS
BEGIN
	select S.* from [dbo].[Participant] P
	 left join [dbo].[Group] G on G.Id=P.GroupId
	 left join [dbo].[Participant] PT on PT.GroupId=G.Id
	 left join [dbo].[Supervisor] S on S.Id=PT.ParticipationId
	 where P.ParticipationId =@ParticipationId
END

GO";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE [dbo].[GetGroupParticipanById] ";
            migrationBuilder.Sql(procedure);
        }
    }
}

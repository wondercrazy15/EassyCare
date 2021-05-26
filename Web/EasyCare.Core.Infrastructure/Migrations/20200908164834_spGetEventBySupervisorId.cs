using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class spGetEventBySupervisorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE [dbo].[GetEventBySupervisorId]
(
	 @SupervisorId uniqueidentifier=null 
)
AS
BEGIN
	select CE.* from [dbo].[Participant] P
	 left join [dbo].[Group] G on G.Id=P.GroupId
	 left join [dbo].[Participant] PT on PT.GroupId=G.Id
	 join [dbo].[CalendarEvent] CE on CE.SupervisorId=PT.ParticipationId
	 where P.ParticipationId =@SupervisorId
END

GO";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE [dbo].[GetEventBySupervisorId] ";
            migrationBuilder.Sql(procedure);
        }
    }
}

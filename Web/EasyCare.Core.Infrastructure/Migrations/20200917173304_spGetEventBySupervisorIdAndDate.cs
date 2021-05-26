using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class spGetEventBySupervisorIdAndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE [dbo].[GetEventBySupervisorIdAndDate] 
(
	 @SupervisorId uniqueidentifier=null,
	 @Currentdate datetime = null
)
AS
BEGIN

Declare @GroupID  uniqueidentifier = NULL
Declare @GroupParticipationCount int = 0


SET  @GroupID= (SELECT TOP 1  GroupId from Participant Where ParticipationId=@SupervisorID)

SET  @GroupParticipationCount=(select COUNT(*) from [dbo].[Participant] P
	where P.ParticipationId =@SupervisorId)


	Select CE.* From (
	Select Distinct EventID From (
	select CE.Id EventID from [dbo].[CalendarEvent]  CE
	JOIN CalendarEventSchedulers CES_Daily ON CE.Id = CES_Daily.CalenderEventId AND CES_Daily.IsDaily=1
	Where Convert(date,@Currentdate)>= Convert(Date,CE.StartDate) 
		AND Convert(date, @Currentdate)  < convert(date,DATEADD(dd, 1,CE.EndDate))

	Union 
	 select CE.Id EventID from [dbo].[CalendarEvent]  CE
	 join CalendarEventSchedulers CES_Weekly ON CE.Id = CES_Weekly.CalenderEventId AND CES_Weekly.IsWeekly=1
	 Where Convert(date,@Currentdate)>= Convert(Date,CE.StartDate) 
		AND Convert(date, @Currentdate)  < convert(date,DATEADD(dd, 1,CE.EndDate))
			AND  DateName(Weekday,@Currentdate )  =CES_Weekly.[WeekDay]
	Union 
	 select CE.Id EventID from [dbo].[CalendarEvent]  CE
	 join CalendarEventSchedulers CES_Monthly ON CE.Id = CES_Monthly.CalenderEventId AND CES_Monthly.IsMonthly=1
	 Where Convert(date,@Currentdate)>= Convert(Date,CE.StartDate) 
		AND Convert(date, @Currentdate)  < convert(date,DATEADD(dd, 1,CE.EndDate))
			AND  DATEPART(DAY,@Currentdate)  =CES_Monthly.MonthDate
	)DistinctEvents
	)AllEvents
	Left Join [CalendarEvent] CE ON AllEvents.EventID = CE.Id
	Where CE.SupervisorId IN (CASE WHEN @GroupParticipationCount>0 THEN 
		( SELECT TOP 1 ParticipationId from Participant Where GroupId=@GroupID)
		ELSE @SupervisorId END)

END

GO";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE [dbo].[GetEventBySupervisorIdAndDate] ";
            migrationBuilder.Sql(procedure);
        }
    }
}

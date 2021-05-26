using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class spGetEventScheduler : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE GetEventScheduler
	
AS
BEGIN

	DECLARE @Currentdate DATETIME=NULL;
	SET @Currentdate =GETDATE();

	Select CE.Id EventId
	,CE.Title
	,[dbo].[fnGetValidEventScheduleDate](CES.PeriodOfTime,CE.StartDate ,CE.EndDate,GETDATE()) as [Date] 
	,T.Code TagId
	From (
	Select Distinct EventID From (

	select CE.Id EventID from [dbo].[CalendarEvent]  CE
	JOIN CalendarEventSchedulers CES_Never ON CE.Id = CES_Never.CalenderEventId AND Convert(Date,CES_Never.OneTimeDate) =Convert(Date,@CurrentDate) 

	Union

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

	--Union 
	-- select CE.Id EventID from [dbo].[CalendarEvent]  CE
	-- join CalendarEventSchedulers CES_2Weekly ON CE.Id = CES_2Weekly.CalenderEventId AND CES_2Weekly.Is2Weekly=1
	-- Where Convert(date,@Currentdate)>= Convert(Date,CE.StartDate) 
	--	AND Convert(date, @Currentdate)  < convert(date,DATEADD(dd, 1,CE.EndDate))
	--		AND  DateName(Weekday,@Currentdate )  =CES_2Weekly.[WeekDay]

	Union 
	 select CE.Id EventID from [dbo].[CalendarEvent]  CE
	 join CalendarEventSchedulers CES_Monthly ON CE.Id = CES_Monthly.CalenderEventId AND CES_Monthly.IsMonthly=1
	 Where Convert(date,@Currentdate)>= Convert(Date,CE.StartDate) 
		AND Convert(date, @Currentdate)  < convert(date,DATEADD(dd, 1,CE.EndDate))
			AND  DATEPART(DAY,@Currentdate)  =CES_Monthly.MonthDate
	)DistinctEvents
	)AllEvents
	Left Join [CalendarEvent] CE ON AllEvents.EventID = CE.Id
	Left Join CalendarEventSchedulers CES ON CE.Id = CES.CalenderEventId
	Left Join [Device] D ON D.SupervisorId=CE.SupervisorId
	Left Join [TAN] T ON T.DeviceId=D.id
	
END
GO


GO";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			string procedure = @"CREATE PROCEDURE [dbo].[GetEventScheduler] ";
			migrationBuilder.Sql(procedure);
		}
    }
}

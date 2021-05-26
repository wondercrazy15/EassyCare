using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class updateSpGetEventScheduler : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"ALTER PROCEDURE [dbo].[GetEventScheduler]
	
AS
BEGIN

	DECLARE @Currentdate DATETIME=NULL;
	SET @Currentdate =GETDATE();

	Select CE.Id EventId
	,CE.Title
	,[dbo].[fnGetValidEventScheduleDate](CES.PeriodOfTime,CE.StartDate ,CE.EndDate,GETDATE()) as [Date] 
	,T.NotificationTagCode Tags, D.SupervisorId,D.Id DeviceId
	From (
	Select Distinct EventID From (

	select CE.Id EventID from [dbo].[CalendarEvent]  CE
	JOIN CalendarEventSchedulers CES_Never ON CE.Id = CES_Never.CalenderEventId 
	AND Convert(Date,CES_Never.OneTimeDate) =Convert(Date,@CurrentDate) 

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

	Union
	select CE.Id EventID from [dbo].[CalendarEvent]  CE
	 join CalendarEventSchedulers CES_2Weekly ON CE.Id = CES_2Weekly.CalenderEventId AND CES_2Weekly.Is2Weekly=1
	 Where Convert(date,@Currentdate)>= Convert(Date,CE.StartDate) 
		AND Convert(date, @Currentdate)  < convert(date,DATEADD(dd, 1,CE.EndDate))
			AND  DateName(Weekday,@Currentdate )  =CES_2Weekly.[WeekDay]
				AND Convert(date, @Currentdate) = (select [dbo].fnGetDrugsScheduleDate(CE.StartDate,CE.EndDate,@Currentdate,14) as [Date])

	
	Union 
	 select CE.Id EventID from [dbo].[CalendarEvent]  CE
	 join CalendarEventSchedulers CES_Monthly ON CE.Id = CES_Monthly.CalenderEventId AND CES_Monthly.IsMonthly=1
	 Where Convert(date,@Currentdate)>= Convert(Date,CE.StartDate) 
		AND Convert(date, @Currentdate)  < convert(date,DATEADD(dd, 1,CE.EndDate))
			AND  DATEPART(DAY,@Currentdate)  =CES_Monthly.MonthDate

	Union
	select CE.Id EventID from [dbo].[CalendarEvent]  CE
	 join CalendarEventSchedulers CES_Yearly ON CE.Id = CES_Yearly.CalenderEventId AND CES_Yearly.IsYearly=1
	 Where Convert(date,@Currentdate)>= Convert(Date,CE.StartDate) 
		AND Convert(date, @Currentdate)  < convert(date,DATEADD(dd, 1,CE.EndDate))
			AND  DateName(MONTH,@Currentdate )  =CES_Yearly.[Month]
				AND DateName(DAY,@Currentdate )=CES_Yearly.MonthDate

	Union 
	 select CE.Id EventID from [dbo].[CalendarEvent]  CE
	 join CalendarEventSchedulers CES_Days ON CE.Id = CES_Days.CalenderEventId
	 Where Convert(date,@Currentdate)>= Convert(Date,CE.StartDate) 
		AND Convert(date, @Currentdate)  < convert(date,DATEADD(dd, 1,CE.EndDate))
			AND Convert(date, @Currentdate) = (select [dbo].fnGetDrugsScheduleDate(CE.StartDate,CE.EndDate,@Currentdate,CES_Days.Days) as [Date])

	)DistinctEvents
	)AllEvents
	 left Join [CalendarEvent] CE ON AllEvents.EventID = CE.Id
	 left Join CalendarEventSchedulers CES ON CE.Id = CES.CalenderEventId
	 left Join [dbo].[CalendarSupervisors] CS ON CS.CalenderEventId=CE.Id
	 left Join [Device] D ON D.SupervisorId=CS.SupervisorId
	 left Join [TAN] T ON T.DeviceId=D.id
	 where CES.PeriodOfTime !='None'
	
END

GO";
			migrationBuilder.Sql(procedure);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			string procedure = @"ALTER PROCEDURE [dbo].[GetEventScheduler] ";
			migrationBuilder.Sql(procedure);
		}
    }
}

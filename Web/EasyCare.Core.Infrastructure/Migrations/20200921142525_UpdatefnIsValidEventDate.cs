using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyCare.Core.Infrastructure.Migrations
{
    public partial class UpdatefnIsValidEventDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			string procedure = @"ALTER FUNCTION [dbo].[fnIsValidEventDate] 
(
	 @SupervisorId uniqueidentifier=null,
	 @Currentdate datetime = null
)
RETURNS BIT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @status BIT=0;

	-- Add the T-SQL statements to compute the return value here
	Declare @GroupID  uniqueidentifier = NULL
	Declare @GroupParticipationCount int = 0
	Declare @TotalCounts int=0

	SET  @GroupID= (SELECT TOP 1  GroupId from Participant Where ParticipationId=@SupervisorID)

	SET  @GroupParticipationCount=(select COUNT(*) from [dbo].[Participant] P
		where P.ParticipationId =@SupervisorId)


	SET @TotalCounts=(Select COUNT(*) From (
	Select Distinct EventID From (

	select CE.Id EventID from [dbo].[CalendarEvent]  CE
	JOIN CalendarEventSchedulers CES_Single ON CE.Id = CES_Single.CalenderEventId 
	Where 
	Convert(date,@Currentdate)>= Convert(Date,CE.StartDate) 
		AND Convert(date, @Currentdate)  < convert(date,DATEADD(dd, 1,CE.EndDate))
			AND Convert(date, CES_Single.OneTimeDate)=Convert(date, @Currentdate)

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
			AND Convert(date, @Currentdate) = (select [dbo].fnGetDrugsScheduleDate(CE.StartDate,CE.EndDate,@Currentdate,1) as [Date])
	
	)DistinctEvents
	)AllEvents
	Left Join [CalendarEvent] CE ON AllEvents.EventID = CE.Id
	Where CE.SupervisorId IN (CASE WHEN @GroupParticipationCount>0 THEN 
		( SELECT TOP 1 ParticipationId from Participant Where GroupId=@GroupID)
		ELSE @SupervisorId END))


	SET @status = CASE WHEN @TotalCounts>0 THEN 1 ELSE 0 END
	
	-- Return the result of the function
	RETURN @status;

END


GO";
			migrationBuilder.Sql(procedure);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			string procedure = @"ALTER function [dbo].[fnIsValidEventDate]";
			migrationBuilder.Sql(procedure);
		}
    }
}

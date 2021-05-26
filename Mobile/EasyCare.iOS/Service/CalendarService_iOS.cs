using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCare.DataService;
using EasyCare.iOS.Service;
using EventKit;
using Foundation;
using Syncfusion.SfCalendar.XForms;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(CalendarService_iOS))]
namespace EasyCare.iOS.Service
{
    public class CalendarService_iOS : ICalandarService
    {
        public EKEventStore eventStore { get; set; }
        private bool accessGranted = false;

        public void CreateService()
        {
            if (eventStore == null)
            {
                eventStore = new EKEventStore();
                eventStore.RequestAccess(EKEntityType.Event,
                    (bool granted, NSError e) =>
                    {
                        if (granted)
                        {
                            accessGranted = true;
                        }
                        else
                        {
                            accessGranted = false;
                            new UIAlertView("Access Denied",
"User Denied Access to Calendar Data", null,
"ok", null).Show();
                        }
                    });
            }
        }

        public async Task<bool> CreateEvent(CalendarEventCollection eventsList)
        {
            try
            {
                var granted = await eventStore.RequestAccessAsync(EKEntityType.Event);//, (bool granted, NSError e) =>

                if (granted.Item1)
                {

                  

                    foreach (var @event in eventsList)
                    {
                        var startdate = @event.StartTime.AddDays(-2);
                        var enddate = @event.EndTime.AddDays(2);
                        NSPredicate nSPredicate = eventStore.PredicateForEvents((NSDate)startdate, (NSDate)enddate, null);
                        var events = eventStore.EventsMatching(nSPredicate);
                        bool IsAdded = false;

                        foreach (var item in events.ToList())
                        {
                            if (item.Title != null && item.Title.Equals(@event.Subject))
                            {
                                IsAdded = true;
                                break;
                            }
                        }

                        if (!IsAdded)
                        {
                            EKEvent newEvent = EKEvent.FromStore(eventStore);
                            // set the alarm for 10 minutes from now
                            //newEvent.AddAlarm(EKAlarm.FromDate((NSDate)appointment.));
                            // make the event start 20 minutes from now and last 30 minutes
                            newEvent.StartDate = (NSDate)@event.StartTime;
                            newEvent.EndDate = (NSDate)@event.EndTime;
                            newEvent.Title = @event.Subject;

                            newEvent.Calendar = eventStore.DefaultCalendarForNewEvents;
                            NSError e;
                            eventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out e);
                        }
                    }
                    return true;
                }
                else
                {
                    new UIAlertView("Access Denied", "User Denied Access to Calendar Data", null, "ok", null).Show();
                    return false;
                }
                   
                // });

                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

       
    }
}

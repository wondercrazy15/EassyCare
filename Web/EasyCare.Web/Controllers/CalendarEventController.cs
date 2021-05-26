using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using EasyCare.Core.Constants;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Dto;
using EasyCare.Core.Infrastructure.Repository;
using EasyCare.Core.Services.Notification;
using EasyCare.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyCare.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarEventController : CrudController<CalendarEvent, CalendarEventDto>
    {
        private readonly ICalendarSupervisorsRepository _calendarSupervisorsRepository;
        private readonly ICalendarEventSchedulersRepository _calendarEventSchedulersRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly ITANRepository _tANRepository;
        private readonly INotificationService _notificationService;
        public CalendarEventController(ICalendarEventRepository repository, ILogger<CalendarEventController> logger, IMapper mapper
            , ICalendarEventSchedulersRepository calendarEventSchedulersRepository
            , ICalendarSupervisorsRepository calendarSupervisorsRepository
            , ISupervisorRepository supervisorRepository
            , ITANRepository tANRepository
            , INotificationService notificationService)
            : base(repository, logger, mapper)
        {
            _calendarEventSchedulersRepository = calendarEventSchedulersRepository;
            _calendarSupervisorsRepository = calendarSupervisorsRepository;
            _supervisorRepository = supervisorRepository;
            _tANRepository = tANRepository;
            _notificationService = notificationService;
        }


        [HttpPost]
        public override async Task<ActionResult<CalendarEventDto>> PostItem(CalendarEventDto item)
        {
            try
            {
                CultureInfo culture = new CultureInfo("en-US");
                DateTime startDate = StringMyDateTimeFormatExtension.ParseMyFormatDateTime(item.StartDate);
                DateTime endDate = StringMyDateTimeFormatExtension.ParseMyFormatDateTime(item.EndDate);
                List<string> CalenderSupervisorIds = new List<string>();

                var currentDate =  DateTime.Now.ToString("MM/dd/yyyy", culture);
                var eventDate = startDate.ToString("MM/dd/yyyy", culture);

                CalendarEventDto _objCalendarEvent = new CalendarEventDto();
                CalendarEvent objCalendarEvent = new CalendarEvent();
                var entity = _mapper.Map<CalendarEvent>(item);
                entity.Type = NotificationTypeConstant.Event;
                var result = await _repository.Add(entity);
                _objCalendarEvent = _mapper.Map<CalendarEventDto>(result);
               

                if (result != null)
                {
                    item.Id = result.Id;
                    var calendarEventSchedulerEntity = _mapper.Map<CustomCalendarEventSchedulers>(item);
                    var eventSchedulersResult = await _calendarEventSchedulersRepository.AddCalendarEventSchedulers(calendarEventSchedulerEntity);
                    _objCalendarEvent.PeriodOfTime = eventSchedulersResult.PeriodOfTime;
                   
                    if (item.CalenderSupervisorId.Length > 0)
                    {
                        var calenderSupervisorResult = await _calendarSupervisorsRepository.AddOrUpdateCalendarSupervisors(result.Id, item.CalenderSupervisorId, startDate);

                        if (calenderSupervisorResult)
                            _objCalendarEvent.CalenderSupervisorId = item.CalenderSupervisorId;

                        //Send current date notification
                        if (currentDate == eventDate)
                        {
                            var eventScheduleDate = await (_repository as ICalendarEventRepository).GetValidEventScheduleDate(item.PeriodOfTime, startDate, endDate, DateTime.Now);
                            _objCalendarEvent.EventScheduleDate= eventScheduleDate.Date;
                            if (eventScheduleDate != null)
                            {
                                var Tags = await _tANRepository.GetTagsByEventId(result.Id);
                                if (Tags.Count() > 0)
                                {
                                    NotificationScheduleRequestDto notificationSchedule = new NotificationScheduleRequestDto();
                                    notificationSchedule.Tags = Tags.Select(x=>x.Tags).ToArray();
                                   
                                    notificationSchedule.ScheduleDate = eventScheduleDate.Date;
                                    notificationSchedule.Text = result.Title;
                                    notificationSchedule.Silent = false;
                                    notificationSchedule.Title = "Upcoming event";
                                    notificationSchedule.Type = NotificationTypeConstant.Event;
                                    notificationSchedule.Action = "Event Notification";

                                    var success = await _notificationService
                                .SendcheduleNotificationAsync(notificationSchedule, HttpContext.RequestAborted);
                                }
                            }
                        }
                    }
                }
                return Ok(_objCalendarEvent);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public override async Task<ActionResult<CalendarEventDto>> PutItem(Guid id, CalendarEventDto item)
        {
            try
            {
                CalendarEventDto objCalendarEvent = new CalendarEventDto();
                DateTime startDate = Convert.ToDateTime(item.StartDate);
                var calendarEventExist = _repository.Get(item.Id);

                if (calendarEventExist.Result != null)
                {
                    var entity = _mapper.Map<CalendarEvent>(item);
                    entity.Type = NotificationTypeConstant.Event;
                    var result = await _repository.Update(entity.Id, entity);
                    if (result)
                    {
                        var calendarEventResult = await _repository.Get(entity.Id);
                        objCalendarEvent = _mapper.Map<CalendarEventDto>(calendarEventResult);

                        var calendarEventSchedulerEntity = _mapper.Map<CustomCalendarEventSchedulers>(item);
                        var calenderSupervisorResult = await _calendarSupervisorsRepository.AddOrUpdateCalendarSupervisors(item.Id, item.CalenderSupervisorId, startDate);
                        if(calenderSupervisorResult)
                        {
                            objCalendarEvent.SupervisorId = item.SupervisorId;
                        }

                        var calendarEventSchedulerResult = await _calendarEventSchedulersRepository.UpdateCalendarEventSchedulers(calendarEventSchedulerEntity);
                        if (calendarEventSchedulerResult)
                        {
                            objCalendarEvent.PeriodOfTime = item.PeriodOfTime;
                            objCalendarEvent.EventSchedule = item.EventSchedule;
                        }
                        
                        //if (calenderSupervisorResult)
                        //{
                        //    objCalendarEvent.Supervisors = new List<SupervisorDto>();
                        //    foreach (var supervisorId in item.CalenderSupervisorId)
                        //    {
                        //        var objSupervisor = _supervisorRepository.Get(Guid.Parse(supervisorId));
                        //        if (objSupervisor.Result != null)
                        //        {
                        //            var _objSupervisor = _mapper.Map<SupervisorDto>(objSupervisor.Result);
                        //            _objSupervisor.Image = string.IsNullOrEmpty(_objSupervisor.Image) ? "/EasyCare/User/" + _objSupervisor.Id : _objSupervisor.Image;
                        //            objCalendarEvent.Supervisors.Add(_objSupervisor);
                        //        }
                        //    }
                        //}
                        return Ok(objCalendarEvent);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, item);
                return BadRequest();
            }
            return Ok();
        }


        [HttpGet("supervisor/{supervisorId}")]
        public async Task<IActionResult> GetBySupervisorId(Guid supervisorId)
        {
            try
            {
                var entities = await (_repository as ICalendarEventRepository).GetBySupervisorId(supervisorId);
                var result = _mapper.Map<IEnumerable<CalendarEventDto>>(entities);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet()]
        [Route("GetEventBySupervisorId/{supervisorId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetEventBySupervisorId(Guid supervisorId)
        {
            try
            {
                List<CalendarEventDto> objCalendarEvent = new List<CalendarEventDto>();
                var entities = await (_repository as ICalendarEventRepository).GetEventBySupervisorId(supervisorId);

                if (entities != null)
                {
                    objCalendarEvent = _mapper.Map<List<CalendarEventDto>>(entities);
                    foreach (var item in objCalendarEvent)
                    {
                        var calendarSupervisorEntities = await _supervisorRepository.GetCalendarSupervisorsByEventId(item.Id);
                        if (calendarSupervisorEntities != null)
                        {
                            item.Supervisors = new List<SupervisorDto>();
                            var result = _mapper.Map<List<SupervisorDto>>(calendarSupervisorEntities);
                            foreach (var items in result)
                            {
                                items.Image = string.IsNullOrEmpty(items.Image) ? "/EasyCare/User/" + items.Id : items.Image;

                            }

                            item.Supervisors.AddRange(result);
                        }
                    }
                    return Ok(objCalendarEvent);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet()]
        [Route("GetEventBySupervisorIdAndDate/{supervisorId}/{Date}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetEventBySupervisorIdAndDate(Guid supervisorId, string Date)
        {
            try
            {
                List<CalendarEventDto> objCalendarEvent = new List<CalendarEventDto>();
                var entities = await (_repository as ICalendarEventRepository).GetEventBySupervisorIdAndDate(supervisorId, Date);

                if (entities != null)
                {
                    objCalendarEvent = _mapper.Map<List<CalendarEventDto>>(entities);
                    foreach (var item in objCalendarEvent)
                    {
                        var calendarSupervisorEntities = await _supervisorRepository.GetCalendarSupervisorsByEventId(item.Id);
                        if (calendarSupervisorEntities != null)
                        {
                            item.Supervisors = new List<SupervisorDto>();
                            var result = _mapper.Map<List<SupervisorDto>>(calendarSupervisorEntities);
                            foreach (var items in result)
                            {
                                items.Image = string.IsNullOrEmpty(items.Image) ? "/EasyCare/User/" + items.Id : items.Image;
                            }
                            item.Supervisors.AddRange(result);
                        }
                    }
                    return Ok(objCalendarEvent);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }


        [HttpGet()]
        [Route("GetEventDateBySupervisorIdAndMonth/{supervisorId}/{Month}/{Year}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetEventDateBySupervisorIdAndMonth(Guid supervisorId, int Month,int Year)
        {
            try
            {
                var entities = await (_repository as ICalendarEventRepository).GetEventDateBySupervisorIdAndMonth(supervisorId, Month,Year);

                if (entities.Count() >0)
                {
                    var result = _mapper.Map<List<CalendarEventDateDto>>(entities);
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet()]
        [Route("GetCalendarEventById/{Id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetCalendarEventById(Guid Id)
        {
            try
            {
                var entity = await _repository.Get(Id);

                if (entity != null)
                {
                    var result = _mapper.Map<CalendarEventDto>(entity);

                    var calendarSupervisorEntities = await _supervisorRepository.GetCalendarSupervisorsByEventId(result.Id);
                    if (calendarSupervisorEntities != null)
                    {
                        result.Supervisors = new List<SupervisorDto>();
                        var calendarSupervisorResult = _mapper.Map<List<SupervisorDto>>(calendarSupervisorEntities);
                        foreach (var item in calendarSupervisorResult)
                        {
                            item.Image = string.IsNullOrEmpty(item.Image) ? "/EasyCare/User/" + item.Id : item.Image;
                        }
                        result.Supervisors.AddRange(calendarSupervisorResult);
                    }

                    var eventSchedulerResult = await _calendarEventSchedulersRepository.GetEventSchedulersByEventId(entity.Id);
                    if(eventSchedulerResult != null)
                    {
                        result.PeriodOfTime = eventSchedulerResult.PeriodOfTime;
                        if (eventSchedulerResult.OneTimeDate != null)
                            result.EventSchedule = EventScheduleconsConstant.Never;
                        else if (eventSchedulerResult.IsDaily)
                            result.EventSchedule = EventScheduleconsConstant.EveryDay;
                        else if (eventSchedulerResult.IsWeekly)
                            result.EventSchedule = EventScheduleconsConstant.EveryWeek;
                        else if (eventSchedulerResult.Is2Weekly)
                            result.EventSchedule = EventScheduleconsConstant.Every2Weeks;
                        else if (eventSchedulerResult.IsMonthly)
                            result.EventSchedule = EventScheduleconsConstant.EveryMonth;
                        else if (eventSchedulerResult.IsYearly)
                            result.EventSchedule = EventScheduleconsConstant.EveryYear;
                        else
                            result.EventSchedule = EventScheduleconsConstant.Never;
                    }

                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }
    }
}
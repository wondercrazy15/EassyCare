using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.Core.Infrastructure.Repository;
using EasyCare.Core.Services.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Chat.V2;

namespace EasyCare.Web.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;
        private readonly ICalendarEventSchedulersRepository _calendarEventSchedulersRepository;
        private readonly ITANRepository _tANRepository;
        private readonly ISupervisorRepository _supervisorRepository;

        public NotificationsController(INotificationService notificationService, ILogger<NotificationsController> logger, ICalendarEventSchedulersRepository calendarEventSchedulersRepository, ITANRepository tANRepository, ISupervisorRepository supervisorRepository)
        {
            _notificationService = notificationService;
            _logger = logger;
            _calendarEventSchedulersRepository = calendarEventSchedulersRepository;
            _tANRepository = tANRepository;
            _supervisorRepository = supervisorRepository;
        }

        [HttpPut]
        [Route("installations")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> UpdateInstallation([Required] [FromBody] DeviceInstallationDto deviceInstallationDto)
        {
            try
            {
                var success = await _notificationService
                    .CreateOrUpdateInstallationAsync(deviceInstallationDto, HttpContext.RequestAborted);

                if (!success)
                    return new UnprocessableEntityResult();

                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, deviceInstallationDto);
                return BadRequest();
            }
        }

        [HttpDelete()]
        [Route("installations/{installationId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<ActionResult> DeleteInstallation([Required][FromRoute] string installationId)
        {
            try
            {
                var success = await _notificationService
                    .DeleteInstallationByIdAsync(installationId, CancellationToken.None);

                if (!success)
                    return new UnprocessableEntityResult();

                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, installationId);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("requests")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> RequestPush([Required] NotificationRequestDto notificationRequest)
        {
            try
            {
                if ((notificationRequest.Silent && string.IsNullOrWhiteSpace(notificationRequest?.Action)) ||
                    (!notificationRequest.Silent && string.IsNullOrWhiteSpace(notificationRequest?.Text)))
                {
                    return new BadRequestResult();
                }

                var success = await _notificationService
                    .RequestNotificationAsync(notificationRequest, HttpContext.RequestAborted);

                if (!success)
                    return new UnprocessableEntityResult();

                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, notificationRequest);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("NotificationScheduler")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> NotificationScheduler()
        {
            try
            {
                NotificationScheduleRequestDto notificationRequest = new NotificationScheduleRequestDto();
                var result = await _calendarEventSchedulersRepository.GetEventScheduler();
                //var success = await _notificationService
                //    .SendcheduleNotificationAsync(notificationRequest, HttpContext.RequestAborted);

                if (result.Count() > 0)
                {
                    var eventIds = result.Select(x => x.EventId).Distinct().ToList();
                    if (eventIds.Count() > 0)
                    {
                        foreach (var item in eventIds)
                        {
                            var eventSchedule = result.Where(x => x.EventId == item).FirstOrDefault();
                            var eventscheduleTags = result.Where(x => x.EventId == item).Select(x => x.Tags).ToList();
                            notificationRequest.Text = eventSchedule.Title;
                            notificationRequest.Silent = false;
                            notificationRequest.Action = "Event Notification";
                            notificationRequest.ScheduleDate = eventSchedule.Date;
                            notificationRequest.Tags = eventscheduleTags.ToArray();
                            notificationRequest.Title = "Upcoming event";
                            notificationRequest.Type = NotificationTypeConstant.Event;

                            var success = await _notificationService
                                .SendcheduleNotificationAsync(notificationRequest, HttpContext.RequestAborted);
                        }
                    }

                }
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("SendMessageNotification")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> SendMessageNotification([FromForm] string URL, [FromForm] TwilioDto Parameters)
        {
            try
            {
                string GroupId = "", SupervisiorId = "";
                var attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(Parameters.Attributes);
                foreach (var item in attributes)
                {
                    if (item.Key == "GroupId")
                    {
                        GroupId = item.Value;
                    }
                    if (item.Key == "SupervisiorId")
                    {
                        SupervisiorId = item.Value;
                    }
                }

                if (!string.IsNullOrEmpty(GroupId) && !string.IsNullOrEmpty(SupervisiorId))
                {
                    Guid groupId = Guid.Parse(GroupId);
                    Guid supervisiorId = Guid.Parse(SupervisiorId);

                    var Tags = await _tANRepository.GetTagsByGroupId(groupId, supervisiorId);
                    if (Tags.Count() > 0)
                    {
                        NotificationRequestDto notificationRequest = new NotificationRequestDto();

                        var objSupervisior = await _supervisorRepository.Get(supervisiorId);
                        if (objSupervisior != null)
                        {
                            notificationRequest.Title = "New Message from " + objSupervisior.FirstName + " " + objSupervisior.SecondName;
                        }
                        notificationRequest.Type = NotificationTypeConstant.Message;
                        notificationRequest.Tags = Tags.Select(x => x.Tags).ToArray();
                        notificationRequest.Text = Parameters.Body;
                        notificationRequest.Action = "Message Notification";
                        notificationRequest.Silent = false;

                        var success = await _notificationService
                        .RequestNotificationAsync(notificationRequest, HttpContext.RequestAborted);

                        if (!success)
                            return new UnprocessableEntityResult();

                        return new OkResult();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
        }

    }
}

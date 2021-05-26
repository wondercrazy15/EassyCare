using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using EasyCare.Core.Constants;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Dto;
using EasyCare.Core.Infrastructure.Repository;
using EasyCare.Core.Services.File;
using EasyCare.Web.Controllers.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyCare.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugsController : CrudController<Drugs, DrugsDto>
    {
        private readonly IFileService _fileService;
        private readonly ICalendarEventRepository _calendarEventRepository;
        private readonly ICalendarEventSchedulersRepository _calendarEventSchedulersRepository;
        public DrugsController(IDrugsRepository repository, ILogger<DrugsController> logger, IMapper mapper, IFileService fileService, ICalendarEventRepository calendarEventRepository, ICalendarEventSchedulersRepository calendarEventSchedulersRepository)
            : base(repository, logger, mapper)
        {
            _fileService = fileService;
            _calendarEventRepository = calendarEventRepository;
            _calendarEventSchedulersRepository = calendarEventSchedulersRepository;
        }
        [HttpPost]
        public override async Task<ActionResult<DrugsDto>> PostItem(DrugsDto item)
        {
            try
            {
                item.Id = Guid.NewGuid();
                string fileDirectory = "Drugs";
                item.Image = await _fileService.SaveFile(item.Image, fileDirectory, item.Id);
                item.IsActive = true;
                var entity = _mapper.Map<Drugs>(item);
                var result = await _repository.Add(entity);

                if (result != null)
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    CalendarEvent eventEntity = new CalendarEvent();
                    eventEntity.Title = item.Name;
                    eventEntity.Type = NotificationTypeConstant.Drugs;
                    eventEntity.StartDate = StringMyDateTimeFormatExtension.ParseMyFormatDateTime(item.StartDate);
                    eventEntity.EndDate = StringMyDateTimeFormatExtension.ParseMyFormatDateTime(item.EndDate);
                    eventEntity.SupervisorId = (Guid)item.SupervisorId;

                    var calendarEventResult = await _calendarEventRepository.Add(eventEntity);

                    if (calendarEventResult != null)
                    {
                        CustomCalendarEventSchedulers calendarEventSchedulerEntity = new CustomCalendarEventSchedulers();

                        calendarEventSchedulerEntity.Id = calendarEventResult.Id;
                        calendarEventSchedulerEntity.StartDate = calendarEventResult.StartDate;
                        calendarEventSchedulerEntity.Days = item.Days;
                        calendarEventSchedulerEntity.PeriodOfTime = "At time of event";
                        calendarEventSchedulerEntity.EventSchedule = EventScheduleconsConstant.EveryDay;

                        var eventSchedulersResult = await _calendarEventSchedulersRepository.AddCalendarEventSchedulers(calendarEventSchedulerEntity);
                    }
                }

                return Ok(entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, item);
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public override async Task<ActionResult<DrugsDto>> PutItem(Guid id, DrugsDto item)
        {
            try
            {
                string fileDirectory = "Drugs";
                var drugsExist = await _repository.Get(item.Id);
                if (drugsExist != null)
                {
                    var deleteFile = await _fileService.DeleteFile(fileDirectory, id);
                    if (deleteFile)
                    {
                        item.Image = await _fileService.SaveFile(item.Image, fileDirectory, id);
                        item.ModifiedDate = DateTime.Now;

                        var entity = _mapper.Map<Drugs>(item);
                        var result = await _repository.Update(entity.Id, entity);
                        if (result)
                        {
                            var objGroup = await _repository.Get(item.Id);
                            return Ok(objGroup);
                        }
                    }
                    return NotFound();

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
        }




        [HttpGet()]
        [Route("GetDrugsByDate/{Date}/{SeniorId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetDrugsByDate(string Date = null, string SeniorId = null)
        {
            try
            {
                List<DrugDto> _objDrugs = new List<DrugDto>();
                var entities = await (_repository as IDrugsRepository).GetDrugsByDate(Date, SeniorId);
                var result = _mapper.Map<IEnumerable<DrugsDto>>(entities).ToList();
                if (entities != null)
                {

                    if (result.Count > 0)
                    {
                        var timeValues = result.Select(x => x.Time).Distinct().ToList();

                        foreach (var item in timeValues)
                        {
                            DrugDto _objDrug = new DrugDto();
                            _objDrug.Drugs = new List<DrugsDto>();


                            var objDrugList = result.Where(x => x.Time == item).ToList();
                            _objDrug.Time = item;
                            _objDrug.Drugs = objDrugList;
                            _objDrugs.Add(_objDrug);
                        }
                    }

                }

                return Ok(_objDrugs);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

    }
}

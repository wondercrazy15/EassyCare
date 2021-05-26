using System;
using System.Threading.Tasks;
using AutoMapper;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Dto;
using EasyCare.Core.Infrastructure.Repository;
using EasyCare.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyCare.Web.Controllers
{
    [Route("api/[controller]")] // -> .../api/Senior with Senior = Filename\Controller. Learn more: https://docs.microsoft.com/de-de/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.1#ar
    [ApiController]
    public class SeniorController : CrudController<Senior, SeniorDto>
    {
        public SeniorController(ISeniorRepository repository, ILogger<SeniorController> logger, IMapper mapper) 
            : base(repository, logger, mapper)
        {
        }
    }

}

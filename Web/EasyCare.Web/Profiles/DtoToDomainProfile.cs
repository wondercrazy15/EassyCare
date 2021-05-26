using AutoMapper;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Dto;

namespace EasyCare.Web.Profiles
{
    public class DtoToDomainProfile : Profile
    {
        public DtoToDomainProfile()
        {
            // CRUD's
            CreateMap<DeviceDto, Device>();
            CreateMap<SeniorDto, Senior>();
            CreateMap<SensorMessageDto, SensorMessage>();
            CreateMap<SignedUpUsersDto, SignedUpUsers>();
            CreateMap<SupervisorDto, Supervisor>();
            CreateMap<TANDto, TAN>();
            CreateMap<CalendarEventDto, CalendarEvent>();
            CreateMap<MessageDto, Message>();
            CreateMap<NotificationMessageDto, NotificationMessage>();
            CreateMap<DrugsDto, Drugs>();
            CreateMap<UserDto, Supervisor>();
            CreateMap<GroupDto, Group>();
            CreateMap<CalendarEventDto, CustomCalendarEventSchedulers>();
        }
    }
}
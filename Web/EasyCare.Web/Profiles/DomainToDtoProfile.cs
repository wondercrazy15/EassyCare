using AutoMapper;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Dto;

namespace EasyCare.Web.Profiles
{
    public class DomainToDtoProfile : Profile
    {
        public DomainToDtoProfile()
        {
            // CRUD's
            CreateMap<Device, DeviceDto>();
            CreateMap<Senior, SeniorDto>();
            CreateMap<SensorMessage, SensorMessageDto>();
            CreateMap<SignedUpUsers, SignedUpUsersDto>();
            CreateMap<Supervisor, SupervisorDto>();
            CreateMap<TAN, TANDto>();
            CreateMap<CalendarEvent, CalendarEventDto>();
            CreateMap<Message, MessageDto>();
            CreateMap<NotificationMessage, NotificationMessageDto>();
            CreateMap<Drugs, DrugsDto>();
            CreateMap<CustomDrugs, DrugsDto>();
            CreateMap<CustomSupervisor, CustomSupervisorDto>();
            CreateMap<Group, GroupDto>();
            CreateMap<Supervisor, SeniorDto>();
            CreateMap<CustomCalendarEventDate, CalendarEventDateDto>();
        }
    }
}
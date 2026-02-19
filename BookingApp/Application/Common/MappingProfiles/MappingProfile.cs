using Application.Commands.Booking.CreateBooking;
using Application.Commands.Event.UpdateEvent;
using Application.DTO.Admin;
using Application.DTO.Booking;
using Application.DTO.Event;
using Application.Parameters.Booking;
using Application.Parameters.Events;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.Common.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Event Mappings
            CreateMap<Event, EventDTO>();
            CreateMap<Event, EventSummaryDTO>();
            CreateMap<Event, AdminEventDTO>()
                .ForMember(dest => dest.OwnerID, opt => opt.MapFrom(src => src.CreatedByUserId));

            CreateMap<UpdateEventParameters, Event>()
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                // Standard .ForAllMembers() and .ForMember() rules were being ignored for nullable value types
                // causing them to be overwritten with default values during PATCH updates.
                // PreCondition worked cz it's a more forceful rule that correctly prevents the mapping.
                .ForMember(dest => dest.MaxAttendees, opt =>
                { 
                    opt.PreCondition(src => src.MaxAttendees.HasValue);
                    opt.MapFrom(x => x.MaxAttendees.Value);
                })
                .ForMember(dest => dest.EventDate, opt =>
                {
                    opt.PreCondition(src => src.EventDate.HasValue);
                    opt.MapFrom(x => x.EventDate!.Value);
                })
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Event, PublicEventSummaryDTO>();

            // Booking Mappings
            CreateMap<CreateBookingParameters, Booking>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BookingStatus.Confirmed));
            CreateMap<Booking, BookingDTO>()
                .ForMember(dest => dest.EventSummary, opt => opt.MapFrom(src => src.Event));
            CreateMap<Booking, AdminBookingDTO>();

            // Value Object Mappings
            CreateMap<Address, AddressDTO>();
            CreateMap<AddressDTO, Address>();
            CreateMap<UpdateAddressDTO, Address>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
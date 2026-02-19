using Application.Commands.Event.CreateEvent;
using Application.Commands.Event.DeleteEvent;
using Application.Commands.Event.UpdateEvent;
using Application.Common;
using Application.DTO.Event;
using Application.Interfaces;
using Application.Parameters.Events;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository,IUnitOfWork unitOfWork,IMapper mapper)
        {
            _eventRepository = eventRepository; 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<EventDTO>>? GetEventByNameAsync(string title,string requestorId)
        {
            var isEventRetrieved = await _eventRepository.GetByTitleAsync(title);
            return MapToResultEventDTO(isEventRetrieved,requestorId);
        }

        public async Task<Result<EventDTO>> GetEventByIdAsync(int eventId)
        {
            var foundEvent = await _eventRepository.GetByIdAsync(eventId);

            if (foundEvent == null)
            {
                return Result<EventDTO>.IsFailure("Event not found");
            }
            return Result<EventDTO>.IsSuccess(_mapper.Map<EventDTO>(foundEvent));
        }

        public async Task<bool> IsTitleUniqueAsync(string title)
        {
            var result = await _eventRepository.GetByTitleAsync(title);
            return result == null;
        }

        async Task<Result<EventDTO>> IEventService.CreateEventAsync(CreateEventParameters parameters)
        {
            if (parameters == null) 
                return Result<EventDTO>.IsFailure("Request cannot be null.");

            var eventToAdd = new Event()
            {
                Title = parameters.Title,
                Description = parameters.Description,
                EventDate = parameters.EventDate,
                MaxAttendees = parameters.MaxAttendees,
                CreatedByUserId = parameters.OwnerId,
                ImageUrl = parameters.ImageUrl,
                Price = parameters.Price,
                Address = new Address(
                        parameters.Address.Country,
                        parameters.Address.CountryCode,
                        parameters.Address.City,
                        parameters.Address.Venue
                )

            };

            await _eventRepository.AddAsync(eventToAdd);
            var isSuccessfulyAdded = await _unitOfWork.CompleteAsync();

            if(isSuccessfulyAdded == 0)
            {
                return Result<EventDTO>.IsFailure("Add event add was cancelled.");
            }

            var eventData = new EventDTO()
            {
                Id = eventToAdd.Id,
                Title = eventToAdd.Title,
                Description = eventToAdd.Description,
                EventDate = eventToAdd.EventDate,
                MaxAttendees = eventToAdd.MaxAttendees,
                ImageUrl= eventToAdd.ImageUrl,
                Price = eventToAdd.Price,
                Address = new AddressDTO
                {
                    Country = eventToAdd.Address.Country,
                    CountryCode = eventToAdd.Address.CountryCode,
                    City = eventToAdd.Address.City,
                    Venue = eventToAdd.Address.Venue
                }
            };

            return Result<EventDTO>.IsSuccess(eventData);
        }

        public async Task<Result<EventDTO>> UpdateEventAsync(UpdateEventParameters parameters)
        {
            var foundEvent = await _eventRepository.GetByIdWithBookingsAsync(parameters.Id);
            if (foundEvent == null) 
                return Result<EventDTO>.IsFailure("Event not found");
            bool isAllowedToChange = foundEvent.CreatedByUserId == parameters.RequestorID;
            if (!isAllowedToChange)
                return Result<EventDTO>.IsFailure("No rights to change.");
            if (parameters.MaxAttendees.HasValue)
            {   // null treats like zero
                if(parameters.MaxAttendees < foundEvent.Bookings.Sum(x => x.NumberOfSeats))
                    return Result<EventDTO>.IsFailure("Can't change max attendess less than already registered");
            }
            if (parameters.EventDate != null)
            {   // cant move to the past
                if (parameters.EventDate.Value < DateTime.UtcNow)
                {
                    return Result<EventDTO>.IsFailure("The event date cannot be set to a date in the past.");
                }
            }

            if (foundEvent.IsCancelled)
                return Result<EventDTO>.IsFailure("Event is cancelled. There is no need to update it anymore");

            // note: _mapper.Map(source, destination) - Update destination variable
            //       _mapper.Map<TDestination>(source) - Create new object out of the source>
            _mapper.Map(parameters, foundEvent);

            var saveResult = await _unitOfWork.CompleteAsync();

            // if there was dupliacte it will also work;

            return Result<EventDTO>.IsSuccess(_mapper.Map<EventDTO>(foundEvent));
        }

        public async Task<Result<bool>> DeleteEventAsync(int eventID, string requestorID)
        {
            var eventToDelete = await _eventRepository.GetByIdWithBookingsAsync(eventID);

            if (eventToDelete == null)
                return Result<bool>.IsFailure("Event not found");

            if (eventToDelete.CreatedByUserId != requestorID)
                return Result<bool>.IsFailure("No rights to delete");

            if (eventToDelete.Bookings.Any())
                return Result<bool>.IsFailure("Cannot delete an event that has active bookings.");

            _eventRepository.Delete(eventToDelete);
            await _unitOfWork.CompleteAsync();

            return Result<bool>.IsSuccess(true);
        }

        private Result<EventDTO> MapToResultEventDTO(Event eventEntity,string requestorId)
        {
            if (eventEntity.CreatedByUserId != requestorId) 
                return Result<EventDTO>.IsFailure("No rights to edit");

            return eventEntity == null
                ? Result<EventDTO>.IsFailure("Event not found")
                : Result<EventDTO>.IsSuccess(_mapper.Map<EventDTO>(eventEntity));
        }

        public async Task<Result<List<PublicEventSummaryDTO>>> GetAllPublicEventsAsync(int page, int pageSize, string? sortBy, string? sortOrder)
        {
            var toSkip = (page - 1) * pageSize;

            var eventsQuery = (await _eventRepository.GetQueryableAsync())
                .Where(e => !e.IsCancelled)
                .Where(e => e.Bookings.Where(b => !b.IsCancelled).Sum(b => b.NumberOfSeats) < e.MaxAttendees);
            // not cancelled and sum number of booked seats < must not exceed max attendees (query)

            var isDescending = sortOrder?.ToLower() == "desc";

            switch (sortBy?.ToLower())
            {
                case "date":
                    eventsQuery = isDescending
                        ? eventsQuery.OrderByDescending(e => e.EventDate)
                        : eventsQuery.OrderBy(e => e.EventDate);
                    break;
                case "price":
                    eventsQuery = isDescending
                        ? eventsQuery.OrderByDescending(e => e.Price)
                        : eventsQuery.OrderBy(e => e.Price);
                    break;
                default:
                    eventsQuery = eventsQuery.OrderBy(e => e.EventDate);
                    break;
            }

            var paginatedEvents = await eventsQuery
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            var publicEventsDto = _mapper.Map<List<PublicEventSummaryDTO>>(paginatedEvents);

            return Result<List<PublicEventSummaryDTO>>.IsSuccess(publicEventsDto);
        }

        public async Task<Result<IEnumerable<PublicEventSummaryDTO>>> GetMyEventsAsync(string ownerId)
        {
            var eventsQuery = await _eventRepository.GetQueryableAsync();

            var userEvents = await eventsQuery
                .Where(e => e.CreatedByUserId == ownerId)
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();

            if (userEvents == null || !userEvents.Any())
            {
                return Result<IEnumerable<PublicEventSummaryDTO>>.IsSuccess(new List<PublicEventSummaryDTO>());
            }

            var eventDtos = _mapper.Map<IEnumerable<PublicEventSummaryDTO>>(userEvents);

            return Result<IEnumerable<PublicEventSummaryDTO>>.IsSuccess(eventDtos);
        }
    }
}
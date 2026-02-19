using Application.Common;
using Application.DTO.Booking;
using Application.DTO.Event;
using Application.Interfaces;
using Application.Parameters.Booking;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository,IEventRepository eventRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<BookingDTO>> CreateBookingAsync(CreateBookingParameters command)
        {
            var eventEntity = await _eventRepository.GetByIdWithBookingsAsync(command.EventId);
            if (eventEntity == null) return Result<BookingDTO>.IsFailure("Event with the specified ID was not found.");
            if (eventEntity.IsCancelled) return Result<BookingDTO>.IsFailure("Event was cancelled.");
            if (eventEntity.Bookings.Any(b => b.ClientId == command.ClientId)) return Result<BookingDTO>.IsFailure("You already have a booking for this event.");
            

            var totalSeatsBooked = eventEntity.Bookings.Sum(b => b.NumberOfSeats);
            if (totalSeatsBooked + command.NumberOfSeats > eventEntity.MaxAttendees)
            {
                var availableSeats = eventEntity.MaxAttendees - totalSeatsBooked;
                return Result<BookingDTO>.IsFailure($"Not enough seats available. Only {availableSeats} left.");
            }

            var bookingToCreate = _mapper.Map<Booking>(command);

            eventEntity.Bookings.Add(bookingToCreate);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
            {
                return Result<BookingDTO>.IsFailure("Database.Error");
            }

            return Result<BookingDTO>.IsSuccess(_mapper.Map<BookingDTO>(bookingToCreate));
        }

        public async Task<Result<BookingDTO>> UpdateBookingAsync(UpdateBookingParameters command)
        {
            var foundBooking = await _bookingRepository.GetByIdWithEventAsync(command.BookingId);
            if (foundBooking == null) 
                return Result<BookingDTO>.IsFailure("Booking not found");
            if (foundBooking.Event.IsCancelled) 
                return Result<BookingDTO>.IsFailure("Event was cancelled. There is no need to update it anymore.");
            if (foundBooking.ClientId != command.ClientId)
                return Result<BookingDTO>.IsFailure("No rights to edit");

            var amountOfBookedSeast = foundBooking.Event.Bookings.Sum(x => x.NumberOfSeats);
            var seatsDifference = command.NumberOfSeats - foundBooking.NumberOfSeats;
            if (seatsDifference > 0)
            {                            //       all seats             -  (all booked seats + aditional seats to book)
                var availableSeats = foundBooking.Event.MaxAttendees - ( amountOfBookedSeast + seatsDifference );

                if (availableSeats < 0)
                {
                    return Result<BookingDTO>.IsFailure($"Cannot add {seatsDifference} seats. Only {availableSeats} available.");
                }

                foundBooking.NumberOfSeats = command.NumberOfSeats;
                if(command.Notes != null) foundBooking.Notes = command.Notes;
                var saveResult = await _unitOfWork.CompleteAsync();
                if (saveResult == 0)
                {
                    // Якщо з якоїсь причини нічого не збереглося
                    return Result<BookingDTO>.IsFailure("Server Error: Could not save changes.");
                }
            }
            var resultDto = _mapper.Map<BookingDTO>(foundBooking);
            return Result<BookingDTO>.IsSuccess(resultDto);

        }

        public async Task<Result<BookingDTO>> GetBookingByIdAsync(int bookingId,string requestorId)
        {
            var result = await _bookingRepository.GetByIdWithEventAsync(bookingId);
            if (result == null) return Result<BookingDTO>.IsFailure("Not found");
            if (result.ClientId != requestorId) return Result<BookingDTO>.IsFailure("Forbiden. No rights");

            return Result<BookingDTO>.IsSuccess(_mapper.Map<BookingDTO>(result));
        }

        public async Task<Result<bool>> DeleteBookingByIdAsync(int bookingId,string requestorId)
        {
            var foundBooking = await _bookingRepository.GetByIdWithEventAsync(bookingId);
            if (foundBooking == null) 
                return Result<bool>.IsFailure("Not found");
            if (foundBooking.ClientId != requestorId) 
                return Result<bool>.IsFailure("Forbided. No rights");
            if (foundBooking.Event.IsCancelled) 
                return Result<bool>.IsFailure("Event was cancelled. There is no need to update it anymore.");


            _bookingRepository.Delete(foundBooking);
            var isDeletedSuccesfully = await _unitOfWork.CompleteAsync();
            if(isDeletedSuccesfully < 0)
            {
                return Result<bool>.IsFailure("Something went wrong. Please try again later");
            }
            return Result<bool>.IsSuccess(true);
        }

        public async Task<Result<List<BookingDTO>>> GetBookingsByUserIdAsync(string userId)
        {
            var query = await _bookingRepository.GetQueryableAsync();

            var bookings = await query
                .Where(b => b.ClientId == userId)
                .Include(b => b.Event)
                .ToListAsync();

            var bookingDtos = bookings.Select(b => new BookingDTO
            {
                Id = b.Id,
                CreatedAt = b.CreatedAt,
                NumberOfSeats = b.NumberOfSeats,
                Status = b.Status.ToString(),
                Notes = b.Notes,
                EventSummary = new EventSummaryDTO
                {
                    Id = b.Event.Id,
                    Title = b.Event.Title,
                    EventDate = b.Event.EventDate,
                    ImageUrl = b.Event.ImageUrl,
                    Price = b.Event.Price
                }
            }).ToList();

            return Result<List<BookingDTO>>.IsSuccess(bookingDtos);
        }
    }
}
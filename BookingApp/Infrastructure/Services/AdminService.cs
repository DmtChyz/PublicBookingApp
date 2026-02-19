using Application.Commands.Admin.Booking;
using Application.Commands.Admin.Booking.AllBookings;
using Application.Commands.Admin.Booking.CancelBookingById;
using Application.Commands.Admin.Booking.GetBookingById;
using Application.Commands.Admin.Event.CancelEvent;
using Application.Commands.Admin.Event.GetAllEvents;
using Application.Commands.Admin.User.GetAllUsers;
using Application.Common;
using Application.DTO.Admin;
using Application.DTO.Booking;
using Application.DTO.Event;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IBookingRepository _bookingRepository;
        private readonly IEventRepository _eventRepository;

        public AdminService(
            IUnitOfWork unitOfWork, IMapper mapper, UserManager<IdentityUser> userManager,
            IBookingRepository bookingRepository,IEventRepository eventRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _bookingRepository = bookingRepository;
            _eventRepository = eventRepository;
        }

        public async Task<Result<List<AdminBookingDTO>>> GetAllBookingsAsync(int page,int pageSize)
        {
            var toSkip = (page - 1) * pageSize;
            var allBookings = await _bookingRepository.GetQueryableWithEventAsync();
                

            var paginatedList = await allBookings
                .OrderBy(x => x.NumberOfSeats)
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();
            return Result<List<AdminBookingDTO>>.IsSuccess(_mapper.Map<List<AdminBookingDTO>>(paginatedList));
        }

        public async Task<Result<AdminBookingDTO>> GetBookingByIdAsync(int bookingId)
        {
            var isFound = await _bookingRepository.GetByIdWithEventAsync(bookingId);
            if(isFound == null)
            {
                return Result<AdminBookingDTO>.IsFailure("Not found");
            }
            return Result<AdminBookingDTO>.IsSuccess(_mapper.Map<AdminBookingDTO>(isFound));
        }

        public async Task<Result<bool>> CancelBookingAsync(int bookingId)
        {
            var foundBooking = await _bookingRepository.GetByIdAsync(bookingId);
            if (foundBooking == null)
            {
                return Result<bool>.IsFailure("Not found");
            }
            if (foundBooking.IsCancelled)
            {
                return Result<bool>.IsFailure("Booking is already cancelled.");
            }
            foundBooking.IsCancelled = true;
            var result = await _unitOfWork.CompleteAsync();
            if(result <= 0)
            {
                return Result<bool>.IsFailure("Something went wrong. Please try again later.");
            }
            /*
             *  marked to add next logic to inform users about cancellation of the event/booking
             */
            return Result<bool>.IsSuccess(true);
        }

        public async Task<Result<List<AdminEventDTO>>> GetAllEventsAsync(int page,int pageSize)
        {
            var toSkip = (page - 1) * pageSize;
            var allEvents = await _eventRepository.GetQueryableAsync();


            var paginatedList = await allEvents
                .OrderBy(x => x.EventDate)
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();
            return Result<List<AdminEventDTO>>.IsSuccess(_mapper.Map<List<AdminEventDTO>>(paginatedList));
        }

        public async Task<Result<AdminEventDTO>> GetEventByIdAsync(int eventId)
        {
            var isFound = await _eventRepository.GetByIdAsync(eventId);
            if(isFound == null)
            {
                return Result<AdminEventDTO>.IsFailure("Not found");
            }
            return Result<AdminEventDTO>.IsSuccess(_mapper.Map<AdminEventDTO>(isFound));
        }

        public async Task<Result<bool>> CancelEventAsync(int eventId)
        {
            var foundEvent = await _eventRepository.GetByIdAsync(eventId);
            if (foundEvent == null)
            {
                return Result<bool>.IsFailure("Not found");
            }
            if (foundEvent.IsCancelled)
            {
                return Result<bool>.IsFailure("Event is already cancelled.");
            }
            foundEvent.IsCancelled = true;
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
            {
                return Result<bool>.IsFailure("Something went wrong. Please try again later.");
            }
            /*
             *  marked to add next logic to inform users about cancellation of the event/booking
             */
            return Result<bool>.IsSuccess(true);
        }

        public async Task<Result<List<UserDTO>>> GetAllUsersAsync(int page,int pageSize)
        {

            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (users == null)
                return Result<List<UserDTO>>.IsFailure("Server error (GetAllUsersAsync) could be empty list.");

            var userDtos = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email
            }).ToList();

            return Result<List<UserDTO>>.IsSuccess(userDtos);
        }

        public async Task<Result<UserDTO>> GetUserByIdAsync(string userId)
        {
            var foundUser = await _userManager.FindByIdAsync(userId);
            if (foundUser == null)
                return Result<UserDTO>.IsFailure("No user found");
            var userDTO = new UserDTO
            {
                Id = foundUser.Id,
                Username = foundUser.UserName,
                Email = foundUser.Email
            };
            return Result<UserDTO>.IsSuccess(userDTO);
        }
    }
}

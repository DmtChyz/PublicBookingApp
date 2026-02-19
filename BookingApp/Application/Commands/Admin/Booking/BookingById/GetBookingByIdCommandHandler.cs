using Application.Commands.Admin.Booking.GetBookingById;
using Application.Common;
using Application.DTO.Admin;
using Application.DTO.Booking;
using Application.Interfaces;
using MediatR;

namespace Application.Commands.Admin.Booking
{
    public class GetBookingByIdCommandHandler : IRequestHandler<GetBookingByIdCommand,Result<AdminBookingDTO>>
    {
        private readonly IAdminService _adminService;

        public GetBookingByIdCommandHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<Result<AdminBookingDTO>> Handle(GetBookingByIdCommand command,CancellationToken token)
        {
            var result = await _adminService.GetBookingByIdAsync(command.BookingId);
            if (!result.Success)
            {
                return Result<AdminBookingDTO>.IsFailure(result.Errors);
            }
            return result;
        }
    }
}

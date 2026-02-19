using Application.Common;
using Application.DTO.Admin;
using Application.DTO.Booking;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.Booking.AllBookings
{
    public class GetAllBookingCommandHandler : IRequestHandler<GetAllBookingsCommand,Result<List<AdminBookingDTO>>>
    {
        private readonly IAdminService _adminService;

        public GetAllBookingCommandHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<Result<List<AdminBookingDTO>>> Handle(GetAllBookingsCommand command,CancellationToken token)
        {
            var result = await _adminService.GetAllBookingsAsync(command.Page,command.PageSize);
            if (!result.Success)
            {
                return Result<List<AdminBookingDTO>>.IsFailure(result.Errors);
            }
            return result;
        }
    }
}

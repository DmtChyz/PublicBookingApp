using Application.Common;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.Booking.CancelBookingById
{
    public class CancelBooknigByIdCommandHandler : IRequestHandler<CancelBookingByIdCommand,Result<bool>>
    {
        private readonly IAdminService _adminService;

        public CancelBooknigByIdCommandHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<Result<bool>> Handle(CancelBookingByIdCommand command,CancellationToken token)
        {
            var result = await _adminService.CancelBookingAsync(command.BookingId);
            if (!result.Success)
            {
                return Result<bool>.IsFailure(result.Errors);
            }
            return result;
        }
    }
}

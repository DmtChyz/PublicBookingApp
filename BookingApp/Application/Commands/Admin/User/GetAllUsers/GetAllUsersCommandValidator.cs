using Application.Common;
using Application.DTO.Admin;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.User.GetAllUsers
{
    public class GetAllUsersCommandValidator : AbstractValidator<GetAllUsersCommand>
    {
        public GetAllUsersCommandValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page must be at least 1");

            RuleFor(x => x.PageSize)
                    .GreaterThan(0).WithMessage("Page size must be greater than 0.");
        }
    }
}

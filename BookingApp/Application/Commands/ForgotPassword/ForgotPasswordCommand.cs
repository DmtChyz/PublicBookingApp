    using Application.Common;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Application.Commands.ForgotPassword
    {
        public class ForgotPasswordCommand : IRequest<Result<bool>>
        {
            public string Email { get; set; }
        }
    }

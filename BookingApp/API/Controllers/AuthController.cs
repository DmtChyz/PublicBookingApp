using Application.Commands.ForgotPassword;
using Application.Commands.RegisterUser;
using Application.Commands.ResetPassword;
using Application.Commands.UserLogin;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        public AuthController(ISender sender,IMapper mapper,IEmailSender emailSender)
        {
            
            _mapper = mapper;
            _sender = sender;
            _emailSender = emailSender;
        }

        [HttpPost("register")] // login , password     with strict rules.     ( Has same fields ) 
        public async Task<IActionResult> Register([FromBody] UserRegisterCommand request) 
        {
            if (request == null) return BadRequest("Something went wrong. Please try again later.");

            var isTokenRetrieved = await _sender.Send(request);
            if (!isTokenRetrieved.Success)
            {
                return BadRequest(isTokenRetrieved.Errors);
            }

            return Ok(isTokenRetrieved.Value);
        }
        [HttpPost("login")] // login , password      without strict rules.
        public async Task<IActionResult> Login([FromBody] UserLoginCommand userToLoginRequest)
        {
            if (userToLoginRequest == null) return BadRequest("Fields are empty.");

            var isTokenRetrieved = await _sender.Send(userToLoginRequest);
            if (!isTokenRetrieved.Success)
            {
                return BadRequest(isTokenRetrieved.Errors);
            }

            return Ok(isTokenRetrieved.Value);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            var command = new ForgotPasswordCommand { Email = forgotPasswordDTO.Email };
            var result = await _sender.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            return Ok();
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var command = new ResetPasswordCommand
            {
                Email = resetPasswordDTO.Email,
                NewPassword = resetPasswordDTO.NewPassword,
                Token = resetPasswordDTO.Token
            };
            var result = await _sender.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }
}

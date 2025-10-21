using HisuianArchives.Api.Extensions;
using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Application.Features.Users.Commands.UpdateProfile;
using HisuianArchives.Application.Features.Users.Commands.UpdateProfileImage;
using HisuianArchives.Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HisuianArchives.Api.Controllers
{

    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(UserSummaryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.GetCurrentUserId();

            // Create the query
            var query = new GetUserByIdQuery { UserId = userId };

            // Send the query through MediatR
            var responseDto = await _mediator.Send(query);

            return Ok(responseDto);
        }

        [HttpPut("profile")] 
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateProfileRequestDto dto)
        {
            var userId = User.GetCurrentUserId();

            // Map the DTO to the command and set the user ID
            var command = _mapper.Map<UpdateProfileCommand>(dto);
            command.UserId = userId;

            // Send the command through MediatR
            var responseDto = await _mediator.Send(command);

            return Ok(responseDto);
        }

        [HttpPut("me/profile-image")]
        public async Task<IActionResult> UpdateProfileImage([FromBody] UpdateProfileImageRequestDto dto)
        {
            var userId = User.GetCurrentUserId();

            // Map the DTO to the command and set the user ID
            var command = _mapper.Map<UpdateProfileImageCommand>(dto);
            command.UserId = userId;

            // Send the command through MediatR
            var responseDto = await _mediator.Send(command);

            return Ok(responseDto);
        }
    }
}

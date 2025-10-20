using HisuianArchives.Api.Extensions;
using HisuianArchives.Application.DTOs.User;
using HisuianArchives.Application.Interfaces;
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
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPut("profile")] 
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateProfileRequestDto dto)
        {
            var userId = User.GetCurrentUserId();

            var updatedUser = await _userService.UpdateProfileAsync(userId, dto.Name, dto.Bio);

            var responseDto = _mapper.Map<UserSummaryResponseDto>(updatedUser);

            return Ok(responseDto);
        }
    }
}

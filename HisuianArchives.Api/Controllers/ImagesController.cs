using HisuianArchives.Api.Extensions;
using HisuianArchives.Application.DTOs.Image;
using HisuianArchives.Application.Features.Images.Commands.UploadImage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HisuianArchives.Api.Controllers;

[ApiController]
[Route("api/images")]
[Authorize]
public class ImagesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ImagesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] ImageUploadRequestDto dto)
    {
        var userId = User.GetCurrentUserId()!;

        // Map the DTO to the command and set the user ID
        var command = _mapper.Map<UploadImageCommand>(dto);
        command.UserId = userId;

        // Send the command through MediatR
        var responseDto = await _mediator.Send(command);

        return Ok(responseDto);
    }
}
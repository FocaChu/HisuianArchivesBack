using MediatR;
using HisuianArchives.Application.DTOs.User;

namespace HisuianArchives.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// Query for retrieving a user by their ID.
/// </summary>
public class GetUserByIdQuery : IRequest<UserSummaryResponseDto>
{
    public Guid UserId { get; set; }
}

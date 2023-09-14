using Domain.DTOs;
using MediatR;

namespace Application.Features.User.GetUserPosts;

public record GetUserPostsRequest(Guid UserId) : IRequest<List<PostDto>>;

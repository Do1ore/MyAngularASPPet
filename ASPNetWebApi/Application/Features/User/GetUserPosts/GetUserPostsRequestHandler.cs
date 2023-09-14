using Domain.DTOs;
using Infrastructure.Abstraction.Repositories;
using MediatR;

namespace Application.Features.User.GetUserPosts;

public class GetUserPostsRequestHandler : IRequestHandler<GetUserPostsRequest, List<PostDto>>
{

    private readonly IPostRepository _postRepository;

    public GetUserPostsRequestHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<List<PostDto>> Handle(GetUserPostsRequest request, CancellationToken cancellationToken)
    {

        var result = await _postRepository.GetPostsById(request.UserId);
        List<PostDto> postDto = new List<PostDto>();
        foreach (var post in result)
        {
            postDto.Add(post);
        }

        return postDto;
    }
}
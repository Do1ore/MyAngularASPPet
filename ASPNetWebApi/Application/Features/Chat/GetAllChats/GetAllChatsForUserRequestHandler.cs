using Domain.MongoEntities.Chat;
using Infrastructure.Abstraction.Repositories;
using MediatR;

namespace Application.Features.Chat.GetAllChats;

public class GetAllChatsForUserRequestHandler : IRequestHandler<GetAllChatsForUserRequest, List<ChatM>>
{
    private readonly IUserRepository _userRepository;

    public GetAllChatsForUserRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<ChatM>> Handle(GetAllChatsForUserRequest request, CancellationToken cancellationToken)
    {
        var chats = await _userRepository.GetChatsForUser(request.UserId);
        return chats;
    }
}
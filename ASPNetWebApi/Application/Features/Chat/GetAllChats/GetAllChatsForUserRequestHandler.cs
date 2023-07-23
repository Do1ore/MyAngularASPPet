using Domain.MongoEntities.Chat;
using Infrastructure.Abstraction.Repositories;
using MediatR;

namespace Application.Features.Chat.GetAllChats;

public class GetAllChatsForUserRequestHandler : IRequestHandler<GetAllChatsForUserRequest, List<Chat_M>>
{
    private readonly IChatMessageRepository _messageRepository;

    public GetAllChatsForUserRequestHandler(IChatMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public Task<List<Chat_M>> Handle(GetAllChatsForUserRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
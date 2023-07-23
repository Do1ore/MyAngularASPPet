using Domain.MongoEntities.Chat;
using Infrastructure.Abstraction.Repositories;
using MediatR;

namespace Application.Features.Chat.GetAllChats;

public class GetAllChatsForUserRequestHandler : IRequestHandler<GetAllChatsForUserRequest, List<ChatM>>
{
    private readonly IChatMessageRepository _messageRepository;

    public GetAllChatsForUserRequestHandler(IChatMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public Task<List<ChatM>> Handle(GetAllChatsForUserRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
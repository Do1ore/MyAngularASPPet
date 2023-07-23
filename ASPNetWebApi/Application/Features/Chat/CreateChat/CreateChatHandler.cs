using Domain.MongoEntities.Chat;
using MediatR;

namespace Application.Features.Chat.CreateChat;

public class CreateChatHandler : IRequestHandler<CreateChatRequest, Chat_M>
{
    
    public Task<Chat_M> Handle(CreateChatRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
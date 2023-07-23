using Domain.MongoEntities.Chat;
using MediatR;

namespace Application.Features.Chat.CreateChat;

public class CreateChatHandler : IRequestHandler<CreateChatRequest, ChatM>
{
    
    public Task<ChatM> Handle(CreateChatRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
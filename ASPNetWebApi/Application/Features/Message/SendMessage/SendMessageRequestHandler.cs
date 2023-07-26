using Domain.MongoEntities.Chat;
using Infrastructure.Abstraction.Repositories;
using MediatR;

namespace Application.Features.Message.SendMessage;

public class SendMessageRequestHandler : IRequestHandler<SendMessageRequest, ChatMessageM>
{
    private readonly IChatMessageRepository _messageRepository;

    public SendMessageRequestHandler(IChatMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<ChatMessageM> Handle(SendMessageRequest request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.AddMessage(request.Message);
        return message;
    }
}
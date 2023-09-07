using Application.Mappers;
using Domain.DTOs;
using Infrastructure.Abstraction.Repositories;
using MediatR;

namespace Application.Features.Message.SendMessage;

public class SendMessageRequestHandler : IRequestHandler<SendMessageRequest, ChatMessageDto>
{
    private readonly IMongoChatMessageRepository _messageRepository;

    public SendMessageRequestHandler(IMongoChatMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<ChatMessageDto> Handle(SendMessageRequest request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.AddMessage(request.Message);
        var mapper = new MessageMapper();

        return mapper.MessageToMessageDto(message);
    }
}
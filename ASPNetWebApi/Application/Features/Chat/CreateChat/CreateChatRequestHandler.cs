using Application.Mappers;
using Domain.MongoEntities.Chat;
using Infrastructure.Abstraction.Repositories;
using MediatR;

namespace Application.Features.Chat.CreateChat;

public class CreateChatRequestHandler : IRequestHandler<CreateChatRequest, ChatM>
{
    private readonly IMongoChatRepository _chatRepository;

    public CreateChatRequestHandler(IMongoChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<ChatM> Handle(CreateChatRequest request, CancellationToken cancellationToken)
    {
        var chat = await _chatRepository.CreateChat(request.Chat);
        return chat;
    }
}
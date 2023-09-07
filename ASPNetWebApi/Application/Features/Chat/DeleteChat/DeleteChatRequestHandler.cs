using Infrastructure.Abstraction.Repositories;
using MediatR;

namespace Application.Features.Chat.DeleteChat;

public class DeleteChatRequestHandler : IRequestHandler<DeleteChatRequest, long>
{
    private readonly IMongoChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;

    public DeleteChatRequestHandler(IMongoChatRepository chatRepository,
        IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }

    public async Task<long> Handle(DeleteChatRequest request, CancellationToken cancellationToken)
    {
        if (!await _userRepository.IsUserExists(request.UserId))
        {
            throw new ArgumentException("User not exists");
        }

        if (!await _chatRepository.IsUserChatAdministrator(request.ChatId, request.UserId))
        {
            throw new ArgumentException("User is not chat administrator");
        }

        return await _chatRepository.DeleteChat(request.ChatId, request.UserId);
    }
}
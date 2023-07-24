using Application.Mappers;
using Domain.DTOs;
using Infrastructure.Abstraction.Repositories;
using MediatR;

namespace Application.Features.Chat.ChatDetails;

public class ChatDetailsRequestHandler : IRequestHandler<ChatDetailsRequest, ChatDetailsDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMongoChatRepository _chatRepository;


    public ChatDetailsRequestHandler(IUserRepository userRepository,
        IMongoChatRepository chatRepository)
    {
        _userRepository = userRepository;
        _chatRepository = chatRepository;
    }

    public async Task<ChatDetailsDto> Handle(ChatDetailsRequest request, CancellationToken cancellationToken)
    {
        var chat = await _chatRepository.GetChatDetails(request.UserId, request.ChatId);
        var ids = chat.AppUserIds.ToList();

        var chatUsers = _userRepository.GetUsersDetails(ids);

        List<AppUserDto> chatUsersDto = new List<AppUserDto>();
        var userMapper = new UserMapper();
        await foreach (var appUser in chatUsers.WithCancellation(cancellationToken))
        {
            chatUsersDto.Add(userMapper.AppUserToAppUserDto(appUser));
        }

        if (chatUsersDto.Count < 1)
        {
            throw new ArgumentException("Chat users cannot be below zero");
        }

        var chatMapper = new ChatMapper();
        var chatDto = chatMapper.ChatToChatDto(chat);

        chatDto.AppUsers = chatUsersDto;

        return chatDto;
    }
}
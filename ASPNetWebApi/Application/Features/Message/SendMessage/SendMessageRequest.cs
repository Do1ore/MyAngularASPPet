using Domain.DTOs;
using Domain.MongoEntities.Chat;
using MediatR;

namespace Application.Features.Message.SendMessage;

public record SendMessageRequest(ChatMessageM Message) : IRequest<ChatMessageDto>;

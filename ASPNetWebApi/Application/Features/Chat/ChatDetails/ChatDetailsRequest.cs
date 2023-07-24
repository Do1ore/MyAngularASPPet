using Domain.DTOs;
using MediatR;

namespace Application.Features.Chat.ChatDetails;

public record ChatDetailsRequest(Guid UserId, Guid ChatId) : IRequest<ChatDetailsDto>;

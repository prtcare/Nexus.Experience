using Nexus.Products.Chat.Domain.Session;

namespace Nexus.Products.Chat.Application.Session.Commands.UpdateSession;

public sealed record UpdateSessionCommand(
    SessionId SessionId,
    SessionStatus Status);
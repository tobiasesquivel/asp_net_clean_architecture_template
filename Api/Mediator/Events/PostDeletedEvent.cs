using api.Interfaces;
using api.Models;

namespace api.Mediator.Events;

public record PostDeletedEvent(Post Post) : IDomainEvent;
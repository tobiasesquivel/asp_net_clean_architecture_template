using api.Interfaces;
using api.Models;

namespace api.Mediator.Events;

public record PostCreatedEvent(Post Post) : IDomainEvent;
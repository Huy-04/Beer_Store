using Domain.Core.Base;
using Domain.Core.Interface.Event;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public Guid CreatedBy { get; protected set; }
    public Guid UpdatedBy { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; }
    public DateTimeOffset UpdatedAt { get; protected set; }

    protected AggregateRoot() : base()
    { }

    protected AggregateRoot(Guid id) : base(id)
    { }

    public void SetUpdatedBy(Guid updatedBy)
    {
        if (UpdatedBy == updatedBy) return;
        UpdatedBy = updatedBy;
        Touch();
    }

    public void Touch()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    protected void SetCreationAudit(Guid createdBy, Guid updatedBy)
    {
        CreatedBy = createdBy;
        UpdatedBy = updatedBy;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IReadOnlyCollection<IDomainEvent> DequeueDomainEvents()
    {
        var snapshot = _domainEvents.ToArray();
        _domainEvents.Clear();
        return snapshot;
    }
}
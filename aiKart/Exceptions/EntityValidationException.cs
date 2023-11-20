using aiKart.Interfaces;

namespace aiKart.Exceptions;
//  generic class with restrictions, custom exception(logging handled by ASP.NET middleware)
public class EntityValidationException<T> : Exception where T : class, IEntity
{
    public T Entity { get; }

    public EntityValidationException(T entity, string message) : base(message)
    {
        Entity = entity;
    }
}

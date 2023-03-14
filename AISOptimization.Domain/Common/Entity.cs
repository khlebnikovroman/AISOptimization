using System.ComponentModel.DataAnnotations;


namespace AISOptimization.Domain.Common;

//https://github.com/vkhorikov/DddAndEFCore/blob/master/src/App/Entity.cs
public class Entity
{
    protected Entity()
    {
    }

    protected Entity(long id)
        : this()
    {
        Id = id;
    }

    [Key]
    public long Id { get; set; }

    public override bool Equals(object obj)
    {
        if (!(obj is Entity other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetRealType() != other.GetRealType())
        {
            return false;
        }

        if (Id == 0 || other.Id == 0)
        {
            return false;
        }

        return Id == other.Id;
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetRealType().ToString() + Id).GetHashCode();
    }

    private Type GetRealType()
    {
        var type = GetType();

        if (type.ToString().Contains("Castle.Proxies."))
        {
            return type.BaseType;
        }

        return type;
    }
}




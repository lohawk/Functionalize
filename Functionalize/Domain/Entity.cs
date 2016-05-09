using System;

namespace Functionalize.Domain
{
    public abstract class Entity
    {
        public virtual long Id { get; protected set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(compareTo, null))
                return false;

            if (ReferenceEquals(this, compareTo))
                return true;

            if (GetRealType() != compareTo.GetRealType())
                return false;

            return !IsTransient() && !compareTo.IsTransient() && Id == compareTo.Id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b) => !(a == b);

        public override int GetHashCode() => (GetRealType().ToString() + Id).GetHashCode();

        public virtual bool IsTransient() => Id == 0;

        public virtual Type GetRealType()
            => GetType().BaseType != null && GetType().Namespace == "System.Data.Entity.DynamicProxies"
                ? GetType().BaseType
                : GetType();
    }
}
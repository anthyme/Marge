namespace Marge.Infrastructure
{
    public abstract class Value
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Value)obj);
        }

        public override int GetHashCode() => ValueSignature.GetHashCode();

        public override string ToString() => ValueSignature.ToString();

        protected bool Equals(Value other) => other.ValueSignature.Equals(ValueSignature);

        protected abstract object ValueSignature { get; }
    }
}

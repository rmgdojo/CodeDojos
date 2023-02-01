namespace Fluently.Core
{
    public static class Assert
    {
        public static bool That<T>(T value, IExpression<T> expression)
        {
            return expression.Compare(value);
        }
    }

    public static class Is
    {
        public static EqualTo<T> EqualTo<T>(T value)
        {
            return new EqualTo<T>(value);
        }

        public static Not Not { get; } = new Not();
    }

    public class Not
    {
        public NotEqualTo<T> EqualTo<T>(T value)
        {
            return new NotEqualTo<T>(value);
        }
    }

    public class EqualTo<T> : IExpression<T>
    {
        protected T _value;

        public virtual bool Compare(T value)
        {
            if (value is null && _value is null) return true;
            if (value is null) return false;
            if (_value is null) return false;

            return value.Equals(_value);
        }

        public EqualTo(T value)
        {
            _value = value;
        }
    }

    public class NotEqualTo<T> : EqualTo<T>, IExpression<T>
    {
        public override bool Compare(T value)
        {
            return !base.Compare(value);
        }

        public NotEqualTo(T value) : base(value)
        {
        }
    }

    public interface IExpression<T>
    {
        bool Compare(T value);
    }
}
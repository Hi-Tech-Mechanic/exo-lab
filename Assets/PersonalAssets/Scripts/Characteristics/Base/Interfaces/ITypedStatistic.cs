public interface ITypedStatistic<T> : IStatistic
{
    public T Value { get; set; }
}

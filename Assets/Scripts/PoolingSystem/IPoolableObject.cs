public interface IPoolableObject<T> where T : IPoolableObject<T>
{
    public Pool<T> _Pool { get; set; }

    public void SetPool(Pool<T> pool)
    {
        _Pool = pool;
    }
}

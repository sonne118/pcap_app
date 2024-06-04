using System.Collections.Concurrent;

namespace worker
{
    public class ObjectPool<V, T>
    {
        public readonly ConcurrentBag<Func<V, T>> _objects;
        private readonly Func<Func<V, T>> _objectGenerator;

        public ObjectPool(Func<Func<V, T>> objectGenerator)
        {
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
            _objects = new ConcurrentBag<Func<V, T>>();
        }

        public Func<V, T> Get() => _objects.TryTake(out Func<V, T> item) ? item : _objectGenerator();

        public void Return(Func<V, T> item) => _objects.Add(item);

    }
}

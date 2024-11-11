namespace wpfapp.Utilities
{
    public class Singleton<T> where T : class   //, new()
    {
        private static Lazy<T>? _instance;
        private static readonly object _lock = new object();
        private Singleton() { }
        public static T Instance(params object[] args)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {

                        _instance = new Lazy<T>(() => (T)Activator.CreateInstance(typeof(T), args));
                    }
                }
            }
            return _instance.Value;
        }
    }
}

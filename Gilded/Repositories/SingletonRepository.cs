namespace Gilded.Repositories
{
    public abstract class SingletonRepository<T>
    where T : SingletonRepository<T>, new()
    {
        private static T _instance;

        public static T Get()
        {
            if (_instance == null)
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
            }

            return _instance;
        }

        public static void Clear()
        {
            if (_instance != null)
            {
                _instance = null;
            }
        }
    }
}

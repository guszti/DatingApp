namespace DatingApp.API.Factory
{
    public abstract class BaseFactory
    {
        public T CreateNew<T>() where T : new()
        {
            return new T();
        }
    }
}
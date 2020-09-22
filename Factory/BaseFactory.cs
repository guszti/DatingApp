namespace DatingApp.API.Factory
{
    public abstract class BaseFactory : IBaseFactory
    {
        public T CreateNew<T>() where T : new()
        {
            return new T();
        }
    }
}
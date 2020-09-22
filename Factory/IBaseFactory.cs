namespace DatingApp.API.Factory
{
    public interface IBaseFactory
    {
        public T CreateNew<T>() where T : new();
    }
}
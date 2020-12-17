namespace Core
{
    public interface ICtxManager
    {
        void Initialize();
        void Shutdown();
    }

    public interface IUpdateableManager
    {
        void Update(float deltaTime);
    }
}

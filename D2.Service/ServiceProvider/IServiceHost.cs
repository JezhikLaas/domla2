namespace D2.Service.ServiceProvider
{
    public interface IServiceHost
    {
        bool ServiceReady { get; }
        void Run();
        T Resolve<T>();
    }
}
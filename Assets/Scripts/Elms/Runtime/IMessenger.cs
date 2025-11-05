namespace Elms.Runtime
{
    public interface IMessenger<T> where T : struct
    {
        T GetMessage();
    }
}
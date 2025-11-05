namespace Elms.Runtime
{
    public delegate void Dispatcher<T>(IMessenger<T> msg) where T : struct;
}
namespace Elms.Runtime
{
    public interface IUpdater<TModel, TMessage> where TModel : struct where TMessage : struct
    {
        (TModel, Cmd<TMessage>) Update(IMessenger<TMessage> msg, TModel model);
    }
}
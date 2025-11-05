using Elms.Runtime;
using UnityEngine;

namespace LudoGame.CoinSystem
{
    public class CoinUpdater : IUpdater<CoinModel, CoinMessage>
    {
        private readonly ICoinServerService _serverService;
        private readonly int _minCoins;
        private readonly int _maxCoins;

        public CoinUpdater(ICoinServerService serverService, int minCoins = 0, int maxCoins = 999999)
        {
            _serverService = serverService;
            _minCoins = minCoins;
            _maxCoins = maxCoins;
        }

        public (CoinModel, Cmd<CoinMessage>) Update(IMessenger<CoinMessage> msg, CoinModel model)
        {
            var message = msg.GetMessage();
            
            switch (message.type)
            {
                case CoinMessage.MessageType.Increment:
                    return UpdateCoins(model, model.currentCoins + message.value);
                
                case CoinMessage.MessageType.Decrement:
                    return UpdateCoins(model, model.currentCoins - message.value);
                
                case CoinMessage.MessageType.SetValue:
                    return UpdateCoins(model, message.value);
                
                case CoinMessage.MessageType.Show:
                    model.isVisible = true;
                    return (model, Cmd<CoinMessage>.None);
                
                case CoinMessage.MessageType.Hide:
                    model.isVisible = false;
                    return (model, Cmd<CoinMessage>.None);
                
                case CoinMessage.MessageType.FetchFromServer:
                    model.isLoading = true;
                    model.pendingOp = CoinModel.PendingOperation.FetchingFromServer;
                    return (model, FetchCoinsCmd());
                
                case CoinMessage.MessageType.FetchSuccess:
                    model.currentCoins = ClampCoins(message.value);
                    model.isLoading = false;
                    model.pendingOp = CoinModel.PendingOperation.None;
                    model.errorMessage = string.Empty;
                    return (model, Cmd<CoinMessage>.None);
                
                case CoinMessage.MessageType.FetchError:
                    model.isLoading = false;
                    model.pendingOp = CoinModel.PendingOperation.None;
                    model.errorMessage = message.error;
                    Debug.LogError($"Failed to fetch coins: {message.error}");
                    return (model, Cmd<CoinMessage>.None);
                
                case CoinMessage.MessageType.SaveToServer:
                    model.isLoading = true;
                    model.pendingOp = CoinModel.PendingOperation.SavingToServer;
                    return (model, SaveCoinsCmd(model.currentCoins));
                
                case CoinMessage.MessageType.SaveSuccess:
                    model.isLoading = false;
                    model.pendingOp = CoinModel.PendingOperation.None;
                    model.errorMessage = string.Empty;
                    Debug.Log("Coins saved successfully");
                    return (model, Cmd<CoinMessage>.None);
                
                case CoinMessage.MessageType.SaveError:
                    model.isLoading = false;
                    model.pendingOp = CoinModel.PendingOperation.None;
                    model.errorMessage = message.error;
                    Debug.LogError($"Failed to save coins: {message.error}");
                    return (model, Cmd<CoinMessage>.None);
                
                case CoinMessage.MessageType.GameWon:
                    var newCoins = model.currentCoins + message.value;
                    model.currentCoins = ClampCoins(newCoins);
                    model.isVisible = true;
                    return (model, SaveCoinsCmd(model.currentCoins));
                
                case CoinMessage.MessageType.GameLost:
                    var lostCoins = model.currentCoins - message.value;
                    model.currentCoins = ClampCoins(lostCoins);
                    model.isVisible = true;
                    return (model, SaveCoinsCmd(model.currentCoins));
                
                case CoinMessage.MessageType.Reset:
                    model.currentCoins = 0;
                    model.isLoading = false;
                    model.isVisible = false;
                    model.errorMessage = string.Empty;
                    model.pendingOp = CoinModel.PendingOperation.None;
                    return (model, Cmd<CoinMessage>.None);
                
                default:
                    return (model, Cmd<CoinMessage>.None);
            }
        }

        private (CoinModel, Cmd<CoinMessage>) UpdateCoins(CoinModel model, int newValue)
        {
            model.currentCoins = ClampCoins(newValue);
            return (model, Cmd<CoinMessage>.None);
        }

        private int ClampCoins(int value)
        {
            return Mathf.Clamp(value, _minCoins, _maxCoins);
        }

        private Cmd<CoinMessage> FetchCoinsCmd()
        {
            return new Cmd<CoinMessage>(dispatcher =>
            {
                _serverService.FetchCoins(
                    onSuccess: coins => dispatcher(new FetchSuccess(coins)),
                    onError: error => dispatcher(new FetchError(error))
                );
            });
        }

        private Cmd<CoinMessage> SaveCoinsCmd(int coins)
        {
            return new Cmd<CoinMessage>(dispatcher =>
            {
                _serverService.SaveCoins(
                    coins,
                    onSuccess: () => dispatcher(new SaveSuccess()),
                    onError: error => dispatcher(new SaveError(error))
                );
            });
        }
    }
}
using System;
using Elms.Runtime;

namespace LudoGame.CoinSystem
{
    [Serializable]
    public struct CoinModel
    {
        public int currentCoins;
        public bool isLoading;
        public bool isVisible;
        public string errorMessage;
        public PendingOperation pendingOp;

        public enum PendingOperation
        {
            None,
            FetchingFromServer,
            SavingToServer
        }
    }

    [Serializable]
    public struct CoinMessage
    {
        public MessageType type;
        public int value;
        public string error;

        public enum MessageType
        {
            // UI Actions
            Increment,
            Decrement,
            SetValue,
            Show,
            Hide,
            
            // Server Operations
            FetchFromServer,
            FetchSuccess,
            FetchError,
            
            SaveToServer,
            SaveSuccess,
            SaveError,
            
            // Game Events
            GameWon,
            GameLost,
            
            // Debug
            Reset
        }
    }

    // Message Creators
    public class IncrementCoin : IMessenger<CoinMessage>
    {
        private readonly int _amount;
        public IncrementCoin(int amount = 1) { _amount = amount; }
        
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.Increment,
            value = _amount
        };
    }

    public class DecrementCoin : IMessenger<CoinMessage>
    {
        private readonly int _amount;
        public DecrementCoin(int amount = 1) { _amount = amount; }
        
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.Decrement,
            value = _amount
        };
    }

    public class SetCoinValue : IMessenger<CoinMessage>
    {
        private readonly int _value;
        public SetCoinValue(int value) { _value = value; }
        
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.SetValue,
            value = _value
        };
    }

    public class ShowCoins : IMessenger<CoinMessage>
    {
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.Show
        };
    }

    public class HideCoins : IMessenger<CoinMessage>
    {
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.Hide
        };
    }

    public class FetchCoinsFromServer : IMessenger<CoinMessage>
    {
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.FetchFromServer
        };
    }

    public class FetchSuccess : IMessenger<CoinMessage>
    {
        private readonly int _coins;
        public FetchSuccess(int coins) { _coins = coins; }
        
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.FetchSuccess,
            value = _coins
        };
    }

    public class FetchError : IMessenger<CoinMessage>
    {
        private readonly string _error;
        public FetchError(string error) { _error = error; }
        
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.FetchError,
            error = _error
        };
    }

    public class SaveCoinsToServer : IMessenger<CoinMessage>
    {
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.SaveToServer
        };
    }

    public class SaveSuccess : IMessenger<CoinMessage>
    {
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.SaveSuccess
        };
    }

    public class SaveError : IMessenger<CoinMessage>
    {
        private readonly string _error;
        public SaveError(string error) { _error = error; }
        
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.SaveError,
            error = _error
        };
    }

    public class GameWon : IMessenger<CoinMessage>
    {
        private readonly int _reward;
        public GameWon(int reward) { _reward = reward; }
        
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.GameWon,
            value = _reward
        };
    }

    public class GameLost : IMessenger<CoinMessage>
    {
        private readonly int _penalty;
        public GameLost(int penalty = 0) { _penalty = penalty; }
        
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.GameLost,
            value = _penalty
        };
    }

    public class ResetCoins : IMessenger<CoinMessage>
    {
        public CoinMessage GetMessage() => new CoinMessage
        {
            type = CoinMessage.MessageType.Reset
        };
    }
}
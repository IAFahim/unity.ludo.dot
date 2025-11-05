using Elms.Runtime;
using UnityEngine;
using VirtueSky.Variables;

namespace LudoGame.CoinSystem
{
    public class CoinController : MonoBehaviour
    {
        [SerializeField] private IntegerVariable coinVariable;
        [SerializeField] private CoinRenderer coinRenderer;
        
        private Elm<CoinModel, CoinMessage> _elm;
        
        private void Start()
        {
            // Initialize function - optionally fetch data on startup
            (CoinModel, Cmd<CoinMessage>) Init()
            {
                var initialModel = new CoinModel 
                { 
                    currentCoins = coinVariable.Value,
                    isVisible = true,
                };
            
                // Optional: Fetch data immediately on startup
                // var fetchCmd = new Cmd<CounterMessage>(dispatcher => dispatcher(new FetchFromServerMessage()));
                // return (initialModel, fetchCmd);
            
                return (initialModel, Cmd<CoinMessage>.None);
            }

            // Create the Elm architecture instance
            var updater = new CoinUpdater(new MockCoinServerService(this), coinVariable.Min, coinVariable.Max);
        
            _elm = new Elm<CoinModel, CoinMessage>(Init, updater, coinRenderer);
        }
    }
}
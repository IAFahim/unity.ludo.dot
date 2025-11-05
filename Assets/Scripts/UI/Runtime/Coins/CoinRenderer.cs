using Elms.Runtime;
using UnityEngine;
using TMPro;

namespace LudoGame.CoinSystem
{
    public class CoinRenderer : MonoBehaviour, IRenderer<CoinModel, CoinMessage>
    {
        [Header("UI References")]
        [SerializeField] private GameObject coinPanel;
        [SerializeField] private TextMeshProUGUI coinText;

        private Dispatcher<CoinMessage> _dispatcher;

        public void Init(Dispatcher<CoinMessage> dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Render(CoinModel model)
        {
            coinText.text = model.currentCoins.ToString();
        }

        private void OnDestroy()
        {
            _dispatcher = null;
        }
    }
}
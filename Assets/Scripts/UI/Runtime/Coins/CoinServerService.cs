using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace LudoGame.CoinSystem
{
    public interface ICoinServerService
    {
        void FetchCoins(Action<int> onSuccess, Action<string> onError);
        void SaveCoins(int coins, Action onSuccess, Action<string> onError);
    }

    // Mock implementation for testing
    public class MockCoinServerService : ICoinServerService
    {
        private readonly MonoBehaviour _coroutineRunner;
        private int _serverCoins = 100; // Mock server data

        public MockCoinServerService(MonoBehaviour coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void FetchCoins(Action<int> onSuccess, Action<string> onError)
        {
            _coroutineRunner.StartCoroutine(FetchCoinsCoroutine(onSuccess, onError));
        }

        public void SaveCoins(int coins, Action onSuccess, Action<string> onError)
        {
            _coroutineRunner.StartCoroutine(SaveCoinsCoroutine(coins, onSuccess, onError));
        }

        private IEnumerator FetchCoinsCoroutine(Action<int> onSuccess, Action<string> onError)
        {
            yield return new WaitForSeconds(1f); // Simulate network delay
            
            // Simulate 90% success rate
            if (UnityEngine.Random.value > 0.1f)
            {
                onSuccess?.Invoke(_serverCoins);
            }
            else
            {
                onError?.Invoke("Network error: Unable to fetch coins");
            }
        }

        private IEnumerator SaveCoinsCoroutine(int coins, Action onSuccess, Action<string> onError)
        {
            yield return new WaitForSeconds(1f); // Simulate network delay
            
            // Simulate 90% success rate
            if (UnityEngine.Random.value > 0.1f)
            {
                _serverCoins = coins;
                onSuccess?.Invoke();
            }
            else
            {
                onError?.Invoke("Network error: Unable to save coins");
            }
        }
    }

    // Real implementation
    public class CoinServerService : ICoinServerService
    {
        private readonly MonoBehaviour _coroutineRunner;
        private readonly string _baseUrl;
        private readonly string _playerId;

        public CoinServerService(MonoBehaviour coroutineRunner, string baseUrl, string playerId)
        {
            _coroutineRunner = coroutineRunner;
            _baseUrl = baseUrl;
            _playerId = playerId;
        }

        public void FetchCoins(Action<int> onSuccess, Action<string> onError)
        {
            _coroutineRunner.StartCoroutine(FetchCoinsCoroutine(onSuccess, onError));
        }

        public void SaveCoins(int coins, Action onSuccess, Action<string> onError)
        {
            _coroutineRunner.StartCoroutine(SaveCoinsCoroutine(coins, onSuccess, onError));
        }

        private IEnumerator FetchCoinsCoroutine(Action<int> onSuccess, Action<string> onError)
        {
            var url = $"{_baseUrl}/player/{_playerId}/coins";
            using var request = UnityWebRequest.Get(url);
            
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var response = JsonUtility.FromJson<CoinResponse>(request.downloadHandler.text);
                    onSuccess?.Invoke(response.coins);
                }
                catch (Exception e)
                {
                    onError?.Invoke($"Failed to parse response: {e.Message}");
                }
            }
            else
            {
                onError?.Invoke($"Request failed: {request.error}");
            }
        }

        private IEnumerator SaveCoinsCoroutine(int coins, Action onSuccess, Action<string> onError)
        {
            var url = $"{_baseUrl}/player/{_playerId}/coins";
            var data = JsonUtility.ToJson(new CoinRequest { coins = coins });
            
            using var request = new UnityWebRequest(url, "POST");
            var bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke();
            }
            else
            {
                onError?.Invoke($"Request failed: {request.error}");
            }
        }

        [Serializable]
        private struct CoinRequest
        {
            public int coins;
        }

        [Serializable]
        private struct CoinResponse
        {
            public int coins;
        }
    }
}
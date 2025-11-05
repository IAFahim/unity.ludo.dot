using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Elm.Runtime;
using TMPro;
using System;
using System.Collections;

// 1. Define your Model (state)
public struct CounterModel
{
    public int Count;
    public bool IsProcessing;
    public bool IsLoadingFromServer;
    public string ErrorMessage;
    public ServerCounterData ServerData; // Store server response
}

// Server response structure
[Serializable]
public struct ServerCounterData
{
    public int count;
    public string message;
    public long timestamp;
}

// 2. Define your Messages (events)
public struct CounterMessage
{
    public MessageType Type;
    public int Value;
    public ServerCounterData ServerData;
    public string Error;

    public enum MessageType
    {
        Increment,
        Decrement,
        Reset,
        SetValue,
        DelayedIncrement,
        
        // Server-related messages
        FetchFromServer,
        FetchSuccess,
        FetchError,
        SaveToServer,
        SaveSuccess,
        SaveError
    }
}

// 3. Create Message wrappers
public class IncrementMessage : IMessenger<CounterMessage>
{
    public CounterMessage GetMessage() => new() { Type = CounterMessage.MessageType.Increment };
}

public class DecrementMessage : IMessenger<CounterMessage>
{
    public CounterMessage GetMessage() => new() { Type = CounterMessage.MessageType.Decrement };
}

public class ResetMessage : IMessenger<CounterMessage>
{
    public CounterMessage GetMessage() => new() { Type = CounterMessage.MessageType.Reset };
}

public class SetValueMessage : IMessenger<CounterMessage>
{
    private readonly int _value;
    public SetValueMessage(int value) => _value = value;
    public CounterMessage GetMessage() => new() { Type = CounterMessage.MessageType.SetValue, Value = _value };
}

public class DelayedIncrementMessage : IMessenger<CounterMessage>
{
    public CounterMessage GetMessage() => new() { Type = CounterMessage.MessageType.DelayedIncrement };
}

// Server-related messages
public class FetchFromServerMessage : IMessenger<CounterMessage>
{
    public CounterMessage GetMessage() => new() { Type = CounterMessage.MessageType.FetchFromServer };
}

public class FetchSuccessMessage : IMessenger<CounterMessage>
{
    private readonly ServerCounterData _data;
    public FetchSuccessMessage(ServerCounterData data) => _data = data;
    public CounterMessage GetMessage() => new() { Type = CounterMessage.MessageType.FetchSuccess, ServerData = _data };
}

public class FetchErrorMessage : IMessenger<CounterMessage>
{
    private readonly string _error;
    public FetchErrorMessage(string error) => _error = error;
    public CounterMessage GetMessage() => new() { Type = CounterMessage.MessageType.FetchError, Error = _error };
}

public class SaveToServerMessage : IMessenger<CounterMessage>
{
    public CounterMessage GetMessage() => new() { Type = CounterMessage.MessageType.SaveToServer };
}

public class SaveSuccessMessage : IMessenger<CounterMessage>
{
    public CounterMessage GetMessage() => new() { Type = CounterMessage.MessageType.SaveSuccess };
}

public class SaveErrorMessage : IMessenger<CounterMessage>
{
    private readonly string _error;
    public SaveErrorMessage(string error) => _error = error;
    public CounterMessage GetMessage() => new() { Type = CounterMessage.MessageType.SaveError, Error = _error };
}

// 4. Implement Updater (business logic)
public class CounterUpdater : IUpdater<CounterModel, CounterMessage>
{
    private readonly MonoBehaviour _context; // For coroutines
    private readonly string _serverUrl;

    public CounterUpdater(MonoBehaviour context, string serverUrl = "https://api.example.com")
    {
        _context = context;
        _serverUrl = serverUrl;
    }

    public (CounterModel, Cmd<CounterMessage>) Update(IMessenger<CounterMessage> msg, CounterModel model)
    {
        var message = msg.GetMessage();
        
        switch (message.Type)
        {
            case CounterMessage.MessageType.Increment:
                model.Count++;
                model.IsProcessing = false;
                return (model, Cmd<CounterMessage>.None);
            
            case CounterMessage.MessageType.Decrement:
                model.Count--;
                return (model, Cmd<CounterMessage>.None);
            
            case CounterMessage.MessageType.Reset:
                model.Count = 0;
                return (model, Cmd<CounterMessage>.None);
            
            case CounterMessage.MessageType.SetValue:
                model.Count = message.Value;
                return (model, Cmd<CounterMessage>.None);
            
            case CounterMessage.MessageType.DelayedIncrement:
                var delayCmd = new Cmd<CounterMessage>(dispatcher => AsyncIncrement(dispatcher));
                model.IsProcessing = true;
                return (model, delayCmd);
            
            // Server data fetching
            case CounterMessage.MessageType.FetchFromServer:
                var fetchCmd = new Cmd<CounterMessage>(dispatcher => FetchDataFromServer(dispatcher));
                model.IsLoadingFromServer = true;
                model.ErrorMessage = null;
                return (model, fetchCmd);
            
            case CounterMessage.MessageType.FetchSuccess:
                model.Count = message.ServerData.count;
                model.ServerData = message.ServerData;
                model.IsLoadingFromServer = false;
                model.ErrorMessage = null;
                return (model, Cmd<CounterMessage>.None);
            
            case CounterMessage.MessageType.FetchError:
                model.IsLoadingFromServer = false;
                model.ErrorMessage = message.Error;
                return (model, Cmd<CounterMessage>.None);
            
            // Server data saving
            case CounterMessage.MessageType.SaveToServer:
                var saveCmd = new Cmd<CounterMessage>(dispatcher => SaveDataToServer(model.Count, dispatcher));
                model.IsLoadingFromServer = true;
                model.ErrorMessage = null;
                return (model, saveCmd);
            
            case CounterMessage.MessageType.SaveSuccess:
                model.IsLoadingFromServer = false;
                model.ErrorMessage = null;
                return (model, Cmd<CounterMessage>.None);
            
            case CounterMessage.MessageType.SaveError:
                model.IsLoadingFromServer = false;
                model.ErrorMessage = message.Error;
                return (model, Cmd<CounterMessage>.None);
            
            default:
                return (model, Cmd<CounterMessage>.None);
        }
    }

    private async void AsyncIncrement(Dispatcher<CounterMessage> dispatcher)
    {
        await System.Threading.Tasks.Task.Delay(1000);
        dispatcher(new IncrementMessage());
    }

    private void FetchDataFromServer(Dispatcher<CounterMessage> dispatcher)
    {
        _context.StartCoroutine(FetchDataCoroutine(dispatcher));
    }

    private IEnumerator FetchDataCoroutine(Dispatcher<CounterMessage> dispatcher)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{_serverUrl}/counter"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var data = JsonUtility.FromJson<ServerCounterData>(request.downloadHandler.text);
                    dispatcher(new FetchSuccessMessage(data));
                }
                catch (Exception e)
                {
                    dispatcher(new FetchErrorMessage($"Parse error: {e.Message}"));
                }
            }
            else
            {
                dispatcher(new FetchErrorMessage($"Network error: {request.error}"));
            }
        }
    }

    private void SaveDataToServer(int count, Dispatcher<CounterMessage> dispatcher)
    {
        _context.StartCoroutine(SaveDataCoroutine(count, dispatcher));
    }

    private IEnumerator SaveDataCoroutine(int count, Dispatcher<CounterMessage> dispatcher)
    {
        var data = new ServerCounterData { count = count, timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() };
        string json = JsonUtility.ToJson(data);

        using (UnityWebRequest request = UnityWebRequest.Put($"{_serverUrl}/counter", json))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                dispatcher(new SaveSuccessMessage());
            }
            else
            {
                dispatcher(new SaveErrorMessage($"Save error: {request.error}"));
            }
        }
    }
}

// 5. Implement Renderer (UI)
public class CounterRenderer : IRenderer<CounterModel, CounterMessage>
{
    private readonly TMP_Text _counterText;
    private readonly Button _incrementButton;
    private readonly Button _decrementButton;
    private readonly Button _resetButton;
    private readonly Button _delayedIncrementButton;
    private readonly TMP_InputField _inputField;
    private readonly Button _submitButton;
    private readonly Button _fetchButton;
    private readonly Button _saveButton;
    private readonly TMP_Text _statusText;
    private Dispatcher<CounterMessage> _dispatcher;

    public CounterRenderer(
        TMP_Text counterText, 
        Button incrementButton, 
        Button decrementButton, 
        Button resetButton, 
        Button delayedIncrementButton, 
        TMP_InputField inputField, 
        Button submitButton,
        Button fetchButton,
        Button saveButton,
        TMP_Text statusText)
    {
        _counterText = counterText;
        _incrementButton = incrementButton;
        _decrementButton = decrementButton;
        _resetButton = resetButton;
        _delayedIncrementButton = delayedIncrementButton;
        _inputField = inputField;
        _submitButton = submitButton;
        _fetchButton = fetchButton;
        _saveButton = saveButton;
        _statusText = statusText;
    }

    public void Init(Dispatcher<CounterMessage> dispatcher)
    {
        _dispatcher = dispatcher;
        
        // Wire up button clicks to dispatch messages
        _incrementButton.onClick.AddListener(() => _dispatcher(new IncrementMessage()));
        _decrementButton.onClick.AddListener(() => _dispatcher(new DecrementMessage()));
        _resetButton.onClick.AddListener(() => _dispatcher(new ResetMessage()));
        _delayedIncrementButton.onClick.AddListener(() => _dispatcher(new DelayedIncrementMessage()));
        _submitButton.onClick.AddListener(() => 
        {
            if (int.TryParse(_inputField.text, out int value))
                _dispatcher(new SetValueMessage(value));
        });
        
        // Server buttons
        _fetchButton.onClick.AddListener(() => _dispatcher(new FetchFromServerMessage()));
        _saveButton.onClick.AddListener(() => _dispatcher(new SaveToServerMessage()));
    }

    public void Render(CounterModel model)
    {
        // Update UI based on model
        _counterText.text = $"Count: {model.Count}";
        
        // Show loading/error status
        if (model.IsLoadingFromServer)
        {
            _statusText.text = "Loading from server...";
            _statusText.color = Color.yellow;
        }
        else if (!string.IsNullOrEmpty(model.ErrorMessage))
        {
            _statusText.text = $"Error: {model.ErrorMessage}";
            _statusText.color = Color.red;
        }
        else if (model.ServerData.timestamp > 0)
        {
            _statusText.text = $"Server data: {model.ServerData.message}";
            _statusText.color = Color.green;
        }
        else
        {
            _statusText.text = "Ready";
            _statusText.color = Color.white;
        }
        
        // Disable buttons when processing
        bool interactable = !model.IsProcessing && !model.IsLoadingFromServer;
        _incrementButton.interactable = interactable;
        _decrementButton.interactable = interactable;
        _delayedIncrementButton.interactable = interactable;
        _submitButton.interactable = interactable;
        _inputField.interactable = interactable;
        _fetchButton.interactable = interactable;
        _saveButton.interactable = interactable;
    }
}

// 6. Create MonoBehaviour to tie it all together
public class CounterController : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private Button incrementButton;
    [SerializeField] private Button decrementButton;
    [SerializeField] private Button delayedIncrementButton;
    [SerializeField] private TMP_InputField textInput;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button fetchButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private string serverUrl = "https://api.example.com";

    private Elm<CounterModel, CounterMessage> _elm;

    private void Start()
    {
        // Initialize function - optionally fetch data on startup
        (CounterModel, Cmd<CounterMessage>) Init()
        {
            var initialModel = new CounterModel 
            { 
                Count = 0, 
                IsProcessing = false,
                IsLoadingFromServer = false,
                ErrorMessage = null
            };
            
            // Optional: Fetch data immediately on startup
            // var fetchCmd = new Cmd<CounterMessage>(dispatcher => dispatcher(new FetchFromServerMessage()));
            // return (initialModel, fetchCmd);
            
            return (initialModel, Cmd<CounterMessage>.None);
        }

        // Create the Elm architecture instance
        var updater = new CounterUpdater(this, serverUrl);
        var renderer = new CounterRenderer(
            counterText, incrementButton, decrementButton, 
            resetButton, delayedIncrementButton, textInput, 
            submitButton, fetchButton, saveButton, statusText);
        
        _elm = new Elm<CounterModel, CounterMessage>(Init, updater, renderer);
    }
}
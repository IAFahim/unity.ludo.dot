using UnityEngine;
using System;
using System.Collections;
using System.Threading.Tasks;
using Google;
using Firebase;
using Firebase.Auth;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleManager : MonoBehaviour
{
    [Header("Google Settings")]
    [Tooltip("Paste your Google Web Client ID here (from Google Cloud Console)")]
    public string webClientId = "722373905981-8h07papjs7bubdb7abpbn3rnogsr6trs.apps.googleusercontent.com";

    [Header("UI")]
    public Text statusText; // ✅ Text to show status on mobile or Editor

    private GoogleSignInConfiguration configuration;
    private string accessToken;
    private string googleName;
    private string googleEmail;
    private string googlePhotoUrl;

    private FirebaseAuth auth;
    private FirebaseUser user;

    private async void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestEmail = true,
            RequestIdToken = true
        };

        // ✅ Initialize Firebase first
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
        }
        else
        {
            UpdateStatus("❌ Could not resolve all Firebase dependencies: " + dependencyStatus);
            return;
        }

// #if UNITY_EDITOR
//         UpdateStatus("🧩 Running in Unity Editor — Google Sign-In is simulated.");
// #else
        // ✅ Auto-login if previous Google session exists
        if (PlayerPrefs.GetString("LoggedType") == "Google")
        {
            UpdateStatus("🔹 Attempting Google Auto-Login...");
            // GoogleSignIn.Configuration = configuration;
            // GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
        }
//#endif
    }

    public void OnGoogleSignInClick()
    {
// #if UNITY_EDITOR
//         // ✅ Simulated login in Unity Editor
//         UpdateStatus("🧩 Simulating Google Sign-In in Editor...");
//         SimulateEditorLogin();
// #else
        UpdateStatus("🔹 Starting Google Sign-In...");
        // GoogleSignIn.Configuration = configuration;
        // GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
//#endif
    }

    private void SimulateEditorLogin()
    {
        googleName = "Editor Test User";
        googleEmail = "testuser@gmail.com";
        googlePhotoUrl = "https://i.pravatar.cc/150?img=3"; // random test avatar
        accessToken = "FAKE_EDITOR_TOKEN";

        UpdateStatus("✅ [Editor Mode] Google Login Success: " + googleName);

        GameManager.Instance.nameMy = googleName;
        GameManager.Instance.avatarMyUrl = googlePhotoUrl;
        GameManager.Instance.logged = false;

        PlayerPrefs.SetString("LoggedType", "Google");
        PlayerPrefs.Save();

        StartCoroutine(DownloadProfilePicture(googlePhotoUrl));
        LoginWithGoogleToPlayFab(accessToken); // ✅ Function name kept same
    }

    // private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    // {
    //     if (task == null)
    //     {
    //         UpdateStatus("❌ Google Sign-In Task is null");
    //         return;
    //     }
    //
    //     if (task.IsFaulted)
    //     {
    //         UpdateStatus("❌ Google Sign-In Failed!");
    //         foreach (var e in task.Exception.InnerExceptions)
    //             Debug.LogError("Google Error: " + e.Message);
    //     }
    //     else if (task.IsCanceled)
    //     {
    //         UpdateStatus("⚠️ Google Sign-In Canceled by user");
    //     }
    //     else
    //     {
    //         GoogleSignInUser user = task.Result;
    //         accessToken = user.IdToken; // ✅ Correct token for Firebase
    //         googleName = user.DisplayName;
    //         googleEmail = user.Email;
    //         googlePhotoUrl = user.ImageUrl?.AbsoluteUri;
    //
    //         UpdateStatus($"✅ Google Login Success\nName: {googleName}");
    //
    //         GameManager.Instance.nameMy = googleName;
    //         GameManager.Instance.avatarMyUrl = googlePhotoUrl;
    //         GameManager.Instance.logged = false;
    //
    //         PlayerPrefs.SetString("LoggedType", "Google");
    //         PlayerPrefs.Save();
    //
    //         if (!string.IsNullOrEmpty(googlePhotoUrl))
    //             StartCoroutine(DownloadProfilePicture(googlePhotoUrl));
    //
    //         // ✅ Function name same, but now works with Firebase
    //         LoginWithGoogleToPlayFab(accessToken);
    //     }
    // }

    private IEnumerator DownloadProfilePicture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite avatarSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            GameManager.Instance.avatarMy = avatarSprite;
            Debug.Log("✅ Google Avatar Loaded Successfully!");
        }
        else
        {
            UpdateStatus("⚠️ Failed to load Google avatar.");
        }
    }

    // ✅ Same function name, but internally now authenticates with Firebase
    private async void LoginWithGoogleToPlayFab(string token)
    {
        UpdateStatus("🔹 Logging in to Firebase...");
        Debug.Log("🔹 Logging in to Firebase using Google ID token...");

        try
        {
            Credential credential = GoogleAuthProvider.GetCredential(token, null);
            user = await auth.SignInWithCredentialAsync(credential);
            UpdateStatus("✅ Firebase Login Successful! User: " + user.DisplayName);

            GameManager.Instance.logged = true;
        }
        catch (Exception e)
        {
            UpdateStatus("❌ Firebase Login Error: " + e.Message);
            Debug.LogError("❌ Firebase Google Login Error: " + e);
        }
    }

    private void OnPlayFabLoginSuccess() { } // dummy to keep structure
    private void OnPlayFabLoginError() { }   // dummy to keep structure

    public void SignOutGoogle()
    {
        UpdateStatus("🔹 Signing out...");
#if !UNITY_EDITOR
        GoogleSignIn.DefaultInstance.SignOut();
#endif
        if (auth != null)
            auth.SignOut();

        PlayerPrefs.DeleteKey("LoggedType");
        GameManager.Instance.logged = false;
    }

    private void UpdateStatus(string message)
    {
        Debug.Log(message);
        if (statusText != null)
            statusText.text = message;
    }
}

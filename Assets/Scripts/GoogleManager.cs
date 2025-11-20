using UnityEngine;
using System;
using System.Collections;
using ElProfesorKudo.Firebase.Event;
using ElProfesorKudo.Firebase.UI;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GoogleManager : MonoBehaviour
{
    public GameObject[] objectsToDisable;
    public PlayFabManager playFabManager;

    private bool isFirebaseReady = false;
    private bool isProcessingLogin = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        // Subscribe to Firebase callbacks
        FirebaseCallbacks.SubscribeGoogleSignInAndroidSuccess(OnAuthenticationFinished);
        FirebaseCallbacks.SubscribeCurrentUserLoaded(OnCurrentUserLoaded);
        FirebaseCallbacks.SubscribeFirebaseReady(OnFirebaseReady);
    }

    // ✅ This is called when Firebase is ready
    private void OnFirebaseReady()
    {
        Debug.Log("✅ Firebase is ready!");
        isFirebaseReady = true;
        CheckAutoLogin();
    }

    // ✅ Check for auto-login only after Firebase is ready
    private void CheckAutoLogin()
    {
        if (!isFirebaseReady)
        {
            Debug.LogWarning("⚠️ Firebase not ready yet, cannot check auto-login");
            return;
        }

        if (isProcessingLogin)
        {
            Debug.LogWarning("⚠️ Already processing login");
            return;
        }

        // Check if user is already logged in with Firebase
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            Debug.Log("✅ User already logged in with Firebase");
            isProcessingLogin = true;
            
            foreach (var o in objectsToDisable)
            {
                o.SetActive(false);
            }
            
            PopulateUserData();
            playFabManager.Login();
            SceneManager.LoadScene("MenuScene");
            return;
        }

        // Check if user was previously logged in with Google
        if (PlayerPrefs.GetString("LoggedType") == "Google")
        {
            Debug.Log("🔹 Previous Google session found, attempting auto-login...");
            isProcessingLogin = true;
            
            foreach (var o in objectsToDisable)
            {
                o.SetActive(false);
            }
            
            // Trigger silent sign-in through your Firebase UI
            FirebaseAuthUIController.Instance.OnClickSignInGoogle();
        }
    }

    private void OnCurrentUserLoaded(FirebaseUser user)
    {
        if (user == null)
        {
            Debug.LogWarning("⚠️ No current user loaded");
            isProcessingLogin = false;
            return;
        }

        if (isProcessingLogin) return; // Prevent duplicate processing
        
        Debug.Log("✅ Current user loaded: " + user.DisplayName);
        isProcessingLogin = true;
        
        PopulateUserData();
        playFabManager.Login();
        SceneManager.LoadScene("MenuScene");
    }

    public void OnGoogleSignInClick()
    {
        if (!isFirebaseReady)
        {
            UpdateStatus("❌ Firebase not ready. Please wait...");
            return;
        }

        if (isProcessingLogin)
        {
            UpdateStatus("⚠️ Login already in progress...");
            return;
        }

        UpdateStatus("🔹 Starting Google Sign-In...");
        isProcessingLogin = true;
        FirebaseAuthUIController.Instance.OnClickSignInGoogle();
    }

    // ✅ FIXED: Now ensures execution on main thread
    private void OnAuthenticationFinished(string userID)
    {
        // Use ContinueWithOnMainThread to ensure Unity operations run on main thread
        System.Threading.Tasks.Task.Run(() => userID).ContinueWithOnMainThread(task =>
        {
            ProcessAuthentication(task.Result);
        });
    }

    private void ProcessAuthentication(string userID)
    {
        if (string.IsNullOrEmpty(userID))
        {
            UpdateStatus("❌ Google Sign-In failed - no user ID");
            isProcessingLogin = false;
            return;
        }

        Debug.Log("✅ Google authentication finished for user: " + userID);

        try
        {
            // Verify Firebase user exists
            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            if (user == null)
            {
                UpdateStatus("❌ Firebase user not found after authentication");
                isProcessingLogin = false;
                return;
            }

            // Populate user data
            PopulateUserData();

            // Save login type and user ID
            PlayerPrefs.SetString("LoggedType", "Google");
            PlayerPrefs.SetString("unique_identifier", userID);
            PlayerPrefs.Save();

            // Mark as logged in
            GameManager.Instance.logged = true;

            // Login to PlayFab and change scene
            UpdateStatus("✅ Login successful! Loading game...");
            playFabManager.Login();
            SceneManager.LoadScene("MenuScene");
        }
        catch (Exception e)
        {
            UpdateStatus("❌ Error during login: " + e.Message);
            Debug.LogError("❌ Login Error: " + e);
            isProcessingLogin = false;
        }
    }

    private void PopulateUserData()
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null)
        {
            Debug.LogWarning("⚠️ Cannot populate user data - no current user");
            return;
        }

        GameManager.Instance.nameMy = user.DisplayName;
        string photoUrl = user.PhotoUrl?.AbsoluteUri;
        GameManager.Instance.avatarMyUrl = photoUrl;

        UpdateStatus($"✅ Welcome, {user.DisplayName}!");

        if (!string.IsNullOrEmpty(photoUrl))
        {
            StartCoroutine(DownloadProfilePicture(photoUrl));
        }
    }

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
            Debug.LogWarning("⚠️ Failed to load Google avatar: " + request.error);
        }
    }

    public void SignOutGoogle()
    {
        if (!isFirebaseReady)
        {
            Debug.LogWarning("⚠️ Cannot sign out - Firebase not ready");
            return;
        }

        UpdateStatus("🔹 Signing out...");
        
        // Sign out from Firebase
        FirebaseAuth.DefaultInstance.SignOut();
        
        // Clear saved data
        PlayerPrefs.DeleteKey("LoggedType");
        PlayerPrefs.DeleteKey("unique_identifier");
        PlayerPrefs.Save();
        
        // Reset game state
        GameManager.Instance.logged = false;
        GameManager.Instance.nameMy = "";
        GameManager.Instance.avatarMyUrl = "";
        GameManager.Instance.avatarMy = null;
        
        isProcessingLogin = false;
        
        UpdateStatus("✅ Signed out successfully");
        
        // Optionally reload login scene
        // SceneManager.LoadScene("LoginScene");
    }

    private void UpdateStatus(string message)
    {
        Debug.Log(message);
    }
    private void OnDestroy()
    {
        // Unsubscribe from callbacks to prevent memory leaks
        FirebaseCallbacks.SubscribeAppleSignInAndroidSuccess(OnAuthenticationFinished, false);
        FirebaseCallbacks.SubscribeCurrentUserLoaded(OnCurrentUserLoaded, false);
        FirebaseCallbacks.SubscribeFirebaseReady(OnFirebaseReady, false);
    }
}
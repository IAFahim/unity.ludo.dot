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
    private bool shouldCheckAutoLogin = false; // Flag for main thread check

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        // Subscribe to Firebase callbacks
        FirebaseCallbacks.SubscribeGoogleSignInAndroidSuccess(OnAuthenticationFinished);
        FirebaseCallbacks.SubscribeCurrentUserLoaded(OnCurrentUserLoaded);
        FirebaseCallbacks.SubscribeFirebaseReady(OnFirebaseReady);
    }

    private void Update()
    {
        // Check auto-login on main thread
        if (shouldCheckAutoLogin && PlayerPrefs.GetInt("HasLoggedOut", 0) == 0)
        {
            shouldCheckAutoLogin = false;
            CheckAutoLogin();
        }
    }

    // ✅ This is called when Firebase is ready (may be on background thread)
    private void OnFirebaseReady()
    {
        Debug.Log("✅ Firebase is ready!");
        isFirebaseReady = true;

        // 🔧 FIX: Set flag instead of calling directly
        shouldCheckAutoLogin = true;
    }

    // ✅ Check for auto-login only after Firebase is ready
    private void CheckAutoLogin()
    {
        // 🔧 FIX: Null check for playFabManager
        if (playFabManager == null)
        {
            Debug.LogError("❌ PlayFabManager is not assigned in GoogleManager!");
            return;
        }

        if (PlayerPrefs.GetString("LoggedType") != "Google")
        {
            playFabManager.Login();
            return;
        }

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

        // 🔧 FIX: Check if user manually logged out
        bool hasLoggedOut = PlayerPrefs.GetInt("HasLoggedOut", 0) == 1;
        if (hasLoggedOut)
        {
            Debug.Log("🔹 User previously logged out - skipping auto-login");
            PlayerPrefs.DeleteKey("HasLoggedOut"); // Clear the flag
            PlayerPrefs.Save();
            return;
        }

        // 🔧 FIX: Null check for FirebaseAuth
        if (FirebaseAuth.DefaultInstance == null)
        {
            Debug.LogError("❌ FirebaseAuth.DefaultInstance is null!");
            return;
        }

        // Check if user is already logged in with Firebase
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            Debug.Log("✅ User already logged in with Firebase");
            isProcessingLogin = true;

            // 🔧 FIX: Null check for objects to disable
            if (objectsToDisable != null)
            {
                foreach (var o in objectsToDisable)
                {
                    if (o != null)
                    {
                        o.SetActive(false);
                    }
                }
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

            // 🔧 FIX: Null check for objects to disable
            if (objectsToDisable != null)
            {
                foreach (var o in objectsToDisable)
                {
                    if (o != null)
                    {
                        o.SetActive(false);
                    }
                }
            }

            // 🔧 FIX: Null check for FirebaseAuthUIController
            if (FirebaseAuthUIController.Instance != null)
            {
                FirebaseAuthUIController.Instance.OnClickSignInGoogle();
            }
            else
            {
                Debug.LogError("❌ FirebaseAuthUIController.Instance is null!");
                isProcessingLogin = false;
            }
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

        if (playFabManager != null)
        {
            playFabManager.Login();
        }

        SceneManager.LoadScene("MenuScene");
    }

    public void OnGoogleSignInClick()
    {
        if (!isFirebaseReady)
        {
            UpdateStatus("❌ Firebase not ready. Please wait...");
            return;
        }

        UpdateStatus("🔹 Starting Google Sign-In...");
        isProcessingLogin = true;

        if (FirebaseAuthUIController.Instance != null)
        {
            FirebaseAuthUIController.Instance.OnClickSignInGoogle();
        }
        else
        {
            Debug.LogError("❌ FirebaseAuthUIController.Instance is null!");
            isProcessingLogin = false;
        }
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

            // 🔧 FIX: Null check for GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.logged = true;
            }

            // Login to PlayFab and change scene
            UpdateStatus("✅ Login successful! Loading game...");

            if (playFabManager != null)
            {
                playFabManager.Login();
            }

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

        // 🔧 FIX: Null check for GameManager
        if (GameManager.Instance == null)
        {
            Debug.LogError("❌ GameManager.Instance is null!");
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

            if (GameManager.Instance != null)
            {
                GameManager.Instance.avatarMy = avatarSprite;
                Debug.Log("✅ Google Avatar Loaded Successfully!");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Failed to load Google avatar: " + request.error);
        }
    }


    private void UpdateStatus(string message)
    {
        Debug.Log(message);
    }

    private void OnDestroy()
    {
        FirebaseCallbacks.SubscribeGoogleSignInAndroidSuccess(OnAuthenticationFinished, false);
        FirebaseCallbacks.SubscribeCurrentUserLoaded(OnCurrentUserLoaded, false);
        FirebaseCallbacks.SubscribeFirebaseReady(OnFirebaseReady, false);
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;

// REMOVED: using Facebook.Unity;
// REMOVED: using Facebook.MiniJSON;

public class FacebookManager : MonoBehaviour
{
    public GameObject facebookLoginButton;
    public GameObject guestLoginButton;
    private PlayFabManager playFabManager;
    public string name;
    public Sprite sprite;
    private GameObject FbLoginButton;
    private bool LoggedIn = false;
    private FacebookFriendsMenu facebookFriendsMenu;
    private bool alreadyGotFriends = false;
    public GameObject splashCanvas;
    public GameObject fbButton;
    public GameObject matchPlayersCanvas;
    public GameObject menuCanvas;
    public GameObject gameTitle;
    public GameObject idLoginDialog;
    public GameObject idRegisterDialog;
    public GameObject forgetPasswordDialog;

    public InputField loginEmail;
    public InputField loginPassword;
    public GameObject loginInvalidEmailorPassword;

    public InputField regiterEmail;
    public InputField registerPassword;
    public InputField registerNickname;
    public GameObject registerInvalidInput;

    public InputField resetPasswordEmail;
    public GameObject resetPasswordInformationText;

    void Start()
    {
        Debug.Log("FBManager start");
        GameManager.Instance.facebookManager = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        FbLoginButton = GameObject.Find("FbLoginButton");
        facebookFriendsMenu = GameManager.Instance.facebookFriendsMenu;

        // Disable FB button if it exists since we removed the SDK
        if (fbButton != null) fbButton.SetActive(false);
    }

    void Awake()
    {
        Debug.Log("FBManager awake (Bypassing Facebook)");
        GameManager.Instance.facebookManager = this;
        DontDestroyOnLoad(transform.gameObject);
        playFabManager = GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>();
        // if (!GameManager.Instance.logged && )
        // {
        //     // Immediately init session without waiting for FB.Init
        //     initSession();
        //     // GameManager.Instance.logged = true;
        // }
    }

    // Simulates the callback SetInit used to do
    private void initSession()
    {
        Debug.Log("Init session (Bypassed)");
        string logType = PlayerPrefs.GetString("LoggedType");

        if (logType.Equals("EmailAccount"))
        {
            playFabManager.LoginWithEmailAccount();
        }
        else if (!logType.Equals("Google"))
        {
            playFabManager.Login();
        }
    }

    public void startRandomGame()
    {
        GameManager.Instance.matchPlayerObject.GetComponent<SetMyData>().MatchPlayer();
        GameManager.Instance.matchPlayerObject.GetComponent<SetMyData>().setBackButton(true);
        playFabManager.JoinRoomAndStartGame();
    }

    // Redirects FB Login button click to Guest Login
    public void FBLogin()
    {
        Debug.Log("FB Login clicked - Redirecting to Guest Login");
        GuestLogin();
    }

    public void FBLinkAccount()
    {
        Debug.Log("FB Link removed");
        // Optional: Show a message saying feature is disabled
    }

    public void FBLoginWithoutLink()
    {
        GuestLogin();
    }

    public void GuestLogin()
    {
        if (!LoggedIn)
        {
            playFabManager.Login();
        }
    }

    // --- UI Handling Methods (Kept intact to prevent missing reference errors) ---

    public void showRegisterDialog()
    {
        idLoginDialog.SetActive(false);
        idRegisterDialog.SetActive(true);
    }

    public void CloseLoginDialog()
    {
        loginInvalidEmailorPassword.SetActive(false);
        loginEmail.text = "";
        loginPassword.text = "";
        idLoginDialog.SetActive(false);
    }

    public void CloseRegisterDialog()
    {
        regiterEmail.text = "";
        registerPassword.text = "";
        registerNickname.text = "";
        registerInvalidInput.SetActive(false);
        idRegisterDialog.SetActive(false);
    }

    public void CloseForgetPasswordDialog()
    {
        resetPasswordEmail.text = "";
        resetPasswordInformationText.SetActive(false);
        forgetPasswordDialog.SetActive(false);
    }

    public void showForgetPasswordDialog()
    {
        forgetPasswordDialog.SetActive(true);
        idLoginDialog.SetActive(false);
    }

    public void IDLoginButtonPressed()
    {
        idLoginDialog.SetActive(true);
    }

    public void IDLogin()
    {
        // Just reuse guest or email login logic here if needed
    }

    // --- Dummy Methods for FB Data (Returning nothing or defaults) ---

    private void callApiToGetName()
    {
        // Do nothing, name handled by PlayFab
    }

    public void getMyProfilePicture(string userID)
    {
        // Do nothing, avatars handled by internal assets
    }

    public IEnumerator loadImageMy(string url)
    {
        yield return null;
    }

    public void getOpponentProfilePicture(string userID)
    {
        // Do nothing
    }

    public void getFacebookInvitableFriends()
    {
        // Feature disabled - maybe show "No friends found" or standard list
        if (facebookFriendsMenu != null)
        {
            facebookFriendsMenu.showFriends(null, null, null);
        }
    }

    public void destroy()
    {
        if (this.gameObject != null)
            DestroyImmediate(this.gameObject);
    }
}
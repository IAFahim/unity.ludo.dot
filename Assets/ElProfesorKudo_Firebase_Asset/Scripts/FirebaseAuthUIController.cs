using UnityEngine;

namespace ElProfesorKudo.Firebase.UI
{
    using ElProfesorKudo.Firebase.Auth;
    using ElProfesorKudo.Firebase.GoogleSignIn.Android;
    using ElProfesorKudo.Firebase.GoogleSignIn;
    using ElProfesorKudo.Firebase.GoogleSignIn.iOS;
    using ElProfesorKudo.Firebase.AppleSignIn;
    using ElProfesorKudo.Firebase.AppleSignIn.iOS;
    using ElProfesorKudo.Firebase.AppleSignIn.Android;
    using ElProfesorKudo.Firebase.Common;
    using ElProfesorKudo.Firebase.PopUp;
    using System;

    public class FirebaseAuthUIController : Singleton<FirebaseAuthUIController>
    {
#if Debug
        [Header("Login UI")]
        [SerializeField] private TMP_InputField _loginEmailInput;
        [SerializeField] private TMP_InputField _loginPasswordInput;

        [Header("Register UI")]
        [SerializeField] private TMP_InputField _registerEmailInput;
        [SerializeField] private TMP_InputField _registerPasswordInput;
        [SerializeField] private TMP_InputField _registerConfirmPasswordInput;
        [SerializeField] private TextMeshProUGUI _notificationTextMeshPro;

        [Header("Forget Password UI")]
        [SerializeField] private TMP_InputField _emailRetrieveInput;
        [Header("Score UI")]
        [SerializeField] private TextMeshProUGUI _scoreTextMeshPro;

        [Header("Parent Panels")]
        [SerializeField] private GameObject _loginPanel;
        [SerializeField] private GameObject _registerPanel;
        [SerializeField] private GameObject _forgetPasswordPanel;
        [SerializeField] private GameObject _googleSignInPanel;
        [SerializeField] private GameObject _appleSignInPanel;
        [SerializeField] private GameObject _uiDataUserPanel;
        [SerializeField] private GameObject _scorePanel;

        [Header("Data User Display UI")]
        [SerializeField] private TextMeshProUGUI _createdAtTextMeshPro;
        [SerializeField] private TextMeshProUGUI _lastLoginTextMeshPro;
        [SerializeField] private TextMeshProUGUI _emailTextMeshPro;
        [SerializeField] private TextMeshProUGUI _descriptionTextMeshPro;
        [SerializeField] private TMP_InputField _descriptionEditInputField;
        [SerializeField] private Image _profilePictureImage;
#endif

        [Header("Class Google Sign In IOS - Android")] [SerializeField]
        private FirebaseGoogleSignInAndroid _androidGoogleSignIn;

        [SerializeField] private FirebaseGoogleSignInIOS _iosGoogleSignIn;
        private FirebaseAbstractGoogleSignIn _googleSignInHandler;

        [Header("Class Apple Sign In IOS - Android")] [SerializeField]
        private FirebaseAppleSignInAndroid _androidAppleSignIn;

        [SerializeField] private FirebaseAppleSignInIOS _iosAppleSignIn;
        private FirebaseAbstractAppleSignIn _appleSignInHandler;


        protected override void Awake()
        {
            base.Awake();
#if UNITY_ANDROID
            _googleSignInHandler = _androidGoogleSignIn;
            _appleSignInHandler = _androidAppleSignIn;
#elif UNITY_IOS
            _googleSignInHandler = _iosGoogleSignIn;
            _appleSignInHandler = _iosAppleSignIn;
#else
            CustomLogger.LogWarning("Platform not supported");
#endif
        }

        private void OnEnable()
        {
            var isForget = PlayerPrefs.GetInt("Forget", 0);
            if (isForget != 0)
            {
                PlayerPrefs.SetInt("Forget", 0);
                OnClickSignOutGoogle();
            }
        }

        public void OnClickLogout()
        {
            FirebaseClassicAuthService.Instance.Logout();
        }
        
        public void OnClickSignInApple()
        {
            _appleSignInHandler.SignIn();
        }

        public void OnClickSignOutApple()
        {
            _appleSignInHandler.SignOut();
        }


        public void OnClickSignInGoogle()
        {
            _googleSignInHandler.SignIn();
        }

        public void OnClickSignOutGoogle()
        {
            _googleSignInHandler.SignOut();
        }
    }
}
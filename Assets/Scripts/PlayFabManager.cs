using UnityEngine;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.SceneManagement;
// REMOVED: using Facebook.Unity;
using System.Collections.Generic;
using ExitGames.Client.Photon.Chat;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using AssemblyCSharp;
using System.Globalization;
using Firebase.Auth;

public class PlayFabManager : Photon.PunBehaviour, IChatClientListener
{
    private Sprite[] avatarSprites;

    public string PlayFabId;
    public string authToken;
    public bool multiGame = true;
    public bool roomOwner = false;
    private FacebookManager fbManager;
    public GameObject fbButton;
    private FacebookFriendsMenu facebookFriendsMenu;
    public ChatClient chatClient;
    private bool alreadyGotFriends = false;
    public GameObject menuCanvas;
    public GameObject MatchPlayersCanvas;
    public GameObject splashCanvas;
    public bool opponentReady = false;
    public bool imReady = false;
    public GameObject playerAvatar;
    public GameObject playerName;
    public GameObject backButtonMatchPlayers;

    public GameObject loginEmail;
    public GameObject loginPassword;
    public GameObject loginInvalidEmailorPassword;
    public GameObject loginCanvas;

    public GameObject regiterEmail;
    public GameObject registerPassword;
    public GameObject registerNickname;
    public GameObject registerInvalidInput;
    public GameObject registerCanvas;

    public GameObject resetPasswordEmail;
    public GameObject resetPasswordInformationText;

    public bool isInLobby = false;
    public bool isInMaster = false;
    private string[] randomNames = new string[] { "Alex", "Ryan", "Sophie", "Mia", "Liam", "Noah", "Olivia", "Emma" }; // (Truncated for brevity, keep your full list)

    void Awake()
    {
        Debug.Log("Playfab awake");
        PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.PhotonCloud;
        PhotonNetwork.PhotonServerSettings.PreferredRegion = CloudRegionCode.eu;
#if UNITY_IOS
        PhotonNetwork.PhotonServerSettings.Protocol = ConnectionProtocol.Tcp;
#else
        PhotonNetwork.PhotonServerSettings.Protocol = ConnectionProtocol.Udp;
#endif

        PlayFabSettings.TitleId = StaticStrings.PlayFabTitleID;
        PhotonNetwork.OnEventCall += this.OnEvent;
        DontDestroyOnLoad(transform.gameObject);
    }

    void OnDestroy()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    public void destroy()
    {
        if (this.gameObject != null)
            DestroyImmediate(this.gameObject);
    }

    void Start()
    {
        Debug.Log("Playfab start");
        PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong;
        GameManager.Instance.playfabManager = this;
        fbManager = GameObject.Find("FacebookManager").GetComponent<FacebookManager>();
        facebookFriendsMenu = GameManager.Instance.facebookFriendsMenu;
        avatarSprites = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().avatars;
    }

    void Update()
    {
        if (chatClient != null) { chatClient.Service(); }
    }

    // handle events:
    private void OnEvent(byte eventcode, object content, int senderid)
    {
        if (eventcode == (int)EnumPhoton.BeginPrivateGame) { LoadGameScene(); }
        else if (eventcode == (int)EnumPhoton.StartWithBots && senderid != PhotonNetwork.player.ID) { LoadBots(); }
        else if (eventcode == (int)EnumPhoton.StartGame) { LoadGameScene(); }
        else if (eventcode == (int)EnumPhoton.ReadyToPlay) { GameManager.Instance.readyPlayersCount++; }
    }

    public void LoadGameWithDelay() { LoadGameScene(); }

    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        if (GameManager.Instance.controlAvatars != null && GameManager.Instance.type == MyGameType.Private)
        {
            PhotonNetwork.LeaveRoom();
            GameManager.Instance.controlAvatars.ShowJoinFailed("Room closed");
        }
        else
        {
            if (newMasterClient.NickName == PhotonNetwork.player.NickName)
            {
                WaitForNewPlayer();
            }
        }
    }

    public void StartGame()
    {
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;
        CancelInvoke("StartGameWithBots");
        Invoke("startGameScene", 3.0f);
    }

    public void startGameScene()
    {
        if (GameManager.Instance.currentPlayersCount >= GameManager.Instance.requiredPlayers || GameManager.Instance.type == MyGameType.Private)
        {
            LoadGameScene();
            if (GameManager.Instance.type == MyGameType.Private)
                PhotonNetwork.RaiseEvent((int)EnumPhoton.BeginPrivateGame, null, true, null);
            else
                PhotonNetwork.RaiseEvent((int)EnumPhoton.StartGame, null, true, null);
        }
        else
        {
            if (PhotonNetwork.isMasterClient)
                WaitForNewPlayer();
        }
    }

    public void LoadGameScene()
    {
        GameManager.Instance.GameScene = "GameScene";
        if (!GameManager.Instance.gameSceneStarted)
        {
            SceneManager.LoadScene(GameManager.Instance.GameScene);
            GameManager.Instance.gameSceneStarted = true;
        }
    }

    public void WaitForNewPlayer()
    {
        if (PhotonNetwork.isMasterClient && GameManager.Instance.type != MyGameType.Private)
        {
            CancelInvoke("StartGameWithBots");
            Invoke("StartGameWithBots", StaticStrings.WaitTimeUntilStartWithBots);
        }
    }

    public void StartGameWithBots()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (PhotonNetwork.room.PlayerCount < GameManager.Instance.requiredPlayers)
            {
                LoadBots();
            }
        }
    }

    public void LoadBots()
    {
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;
        if (PhotonNetwork.isMasterClient) Invoke("AddBots", 3.0f);
        else AddBots();
    }

    public void AddBots()
    {
        if (PhotonNetwork.room.PlayerCount < GameManager.Instance.requiredPlayers)
        {
            if (PhotonNetwork.isMasterClient)
                PhotonNetwork.RaiseEvent((int)EnumPhoton.StartWithBots, null, true, null);

            for (int i = 0; i < GameManager.Instance.requiredPlayers - 1; i++)
            {
                if (GameManager.Instance.opponentsIDs[i] == null)
                    StartCoroutine(AddBot(i));
            }
        }
    }

    public IEnumerator AddBot(int i)
    {
        yield return new WaitForSeconds(i + UnityEngine.Random.Range(0.0f, 0.9f));
        GameManager.Instance.opponentsAvatars[i] = avatarSprites[UnityEngine.Random.Range(0, avatarSprites.Length - 1)];
        GameManager.Instance.opponentsIDs[i] = "_BOT" + i;
        GameManager.Instance.opponentsNames[i] = "BOT" + UnityEngine.Random.Range(100000, 999999);
        GameManager.Instance.controlAvatars.PlayerJoined(i, "_BOT" + i);
    }

    public void resetPassword()
    {
        resetPasswordInformationText.SetActive(false);
        SendAccountRecoveryEmailRequest request = new SendAccountRecoveryEmailRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            Email = resetPasswordEmail.GetComponent<Text>().text
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, (result) =>
        {
            resetPasswordInformationText.SetActive(true);
            resetPasswordInformationText.GetComponent<Text>().text = "Email sent to your address. Check your inbox";
        }, (error) =>
        {
            resetPasswordInformationText.SetActive(true);
            resetPasswordInformationText.GetComponent<Text>().text = "Account with specified email doesn't exist";
        });
    }

    public void setInitNewAccountData(bool fb)
    {
        Dictionary<string, string> data = MyPlayerData.InitialUserData(fb);
        GameManager.Instance.myPlayerData.UpdateUserData(data);
    }

    public void updateBoughtChats(int index)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(MyPlayerData.ChatsKey, GameManager.Instance.myPlayerData.GetChats() + ";'" + index + "'");
        GameManager.Instance.myPlayerData.UpdateUserData(data);
    }

    public void UpdateBoughtEmojis(int index)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(MyPlayerData.EmojiKey, GameManager.Instance.myPlayerData.GetEmoji() + ";'" + index + "'");
        GameManager.Instance.myPlayerData.UpdateUserData(data);
    }

    public void addCoinsRequest(int count)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(MyPlayerData.CoinsKey, "" + (GameManager.Instance.myPlayerData.GetCoins() + count));
        GameManager.Instance.myPlayerData.UpdateUserData(data);
    }

    public void getPlayerDataRequest()
    {
        GetUserDataRequest getdatarequest = new GetUserDataRequest() { PlayFabId = GameManager.Instance.playfabManager.PlayFabId };
        PlayFabClientAPI.GetUserData(getdatarequest, (result) =>
        {
            Dictionary<string, UserDataRecord> data = result.Data;
            GameManager.Instance.myPlayerData = new MyPlayerData(data, true);
            StartCoroutine(loadSceneMenu());
        }, (error) => { Debug.Log("Data updated error " + error.ErrorMessage); }, null);
    }

    private IEnumerator loadSceneMenu()
    {
        yield return new WaitForSeconds(0.1f);
        if (isInMaster && isInLobby) SceneManager.LoadScene("MenuScene");
        else StartCoroutine(loadSceneMenu());
    }

    public void RegisterNewAccountWithID()
    {
        string email = regiterEmail.GetComponent<Text>().text;
        string password = registerPassword.GetComponent<Text>().text;
        string nickname = registerNickname.GetComponent<Text>().text;

        registerInvalidInput.SetActive(false);

        if (Regex.IsMatch(email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$") && password.Length >= 6 && nickname.Length > 0)
        {
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                Email = email,
                Password = password,
                RequireBothUsernameAndEmail = false
            };

            PlayFabClientAPI.RegisterPlayFabUser(request, (result) =>
            {
                PlayFabId = result.PlayFabId;
                registerCanvas.SetActive(false);
                PlayerPrefs.SetString("email_account", email);
                PlayerPrefs.SetString("password", password);
                PlayerPrefs.SetString("LoggedType", "EmailAccount");
                PlayerPrefs.Save();
                GameManager.Instance.nameMy = nickname;
                setInitNewAccountData(false);

                UpdateUserTitleDisplayNameRequest displayNameRequest = new UpdateUserTitleDisplayNameRequest() { DisplayName = GameManager.Instance.playfabManager.PlayFabId };
                PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, null, null, null);

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("LoggedType", "EmailAccount");
                data.Add("PlayerName", GameManager.Instance.nameMy);
                GameManager.Instance.myPlayerData.UpdateUserData(data);
                GetPhotonToken();
            },
                (error) =>
                {
                    registerInvalidInput.SetActive(true);
                    registerInvalidInput.GetComponent<Text>().text = error.ErrorMessage;
                });
        }
        else
        {
            registerInvalidInput.SetActive(true);
            registerInvalidInput.GetComponent<Text>().text = "Invalid input specified";
        }
    }

    // REMOVED FB LINK
    public void LinkFacebookAccount() { }

    // REMOVED FB LOGIN. If called, redirects to Guest.
    public void LoginWithFacebook()
    {
        Login(); 
    }

    public void CheckIfFirstTitleLogin(string id, bool fb)
    {
        GetUserDataRequest getdatarequest = new GetUserDataRequest() { PlayFabId = id };
        PlayFabClientAPI.GetUserData(getdatarequest, (result) =>
        {
            Dictionary<string, UserDataRecord> data = result.Data;
            if (!data.ContainsKey(MyPlayerData.TitleFirstLoginKey))
            {
                setInitNewAccountData(fb);
            }
        }, null, null);
    }

    public void LoginWithEmailAccount()
    {
        loginInvalidEmailorPassword.SetActive(false);
        string email = "", password = "";
        if (PlayerPrefs.HasKey("email_account"))
        {
            email = PlayerPrefs.GetString("email_account");
            password = PlayerPrefs.GetString("password");
        }
        else
        {
            email = loginEmail.GetComponent<Text>().text;
            password = loginPassword.GetComponent<Text>().text;
        }

        LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            Email = email,
            Password = password
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            PlayFabId = result.PlayFabId;
            loginCanvas.SetActive(false);
            PlayerPrefs.SetString("email_account", email);
            PlayerPrefs.SetString("password", password);
            PlayerPrefs.SetString("LoggedType", "EmailAccount");
            PlayerPrefs.Save();

            if (result.NewlyCreated) setInitNewAccountData(false);
            else CheckIfFirstTitleLogin(PlayFabId, false);

            GetUserDataRequest getdatarequest = new GetUserDataRequest() { PlayFabId = result.PlayFabId };
            PlayFabClientAPI.GetUserData(getdatarequest, (result2) =>
            {
                Dictionary<string, UserDataRecord> data2 = result2.Data;
                if (data2.ContainsKey("PlayerName")) GameManager.Instance.nameMy = data2["PlayerName"].Value;
                else
                {
                    Dictionary<string, string> data5 = new Dictionary<string, string>();
                    data5.Add("PlayerName", GameManager.Instance.nameMy);
                    GameManager.Instance.myPlayerData.UpdateUserData(data5);
                }
            }, null, null);

            GetPhotonToken();
        },
             (error) => { loginInvalidEmailorPassword.SetActive(true); });
    }

    public void Login()
    {
        string customId = "";
        if (PlayerPrefs.HasKey("unique_identifier"))
        {
            customId = PlayerPrefs.GetString("unique_identifier");
        }
        else
        {
            customId = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString("unique_identifier", customId);
        }




        Debug.Log("UNIQUE IDENTIFIER: " + customId);

        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            CustomId = customId
        };



        PlayFabClientAPI.LoginWithCustomID(request, (result) =>
        {
            PlayFabId = result.PlayFabId;
            Debug.Log("Got PlayFabID: " + PlayFabId);

            Dictionary<string, string> data = new Dictionary<string, string>();

            if (result.NewlyCreated)
            {
                Debug.Log("(new account)");
                setInitNewAccountData(false);

                string name = result.PlayFabId;
                name = "Guest";
                for (int i = 0; i < 6; i++)
                {
                    name += UnityEngine.Random.Range(0, 9);
                }

                if (FirebaseAuth.DefaultInstance?.CurrentUser != null) name = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
                data.Add("PlayerName", name);
                GameManager.Instance.nameMy = name;
                
            }
            else
            {
                CheckIfFirstTitleLogin(PlayFabId, false);
                Debug.Log("(existing account)");
            }



            // string name = result.PlayFabId;
            // if (PlayerPrefs.HasKey("GuestPlayerName"))
            // {
            //     //name = PlayerPrefs.GetString("GuestPlayerName");
            // }
            // else
            // {
            //     name = "Guest";
            //     for (int i = 0; i < 6; i++)
            //     {
            //         name += UnityEngine.Random.Range(0, 9);
            //     }
            //     PlayerPrefs.SetString("GuestPlayerName", name);
            //     PlayerPrefs.Save();
            //     data.Add("PlayerName", name);
            // }


            data.Add("LoggedType", "Guest");



            UpdateUserTitleDisplayNameRequest displayNameRequest = new UpdateUserTitleDisplayNameRequest()
            {
                //DisplayName = name,
                DisplayName = GameManager.Instance.playfabManager.PlayFabId
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, (response) =>
            {
                Debug.Log("Title Display name updated successfully");
            }, (error) =>
            {
                Debug.Log("Title Display name updated error: " + error.Error);

            }, null);


            GameManager.Instance.myPlayerData.UpdateUserData(data);

            GameManager.Instance.nameMy = name;

            // PlayerPrefs.SetString("LoggedType", "Guest");
            // PlayerPrefs.Save();


            GetPhotonToken();

        },
            (error) =>
            {
                Debug.Log("Error logging in player with custom ID:");
                Debug.Log(error.ErrorMessage);
                // GameManager.Instance.connectionLost.showDialog();
            });
    }

    public void GetPlayfabFriends()
    {
        // Modified to only fetch PlayFab friends, ignoring FB friends
        if (alreadyGotFriends)
        {
             facebookFriendsMenu.showFriends(null, null, null);
        }
        else
        {
            GetFriendsListRequest request = new GetFriendsListRequest();
            request.IncludeFacebookFriends = false; // Changed to false
            PlayFabClientAPI.GetFriendsList(request, (result) =>
            {
                var friends = result.Friends;
                List<string> playfabFriends = new List<string>();
                List<string> playfabFriendsName = new List<string>();
                List<string> playfabFriendsFacebookId = new List<string>();
                List<string> friendsToStatus = new List<string>();

                chatClient.RemoveFriends(GameManager.Instance.friendsIDForStatus.ToArray());

                int index = 0;
                foreach (var friend in friends)
                {
                    playfabFriends.Add(friend.FriendPlayFabId);
                    GetUserDataRequest getdatarequest = new GetUserDataRequest() { PlayFabId = friend.TitleDisplayName };
                    int ind2 = index;

                    PlayFabClientAPI.GetUserData(getdatarequest, (result2) =>
                    {
                        Dictionary<string, UserDataRecord> data2 = result2.Data;
                        playfabFriendsName[ind2] = data2["PlayerName"].Value;
                        GameManager.Instance.facebookFriendsMenu.updateName(ind2, data2["PlayerName"].Value, friend.TitleDisplayName);
                    }, null, null);

                    playfabFriendsName.Add("");
                    friendsToStatus.Add(friend.FriendPlayFabId);
                    index++;
                }

                GameManager.Instance.friendsIDForStatus = friendsToStatus;
                chatClient.AddFriends(friendsToStatus.ToArray());
                GameManager.Instance.facebookFriendsMenu.addPlayFabFriends(playfabFriends, playfabFriendsName, playfabFriendsFacebookId);
                GameManager.Instance.facebookFriendsMenu.showFriends(null, null, null);
                
            }, OnPlayFabError);
        }
    }

    void OnPlayFabError(PlayFabError error) { Debug.Log("Playfab Error: " + error.ErrorMessage); }

    // #######################  PHOTON  ##########################

    void GetPhotonToken()
    {
        GetPhotonAuthenticationTokenRequest request = new GetPhotonAuthenticationTokenRequest();
        request.PhotonApplicationId = StaticStrings.PhotonAppID.Trim();
        PlayFabClientAPI.GetPhotonAuthenticationToken(request, OnPhotonAuthenticationSuccess, OnPlayFabError);
    }

    void OnPhotonAuthenticationSuccess(GetPhotonAuthenticationTokenResult result)
    {
        string photonToken = result.PhotonCustomAuthenticationToken;
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
        PhotonNetwork.AuthValues.AddAuthParameter("username", this.PlayFabId);
        PhotonNetwork.AuthValues.AddAuthParameter("Token", result.PhotonCustomAuthenticationToken);
        PhotonNetwork.AuthValues.UserId = this.PlayFabId;
        PhotonNetwork.ConnectUsingSettings("1.4");
        PhotonNetwork.playerName = this.PlayFabId;
        authToken = result.PhotonCustomAuthenticationToken;
        getPlayerDataRequest();
        connectToChat();
    }

    public void connectToChat()
    {
        chatClient = new ChatClient(this);
        GameManager.Instance.chatClient = chatClient;
        ExitGames.Client.Photon.Chat.AuthenticationValues authValues = new ExitGames.Client.Photon.Chat.AuthenticationValues();
        authValues.UserId = this.PlayFabId;
        authValues.AuthType = ExitGames.Client.Photon.Chat.CustomAuthenticationType.Custom;
        authValues.AddAuthParameter("username", this.PlayFabId);
        authValues.AddAuthParameter("Token", authToken);
        chatClient.Connect(StaticStrings.PhotonChatID, "1.4", authValues);
    }

    public override void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged) { }
    public void OnConnected() { chatClient.Subscribe(new string[] { "invitationsChannel" }); }
    
    public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        GameManager.Instance.opponentDisconnected = true;
        GameManager.Instance.invitationID = "";
        if (GameManager.Instance.controlAvatars != null)
        {
            if (PhotonNetwork.room.PlayerCount > 1) GameManager.Instance.controlAvatars.startButtonPrivate.GetComponent<Button>().interactable = true;
            else GameManager.Instance.controlAvatars.startButtonPrivate.GetComponent<Button>().interactable = false;
            
            int index = GameManager.Instance.opponentsIDs.IndexOf(player.NickName);
            GameManager.Instance.controlAvatars.PlayerDisconnected(index);
        }
    }

    public void showMenu()
    {
        menuCanvas.gameObject.SetActive(true);
        playerName.GetComponent<Text>().text = GameManager.Instance.nameMy;
        if (GameManager.Instance.avatarMy != null) playerAvatar.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;
        splashCanvas.SetActive(false);
    }

    public void OnSubscribed(string[] channels, bool[] results) { chatClient.SetOnlineStatus(ChatUserStatus.Online); }

    public void challengeFriend(string id, string message)
    {
        chatClient.SendPrivateMessage(id, "INVITE_TO_PLAY_PRIVATE;" + GameManager.Instance.nameMy + ";" + message);
        GameManager.Instance.invitationID = id;
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (!sender.Equals(this.PlayFabId))
        {
            if (message.ToString().Contains("INVITE_TO_PLAY_PRIVATE"))
            {
                GameManager.Instance.invitationID = sender;
                string[] messageSplit = message.ToString().Split(';');
                string whoInvite = messageSplit[1];
                string payout = messageSplit[2];
                string roomID = messageSplit[3];
                GameManager.Instance.payoutCoins = int.Parse(payout);
                GameManager.Instance.invitationDialog.GetComponent<PhotonChatListener>().showInvitationDialog(0, whoInvite, payout, roomID, 0);
            }
        }
        if (!(GameManager.Instance.invitationID.Length == 0 || !GameManager.Instance.invitationID.Equals(sender)))
        {
            GameManager.Instance.invitationID = "";
        }
    }

    public void DebugReturn(DebugLevel level, string message) { }
    public void OnChatStateChange(ChatState state) { }
    public override void OnDisconnectedFromPhoton() { switchUser(); }
    public void DisconnecteFromPhoton() { PhotonNetwork.Disconnect(); }

    public void switchUser()
    {
        GameManager.Instance.playfabManager.destroy();
        GameManager.Instance.facebookManager.destroy();
        // GameManager.Instance.connectionLost.destroy();
        GameManager.Instance.avatarMy = null;
        GameManager.Instance.logged = false;
        GameManager.Instance.resetAllData();
        SceneManager.LoadScene("LoginSplash");
    }

    public void OnDisconnected() { connectToChat(); }
    public void OnGetMessages(string channelName, string[] senders, object[] messages) { }
    public void OnUnsubscribed(string[] channels) { }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        bool foundFriend = false;
        for (int i = 0; i < GameManager.Instance.friendsStatuses.Count; i++)
        {
            string[] friend = GameManager.Instance.friendsStatuses[i];
            if (friend[0].Equals(user))
            {
                GameManager.Instance.friendsStatuses[i][1] = "" + status;
                foundFriend = true;
                break;
            }
        }
        if (!foundFriend) GameManager.Instance.friendsStatuses.Add(new string[] { user, "" + status });
        if (GameManager.Instance.facebookFriendsMenu != null) GameManager.Instance.facebookFriendsMenu.updateFriendStatus(status, user);
    }

    public override void OnConnectedToMaster() { isInMaster = true; PhotonNetwork.JoinLobby(); }
    public override void OnJoinedLobby() { isInLobby = true; }

    public void JoinRoomAndStartGame()
    {
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
            {"m", GameManager.Instance.mode.ToString() +  GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString()}
         };
        StartCoroutine(TryToJoinRandomRoom(expectedCustomRoomProperties));
    }

    public IEnumerator TryToJoinRandomRoom(ExitGames.Client.Photon.Hashtable roomOptions)
    {
        while (true)
        {
            if (isInLobby && isInMaster) { PhotonNetwork.JoinRandomRoom(roomOptions, 0); break; }
            else { yield return new WaitForSeconds(0.05f); }
        }
    }

    public void OnPhotonRandomJoinFailed()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new String[] { "m", "v" };
        string BotMoves = generateBotMoves();
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
            { "m", GameManager.Instance.mode.ToString() +  GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString()},
            {"bt", BotMoves},
            {"fp", UnityEngine.Random.Range(0, GameManager.Instance.requiredPlayers)}
         };
        roomOptions.MaxPlayers = (byte)GameManager.Instance.requiredPlayers;
        StartCoroutine(TryToCreateGameAfterFailedToJoinRandom(roomOptions));
    }

    public string generateBotMoves()
    {
        string BotMoves = "";
        int BotCount = 100;
        for (int i = 0; i < BotCount; i++)
        {
            BotMoves += (UnityEngine.Random.Range(1, 7)).ToString();
            if (i < BotCount - 1) BotMoves += ",";
        }
        BotMoves += ";";
        float minValue = GameManager.Instance.playerTime / 10;
        if (minValue < 1.5f) minValue = 1.5f;
        for (int i = 0; i < BotCount; i++)
        {
            BotMoves += (UnityEngine.Random.Range(minValue, GameManager.Instance.playerTime / 8)).ToString();
            if (i < BotCount - 1) BotMoves += ",";
        }
        return BotMoves;
    }

    public void extractBotMoves(string data)
    {
        GameManager.Instance.botDiceValues = new List<int>();
        GameManager.Instance.botDelays = new List<float>();
        string[] d1 = data.Split(';');
        string[] diceValues = d1[0].Split(',');
        for (int i = 0; i < diceValues.Length; i++) GameManager.Instance.botDiceValues.Add(int.Parse(diceValues[i]));
        string[] delays = d1[1].Split(',');
        for (int i = 0; i < delays.Length; i++) GameManager.Instance.botDelays.Add(float.Parse(delays[i]));
    }

    public void OnLeftLobby() { isInLobby = false; isInMaster = false; }

    public IEnumerator TryToCreateGameAfterFailedToJoinRandom(RoomOptions roomOptions)
    {
        while (true)
        {
            if (isInLobby && isInMaster) { PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default); break; }
            else { yield return new WaitForSeconds(0.05f); }
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.room.CustomProperties.ContainsKey("bt")) extractBotMoves(PhotonNetwork.room.CustomProperties["bt"].ToString());
        if (PhotonNetwork.room.CustomProperties.ContainsKey("fp")) GameManager.Instance.firstPlayerInGame = int.Parse(PhotonNetwork.room.CustomProperties["fp"].ToString());
        else GameManager.Instance.firstPlayerInGame = 0;

        GameManager.Instance.avatarOpponent = null;
        GameManager.Instance.currentPlayersCount = 1;
        GameManager.Instance.controlAvatars.setCancelButton();
        if (PhotonNetwork.room.PlayerCount == 1)
        {
            GameManager.Instance.roomOwner = true;
            WaitForNewPlayer();
        }
        else if (PhotonNetwork.room.PlayerCount >= GameManager.Instance.requiredPlayers)
        {
            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;
        }

        if (!roomOwner)
        {
            GameManager.Instance.backButtonMatchPlayers.SetActive(false);
            for (int i = 0; i < PhotonNetwork.otherPlayers.Length; i++)
            {
                int ii = i;
                int index = GetFirstFreeSlot();
                GameManager.Instance.opponentsIDs[index] = PhotonNetwork.otherPlayers[ii].NickName;
                GetUserDataRequest getdatarequest = new GetUserDataRequest() { PlayFabId = PhotonNetwork.otherPlayers[ii].NickName };
                string otherID = PhotonNetwork.otherPlayers[ii].NickName;

                PlayFabClientAPI.GetUserData(getdatarequest, (result) =>
                {
                    Dictionary<string, UserDataRecord> data = result.Data;
                    bool fbAvatar = false;
                    int avatarIndex = 0;
                    if (data.ContainsKey("LoggedType") && data["LoggedType"].Value.Equals("Facebook") && data.ContainsKey(MyPlayerData.AvatarIndexKey) && data[MyPlayerData.AvatarIndexKey].Value.Equals("fb"))
                    {
                        fbAvatar = true; 
                    }
                    else if (data.ContainsKey(MyPlayerData.AvatarIndexKey) && !data[MyPlayerData.AvatarIndexKey].Value.Equals("fb"))
                    {
                        avatarIndex = int.Parse(data[MyPlayerData.AvatarIndexKey].Value.ToString());
                    }
                    getOpponentData(data, index, fbAvatar, avatarIndex, otherID);
                }, null, null);
            }
        }
    }

    public void CreatePrivateRoom()
    {
        GameManager.Instance.JoinedByID = false;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        string roomName = "";
        for (int i = 0; i < 8; i++) roomName = roomName + UnityEngine.Random.Range(0, 10);
        roomOptions.CustomRoomPropertiesForLobby = new String[] { "pc" };
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "pc", GameManager.Instance.payoutCoins} };
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        roomOwner = true;
        GameManager.Instance.roomOwner = true;
        GameManager.Instance.currentPlayersCount = 1;
        GameManager.Instance.controlAvatars.updateRoomID(PhotonNetwork.room.Name);
    }

    public override void OnLeftRoom()
    {
        roomOwner = false;
        GameManager.Instance.roomOwner = false;
        GameManager.Instance.resetAllData();
    }

    public int GetFirstFreeSlot()
    {
        int index = 0;
        for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++) { if (GameManager.Instance.opponentsIDs[i] == null) { index = i; break; } }
        return index;
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg) { CreatePrivateRoom(); }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        if (GameManager.Instance.type == MyGameType.Private) { if (GameManager.Instance.controlAvatars != null) GameManager.Instance.controlAvatars.ShowJoinFailed(codeAndMsg[1].ToString()); }
        else { GameManager.Instance.facebookManager.startRandomGame(); }
    }

    private void GetPlayerDataRequest(string playerID) { }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        CancelInvoke("StartGameWithBots");
        if (PhotonNetwork.room.PlayerCount >= GameManager.Instance.requiredPlayers) { PhotonNetwork.room.IsOpen = false; PhotonNetwork.room.IsVisible = false; }
        if (PhotonNetwork.room.PlayerCount > 1) GameManager.Instance.controlAvatars.startButtonPrivate.GetComponent<Button>().interactable = true;
        else GameManager.Instance.controlAvatars.startButtonPrivate.GetComponent<Button>().interactable = true;

        int index = GetFirstFreeSlot();
        GameManager.Instance.opponentsIDs[index] = newPlayer.NickName;
        GetUserDataRequest getdatarequest = new GetUserDataRequest() { PlayFabId = newPlayer.NickName };

        PlayFabClientAPI.GetUserData(getdatarequest, (result) =>
        {
            Dictionary<string, UserDataRecord> data = result.Data;
            bool fbAvatar = false;
            int avatarIndex = 0;
            if (data.ContainsKey("LoggedType") && data["LoggedType"].Value.Equals("Facebook") && data.ContainsKey(MyPlayerData.AvatarIndexKey) && data[MyPlayerData.AvatarIndexKey].Value.Equals("fb"))
            {
                 fbAvatar = true;
            }
            else if (data.ContainsKey(MyPlayerData.AvatarIndexKey) && !data[MyPlayerData.AvatarIndexKey].Value.Equals("fb"))
            {
                 avatarIndex = int.Parse(data[MyPlayerData.AvatarIndexKey].Value.ToString());
            }
            getOpponentData(data, index, fbAvatar, avatarIndex, newPlayer.NickName);
        }, null, null);
    }

    private void getOpponentData(Dictionary<string, UserDataRecord> data, int index, bool fbAvatar, int avatarIndex, string id)
    {
        if (data.ContainsKey("PlayerName")) GameManager.Instance.opponentsNames[index] = data["PlayerName"].Value;
        else GameManager.Instance.opponentsNames[index] = "Guest857643";

        if (data.ContainsKey("PlayerAvatarUrl") && fbAvatar)
        {
            // Fallback for removed FB SDK: Don't try to load FB url if we can't verify it, or just try anyway
            StartCoroutine(loadImageOpponent(data["PlayerAvatarUrl"].Value, index, id));
        }
        else
        {
            GameManager.Instance.opponentsAvatars[index] = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().avatars[avatarIndex];
            GameManager.Instance.controlAvatars.PlayerJoined(index, id);
        }
    }

    public IEnumerator loadImageOpponent(string url, int index, string id)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
             GameManager.Instance.opponentsAvatars[index] = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f), 32);
        }
        GameManager.Instance.controlAvatars.PlayerJoined(index, id);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class Playfabmanager : MonoBehaviour
{
    private static Playfabmanager _instance;

    public static Playfabmanager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }








    // Start is called before the first frame update
    void Start()
    {
        login();
    }

    void login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true

    };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
   void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in /creating account");
        
    }
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName="Score",
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard send");
    }
}

/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;
using PlayFab.ClientModels;
using PlayFab;
using UnityEngine.SceneManagement;
using AssemblyCSharp;

public class PlayFabAddFriend : MonoBehaviour
{

    public GameObject menuObject;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void AddFriend()
    {
        menuObject.GetComponent<Animator>().Play("hideMenuAnimation");
        if (!GameManager.Instance.offlineMode)
        {
            PhotonNetwork.RaiseEvent(192, 1, true, null);



            AddFriendRequest request = new AddFriendRequest()
            {
                FriendPlayFabId = PhotonNetwork.otherPlayers[0].name
            };



            PlayFabClientAPI.AddFriend(request, (result) =>
            {
                Debug.Log("Added friend successfully");
                GameManager.Instance.friendButtonMenu.SetActive(false);
                GameManager.Instance.smallMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(GameManager.Instance.smallMenu.GetComponent<RectTransform>().sizeDelta.x, 260.0f);
            }, (error) =>
            {
                Debug.Log("Error adding friend: " + error.Error);
            }, null);
        }

    }

    public void showMenu()
    {
        menuObject.GetComponent<Animator>().Play("ShowMenuAnimation");
    }

    public void hideMenu()
    {
        menuObject.GetComponent<Animator>().Play("hideMenuAnimation");
    }

    public void LeaveGame()
    {
        // if (StaticStrings.showAdWhenLeaveGame)
        //     AdsManager.Instance.adsScript.ShowAd();
        SceneManager.LoadScene("MenuScene");
        PhotonNetwork.BackgroundTimeout = StaticStrings.photonDisconnectTimeoutLong; ;
        Debug.Log("Timeout 3");
        //GameManager.Instance.cueController.removeOnEventCall();
        PhotonNetwork.LeaveRoom();

        GameManager.Instance.playfabManager.roomOwner = false;
        GameManager.Instance.roomOwner = false;
        GameManager.Instance.resetAllData();

    }
}

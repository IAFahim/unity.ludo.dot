/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class CheckIfPlayerIsFriend : MonoBehaviour {

    public GameObject AddFriendButton;
    public GameObject mainObject;
    // Use this for initialization
    void Start() {
        GameManager.Instance.smallMenu = mainObject;
        GameManager.Instance.friendButtonMenu = AddFriendButton;

        if (!GameManager.Instance.offlineMode) {
            GetFriendsListRequest request = new GetFriendsListRequest();
            request.IncludeFacebookFriends = true;
            PlayFabClientAPI.GetFriendsList(request, (result) => {
                var friends = result.Friends;
                foreach (var friend in friends) {
                    if (PhotonNetwork.otherPlayers[0].name.Equals(friend.FriendPlayFabId)) {
                        Debug.Log("Already friends");
                        AddFriendButton.SetActive(false);
                        mainObject.GetComponent<RectTransform>().sizeDelta = new Vector2(mainObject.GetComponent<RectTransform>().sizeDelta.x, 260.0f);
                        break;
                    }
                }
            }, OnPlayFabError);
        } else {
            AddFriendButton.SetActive(false);
            mainObject.GetComponent<RectTransform>().sizeDelta = new Vector2(mainObject.GetComponent<RectTransform>().sizeDelta.x, 260.0f);
        }

    }

    void OnPlayFabError(PlayFabError error) {
        Debug.Log("Playfab Error: " + error.ErrorMessage);
    }

    // Update is called once per frame
    void Update() {

    }
}

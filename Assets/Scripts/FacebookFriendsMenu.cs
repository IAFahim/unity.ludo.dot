using UnityEngine;
using System.Collections;
// REMOVED: using Facebook.Unity;
using System.Collections.Generic;
// REMOVED: using Facebook.MiniJSON;
using UnityEngine.UI;
using PlayFab.ClientModels;
using PlayFab;
using ExitGames.Client.Photon.Chat;
using AssemblyCSharp;

public class FacebookFriendsMenu : MonoBehaviour
{
    public GameObject list;
    public GameObject friendPrefab;
    public GameObject friendPrefab2;
    public GameObject friendsMenu;
    public GameObject mainMenu;
    public InputField filterInputField;
    private PlayFabManager playFabManager;
    public GameObject confirmDialog;
    public GameObject confirmDialogText;
    public GameObject confirmDialogButton;

    private List<GameObject> friendsObjects = new List<GameObject>();
    private Sprite[] playersAvatars;

    void Start()
    {
        playFabManager = GameManager.Instance.playfabManager;
    }

    // ... (Keep updateName, addPlayFabFriends, updateFriendStatus) ...
    
    public void updateName(int i, string text, string id)
    {
         if (friendsObjects != null && friendsObjects.Count > 0 && i <= friendsObjects.Count - 1 && friendsObjects[i] != null)
        {
            friendsObjects[i].SetActive(true);
            friendsObjects[i].transform.Find("FriendName").GetComponent<Text>().text = text;
        }
    }

    public void addPlayFabFriends(List<string> playfabIDs, List<string> playfabFBName, List<string> playfabFBID)
    {
        playersAvatars = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().avatars;
        friendsObjects = new List<GameObject>();
        friendsMenu.gameObject.SetActive(true);

        for (int i = 0; i < playfabIDs.Count; i++)
        {
            GameObject friend = Instantiate(friendPrefab2, Vector3.zero, Quaternion.identity) as GameObject;
            string name = playfabFBName[i];
            if (playfabFBName[i].Length > 13) name = playfabFBName[i].Substring(0, 12) + "...";
            friend.transform.Find("FriendName").GetComponent<Text>().text = name;

            string friendName = playfabFBName[i];
            string friendID = playfabIDs[i];

            friend.GetComponent<PlayFabFriendScript>().playfabID = friendID;
            friend.transform.Find("InviteFriendButton").GetComponent<Button>().onClick.RemoveAllListeners();
            friend.transform.Find("DeleteFriend").GetComponent<Button>().onClick.RemoveAllListeners();
            friend.transform.Find("InviteFriendButton").GetComponent<Button>().onClick.AddListener(() => ChallengeFriend(friendID));
            friend.transform.Find("DeleteFriend").GetComponent<Button>().onClick.AddListener(() => RemoveFriend(friendID, friendName, friend));

            getFriendImageUrl(friendID, friend.transform.Find("Avatar/FriendAvatar").GetComponent<Image>(), friend.transform.Find("Avatar/FriendAvatar").gameObject);

            friend.transform.parent = list.transform;
            friend.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            friendsObjects.Add(friend);

            if (playfabFBName[i].Length < 1) friendsObjects[i].SetActive(false);
        }
    }

    public void updateFriendStatus(int status, string id)
    {
        foreach (GameObject friend in friendsObjects)
        {
            if (friend.GetComponent<PlayFabFriendScript>().playfabID.Equals(id))
            {
                if (status == ChatUserStatus.Online)
                {
                    friend.GetComponent<PlayFabFriendScript>().statusIndicatorText.GetComponent<Text>().text = "Online";
                    friend.GetComponent<PlayFabFriendScript>().statusIndicator.GetComponent<Image>().color = Color.green;
                }
                else if (status == ChatUserStatus.Offline)
                {
                    friend.GetComponent<PlayFabFriendScript>().statusIndicatorText.GetComponent<Text>().text = "Offline";
                    friend.GetComponent<PlayFabFriendScript>().statusIndicator.GetComponent<Image>().color = Color.red;
                }
            }
        }
    }

    public void getFriendImageUrl(string id, Image image, GameObject imobject)
    {
        GetUserDataRequest getdatarequest = new GetUserDataRequest() { PlayFabId = id };

        PlayFabClientAPI.GetUserData(getdatarequest, (result) =>
        {
            Dictionary<string, UserDataRecord> data = result.Data;
            imobject.SetActive(true);
            if (data[MyPlayerData.AvatarIndexKey].Value.Equals("fb"))
            {
                if (data.ContainsKey("PlayerAvatarUrl"))
                    filterInputField.GetComponent<MonoBehaviour>().StartCoroutine(loadImage(data["PlayerAvatarUrl"].Value, image));
            }
            else
            {
                image.sprite = playersAvatars[int.Parse(data[MyPlayerData.AvatarIndexKey].Value)];
            }
        }, null, null);
    }

    public void showFriends(List<string> friendsNames, List<string> friendsIDs, List<string> friendsAvatars)
    {
        friendsMenu.gameObject.SetActive(true);
        if (friendsNames != null)
        {
            for (int i = 0; i < friendsNames.Count; i++)
            {
                GameObject friend = Instantiate(friendPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                string name = friendsNames[i];
                if (friendsNames[i].Length > 13) name = friendsNames[i].Substring(0, 12) + "...";
                friend.transform.Find("FriendName").GetComponent<Text>().text = name;

                string friendID = friendsIDs[i];
                friend.transform.Find("InviteFriendButton").GetComponent<Button>().onClick.RemoveAllListeners();
                friend.transform.Find("InviteFriendButton").GetComponent<Button>().onClick.AddListener(() => InviteFriend(friendID));
                friend.GetComponent<MonoBehaviour>().StartCoroutine(loadImage(friendsAvatars[i], friend.transform.Find("Avatar/FriendAvatar").GetComponent<Image>()));
                friend.transform.parent = list.transform;
                friend.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                friendsObjects.Add(friend);
                
                for (int j = 0; j < GameManager.Instance.friendsStatuses.Count; j++)
                {
                    string[] friend1 = GameManager.Instance.friendsStatuses[j];
                    if (friend1[0].Equals(friendID))
                    {
                        if (friend1[1].Equals("" + ChatUserStatus.Online))
                            GameManager.Instance.facebookFriendsMenu.updateFriendStatus(ChatUserStatus.Online, friendID);
                        break;
                    }
                }
            }
        }
    }

    public void InviteFriend(string i)
    {
        Debug.Log("Invite friend FB removed: " + i);
        // Immediately trigger reward as if invite was sent
        GameManager.Instance.playfabManager.addCoinsRequest(StaticStrings.rewardCoinsForFriendInvite);
    }

    // ... (Keep RemoveFriend, ChallengeFriend, loadImage, hideFriends, FilterFriends) ...

    public void RemoveFriend(string id, string name, GameObject friend)
    {
        confirmDialog.SetActive(true);
        confirmDialogText.GetComponent<Text>().text = "Remove " + friend.transform.Find("FriendName").GetComponent<Text>().text + " from your friends?";
        string friendID = id;
        confirmDialogButton.GetComponent<Button>().onClick.RemoveAllListeners();
        confirmDialogButton.GetComponent<Button>().onClick.AddListener(() => removeFriendRequest(friendID, friend));
    }

    public void removeFriendRequest(string id, GameObject friend)
    {
        RemoveFriendRequest request = new RemoveFriendRequest() { FriendPlayFabId = id };
        PlayFabClientAPI.RemoveFriend(request, (result) => { friend.SetActive(false); }, null, null);
    }

    public void hideFriends()
    {
        filterInputField.text = "";
        foreach (GameObject o in friendsObjects) Destroy(o);
        friendsMenu.gameObject.SetActive(false);
    }

    public void FilterFriends()
    {
        string search = filterInputField.text;
        for (int i = 0; i < friendsObjects.Count; i++)
        {
            if (friendsObjects[i].transform.Find("FriendName").GetComponent<Text>().text.Length > 0) friendsObjects[i].SetActive(true);
            if (!friendsObjects[i].transform.Find("FriendName").GetComponent<Text>().text.ToLower().Contains(search.ToLower()))
                friendsObjects[i].SetActive(false);
        }
    }

    public void ChallengeFriend(string id)
    {
        GameManager.Instance.facebookFriendsMenu.hideFriends();
        GameManager.Instance.playfabManager.challengeFriend(id, GameManager.Instance.payoutCoins + ";" + GameManager.Instance.privateRoomID);
    }

    // Removed loadImageFBID

    public IEnumerator loadImage(string url, Image image)
    {
        WWW www = new WWW(url);
        yield return www;
        image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f), 32);
    }
}
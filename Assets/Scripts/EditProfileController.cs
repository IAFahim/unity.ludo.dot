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
using UnityEngine;
using UnityEngine.UI;

public class EditProfileController : MonoBehaviour
{

    public GameObject changeName;
    public GameObject gridView;
    public GameObject buttonPrefab;

    private string avatarIndex;

    public GameObject PlayerNameMain;
    public GameObject PlayerAvatarMain;

    private StaticGameVariablesController staticController;

    private List<GameObject> buttons = new List<GameObject>();
    // Use this for initialization
    void Start()
    {

        avatarIndex = GameManager.Instance.myPlayerData.GetAvatarIndex();

        staticController = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>();
        changeName.GetComponent<InputField>().text = GameManager.Instance.nameMy;

        if (GameManager.Instance.facebookAvatar != null)
        {
            GameObject button = Instantiate(buttonPrefab);
            button.GetComponent<ProfilePictureController>().picture.GetComponent<Image>().sprite = GameManager.Instance.facebookAvatar;
            button.transform.SetParent(gridView.transform, false);

            GameObject border = button.GetComponent<ProfilePictureController>().frame;
            if (GameManager.Instance.myPlayerData.GetAvatarIndex().Equals("fb"))
            {
                border.GetComponent<Image>().color = Color.green;
            }

            string index = "fb";
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() => ClickButton(index, border));

            buttons.Add(border);
        }



        for (int i = 0; i < staticController.avatars.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            button.GetComponent<ProfilePictureController>().picture.GetComponent<Image>().sprite = staticController.avatars[i];
            button.transform.SetParent(gridView.transform, false);

            GameObject border = button.GetComponent<ProfilePictureController>().frame;
            if (GameManager.Instance.myPlayerData.GetAvatarIndex().Equals(i + ""))
            {
                border.GetComponent<Image>().color = Color.green;
            }

            string index = "" + i;
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() => ClickButton(index, border));

            buttons.Add(border);
        }
    }

    public void ClickButton(string avatarIndex, GameObject border)
    {
        this.avatarIndex = avatarIndex;

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponent<Image>().color = Color.white;

        }
        border.GetComponent<Image>().color = Color.green;
    }

    public void Save()
    {


        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add(MyPlayerData.AvatarIndexKey, avatarIndex);
        data.Add(MyPlayerData.PlayerName, changeName.GetComponent<InputField>().text);

        GameManager.Instance.myPlayerData.UpdateUserData(data);


        PlayerNameMain.GetComponent<Text>().text = changeName.GetComponent<InputField>().text;
        GameManager.Instance.nameMy = changeName.GetComponent<InputField>().text;


        if (avatarIndex.Equals("fb"))
        {
            PlayerAvatarMain.GetComponent<Image>().sprite = GameManager.Instance.facebookAvatar;
            GameManager.Instance.avatarMy = GameManager.Instance.facebookAvatar;
        }
        else
        {
            PlayerAvatarMain.GetComponent<Image>().sprite = staticController.avatars[int.Parse(avatarIndex)];
            GameManager.Instance.avatarMy = staticController.avatars[int.Parse(avatarIndex)];
        }



        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

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

public class PlayerAvatarController : MonoBehaviour
{

    public GameObject Name;
    public GameObject Avatar;
    public GameObject Timer;
    public GameObject leftRoomObject;
    public GameObject MainObject;
    public GameObject Crown;
    public GameObject Position;
    public Sprite[] PositionSprites;

    [HideInInspector]
    public bool Active = true;
    [HideInInspector]
    public bool finished = false;
    public AudioSource PlayerLeftRoomAudio;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerLeftRoom()
    {
        if (!finished)
        {
            PlayerLeftRoomAudio.Play();
            Active = false;
            Name.GetComponent<Text>().text = "";
            MainObject.transform.localScale = new Vector2(0.8f, 0.8f);
            leftRoomObject.SetActive(true);
        }
    }

    public void PlayerFinishedGame()
    {

    }

    public void setPositionSprite(int index)
    {
        Position.SetActive(true);
        Position.GetComponent<Image>().sprite = PositionSprites[index - 1];
    }
}

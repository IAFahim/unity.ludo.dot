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
using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;

public class GameDiceController : MonoBehaviour
{

    public Sprite[] diceValueSprites;
    public GameObject arrowObject;
    public GameObject diceValueObject;
    public GameObject diceAnim;

    // Use this for initialization
    public bool isMyDice = false;
    public GameObject LudoController;
    public LudoGameController controller;
    public int player = 1;
    private Button button;

    public GameObject notInteractable;

    private int steps = 0;
    void Start()
    {
        button = GetComponent<Button>();
        controller = LudoController.GetComponent<LudoGameController>();

        button.interactable = false;
    }

    public void SetDiceValue()
    {
        Debug.Log("Set dice value called");
        diceValueObject.GetComponent<Image>().sprite = diceValueSprites[steps - 1];
        diceValueObject.SetActive(true);
        diceAnim.SetActive(false);
        controller.gUIController.restartTimer();
        if (isMyDice)
            controller.HighlightPawnsToMove(player, steps);
        if (GameManager.Instance.currentPlayer.isBot)
        {
            controller.HighlightPawnsToMove(player, steps);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableShot()
    {
        if (GameManager.Instance.currentPlayer.isBot)
        {
            GameManager.Instance.miniGame.BotTurn(false);
            notInteractable.SetActive(false);
        }
        else
        {
            if (PlayerPrefs.GetInt(StaticStrings.VibrationsKey, 0) == 0)
            {
                Debug.Log("Vibrate");
#if UNITY_ANDROID || UNITY_IOS
                Handheld.Vibrate();
#endif
            }
            else
            {
                Debug.Log("Vibrations OFF");
            }
            controller.gUIController.myTurnSource.Play();
            notInteractable.SetActive(false);
            button.interactable = true;
            arrowObject.SetActive(true);
        }
    }

    public void DisableShot()
    {
        notInteractable.SetActive(true);
        button.interactable = false;
        arrowObject.SetActive(false);
    }

    public void EnableDiceShadow()
    {
        notInteractable.SetActive(true);
    }

    public void DisableDiceShadow()
    {
        notInteractable.SetActive(false);
    }
    int aa = 0;
    int bb = 0;
    public void RollDice()
    {
        if (isMyDice)
        {

            controller.nextShotPossible = false;
            controller.gUIController.PauseTimers();
            button.interactable = false;
            Debug.Log("Roll Dice");
            arrowObject.SetActive(false);
            // if (aa % 2 == 0) steps = 6;
            // else steps = 2;
            // aa++;
            steps = Random.Range(1, 7);

            RollDiceStart(steps);
            string data = steps + ";" + controller.gUIController.GetCurrentPlayerIndex();
            PhotonNetwork.RaiseEvent((int)EnumGame.DiceRoll, data, true, null);

            Debug.Log("Value: " + steps);
        }
    }

    public void RollDiceBot(int value)
    {

        controller.nextShotPossible = false;
        controller.gUIController.PauseTimers();

        Debug.Log("Roll Dice bot");

        // if (bb % 2 == 0) steps = 6;
        // else steps = 2;
        // bb++;

        steps = value;

        RollDiceStart(steps);


    }

    public void RollDiceStart(int steps)
    {
        GetComponent<AudioSource>().Play();
        this.steps = steps;
        diceValueObject.SetActive(false);
        diceAnim.SetActive(true);
        diceAnim.GetComponent<Animator>().Play("RollDiceAnimation");
    }
}

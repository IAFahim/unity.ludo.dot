using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;
using System;

public class Demo : MonoBehaviour
{
    [SerializeField] private Button uiSpinButton;
    [SerializeField] private Button uiSpinButtonWatchad;
    [SerializeField] private Text uiSpinButtonText;

    [SerializeField] private PickerWheel pickerWheel;

    private void Start()
    {
        // Free spin button
        uiSpinButton.onClick.AddListener(() =>
        {
            uiSpinButton.interactable = false;
            uiSpinButtonWatchad.interactable = false;
            uiSpinButtonText.text = "Spinning";

            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                Debug.Log(
                   @" <b>Index:</b> " + wheelPiece.Index + "           <b>Label:</b> " + wheelPiece.Label
                   + "\n <b>Amount:</b> " + wheelPiece.Amount + "      <b>Chance:</b> " + wheelPiece.Chance + "%"
                );

                AddCoinsBasedOnIndex(wheelPiece.Index);

                // Mark that user used their daily free spin
                PlayerPrefs.SetString("LastFreeSpinDate", DateTime.Now.ToString("yyyy-MM-dd"));
                PlayerPrefs.Save();

                uiSpinButtonText.text = "Spin";
                UpdateSpinButtons();
            });

            pickerWheel.Spin();
        });

        UpdateSpinButtons();
    }

    private void Update()
    {
        UpdateSpinButtons();
    }

    private void UpdateSpinButtons()
    {
        string lastSpinDate = PlayerPrefs.GetString("LastFreeSpinDate", "");
        string todayDate = DateTime.Now.ToString("yyyy-MM-dd");

        // If user hasn't spun today, show free spin button
        if (lastSpinDate != todayDate)
        {
            uiSpinButton.gameObject.SetActive(true);
            uiSpinButtonWatchad.gameObject.SetActive(false);
        }
        else
        {
            // User already spun today, show ad button
            uiSpinButton.gameObject.SetActive(false);
            uiSpinButtonWatchad.gameObject.SetActive(true);
        }
    }

    public void watchad()
    {
        // Disable button to prevent multiple clicks
        uiSpinButtonWatchad.interactable = false;

        // Show rewarded ad
        Gley.MobileAds.API.ShowRewardedVideo(CompleteMethod);
    }

    private void CompleteMethod(bool completed)
    {
        if (completed)
        {
            uiSpinButtonText.text = "Spinning";

            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                AddCoinsBasedOnIndex(wheelPiece.Index);

                // Re-enable ad button after spin completes
                uiSpinButtonWatchad.interactable = true;
                uiSpinButtonText.text = "Spin";
            });

            pickerWheel.Spin();
        }
        else
        {
            // Ad not completed, re-enable button
            uiSpinButtonWatchad.interactable = true;
            Debug.Log("Ad not completed — no spin");
        }
    }

    private void AddCoinsBasedOnIndex(int index)
    {
        int coinsToAdd = 0;
        
        switch (index)
        {
            case 0: coinsToAdd = 100; break;
            case 1: coinsToAdd = 200; break;
            case 2: coinsToAdd = 300; break;
            case 3: coinsToAdd = 400; break;
            case 4: coinsToAdd = 500; break;
        }

        if (coinsToAdd > 0)
        {
            GameManager.Instance.playfabManager.addCoinsRequest(coinsToAdd);
        }
    }
}
using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;
using System.Drawing;

public class Demo : MonoBehaviour
{
    [SerializeField] private Button uiSpinButton;
    [SerializeField] private Button uiSpinButtonWatchad;
    [SerializeField] private Text uiSpinButtonText;

    [SerializeField] private PickerWheel pickerWheel;

    private void Start()
    {
        uiSpinButton.onClick.AddListener(() =>
        {
            uiSpinButton.interactable = false;
            uiSpinButtonWatchad.interactable = false; // ✅ Disable WatchAd during spin
            uiSpinButtonText.text = "Spinning";

            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                Debug.Log(
                   @" <b>Index:</b> " + wheelPiece.Index + "           <b>Label:</b> " + wheelPiece.Label
                   + "\n <b>Amount:</b> " + wheelPiece.Amount + "      <b>Chance:</b> " + wheelPiece.Chance + "%"
                );

                if (wheelPiece.Index == 0)
                {
                    GameManager.Instance.playfabManager.addCoinsRequest(100);
                }
                if (wheelPiece.Index == 1)
                {
                    GameManager.Instance.playfabManager.addCoinsRequest(200);
                }
                if (wheelPiece.Index == 2)
                {
                    GameManager.Instance.playfabManager.addCoinsRequest(300);
                }
                if (wheelPiece.Index == 3)
                {
                    GameManager.Instance.playfabManager.addCoinsRequest(400);
                }
                if (wheelPiece.Index == 4)
                {
                    GameManager.Instance.playfabManager.addCoinsRequest(500);
                }

                uiSpinButton.interactable = true;
                uiSpinButtonText.text = "Spin";

                // ✅ After spin ends, enable WatchAd button again
                uiSpinButtonWatchad.interactable = true;

                // ✅ Save that user already did first spin
                PlayerPrefs.SetInt("myspin", 1);
                PlayerPrefs.Save();
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
        if (PlayerPrefs.GetInt("myspin", 0) == 0)
        {
            uiSpinButton.gameObject.SetActive(true);
            uiSpinButtonWatchad.gameObject.SetActive(false);
        }
        else
        {
            uiSpinButton.gameObject.SetActive(false);
            uiSpinButtonWatchad.gameObject.SetActive(true);
        }
    }

    public void watchad()
    {
        // ✅ Disable button instantly to prevent multiple clicks
        uiSpinButtonWatchad.interactable = false;

        // Show rewarded ad and call CompleteMethod when finished
        Gley.MobileAds.API.ShowRewardedVideo(CompleteMethod);
    }

    private void CompleteMethod(bool completed)
    {
        if (completed)
        {
            uiSpinButtonText.text = "Spinning";

            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                if (wheelPiece.Index == 0)
                {
                    GameManager.Instance.playfabManager.addCoinsRequest(100);
                }
                if (wheelPiece.Index == 1)
                {
                    GameManager.Instance.playfabManager.addCoinsRequest(200);
                }
                if (wheelPiece.Index == 2)
                {
                    GameManager.Instance.playfabManager.addCoinsRequest(300);
                }
                if (wheelPiece.Index == 3)
                {
                    GameManager.Instance.playfabManager.addCoinsRequest(400);
                }
                if (wheelPiece.Index == 4)
                {
                    GameManager.Instance.playfabManager.addCoinsRequest(500);
                }

                // ✅ Re-enable WatchAd button after spin completes
                uiSpinButtonWatchad.interactable = true;
                uiSpinButtonText.text = "Spin";
            });

            pickerWheel.Spin();
        }
        else
        {
            // Ad not completed → re-enable button so user can retry
            uiSpinButtonWatchad.interactable = true;
            Debug.Log("Ad not completed — no spin");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardController : MonoBehaviour
{
    [Header("Here You Put Timer Text.")]
    public Text RemainTime;

    [Header("Here You Put Playerpref Id Of Your Coins Or Reward According TO Your Game.")]

    public string Day1_Key;
    public string Day2_Key;
    public string Day3_Key;
    public string Day4_Key;
    public string Day5_Key;
    public string Day6_Key;
    public string Day7_Key;

    [Header("Here You Put Reward Amount Of Your Coins Or Reward As You Want.")]
    public int Day1_Reward_Amount;
    public int Day2_Reward_Amount;
    public int Day3_Reward_Amount;
    public int Day4_Reward_Amount;
    public int Day5_Reward_Amount;
    public int Day6_Reward_Amount;
    public int Day7_Reward_Amount;

  
    void Start()
    {
        GleyDailyRewards.Calendar.AddClickListener(CalendarButtonClicked);
        GleyDailyRewards.Calendar.SetValueFormatter(FormatValue);


       
        if (PlayerPrefs.GetInt("RewardPanel", 0) == 0)
        {
            ShowCalendar();
        }


    }
    private void Update()
    {
        {
            RemainTime.text = GleyDailyRewards.CalendarManager.Instance.GetRemainingTime();
        }
        if (GleyDailyRewards.CalendarManager.Instance.TimeExpired())
        {
            RemainTime.text = "Claim";
        }

    }

    private string FormatValue(int aValue)
    {
        string formattedText = aValue.ToString();

        int db = 0;
        for (int i = aValue.ToString().Length; i > 1; i--)
        {
            db++;
            if (db % 3 == 0)
            {
                formattedText = formattedText.Insert(i - 1, ".");
            }
        }

        return formattedText;
    }
    private void CalendarButtonClicked(int dayNumber, int rewardValue, Sprite rewardSprite)
    {
       
        switch(dayNumber)
        {
            case 1:
                Debug.LogError("HamidDay1");
                rewardValue = Day1_Reward_Amount;
              //  PlayerPrefs.SetInt(Day1_Key, PlayerPrefs.GetInt(Day1_Key, 0) + rewardValue);
                GameManager.Instance.playfabManager.addCoinsRequest(rewardValue);
                break;
            case 2:
                Debug.LogError("HamidDay2");
                rewardValue = Day2_Reward_Amount;
                GameManager.Instance.playfabManager.addCoinsRequest(rewardValue);
               // PlayerPrefs.SetInt(Day2_Key, PlayerPrefs.GetInt(Day2_Key, 0) + rewardValue);
                break;
            case 3:
                Debug.LogError("HamidDay3");
                rewardValue = Day3_Reward_Amount;
               // PlayerPrefs.SetInt(Day3_Key, PlayerPrefs.GetInt(Day3_Key, 0) + rewardValue);
                break;
            case 4:
                Debug.LogError("HamidDay4");
                rewardValue = Day4_Reward_Amount;
                GameManager.Instance.playfabManager.addCoinsRequest(rewardValue);
                break;
            case 5:
                Debug.LogError("HamidDay5");
                rewardValue = Day5_Reward_Amount;
                GameManager.Instance.playfabManager.addCoinsRequest(rewardValue);
                break;
            case 6:
                Debug.LogError("HamidDay6");
                rewardValue = Day6_Reward_Amount;
                GameManager.Instance.playfabManager.addCoinsRequest(rewardValue);
                break;
            case 7:
                Debug.LogError("HamidDay7");
                rewardValue = Day7_Reward_Amount;
                GameManager.Instance.playfabManager.addCoinsRequest(rewardValue);
                //  PlayerPrefs.SetInt(Day7_Key, PlayerPrefs.GetInt(Day7_Key, 0) + rewardValue);
                ResetCalendar();
                break;
        }

       
    }
    public void ShowCalendar()
    {
        //call this method anywhere in your code to open the Calendar Popup
        GleyDailyRewards.Calendar.Show();
        PlayerPrefs.SetInt("RewardPanel", 1);
    }

    public void ResetCalendar()
    {
        GleyDailyRewards.Calendar.Reset();
    }
  
}

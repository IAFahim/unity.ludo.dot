using UnityEngine;
using UnityEngine.UI;

public class CalendarExample : MonoBehaviour
{
    public Text UIRewardText;
    int reward;

    void Start()
    {
        //uncomment this to clear your save
        //PlayerPrefs.DeleteAll();

        //You can add this listener anywhere in your code and your method will be called every time a Day Button is clicked
        GleyDailyRewards.Calendar.AddClickListener(CalendarButtonClicked);
        GleyDailyRewards.Calendar.SetValueFormatter(FormatValue);
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

    /// <summary>
    /// Triggered every time a day button is clicked
    /// </summary>
    /// <param name="dayNumber">current clicked day</param>
    /// <param name="rewardValue">the reward value for current day</param>
    /// <param name="rewardSprite">the sprite of the reward</param>
    private void CalendarButtonClicked(int dayNumber, int rewardValue, Sprite rewardSprite)
    {

        switch (dayNumber)
        {
            case 1:
                Debug.LogError("HamidDay1");
                rewardValue = 2000;
                break;
            case 2:
                Debug.LogError("HamidDay1");
                rewardValue = 3000;
                break;
            case 3:
                Debug.LogError("HamidDay1");
                rewardValue = 4000;
                break;
            case 4:
                Debug.LogError("HamidDay1");
                rewardValue = 5000;
                break;
            case 5:
                Debug.LogError("HamidDay1");
                rewardValue = 6000;
                break;
            case 6:
                Debug.LogError("HamidDay1");
                rewardValue = 7000;
                break;
            case 7:
                Debug.LogError("HamidDay1");
                rewardValue = 8000;
                ResetCalendar();
                break;
        }

        Debug.Log("Click " + dayNumber + " " + rewardValue);
        // reward += rewardValue;
        UIRewardText.text = reward.ToString();
    }


    public void ShowCalendar()
    {
        //call this method anywhere in your code to open the Calendar Popup
        GleyDailyRewards.Calendar.Show();
    }

    public void ResetCalendar()
    {
        GleyDailyRewards.Calendar.Reset();
    }
}





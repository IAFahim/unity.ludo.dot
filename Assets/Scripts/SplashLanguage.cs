//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class SplashLanguage : MonoBehaviour
//{
//    public Text Loginwithfacebook;
//    public Text GuestMode;
//    public Text ChooseLanguage;

//    void Start()
//    {
//        RefreshTexts();
//    }
//    void RefreshTexts()
//    {
//        Loginwithfacebook.text = GleyLocalization.Manager.GetText("loginID");
//        GuestMode.text = GleyLocalization.Manager.GetText("GuestID");
//        ChooseLanguage.text = GleyLocalization.Manager.GetText("Chlanguage");

//    }
//    public void NextLanguage()
//    {
//        GleyLocalization.Manager.NextLanguage();
//        RefreshTexts();
//        SaveLanguage();
//    }
//    public void PrevLanguage()
//    {
//        GleyLocalization.Manager.PreviousLanguage();
//        RefreshTexts();
//        SaveLanguage();
//    }
//    public void SaveLanguage()
//    {
//        GleyLocalization.Manager.SetCurrentLanguage(GleyLocalization.Manager.GetCurrentLanguage());
//    }
//}

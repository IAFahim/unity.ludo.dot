//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Menulanguage : MonoBehaviour
//{
//    public Text player;
//    public Text Free;
//    public Text Oneplayer;
//    public Text fourplayer;
//    public Text Privatemode;
//    public Text store;
//    public Text switchaccount;
//    public Text linkfacebook;
//    public Text store2;
//    public Text buy;
//    public Text buy1;
//    public Text buy2;
//    public Text buy3;
//    public Text buy4;
//    public Text close;
//    public Text notcoins;
//    public Text okay;
//    public Text playerdetails;
//    public Text player1;
//    public Text totalearn;
//    public Text currentmoney;
//    public Text statics;
//    public Text gameplayed;
//    public Text gamewon;
//    public Text winrate;
//    public Text player2win;
//    public Text player4win;
//    public Text close1;
//    public Text EditProfile;
//    public Text Editname;
//    public Text ChosseProfilepicture;
//    public Text save;
//    public Text cancel;
//    public Text notcoins1;
//    public Text watchvideo;
//    public Text watchtitle;
//    public Text watch;
//    public Text cancel1;
//    public Text setting;
//    public Text sound;
//    public Text notification;
//    public Text vibration;
//    public Text friendrequest;
//    public Text privateroomrequest;
//    public Text close2;
//    public Text rate;
//    public Text title;
//    public Text ratebtn;
//    public Text cancel2;
//    public Text switchaccount1;
//    public Text switchtitles;
//    public Text switchbtn;
//    public Text cancel3;
//    public Text privateroom;
//    public Text gamemode;
//    public Text classic;
//    public Text quick;
//    public Text master;
//    public Text choosebet;
//    public Text play;
//    public Text cancel4;
//    public Text Join;
//    public Text RoomCode;
//    public Text Classic2;
//    public Text classicinfo;
//    public Text close3;
//    public Text master1;
//    public Text masterinfo;
//    public Text close4;
//    public Text quick1;
//    public Text quickinfo;
//    public Text close5;

//    void Start()
//    {
//        RefreshTexts();
//        GleyLocalization.Manager.GetCurrentLanguage();
//    }
//    //BuyID
//    //LinkID
//    // storeid
//    //NotmonId
//    //okaId
//    void RefreshTexts()
//    {
//        player.text = GleyLocalization.Manager.GetText("InfoID");
//        Free.text = GleyLocalization.Manager.GetText("FreeID");
//        Oneplayer.text = GleyLocalization.Manager.GetText("mode1");
//        fourplayer.text = GleyLocalization.Manager.GetText("mode2");
//        Privatemode.text = GleyLocalization.Manager.GetText("mode3");
//        store.text = GleyLocalization.Manager.GetText("storeid");
//        switchaccount.text = GleyLocalization.Manager.GetText("switchId");
//        linkfacebook.text = GleyLocalization.Manager.GetText("LinkID");
//        store2.text = GleyLocalization.Manager.GetText("storeid");
//        buy.text = GleyLocalization.Manager.GetText("BuyID");
//        buy1.text = GleyLocalization.Manager.GetText("BuyID");
//        buy2.text = GleyLocalization.Manager.GetText("BuyID");
//        buy3.text = GleyLocalization.Manager.GetText("BuyID");
//        buy4.text = GleyLocalization.Manager.GetText("BuyID");
//        close.text = GleyLocalization.Manager.GetText("closeid");
//        notcoins.text = GleyLocalization.Manager.GetText("NotmonId");
//        notcoins1.text = GleyLocalization.Manager.GetText("NotmonId");
//        okay.text = GleyLocalization.Manager.GetText("okaId");
//        playerdetails.text = GleyLocalization.Manager.GetText("pdeID");
//        player1.text = GleyLocalization.Manager.GetText("InfoID");
//        totalearn.text = GleyLocalization.Manager.GetText("TEarnID");
//        currentmoney.text = GleyLocalization.Manager.GetText("CEarnId");
//        statics.text = GleyLocalization.Manager.GetText("StateID");
//        gameplayed.text = GleyLocalization.Manager.GetText("gameP");
//        gamewon.text = GleyLocalization.Manager.GetText("gamewonID");
//        winrate.text = GleyLocalization.Manager.GetText("WinID");
//        player2win.text = GleyLocalization.Manager.GetText("player2wnID");
//        player4win.text = GleyLocalization.Manager.GetText("player4winid");
//        close1.text = GleyLocalization.Manager.GetText("closeid");
//        EditProfile.text = GleyLocalization.Manager.GetText("EditID");
//        Editname.text = GleyLocalization.Manager.GetText("EditnameID");
//        ChosseProfilepicture.text = GleyLocalization.Manager.GetText("ChoosepicID");
//        save.text = GleyLocalization.Manager.GetText("saveId");
//        cancel.text = GleyLocalization.Manager.GetText("CancelId");
//        watchvideo.text = GleyLocalization.Manager.GetText("WatVideoID");
//        watchtitle.text = GleyLocalization.Manager.GetText("watchtitleID");
//        watch.text = GleyLocalization.Manager.GetText("watchidbtn");
//        cancel1.text = GleyLocalization.Manager.GetText("CancelId");
//        setting.text = GleyLocalization.Manager.GetText("stngID");
//        sound.text = GleyLocalization.Manager.GetText("sndid");
//        notification.text = GleyLocalization.Manager.GetText("notID");
//        vibration.text = GleyLocalization.Manager.GetText("vibId");
//        friendrequest.text = GleyLocalization.Manager.GetText("friendrequestId");
//        privateroomrequest.text = GleyLocalization.Manager.GetText("PrivateroomrequestId");
//        close2.text = GleyLocalization.Manager.GetText("closeid");
//        rate.text = GleyLocalization.Manager.GetText("pleaserateid");
//        title.text = GleyLocalization.Manager.GetText("RateTitle");
//        ratebtn.text = GleyLocalization.Manager.GetText("RateID");
//        cancel2.text = GleyLocalization.Manager.GetText("CancelId");
//        switchaccount1.text = GleyLocalization.Manager.GetText("switchId");
//        switchtitles.text = GleyLocalization.Manager.GetText("SwitTitleID");
//        switchbtn.text = GleyLocalization.Manager.GetText("okaId");
//        cancel3.text = GleyLocalization.Manager.GetText("CancelId");
//        privateroom.text = GleyLocalization.Manager.GetText("mode3");
//        gamemode.text = GleyLocalization.Manager.GetText("gamemodeID");
//        classic.text = GleyLocalization.Manager.GetText("classicid");
//        quick.text = GleyLocalization.Manager.GetText("quickId");
//        master.text = GleyLocalization.Manager.GetText("masterid");
//        choosebet.text = GleyLocalization.Manager.GetText("choostbetid");
//        play.text = GleyLocalization.Manager.GetText("playid");
//        cancel3.text = GleyLocalization.Manager.GetText("CancelId");
//        Join.text = GleyLocalization.Manager.GetText("JoinID");
//        RoomCode.text = GleyLocalization.Manager.GetText("RoomCode");
//        Classic2.text = GleyLocalization.Manager.GetText("classicid");
//        classicinfo.text = GleyLocalization.Manager.GetText("InfoClassic");
//        close3.text = GleyLocalization.Manager.GetText("closeid");
//        master1.text = GleyLocalization.Manager.GetText("masterid");
//        masterinfo.text = GleyLocalization.Manager.GetText("masterinfoID");
//        close4.text = GleyLocalization.Manager.GetText("closeid");
//        quick1.text = GleyLocalization.Manager.GetText("quickId");
//        quickinfo.text = GleyLocalization.Manager.GetText("quickinfo");
//        close5.text = GleyLocalization.Manager.GetText("closeid");





//        // GuestMode.text = GleyLocalization.Manager.GetText("GuestID");


//    }
//}

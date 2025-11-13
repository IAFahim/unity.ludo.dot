/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

public class Enums
{

}


public enum AdLocation
{
    GameStart,
    GameOver,
    LevelComplete,
    Pause,
    FacebookFriends,
    GameFinishWindow,
    StoreWindow,
    GamePropertiesWindow

};

public enum MyGameType
{
    TwoPlayer, FourPlayer, Private
};

public enum MyGameMode
{
    Classic, Master, Quick
}

public enum EnumPhoton
{
    ReadyToPlay = 179,
    BeginPrivateGame = 171,
    NextPlayerTurn = 172,
    StartWithBots = 173,
    StartGame = 174,
    SendChatMessage = 175,
    SendChatEmojiMessage = 176,
    AddFriend = 177,
    FinishedGame = 178,
}

public enum EnumGame
{
    DiceRoll = 50,
    PawnMove = 51,
    PawnRemove = 52,
}

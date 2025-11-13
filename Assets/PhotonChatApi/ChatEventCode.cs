/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// ----------------------------------------------------------------------------------------------------------------------
// <summary>The Photon Chat Api enables clients to connect to a chat server and communicate with other clients.</summary>
// <remarks>ChatClient is the main class of this api.</remarks>
// <copyright company="Exit Games GmbH">Photon Chat Api - Copyright (C) 2014 Exit Games GmbH</copyright>
// ----------------------------------------------------------------------------------------------------------------------

namespace ExitGames.Client.Photon.Chat
{
    /// <summary>
    /// Wraps up internally used constants in Photon Chat events. You don't have to use them directly usually.
    /// </summary>
    public class ChatEventCode
    {
        public const byte ChatMessages = 0;
        public const byte Users = 1;// List of users or List of changes for List of users
        public const byte PrivateMessage = 2;
        public const byte FriendsList = 3;
        public const byte StatusUpdate = 4;
        public const byte Subscribe = 5;
        public const byte Unsubscribe = 6;
    }
}

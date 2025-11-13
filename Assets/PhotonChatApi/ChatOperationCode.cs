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
    /// Wraps up codes for operations used internally in Photon Chat. You don't have to use them directly usually.
    /// </summary>
    public class ChatOperationCode
    {
        /// <summary>(230) Operation Authenticate.</summary>
        public const byte Authenticate = 230;

        /// <summary>(0) Operation to subscribe to chat channels.</summary>
        public const byte Subscribe = 0;
        /// <summary>(1) Operation to unsubscribe from chat channels.</summary>
        public const byte Unsubscribe = 1;
        /// <summary>(2) Operation to publish a message in a chat channel.</summary>
        public const byte Publish = 2;
        /// <summary>(3) Operation to send a private message to some other user.</summary>
        public const byte SendPrivate = 3;

        /// <summary>(4) Not used yet.</summary>
        public const byte ChannelHistory = 4;

        /// <summary>(5) Set your (client's) status.</summary>
        public const byte UpdateStatus = 5;
        /// <summary>(6) Add friends the list of friends that should update you of their status.</summary>
        public const byte AddFriends = 6;
        /// <summary>(7) Remove friends from list of friends that should update you of their status.</summary>
        public const byte RemoveFriends = 7;
    }
}

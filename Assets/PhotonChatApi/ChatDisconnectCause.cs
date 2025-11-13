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
    /// <summary>Enumaration of causes for Disconnects (used in LoadBalancingClient.DisconnectedCause).</summary>
    /// <remarks>Read the individual descriptions to find out what to do about this type of disconnect.</remarks>
    public enum ChatDisconnectCause
    {
        /// <summary>No error was tracked.</summary>
        None,
        /// <summary>OnStatusChanged: The CCUs count of your Photon Server License is exausted (temporarily).</summary>
        DisconnectByServerUserLimit,
        /// <summary>OnStatusChanged: The server is not available or the address is wrong. Make sure the port is provided and the server is up.</summary>
        ExceptionOnConnect,
        /// <summary>OnStatusChanged: The server disconnected this client. Most likely the server's send buffer is full (receiving too much from other clients).</summary>
        DisconnectByServer,
        /// <summary>OnStatusChanged: This client detected that the server's responses are not received in due time. Maybe you send / receive too much?</summary>
        TimeoutDisconnect,
        /// <summary>OnStatusChanged: Some internal exception caused the socket code to fail. Contact Exit Games.</summary>
        Exception,
        /// <summary>OnOperationResponse: Authenticate in the Photon Cloud with invalid AppId. Update your subscription or contact Exit Games.</summary>
        InvalidAuthentication,
        /// <summary>OnOperationResponse: Authenticate (temporarily) failed when using a Photon Cloud subscription without CCU Burst. Update your subscription.</summary>
        MaxCcuReached,
        /// <summary>OnOperationResponse: Authenticate when the app's Photon Cloud subscription is locked to some (other) region(s). Update your subscription or master server address.</summary>
        InvalidRegion,
        /// <summary>OnOperationResponse: Operation that's (currently) not available for this client (not authorized usually). Only tracked for op Authenticate.</summary>
        OperationNotAllowedInCurrentState,
        /// <summary>OnOperationResponse: Authenticate in the Photon Cloud with invalid client values or custom authentication setup in Cloud Dashboard.</summary>
        CustomAuthenticationFailed,
    }
}

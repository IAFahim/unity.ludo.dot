/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;

namespace PlayFab.UUnit
{
    /// <summary>
    /// Throw this exception, via UUnitAssert utility function, in order to define when a test has been skipped.
    /// The only information shown will be the "skipped" notification
    /// </summary>
    public class UUnitSkipException : Exception { }

        /// <summary>
    /// Throw this exception, via UUnitAssert utility functions, in order to define when a test has failed.
    /// The traceback and message will automatically be displayed as a failure
    /// </summary>
    public class UUnitAssertException : Exception
    {
        public object expected;
        public object received;
        public string message;

        public UUnitAssertException(string message)
            : base(message)
        {
            this.message = message;
        }

        public UUnitAssertException(object expected, object received, string message)
            : base("[UUnit] - Assert Failed - Expected: " + expected + " Received: " + received + "\n\t\t(" + message + ")")
        {
            this.expected = (expected == null) ? "null" : expected;
            this.received = (received == null) ? "null" : received;
            this.message = (message == null) ? "" : message;
        }

        public UUnitAssertException(object expected, object received)
            : base("[UUnit] - Assert Failed - Expected: " + expected + " Received: " + received)
        {
            this.expected = (expected == null) ? "null" : expected;
            this.received = (received == null) ? "null" : received;
        }
    }
}

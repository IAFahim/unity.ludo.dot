/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System.Collections.Generic;
using System.Text;

namespace PlayFab.UUnit
{
    public class UUnitTestResult
    {
        public enum TestState
        {
            PASSED,
            FAILED,
            SKIPPED,
        }

        private int runCount = 0, successCount = 0, failedCount = 0, skippedCount = 0;

        private static StringBuilder sb = new StringBuilder();
        List<string> messages = new List<string>();

        public void TestStarted()
        {
            runCount += 1;
        }

        public void TestComplete(string testName, TestState success, long stopwatchMS, string message) // TODO: Separate the message and the stack-trace for improved formatting
        {
            sb.Length = 0;
            sb.Append(stopwatchMS);
            while (sb.Length < 10)
                sb.Insert(0, ' ');
            sb.Append(" ms - ").Append(success.ToString());
            sb.Append(" - ").Append(testName);
            if (!string.IsNullOrEmpty(message))
                sb.Append(" - ").Append(message);
            messages.Add(sb.ToString());
            sb.Length = 0;

            switch (success)
            {
                case (TestState.PASSED):
                    successCount += 1; break;
                case (TestState.FAILED):
                    failedCount += 1; break;
                case (TestState.SKIPPED):
                    skippedCount += 1; break;
            }
        }

        public string Summary()
        {
            sb.Length = 0;
            sb.AppendFormat("Testing complete:  {0} test run, {1} tests passed, {2} tests failed, {3} tests skipped.", runCount, successCount, failedCount, skippedCount);
            messages.Add(sb.ToString());
            return string.Join("\n", messages.ToArray());
        }

        /// <summary>
        /// Return that tests were run, and all of them reported success
        /// </summary>
        public bool AllTestsPassed()
        {
            return runCount > 0 && runCount == (successCount + skippedCount) && failedCount == 0;
        }
    }
}

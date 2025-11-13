/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using System.Diagnostics;
using System.Reflection;

namespace PlayFab.UUnit
{
    public class UUnitTestCase
    {
        private delegate void UUnitTestDelegate();

        Stopwatch setUpStopwatch = new Stopwatch();
        Stopwatch tearDownStopwatch = new Stopwatch();
        Stopwatch eachTestStopwatch = new Stopwatch();
        private string testMethodName;

        public void SetTest(string testMethodName)
        {
            this.testMethodName = testMethodName;
        }

        public void Run(UUnitTestResult testResult)
        {
            UUnitTestResult.TestState testState = UUnitTestResult.TestState.FAILED;
            string message = null;
            eachTestStopwatch.Reset();
            setUpStopwatch.Reset();
            tearDownStopwatch.Reset();

            try
            {
                testResult.TestStarted();

                setUpStopwatch.Start();
                SetUp();
                setUpStopwatch.Stop();

                Type type = this.GetType();
                MethodInfo method = type.GetMethod(testMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                UUnitAssert.NotNull(method, "Could not execute: " + testMethodName + ", it's probably not public."); // Limited access to loaded assemblies
                eachTestStopwatch.Start();
                method.Invoke(this, null);
                testState = UUnitTestResult.TestState.PASSED;
            }
            catch (UUnitSkipException)
            {
                // message remains null
                testState = UUnitTestResult.TestState.SKIPPED;
            }
            catch (UUnitAssertException e)
            {
                message = e.ToString();
                testState = UUnitTestResult.TestState.FAILED;
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException is UUnitSkipException)
                {
                    // message remains null
                    testState = UUnitTestResult.TestState.SKIPPED;
                }
                else
                {
                    message = e.InnerException.ToString();
                    testState = UUnitTestResult.TestState.FAILED;
                }
            }
            catch (Exception e)
            {
                message = e.ToString();
                testState = UUnitTestResult.TestState.FAILED;
            }
            finally
            {
                eachTestStopwatch.Stop();

                if (testState != UUnitTestResult.TestState.SKIPPED)
                {
                    try
                    {
                        tearDownStopwatch.Start();
                        TearDown();
                        tearDownStopwatch.Stop();
                    }
                    catch (Exception e)
                    {
                        message = e.ToString();
                        testState = UUnitTestResult.TestState.FAILED;
                    }
                }
            }

            testResult.TestComplete(testMethodName, testState, eachTestStopwatch.ElapsedMilliseconds, message);
        }

        protected virtual void SetUp()
        {
        }

        protected virtual void TearDown()
        {
        }
    }
}

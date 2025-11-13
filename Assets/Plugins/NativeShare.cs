/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

/*
 * https://github.com/ChrisMaire/unity-native-sharing
 */

public class NativeShare : MonoBehaviour
{
    public string ScreenshotName = "ScreenshotLudo.png";

    public void ShareScreenshotWithText(string text)
    {
        string screenShotPath = Application.persistentDataPath + "/" + ScreenshotName;
        if (File.Exists(screenShotPath)) File.Delete(screenShotPath);

        ScreenCapture.CaptureScreenshot(ScreenshotName);
        //ScreenCapture.CaptureScreenshot(ScreenshotName);

        Debug.Log("Screenshot path: " + screenShotPath + " Text: " + text);
        StartCoroutine(delayedShare(screenShotPath, text));
    }

    //CaptureScreenshot runs asynchronously, so you'll need to either capture the screenshot early and wait a fixed time
    //for it to save, or set a unique image name and check if the file has been created yet before sharing.
    private IEnumerator delayedShare(string screenShotPath, string text)
    {
        Debug.Log("Delay share");
        while (!File.Exists(screenShotPath))
        {
            yield return new WaitForSeconds(.05f);
        }

        Share(text, screenShotPath, "");
        yield return null;
    }

    public void Share(string shareText, string imagePath, string url, string subject = "")
    {
#if UNITY_ANDROID
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        if (imagePath != null)
        {
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + imagePath);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("setType", "image/png");
        }
        else
        {
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        }

        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareText);

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, subject);
        currentActivity.Call("startActivity", jChooser);
#elif UNITY_IOS
		CallSocialShareAdvanced(shareText, subject, url, imagePath);
#else
        Debug.Log("No sharing set up for this platform.");
#endif
    }

#if UNITY_IOS
	public struct ConfigStruct
	{
		public string title;
		public string message;
	}

	[DllImport ("__Internal")] private static extern void showAlertMessage(ref ConfigStruct conf);

	public struct SocialSharingStruct
	{
		public string text;
		public string url;
		public string image;
		public string subject;
	}

	[DllImport ("__Internal")] private static extern void showSocialSharing(ref SocialSharingStruct conf);

	public static void CallSocialShare(string title, string message)
	{
		ConfigStruct conf = new ConfigStruct();
		conf.title  = title;
		conf.message = message;
		showAlertMessage(ref conf);
	}


	public static void CallSocialShareAdvanced(string defaultTxt, string subject, string url, string img)
	{
		SocialSharingStruct conf = new SocialSharingStruct();
		conf.text = defaultTxt;
		if(url != null) {
			conf.url = url;
			conf.image = img;
		}
		conf.subject = subject;

		showSocialSharing(ref conf);
	}
#endif
}

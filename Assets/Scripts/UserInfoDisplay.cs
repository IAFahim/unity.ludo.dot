using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UserInfoDisplay : MonoBehaviour
{
    public Text usernameText;
    public Image userPhoto;

    void Start()
    {
        string username = PlayerPrefs.GetString("username", "Guest");
        string photoUrl = PlayerPrefs.GetString("photoUrl", "");

        usernameText.text = username;

        if (!string.IsNullOrEmpty(photoUrl))
            StartCoroutine(LoadUserPhoto(photoUrl));
    }

    IEnumerator LoadUserPhoto(string url)
    {
        using (var www = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Texture2D tex = ((UnityEngine.Networking.DownloadHandlerTexture)www.downloadHandler).texture;
                userPhoto.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
            }
        }
    }
}

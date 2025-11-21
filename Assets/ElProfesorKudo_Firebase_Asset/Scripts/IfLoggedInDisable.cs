using System;
using Firebase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MonoBehaviour = Photon.MonoBehaviour;
using Firebase.Auth;

namespace ElProfesorKudo.Firebase.UI
{
    public class IfLoggedInDisable: MonoBehaviour
    {
        public Image image;
        public Color logOutColor;
        public Text text;
        public string logoutText = "LogOut from Google";

        private void OnValidate()
        {
            image = GetComponent<Image>();
            text = GetComponentInChildren<Text>();
        }

        private void OnEnable()
        {
            if (FirebaseAuth.DefaultInstance?.CurrentUser != null)
            {
                image.color = logOutColor;
                text.text = logoutText;
            }
        }
    }
}
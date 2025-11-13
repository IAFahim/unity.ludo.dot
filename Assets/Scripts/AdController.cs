using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdController : MonoBehaviour
{

    public static AdController instance;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
       // Advertisements.Instance.Initialize();
       Gley.MobileAds.API.Initialize();
       
    }


    public void ShowAds()
    {
        Gley.MobileAds.API.ShowInterstitial();
      //  Advertisements.Instance.ShowInterstitial();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

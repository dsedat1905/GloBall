using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class RewardReklam : MonoBehaviour
{
     RewardBasedVideoAd ödüllüreklam;
  
    // Start is called before the first frame update
    void Start()
    {
        
#if UNITY_ANDROID
        string appId = "ca-app-pub-3940256099942544~3347511713";
#elif UNITY_IPHONE
            string appId = "todo";
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        ödüllüreklam = RewardBasedVideoAd.Instance;
        ÖdüllüReklamİsteği();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     void ÖdüllüReklamİsteği()
    {
        AdRequest reklamIstegi = new AdRequest.Builder().Build();
        ödüllüreklam.LoadAd(reklamIstegi, "ca-app-pub-3940256099942544/5224354917");
      


    }
    public void ReklamıGöster()
    {
        if (!ödüllüreklam.IsLoaded())
        {
            Debug.Log("Reklam Yüklenmedi");
        }
        else
        {
            
            ödüllüreklam.Show();
 Application.LoadLevel(Application.loadedLevel);
          
        }
           
       
        
    }

    private void Ödüllüreklam_OnAdClosed(object sender, System.EventArgs e)
    {
        AdRequest reklamIstegi = new AdRequest.Builder().Build();
        ödüllüreklam.LoadAd(reklamIstegi, "ca-app-pub-3940256099942544/5224354917");
 Application.LoadLevel(0);
    }
private void Ödüllüreklam_OnAdRewarded(object sender, Reward Elma)
    {
       Application.LoadLevel(Application.loadedLevel);

        
    }
  
}

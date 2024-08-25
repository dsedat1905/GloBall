﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class ReklamBanner3 : MonoBehaviour
{
    private BannerView reklamObjesi;
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

        BannerReklamGösterme();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BannerReklamGösterme()
    {

        reklamObjesi = new BannerView("ca-app-pub-3940256099942544/6300978111", AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest reklamIstegi = new AdRequest.Builder().Build();
        reklamObjesi.LoadAd(reklamIstegi);

    }
    void OnDestroy()
    {
        if (reklamObjesi != null)
            reklamObjesi.Destroy();
    }
}
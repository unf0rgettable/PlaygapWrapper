using System;
using System.Collections;
using System.Collections.Generic;
using LittleBitGames.Ads.Configs;
using LittleBitGames.Environment;
using LittleBitGames.Environment.Ads;
using Playgap;
using UnityEngine;

public class PlaygapInitializer : IMediationNetworkInitializer
{
    public event Action OnMediationInitialized;
        
    private readonly AdsConfig _config;
        
    public PlaygapInitializer(AdsConfig config) => _config = config;
        
    private bool IsDebugMode => _config.Mode is ExecutionMode.Debug;
        
    public bool IsInitialized { get; private set; }


    private bool _isMaxInit;
    private bool _isPlaygapInit;
    
    public void Initialize()
    {
        InitMax();
        #if !UNITY_EDITOR
        InitPlaygap();
        #endif
    }
    
    private void InitMax()
    {
        global::MaxSdk.SetSdkKey(_config.PlaygapSettings.MaxSdkKey);
        global::MaxSdk.SetUserId("USER_ID");
        global::MaxSdk.InitializeSdk();

        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfig =>
        {
            _isMaxInit = true;

            if (_isPlaygapInit)
            {
                IsInitialized = true;
                OnMediationInitialized?.Invoke();
            }
            
            if (IsDebugMode) 
                global::MaxSdk.ShowMediationDebugger();
        };
    }

    private void InitPlaygap()
    {
        PlaygapAds.OnInitializationComplete = (string error) => {
            if (error != null)
            {
                Debug.Log("Initialzation failed triggered: " + error);
            }
            else
            {
                Debug.Log("Initialization completed triggered");
                _isPlaygapInit = true;

                if (_isMaxInit)
                {
                    IsInitialized = true;
                    OnMediationInitialized?.Invoke();
                }
            }
        };
        
        PlaygapAds.Initialize(_config.PlaygapSettings.PlaygapApiKey);
    }
}

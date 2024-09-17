using System;

namespace LittleBitGames.Ads.AdUnits
{
    public class PlaygapInterEvents : IAdUnitEvents
    {
        public event Action<string, IAdInfo> OnAdRevenuePaid;
        public event Action<string, IAdInfo> OnAdLoaded;
        public event Action<string, IAdErrorInfo> OnAdLoadFailed;
        public event Action<string, IAdInfo> OnAdFinished;
        public event Action<string, IAdInfo> OnAdClicked;
        public event Action<string, IAdInfo> OnAdHidden;
        public event Action<string, IAdErrorInfo, IAdInfo> OnAdDisplayFailed;

        public PlaygapInterEvents()
        {

            Playgap.PlaygapAds.OnShowCompleted += () =>
            {
                OnAdRevenuePaid?.Invoke("", null);
                OnAdFinished?.Invoke("", null);
            };
        }
        
    }
}
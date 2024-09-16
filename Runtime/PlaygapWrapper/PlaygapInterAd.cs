using LittleBit.Modules.CoreModule;
using LittleBitGames.Ads.AdUnits;

namespace LittleBitGames.Ads.MediationNetworks.MaxSdk
{
    public sealed class PlaygapInterAd : AdUnitLogic
    {
        private readonly IAdUnitKey _key;

        public PlaygapInterAd(IAdUnitKey key, ICoroutineRunner coroutineRunner) : base(key,
            new PlaygapInterEvents(), coroutineRunner) => _key = key;

        protected override bool IsAdReady()
        {
            bool isOffline = false;
            
            Playgap.PlaygapAds.ObserveNetwork((b) =>
            {
                isOffline = b;
            });
            
            
            if (isOffline)
            {
                return true;
            }
            else
            {
                return global::MaxSdk.IsInterstitialReady(_key.StringValue);
            }
        }

        protected override void ShowAd() => Playgap.PlaygapAds.ShowInterstitial();
        public override void Load() => global::MaxSdk.LoadInterstitial(_key.StringValue);
    }
}
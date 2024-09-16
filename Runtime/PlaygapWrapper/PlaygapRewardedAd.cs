using LittleBit.Modules.CoreModule;
using LittleBitGames.Ads.AdUnits;

namespace LittleBitGames.Ads.MediationNetworks.MaxSdk
{
    public sealed class PlaygapRewardedAd : AdUnitLogic
    {
        private readonly IAdUnitKey _key;

        public PlaygapRewardedAd(IAdUnitKey key, ICoroutineRunner coroutineRunner) : base(key,
            new PlaygapRewardedEvents(), coroutineRunner) => _key = key;

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
                return global::MaxSdk.IsRewardedAdReady(_key.StringValue);
            }
        }

        protected override void ShowAd() => Playgap.PlaygapAds.ShowRewarded();
        public override void Load() => global::MaxSdk.LoadRewardedAd(_key.StringValue);
    }
}
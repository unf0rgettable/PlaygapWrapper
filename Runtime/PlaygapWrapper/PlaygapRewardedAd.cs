using LittleBit.Modules.CoreModule;
using LittleBitGames.Ads.AdUnits;

namespace LittleBitGames.Ads.MediationNetworks.MaxSdk
{
    public sealed class PlaygapRewardedAd : AdUnitLogic
    {
        private readonly IAdUnitKey _key;

        public PlaygapRewardedAd(IAdUnitKey key, ICoroutineRunner coroutineRunner) : base(key,
            new PlaygapRewardedEvents(), coroutineRunner) => _key = key;

        protected override bool IsAdReady() => true;

        protected override void ShowAd()
        {
            bool isOffline = false;
            
            Playgap.PlaygapAds.ObserveNetwork((b) =>
            {
                isOffline = b;
            });
            
            if (isOffline)
            {
                Playgap.PlaygapAds.ShowRewarded();
            }
            else
            {
                if(global::MaxSdk.IsRewardedAdReady(_key.StringValue))
                    global::MaxSdk.ShowRewardedAd(_key.StringValue);
                else
                    Playgap.PlaygapAds.ShowRewarded();
            }
        }

        public override void Load()
        {
            global::MaxSdk.LoadRewardedAd(_key.StringValue);
        }
    }
}
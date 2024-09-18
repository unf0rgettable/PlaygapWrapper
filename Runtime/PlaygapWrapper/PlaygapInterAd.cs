using LittleBit.Modules.CoreModule;
using LittleBitGames.Ads.AdUnits;

namespace LittleBitGames.Ads.MediationNetworks.MaxSdk
{
    public sealed class PlaygapInterAd : AdUnitLogic
    {
        private readonly IAdUnitKey _key;

        public PlaygapInterAd(IAdUnitKey key, ICoroutineRunner coroutineRunner) : base(key,
            new PlaygapInterEvents(), coroutineRunner) => _key = key;

        protected override bool IsAdReady() => true;
        protected override void ShowAd() => Playgap.PlaygapAds.ShowInterstitial();
        public override void Load() { }
    }
}
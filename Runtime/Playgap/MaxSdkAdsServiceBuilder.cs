using System;
using LittleBit.Modules.CoreModule;
using LittleBitGames.Ads.AdUnits;
using LittleBitGames.Ads.Configs;
using LittleBitGames.Ads.MediationNetworks.MaxSdk;
using LittleBitGames.Environment.Ads;
using UnityEngine.Scripting;

namespace LittleBitGames.Ads
{
    public class PlaygapServiceBuilder : IAdsServiceBuilder
    {
        private readonly PlaygapAdUnitsFactory _adUnitsFactory;
        private readonly PlaygapInitializer _initializer;

        private IAdUnit _inter, _rewarded,_banner;
        private AdsConfig _adsConfig;

        public IMediationNetworkInitializer Initializer => _initializer;

        [Preserve]
        public PlaygapServiceBuilder(AdsConfig adsConfig, ICoroutineRunner coroutineRunner)
        {
            _adsConfig = adsConfig;
            _adUnitsFactory = new PlaygapAdUnitsFactory(coroutineRunner, adsConfig);
            _initializer = new PlaygapInitializer(adsConfig);

            if (!ValidateMaxSdkKey())
                throw new Exception($"Max sdk key is invalid! Key: {_adsConfig.PlaygapSettings.MaxSdkKey}");
        }

        private bool ValidateMaxSdkKey() => !string.IsNullOrEmpty(_adsConfig.PlaygapSettings.MaxSdkKey);

        public IAdsService QuickBuild()
        {
            if (!string.IsNullOrEmpty(_adsConfig.PlaygapSettings.PlatformSettings.MaxInterAdUnitKey) && _adsConfig.IsInter) 
                BuildInterAdUnit();
            if (!string.IsNullOrEmpty(_adsConfig.PlaygapSettings.PlatformSettings.MaxRewardedAdUnitKey) && _adsConfig.IsRewarded) 
                BuildRewardedAdUnit();
            if (!string.IsNullOrEmpty(_adsConfig.PlaygapSettings.PlatformSettings.MaxBannerAdUnitKey) && _adsConfig.IsBanner) 
                BuildBannerAdUnit();

            return GetResult();
        }

        public void BuildInterAdUnit() =>
            _inter = _adUnitsFactory.CreateInterAdUnit();

        public void BuildRewardedAdUnit() =>
            _rewarded = _adUnitsFactory.CreateRewardedAdUnit();
        public void BuildBannerAdUnit() =>
            _banner = _adUnitsFactory.CreateBannerAdUnit();

        public IAdsService GetResult() => new AdsService(_initializer, _inter, _rewarded, _banner);
    }
}
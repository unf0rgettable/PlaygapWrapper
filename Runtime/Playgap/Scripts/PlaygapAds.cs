using System;
using UnityEngine;
using static Playgap.IPlaygapAds;

namespace Playgap
{
    public class PlaygapAds
    {
        public static Action<string> OnInitializationComplete;

        #region Show Callbacks
        public static Action<string> OnShowFailed;
        public static Action<string> OnShowImpression;
        public static Action<string> OnShowPlaybackEvent;
        public static Action OnShowCompleted;
        public static Action<string> OnUserEarnedReward;
        #endregion

        #region Claim Rewards Callbacks
        public static Action OnRewardScreenShown;
        public static Action<string> OnRewardScreenFailed;
        public static Action OnRewardScreenClosed;
        public static Action<string[]> OnUserClaimedRewards;
        #endregion

        public static void ObserveNetwork(Action<bool> observer)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    PlaygapAds_Android.ObserveNetwork(observer);
                    break;

                case RuntimePlatform.IPhonePlayer:
                    PlaygapAds_iOS.ObserveNetwork(observer);
                    break;

                default:
                    throw new PlatformNotSupportedException();
            }
        }

        public static void Initialize(string apiKey)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    PlaygapAds_Android.Initialize(apiKey, (error) => { OnInitializationComplete?.Invoke(error); });
                    break;

                case RuntimePlatform.IPhonePlayer:
                    PlaygapAds_iOS.Initialize(apiKey, (error) => { OnInitializationComplete?.Invoke(error); });
                    break;

                default:
                    throw new PlatformNotSupportedException();
            }
        }

        public static void ShowRewarded()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    PlaygapAds_Android.ShowRewarded(
                        (error) => { OnShowFailed?.Invoke(error); },
                        (impressionId) => { OnShowImpression?.Invoke(impressionId); },
                        (period) => { OnShowPlaybackEvent?.Invoke(period); },
                        () => { OnShowCompleted?.Invoke(); },
                        (rewardId) => { OnUserEarnedReward?.Invoke(rewardId); }
                    );
                    break;

                case RuntimePlatform.IPhonePlayer:
                    PlaygapAds_iOS.ShowRewarded(
                        (error) => { OnShowFailed?.Invoke(error); },
                        (impressionId) => { OnShowImpression?.Invoke(impressionId); },
                        (period) => { OnShowPlaybackEvent?.Invoke(period); },
                        () => { OnShowCompleted?.Invoke(); },
                        (rewardId) => { OnUserEarnedReward?.Invoke(rewardId); }
                    );
                    break;

                default:
                    throw new PlatformNotSupportedException();
            }
        }

        public static void ShowInterstitial()

        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    PlaygapAds_Android.ShowInterstitial(
                        (error) => { OnShowFailed?.Invoke(error); },
                        (impressionId) => { OnShowImpression?.Invoke(impressionId); },
                        (period) => { OnShowPlaybackEvent?.Invoke(period); },
                        () => { OnShowCompleted?.Invoke(); },
                        (rewardId) => { OnUserEarnedReward?.Invoke(rewardId); }
                    );
                    break;

                case RuntimePlatform.IPhonePlayer:
                    PlaygapAds_iOS.ShowInterstitial(
                        (error) => { OnShowFailed?.Invoke(error); },
                        (impressionId) => { OnShowImpression?.Invoke(impressionId); },
                        (period) => { OnShowPlaybackEvent?.Invoke(period); },
                        () => { OnShowCompleted?.Invoke(); },
                        (rewardId) => { OnUserEarnedReward?.Invoke(rewardId); }
                    );
                    break;

                default:
                    throw new PlatformNotSupportedException();
            }
        }

        public static void ClaimRewards()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    PlaygapAds_Android.ClaimRewards(
                        () => { OnRewardScreenShown?.Invoke(); },
                        (error) => { OnRewardScreenFailed?.Invoke(error); },
                        () => { OnRewardScreenClosed?.Invoke(); },
                        (rewardIds) => { OnUserClaimedRewards?.Invoke(rewardIds); }
                    );
                    break;

                case RuntimePlatform.IPhonePlayer:
                    PlaygapAds_iOS.ClaimRewards(
                        () => { OnRewardScreenShown?.Invoke(); },
                        (error) => { OnRewardScreenFailed?.Invoke(error); },
                        () => { OnRewardScreenClosed?.Invoke(); },
                        (rewardIds) => { OnUserClaimedRewards?.Invoke(rewardIds); }
                    );
                    break;

                default:
                    throw new PlatformNotSupportedException();
            }
        }

        public static Rewards CheckRewards()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return PlaygapAds_Android.CheckRewards();

                case RuntimePlatform.IPhonePlayer:
                    return PlaygapAds_iOS.CheckRewards();

                default:
                    throw new PlatformNotSupportedException();
            }
        }

        internal static void SendEvent(string eventType, Payload payload, string objectId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    PlaygapAds_Android.SendEvent(eventType, payload, objectId);
                    break;

                case RuntimePlatform.IPhonePlayer:
                    PlaygapAds_iOS.SendEvent(eventType, payload, objectId);
                    break;

                default:
                    throw new PlatformNotSupportedException();
            }
        }
    }
}
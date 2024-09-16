using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using static Playgap.IPlaygapAds;

namespace Playgap
{
    sealed class PlaygapAds_iOS: IPlaygapAds
    {

#if UNITY_IOS
        [DllImport("__Internal")]
#endif
        private static extern void PlaygapAds_initialize(string apiKey, Action<string> completion);

#if UNITY_IOS
        [DllImport("__Internal")]
#endif
        private static extern void PlaygapAds_showRewarded(
            Action<string> showFailed,
            Action<string> showImpression,
            Action<string> showPlaybackEvent,
            Action<string> showCompleted,
            Action<string> userEarnedReward
        );

#if UNITY_IOS
        [DllImport("__Internal")]
#endif
        private static extern void PlaygapAds_showInterstitial(
            Action<string> showFailed,
            Action<string> showImpression,
            Action<string> showPlaybackEvent,
            Action<string> showCompleted,
            Action<string> userEarnedReward
        );

#if UNITY_IOS
        [DllImport("__Internal")]
#endif
        private static extern void PlaygapAds_claimRewards(
            Action rewardScreenShown,
            Action<string> rewardScreenFailed,
            Action rewardScreenClosed,
            Action storeClick,
            Action<string> userClaimedRewards
        );

#if UNITY_IOS
        [DllImport("__Internal")]
#endif
        private static extern string PlaygapAds_checkRewards();

#if UNITY_IOS
        [DllImport("__Internal")]
#endif
        private static extern void PlaygapAds_observeNetwork(Action<bool> observer);

#if UNITY_IOS
        [DllImport("__Internal")]
#endif
        private static extern void PlaygapAds_sendEvent(string eventName, string json, string objectId);

        internal static void Initialize(string apiKey, Action<string> OnInitializationComplete)
        {
            IPlaygapAds.OnInitializationComplete = OnInitializationComplete;

            PlaygapAds_initialize(apiKey, InitializeHandler);
        }

        internal static void ShowRewarded(
            Action<string> OnShowFailed,
            Action<string> OnShowImpression,
            Action<string> OnShowPlaybackEvent,
            Action OnShowCompleted,
            Action<string> OnUserEarnedReward
        ) {
            IPlaygapAds.OnShowFailed = OnShowFailed;
            IPlaygapAds.OnShowImpression = OnShowImpression;
            IPlaygapAds.OnShowPlaybackEvent = OnShowPlaybackEvent;
            IPlaygapAds.OnShowCompleted = OnShowCompleted;
            IPlaygapAds.OnUserEarnedReward = OnUserEarnedReward;

            PlaygapAds_showRewarded(
                ShowFailedHandler,
                ShowImpressionHandler,
                ShowPlaybackEventHandler,
                ShowCompletedHandler,
                UserEarnedReward
            );
        }

        internal static void ShowInterstitial(
            Action<string> OnShowFailed,
            Action<string> OnShowImpression,
            Action<string> OnShowPlaybackEvent,
            Action OnShowCompleted,
            Action<string> OnUserEarnedReward
        ) {
            IPlaygapAds.OnShowFailed = OnShowFailed;
            IPlaygapAds.OnShowImpression = OnShowImpression;
            IPlaygapAds.OnShowPlaybackEvent = OnShowPlaybackEvent;
            IPlaygapAds.OnShowCompleted = OnShowCompleted;
            IPlaygapAds.OnUserEarnedReward = OnUserEarnedReward;

            PlaygapAds_showInterstitial(
                ShowFailedHandler,
                ShowImpressionHandler,
                ShowPlaybackEventHandler,
                ShowCompletedHandler,
                UserEarnedReward
            );
        }

        internal static void ClaimRewards(
            Action OnRewardScreenShown,
            Action<string> OnRewardScreenFailed,
            Action OnRewardScreenClosed,
            Action<string[]> OnUserClaimedRewards
        )
        {
            IPlaygapAds.OnRewardScreenShown = OnRewardScreenShown;
            IPlaygapAds.OnRewardScreenFailed = OnRewardScreenFailed;
            IPlaygapAds.OnRewardScreenClosed = OnRewardScreenClosed;
            IPlaygapAds.OnUserClaimedRewards = OnUserClaimedRewards;

            PlaygapAds_claimRewards(RewardScreenShown, RewardScreenFailed, RewardScreenClosed, StoreClick, UserClaimedOfflineReward);
        }

        internal static Rewards CheckRewards()
        {
            Rewards rewards = new();

            var json = PlaygapAds_checkRewards();
            if (json != null)
            {
                rewards = JsonUtility.FromJson<Rewards>(json);
            }

            return rewards;
        }

        internal static void ObserveNetwork(Action<bool> observer)
        {
            networkObserver = observer;
            PlaygapAds_observeNetwork(NetworkObserver);
        }

        internal static void SendEvent(string eventType, Payload payload, string objectId)
        {
            PlaygapAds_sendEvent(eventType, JsonUtility.ToJson(payload), objectId);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void InitializeHandler(string error)
        {
            OnInitializationComplete?.Invoke(error);
        }

        #region ShowDelegate
        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void ShowFailedHandler(string error)
        {
            OnShowFailed?.Invoke(error);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void ShowImpressionHandler(string impressionId)
        {
            OnShowImpression?.Invoke(impressionId);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void ShowPlaybackEventHandler(string period)
        {
            OnShowPlaybackEvent?.Invoke(period);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void ShowCompletedHandler(string rewardId)
        {
            OnShowCompleted?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void UserEarnedReward(string rewardId)
        {
            OnUserEarnedReward?.Invoke(rewardId);
        }
        #endregion

        #region ClaimRewardsDelegate
        [MonoPInvokeCallback(typeof(Action))]
        private static void RewardScreenShown()
        {
            OnRewardScreenShown?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void RewardScreenFailed(string error)
        {
            OnRewardScreenFailed?.Invoke(error);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void RewardScreenClosed()
        {
            OnRewardScreenClosed?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void StoreClick()
        {
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void UserClaimedOfflineReward(string rewardIds)
        {
            OnUserClaimedRewards?.Invoke(rewardIds.Split("::"));
        }
        #endregion

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void NetworkObserver(bool isConnected)
        {
            networkObserver?.Invoke(isConnected);
        }
    }
}
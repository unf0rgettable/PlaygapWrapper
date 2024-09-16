using System;
using UnityEngine;
using static Playgap.IPlaygapAds;

namespace Playgap
{
    sealed class PlaygapAds_Android : IPlaygapAds
    {
        internal static void Initialize(string apiKey, Action<string> OnInitializationComplete)
        {
            PlaygapEventScheduler.Create();

            IPlaygapAds.OnInitializationComplete = OnInitializationComplete;

            sdk.CallStatic("initialize", activity, apiKey, new InitializationListener());
        }

        internal static void ShowRewarded(
            Action<string> OnShowFailed,
            Action<string> OnShowImpression,
            Action<string> OnShowPlaybackEvent,
            Action OnShowCompleted,
            Action<string> OnUserEarnedReward
        )
        {
            IPlaygapAds.OnShowFailed = OnShowFailed;
            IPlaygapAds.OnShowImpression = OnShowImpression;
            IPlaygapAds.OnShowPlaybackEvent = OnShowPlaybackEvent;
            IPlaygapAds.OnShowCompleted = OnShowCompleted;
            IPlaygapAds.OnUserEarnedReward = OnUserEarnedReward;

            sdk.CallStatic("showRewarded", activity, new ShowListener());
        }

        internal static void ShowInterstitial(
            Action<string> OnShowFailed,
            Action<string> OnShowImpression,
            Action<string> OnShowPlaybackEvent,
            Action OnShowCompleted,
            Action<string> OnUserEarnedReward
        )
        {
            IPlaygapAds.OnShowFailed = OnShowFailed;
            IPlaygapAds.OnShowImpression = OnShowImpression;
            IPlaygapAds.OnShowPlaybackEvent = OnShowPlaybackEvent;
            IPlaygapAds.OnShowCompleted = OnShowCompleted;
            IPlaygapAds.OnUserEarnedReward = OnUserEarnedReward;

            sdk.CallStatic("showInterstitial", activity, new ShowListener());
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

            sdk.CallStatic("claimRewards", activity, new ClaimRewardsListener());
        }

        internal static Rewards CheckRewards()
        {
            Rewards rewards = new();

            var json = sdk.CallStatic<string>("checkRewards");
            if (json != null)
            {
                rewards = JsonUtility.FromJson<Rewards>(json);
            }

            return rewards;
        }

        internal static void ObserveNetwork(Action<bool> observer)
        {
            networkObserver = observer;
            sdk.CallStatic("registerNetworkObserver", activity, new NetworkListener());
        }

        internal static void SendEvent(string eventType, Payload payload, string objectId)
        {
            sdk.CallStatic("sendEvent", eventType, JsonUtility.ToJson(payload), objectId);
        }

        private static AndroidJavaClass sdk
        {
            get
            {
                return new AndroidJavaClass("io.playgap.sdk.PlaygapAdsBridge");
            }
        }

        private static AndroidJavaObject activity
        {
            get
            {
                return new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                    .GetStatic<AndroidJavaObject>("currentActivity");
            }
        }
    }

    sealed class NetworkListener : AndroidJavaProxy
    {
        public NetworkListener() : base("io.playgap.sdk.NetworkListenerBridge") { }

        public void onNetworkStateChanged(Boolean isConnected)
        {
            PlaygapEventScheduler.Scheduler.ScheduleOnUpdate(() =>
            {
                networkObserver?.Invoke(isConnected);
            });
        }
    }

    sealed class InitializationListener : AndroidJavaProxy
    {
        public InitializationListener() : base("io.playgap.sdk.InitializationListenerBridge") { }

        public void onInitializationError(string error)
        {
            PlaygapEventScheduler.Scheduler.ScheduleOnUpdate(() =>
            {
                OnInitializationComplete(error);
            });
        }

        public void onInitialized()
        {
            PlaygapEventScheduler.Scheduler.ScheduleOnUpdate(() =>
            {
                OnInitializationComplete(null);
            });
        }
    }

    sealed class ShowListener : AndroidJavaProxy
    {

        internal ShowListener() : base("io.playgap.sdk.ShowListenerBridge") { }

        internal void onShowFailed(string error)
        {
            PlaygapEventScheduler.Scheduler.ScheduleOnUpdate(() =>
            {
                OnShowFailed(error);
            });
        }

        internal void onShowImpression(string impressionId)
        {
            PlaygapEventScheduler.Scheduler.ScheduleOnUpdate(() =>
            {
                OnShowImpression(impressionId);
            });
        }

        internal void onShowPlaybackEvent(string playbackEvent)
        {
            PlaygapEventScheduler.Scheduler.ScheduleOnUpdate(() =>
            {
                OnShowPlaybackEvent(playbackEvent);
            });
        }

        internal void onShowCompleted()
        {
            PlaygapEventScheduler.Scheduler.ScheduleOnUpdate(() =>
            {
                OnShowCompleted();
            });
        }

        internal void onUserEarnedReward(string rewardId)
        {
            PlaygapEventScheduler.Scheduler.ScheduleOnUpdate(() =>
            {
                OnUserEarnedReward(rewardId);
            });
        }
    }

    sealed class ClaimRewardsListener : AndroidJavaProxy
    {

        internal ClaimRewardsListener() : base("io.playgap.sdk.ClaimRewardsListenerBridge") { }

        internal void onRewardScreenShown()
        {
            PlaygapEventScheduler.Scheduler.ScheduleOnUpdate(() =>
            {
                OnRewardScreenShown();
            });
        }

        internal void onRewardScreenFailed(string error)
        {
            PlaygapEventScheduler.Scheduler.ScheduleOnUpdate(() =>
            {
                OnRewardScreenFailed(error);
            });
        }

        internal void onRewardScreenClosed()
        {
            PlaygapEventScheduler.Scheduler.ScheduleOnUpdate(() =>
            {
                OnRewardScreenClosed();
            });
        }

        internal void onUserClaimedRewards(string[] rewardIds)
        {
            PlaygapEventScheduler.Scheduler.ScheduleOnUpdate(() =>
            {
                OnUserClaimedRewards(rewardIds);
            });
        }
    }
}

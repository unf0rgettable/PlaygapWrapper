using System;
using System.Collections.Generic;

namespace Playgap
{
    public interface IPlaygapAds
    {
        [Serializable]
        public struct Rewards
        {
            public List<string> unclaimed;
            public List<string> claimed;
        }

        [Serializable]
        internal struct Payload
        {
            public string identifier;
            public bool nofill;
            public string format;
            public string mediation;
            public string reason;
            public double revenue;
            public string adNetwork;
        }

        internal static Action<bool> networkObserver;

        internal static Action<string> OnInitializationComplete;

        #region Show Callbacks
        internal static Action<string> OnShowFailed;
        internal static Action<string> OnShowImpression;
        internal static Action<string> OnShowPlaybackEvent;
        internal static Action OnShowCompleted;
        internal static Action<string> OnUserEarnedReward;
        #endregion

        #region Claim Rewards Callbacks
        internal static Action OnRewardScreenShown;
        internal static Action<string> OnRewardScreenFailed;
        internal static Action OnRewardScreenClosed;
        internal static Action<string[]> OnUserClaimedRewards;
        #endregion

        internal static void Initialize(string apiKey, Action<string> completion) => throw new NotImplementedException();

        internal static void ShowRewarded(
            Action<string> OnShowFailed,
            Action<string> OnShowImpression,
            Action<string> OnShowPlaybackEvent,
            Action OnShowCompleted,
            Action<string> OnUserEarnedReward
        ) => throw new NotImplementedException();

        internal static void ShowInterstitial(
            Action<string> OnShowFailed,
            Action<string> OnShowImpression,
            Action<string> OnShowPlaybackEvent,
            Action OnShowCompleted,
            Action<string> OnUserEarnedReward
        ) => throw new NotImplementedException();

        internal static void ClaimRewards(
            Action OnRewardScreenShown,
            Action<string> OnRewardScreenFailed,
            Action OnRewardScreenClosed,
            Action<string[]> OnUserClaimedRewards
        ) => throw new NotImplementedException();

        internal static Rewards CheckRewards() => throw new NotImplementedException();

        internal static void SendEvent(string eventType, Payload payload, string objectId) => throw new NotImplementedException();
    }
}

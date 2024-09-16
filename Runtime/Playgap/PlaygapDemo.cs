using UnityEngine;

namespace Playgap
{
    public class PlaygapDemo : MonoBehaviour
    {
        public void InitializePressed()
        {
            Debug.Log("PLAYGAP DEMO Initialize attempt triggered");

            PlaygapAds.OnInitializationComplete = OnInitializationComplete;
            PlaygapAds.Initialize("tj8SxMjJ9Mlya5Nn");
            PlaygapAds.ObserveNetwork((isConnected) => {
                Debug.Log("PLAYGAP DEMO is connected to network " + isConnected);
            });
        }

        public void ShowRewardedAdPressed()
        {
            Debug.Log("PLAYGAP DEMO Show Rewarded attempt triggered");

            PlaygapAds.OnShowFailed = OnShowFailed;
            PlaygapAds.OnShowImpression = OnShowImpression;
            PlaygapAds.OnShowPlaybackEvent = OnShowPlaybackEvent;
            PlaygapAds.OnShowCompleted = OnShowCompleted;
            PlaygapAds.OnUserEarnedReward = OnUserEarnedReward;
            PlaygapAds.ShowRewarded();
        }

        public void ShowInterstitialAdPressed()
        {
            Debug.Log("PLAYGAP DEMO Show Interstitial attempt triggered");

            PlaygapAds.OnShowFailed = OnShowFailed;
            PlaygapAds.OnShowImpression = OnShowImpression;
            PlaygapAds.OnShowPlaybackEvent = OnShowPlaybackEvent;
            PlaygapAds.OnShowCompleted = OnShowCompleted;
            PlaygapAds.ShowInterstitial();
        }

        public void ClaimRewardsPressed()
        {
            Debug.Log("PLAYGAP Claim Rewards attempt triggered");

            PlaygapAds.OnRewardScreenShown = OnRewardScreenShown;
            PlaygapAds.OnRewardScreenFailed = OnRewardScreenFailed;
            PlaygapAds.OnRewardScreenClosed = OnRewardScreenClosed;
            PlaygapAds.ClaimRewards();
        }

        private void OnInitializationComplete(string error)
        {
            if (error != null)
            {
                Debug.Log("PLAYGAP DEMO Initialzation failed triggered: " + error);
            }
            else
            {
                PlaygapAds.OnUserClaimedRewards = OnUserClaimedRewards;
                Debug.Log("PLAYGAP DEMO Initialization completed triggered");
            }
        }

        #region ShowDelegate
        private void OnShowFailed(string error)
        {
            Debug.Log("PLAYGAP DEMO Show failed triggered: " + error);
        }

        private void OnShowImpression(string impressionId)
        {
            Debug.Log("PLAYGAP DEMO Impression triggered for id: " + impressionId);
        }

        private void OnShowPlaybackEvent(string period)
        {
            Debug.Log("PLAYGAP DEMO Playback event triggered: " + period);
        }

        private void OnShowSkipped()
        {
            Debug.Log("PLAYGAP DEMO Show skipped");
        }

        private void OnShowCompleted()
        {
            Debug.Log("PLAYGAP DEMO Show completed triggered");
        }

        private void OnUserEarnedReward(string rewardId)
        {
            Debug.Log("PLAYGAP DEMO User Rewarded triggered with reward id: " + rewardId);
        }
        #endregion

        #region ClaimRewardsDelegate
        private void OnRewardScreenShown()
        {
            Debug.Log("PLAYGAP DEMO Claim Reward screen shown triggered");
        }

        private void OnRewardScreenFailed(string error)
        {
            Debug.Log("PLAYGAP DEMO Claim Reward screen failed to show triggered: " + error);
        }

        private void OnRewardScreenClosed()
        {
            Debug.Log("PLAYGAP DEMO Claim Reward screen closed triggered");
        }

        private void OnStoreClick()
        {
            Debug.Log("PLAYGAP DEMO Store click triggered");
        }
        #endregion

        private void OnUserClaimedRewards(string[] rewardIds)
        {
            Debug.Log("PLAYGAP DEMO User claimed reward triggered with ids: " + rewardIds);
        }
    }
}
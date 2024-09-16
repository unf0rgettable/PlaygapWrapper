package io.playgap.sdk;

import android.app.Activity;
import android.content.Context;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.util.ArrayList;
import java.util.List;
import java.util.HashMap;
import java.lang.reflect.Type;

class PlaygapAdsBridge {

    private static final List<String> claimedOfflineRewards = new ArrayList<String>();

    public static void registerNetworkObserver(Context context, NetworkListenerBridge listener) {
        PlaygapAds.Companion.registerNetworkObserver(context, listener::onNetworkStateChanged);
    }
    
    public static void initialize(Context context, String apiKey, InitializationListenerBridge listener) {
        PlaygapAds.Companion.initialize(context, apiKey, new InitializationListener() {

            @Override
            public void onInitializationError(InitError error) {
                listener.onInitializationError(error.getType().description(context));
            }

            @Override
            public void onInitialized() {
                listener.onInitialized();
            }
        });
    }

    public static void showRewarded(Activity activity, ShowListenerBridge listener) {
        PlaygapAds.Companion.showRewarded(activity, new ShowListener() {
            @Override
            public void onUserEarnedReward(PlaygapReward playgapReward) {
                listener.onUserEarnedReward(playgapReward.getId());
            }

            @Override
            public void onShowCompleted() {
                listener.onShowCompleted();
            }

            @Override
            public void onShowPlaybackEvent(PlaybackEvent event) {
                listener.onShowPlaybackEvent(event.raw());
            }

            @Override
            public void onShowImpression(String impressionId) {
                listener.onShowImpression(impressionId);
            }

            @Override
            public void onShowFailed(ShowError error) {
                listener.onShowFailed(error.getType().description(activity));
            }
        });
    }

    public static void showInterstitial(Activity activity, ShowListenerBridge listener) {
        PlaygapAds.Companion.showInterstitial(activity, new ShowListener() {
            @Override
            public void onUserEarnedReward(PlaygapReward playgapReward) {
                listener.onUserEarnedReward(playgapReward.getId());
            }

            @Override
            public void onShowCompleted() {
                listener.onShowCompleted();
            }

            @Override
            public void onShowPlaybackEvent(PlaybackEvent event) {
                listener.onShowPlaybackEvent(event.raw());
            }

            @Override
            public void onShowImpression(String impressionId) {
                listener.onShowImpression(impressionId);
            }

            @Override
            public void onShowFailed(ShowError error) {
                listener.onShowFailed(error.getType().description(activity));
            }
        });
    }

    public static String checkRewards() {
        PlaygapRewards rewards = PlaygapAds.Companion.checkRewards();
        if (rewards == null) {
            return null;
        }

        return new Gson().toJson(rewards);
    }

    public static void claimRewards(Activity activity, ClaimRewardsListenerBridge listener) {
        PlaygapAds.Companion.claimRewards(activity, new ClaimRewardsListener() {
            
            @Override
            public void onRewardScreenShown() {
                listener.onRewardScreenShown();
            }

            @Override
            public void onRewardScreenFailed(ClaimRewardError error) {
                listener.onRewardScreenFailed(error.getType().description(activity));
            }

            @Override
            public void onRewardScreenClosed() {
                if (!claimedOfflineRewards.isEmpty()) {
                    listener.onUserClaimedRewards(claimedOfflineRewards.toArray(new String[0]));
                    claimedOfflineRewards.clear();
                }

                listener.onRewardScreenClosed();
            }

            @Override
            public void onUserClaimedReward(PlaygapReward reward) {
                claimedOfflineRewards.add(reward.getId());
            }
        });
    }

    public static void sendEvent(String eventType, String payload, String objectId) {
        Gson gson = new Gson();

        Type mapType = new TypeToken<HashMap<String, String>>(){}.getType();
        HashMap<String, String> payloadMap = gson.fromJson(payload, mapType);
        
        PlaygapAnalytics.Companion.sendEvent(eventType, payloadMap, objectId);
    }
}

interface NetworkListenerBridge {
    void onNetworkStateChanged(Boolean isConnected);
}

interface InitializationListenerBridge {
    void onInitializationError(String error);

    void onInitialized();
}

interface ShowListenerBridge {
    void onShowFailed(String error);

    void onShowImpression(String impressionId);

    void onShowPlaybackEvent(String event);

    void onShowCompleted();

    void onUserEarnedReward(String rewardId);
}

interface ClaimRewardsListenerBridge {

    void onRewardScreenShown();

    void onRewardScreenFailed(String error);

    void onRewardScreenClosed();

    void onUserClaimedRewards(String[] rewardIds);
}
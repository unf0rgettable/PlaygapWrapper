import Playgap

@objc
public final class PlaygapAdsBridge: NSObject {

    private static var showHandler: ShowHandler?
    private static var claimRewardsHandler: ClaimRewardsHandler?

    @objc
    public static func observeNetwork(observer: @escaping (Bool) -> Void) {
        PlaygapAds.networkObserver = observer
    }

    @objc
    public static func initialize(apiKey: String, completion: @escaping (Error?) -> Void) {
        PlaygapAds.initialize(apiKey: apiKey, completion: completion)
    }

    @objc
    public static func showRewarded(
        onShowFailed: @escaping (_ error: String) -> Void,
        onShowImpression: @escaping (_ impressionId: String) -> Void,
        onShowPlaybackEvent: @escaping (_ period: String) -> Void,
        onShowCompleted: @escaping () -> Void,
        onUserEarnedReward: @escaping (_ rewardId: String) -> Void
    ) {
        let handler = ShowHandler(
            showFailed: onShowFailed,
            showImpression: onShowImpression,
            showPlaybackEvent: onShowPlaybackEvent,
            showCompleted: onShowCompleted,
            userEarnedReward: onUserEarnedReward
        )

        PlaygapAds.showRewarded(from: UnityGetGLViewController(), delegate: handler)
        showHandler = handler
    }

    @objc
    public static func showInterstitial(
        onShowFailed: @escaping (_ error: String) -> Void,
        onShowImpression: @escaping (_ impressionId: String) -> Void,
        onShowPlaybackEvent: @escaping (_ period: String) -> Void,
        onShowCompleted: @escaping () -> Void,
        onUserEarnedReward: @escaping (_ rewardId: String) -> Void
    ) {
        let handler = ShowHandler(
            showFailed: onShowFailed,
            showImpression: onShowImpression,
            showPlaybackEvent: onShowPlaybackEvent,
            showCompleted: onShowCompleted,
            userEarnedReward: onUserEarnedReward
        )

        PlaygapAds.showInterstitial(from: UnityGetGLViewController(), delegate: handler)
        showHandler = handler
    }

    @objc
    public static func claimRewards(
        onRewardScreenShown: @escaping () -> Void,
        onRewardScreenFailed: @escaping (_ error: String) -> Void,
        onRewardScreenClosed: @escaping () -> Void,
        onStoreClick: @escaping () -> Void,
        onUserClaimedRewards: @escaping (_ rewardIds: [String]) -> Void
    ) {
        let handler = ClaimRewardsHandler(
            rewardScreenShown: onRewardScreenShown,
            rewardScreenFailed: onRewardScreenFailed,
            rewardScreenClosed: onRewardScreenClosed,
            storeClick: onStoreClick,
            userClaimedRewards: onUserClaimedRewards
        )

        PlaygapAds.claimRewards(from: UnityGetGLViewController(), delegate: handler)
        claimRewardsHandler = handler
    }

    @objc
    public static func checkRewards() -> String? {
        guard let rewards = PlaygapAds.checkRewards() else { return nil }

        let mapped = RewardsObjc(unclaimed: rewards.unclaimed, claimed: rewards.claimed)

        guard let json = try? JSONEncoder().encode(mapped) else { return nil }

        return String(data: json, encoding: String.Encoding.utf8)
    }

    @objc
    public static func sendEvent(
        _ eventType: String,
        _ payload: String,
        _ objectId: String
    ) {
        PlaygapAnalytics.sendEvent(eventType, adId: objectId, payloadJson: payload)
    }
}

private final class ShowHandler: ShowDelegate {

    private let showFailed: (_ error: String) -> Void
    private let showImpression: (_ impressionId: String) -> Void
    private let showPlaybackEvent: (_ period: String) -> Void
    private let showCompleted: () -> Void
    private let userEarnedReward: (_ rewardId: String) -> Void

    init(
        showFailed: @escaping (_: String) -> Void,
        showImpression: @escaping (_: String) -> Void,
        showPlaybackEvent: @escaping (_ period: String) -> Void,
        showCompleted: @escaping () -> Void,
        userEarnedReward: @escaping (_: String) -> Void
    ) {
        self.showFailed = showFailed
        self.showImpression = showImpression
        self.showPlaybackEvent = showPlaybackEvent
        self.showCompleted = showCompleted
        self.userEarnedReward = userEarnedReward
    }

    func onShowFailed(error: Playgap.PlaygapAds.ShowError) {
        showFailed(error.localizedDescription)
    }

    func onShowImpression(impressionId: String) {
        showImpression(impressionId)
    }

    func onUserEarnedReward(_ reward: Playgap.Reward) {
        userEarnedReward(reward.id)
    }

    func onShowPlaybackEvent(_ event: PlaybackEvent) {
        showPlaybackEvent(event.rawValue)
    }

    func onShowCompleted() {
        showCompleted()
    }
}

private final class ClaimRewardsHandler: ClaimRewardsDelegate {

    private let rewardScreenShown: () -> Void
    private let rewardScreenFailed: (_ error: String) -> Void
    private let rewardScreenClosed: () -> Void
    private let storeClick: () -> Void
    private let userClaimedRewards: (_ rewardIds: [String]) -> Void
    private var claimedOfflineRewards: [String] = []

    init(
        rewardScreenShown: @escaping () -> Void,
        rewardScreenFailed: @escaping (_: String) -> Void,
        rewardScreenClosed: @escaping () -> Void,
        storeClick: @escaping () -> Void,
        userClaimedRewards: @escaping (_: [String]) -> Void
    ) {
        self.rewardScreenShown = rewardScreenShown
        self.rewardScreenFailed = rewardScreenFailed
        self.rewardScreenClosed = rewardScreenClosed
        self.storeClick = storeClick
        self.userClaimedRewards = userClaimedRewards
    }

    public func onRewardScreenShown() {
        rewardScreenShown()
    }

    public func onRewardScreenFailed(_ error: Playgap.PlaygapAds.ClaimRewardsError) {
        rewardScreenFailed(error.localizedDescription)
    }

    public func onRewardScreenClosed() {
        if !claimedOfflineRewards.isEmpty {
            userClaimedRewards(claimedOfflineRewards)
            claimedOfflineRewards.removeAll()
        }

        rewardScreenClosed()
    }

    public func onStoreClick() {
        storeClick()
    }

    public func onUserClaimedReward(_ reward: Playgap.Reward) {
        claimedOfflineRewards.append(reward.id)
    }
}

struct RewardsObjc: Encodable {

    let unclaimed: [String]
    let claimed: [String]
}

import Foundation

@_cdecl("PlaygapAds_observeNetwork")
public func PlaygapAds_observeNetwork(observer: @convention(c) @escaping (Bool) -> Void) {
    PlaygapAdsBridge.observeNetwork(observer: observer)
}

@_cdecl("PlaygapAds_initialize")
public func PlaygapAds_initialize(
    apiKey: UnsafePointer<CChar>,
    completion: @convention(c) @escaping (_ error: UnsafePointer<CChar>?) -> Void
) {
    PlaygapAdsBridge.initialize(apiKey: String(cString: apiKey)) { error in
        completion(error?.localizedDescription)
    }
}

@_cdecl("PlaygapAds_showRewarded")
public func PlaygapAds_showRewarded(
    onShowFailed: @convention(c) @escaping (_ error: UnsafePointer<CChar>?) -> Void,
    onShowImpression: @convention(c) @escaping (_ impressionId: UnsafePointer<CChar>?) -> Void,
    onShowPlaybackEvent: @convention(c) @escaping (_ period: UnsafePointer<CChar>?) -> Void,
    onShowCompleted: @convention(c) @escaping () -> Void,
    onUserEarnedReward: @convention(c) @escaping (_ rewardId: UnsafePointer<CChar>?) -> Void
) {
    PlaygapAdsBridge.showRewarded(
        onShowFailed: { onShowFailed($0) },
        onShowImpression: { onShowImpression($0) },
        onShowPlaybackEvent: { onShowPlaybackEvent($0) },
        onShowCompleted: { onShowCompleted() },
        onUserEarnedReward: { onUserEarnedReward($0) }
    )
}

@_cdecl("PlaygapAds_showInterstitial")
public func PlaygapAds_showInterstitial(
    onShowFailed: @convention(c) @escaping (_ error: UnsafePointer<CChar>?) -> Void,
    onShowImpression: @convention(c) @escaping (_ impressionId: UnsafePointer<CChar>?) -> Void,
    onShowPlaybackEvent: @convention(c) @escaping (_ period: UnsafePointer<CChar>?) -> Void,
    onShowCompleted: @convention(c) @escaping () -> Void,
    onUserEarnedReward: @convention(c) @escaping (_ rewardId: UnsafePointer<CChar>?) -> Void
) {
    PlaygapAdsBridge.showInterstitial(
        onShowFailed: { onShowFailed($0) },
        onShowImpression: { onShowImpression($0) },
        onShowPlaybackEvent: { onShowPlaybackEvent($0) },
        onShowCompleted: { onShowCompleted() },
        onUserEarnedReward: { onUserEarnedReward($0) }
    )
}

@_cdecl("PlaygapAds_claimRewards")
public func PlaygapAds_claimRewards(
    onRewardScreenShown: @convention(c) @escaping () -> Void,
    onRewardScreenFailed: @convention(c) @escaping (_ error: UnsafePointer<CChar>?) -> Void,
    onRewardScreenClosed: @convention(c) @escaping () -> Void,
    onStoreClick: @convention(c) @escaping () -> Void,
    onUserClaimedRewards: @convention(c) @escaping (_ rewardId: UnsafePointer<CChar>?) -> Void
) {
    PlaygapAdsBridge.claimRewards(
        onRewardScreenShown: onRewardScreenShown,
        onRewardScreenFailed: { onRewardScreenFailed($0) },
        onRewardScreenClosed: onRewardScreenClosed,
        onStoreClick: onStoreClick,
        onUserClaimedRewards: { onUserClaimedRewards($0.joined(separator: "::")) }
    )
}

@_cdecl("PlaygapAds_checkRewards")
public func PlaygapAds_checkRewards() -> UnsafePointer<CChar>? {
    guard let json = PlaygapAdsBridge.checkRewards() else { return nil }

    return UnsafePointer(strdup(json))
}

@_cdecl("PlaygapAds_sendEvent")
public func PlaygapAds_sendEvent(
    _ eventType: UnsafePointer<CChar>,
    _ payload: UnsafePointer<CChar>,
    _ objectId: UnsafePointer<CChar>?
) {
    PlaygapAdsBridge.sendEvent(
        String(cString: eventType),
        String(cString: payload),
        objectId == nil  ? "" : String(cString: objectId!)
    )
}

namespace LittleBitGames.Ads.AdUnits
{
    public class AdErrorInfo : IAdErrorInfo
    {
        public AdErrorInfo()
        {

        }
        
        public string Message { get; private set; }
        public int MediatedNetworkErrorCode { get; private set; }
        public string MediatedNetworkErrorMessage { get; private set; }
    }
}
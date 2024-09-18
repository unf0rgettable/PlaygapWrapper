namespace LittleBitGames.Ads.AdUnits
{
    public class AdInfo : IAdInfo
    {
        public AdInfo()
        {

        }

        public string AdUnitIdentifier { get; }
        public string AdFormat { get; }
        public string NetworkName { get; }
        public string NetworkPlacement { get; }
        public string Placement { get; }
        public string CreativeIdentifier { get; }
        public double Revenue { get; }
        public string RevenuePrecision { get; }
        public string DspName { get; }
    }
}
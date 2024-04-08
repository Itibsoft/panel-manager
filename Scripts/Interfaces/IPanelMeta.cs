namespace Itibsoft.PanelManager
{
    public interface IPanelMeta
    {
        public PanelType PanelType { get; set; }
        public int Order { get; set; }
        public string AssetId { get; set; }
    }
}
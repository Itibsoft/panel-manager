namespace Itibsoft.PanelManager
{
	public interface IPanelManager
	{
		public TPanelController LoadPanel<TPanelController>() where TPanelController : IPanelController;
	}
}

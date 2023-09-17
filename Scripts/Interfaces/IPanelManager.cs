namespace Assets.Project.Scripts.UI
{
	public interface IPanelManager
	{
		public TPanelController LoadPanel<TPanelController>() where TPanelController : IPanelController;
	}
}

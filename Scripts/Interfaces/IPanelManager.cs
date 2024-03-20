using System;

namespace Itibsoft.PanelManager
{
	public interface IPanelManager
	{
		public PanelDispatcher PanelDispatcher { get; }
		public TPanelController LoadPanel<TPanelController>() where TPanelController : IPanelController;
		public void OpenPanels(params Type[] typeControllers);
		public void ClosePanels(params Type[] typeControllers);
	}
}

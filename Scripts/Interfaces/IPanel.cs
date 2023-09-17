using System;
using Project.Scripts.PanelManager.Enum;
using UnityEngine;

namespace Assets.Project.Scripts.UI
{
	public interface IPanel : IDisposable
	{
		public PanelType Type { get; }
		public void SetActive(bool isActive);
		public void SetParent(Transform parent);
		public void SetStretch();
	}
}

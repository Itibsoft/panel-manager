using System;
using UnityEngine;

namespace Itibsoft.PanelManager
{
	public interface IPanel : IDisposable
	{
		public PanelType Type { get; }
		public void SetActive(bool isActive);
		public void SetParent(Transform parent);
		public void SetStretch();
		public GameObject GetGameObject();
	}
}

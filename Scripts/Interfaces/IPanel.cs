using System;
using UnityEngine;

namespace Itibsoft.PanelManager
{
	public interface IPanel : IDisposable
	{
		public PanelAttribute Meta { get; }
		public RectTransform RectTransform { get; }
		public void SetActive(bool isActive);
		public void SetParent(Transform parent);
		public void SetStretch();
		public GameObject GetGameObject();
	}
}

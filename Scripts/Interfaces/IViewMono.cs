using System;
using a;
using UnityEngine;

namespace Itibsoft.PanelManager
{
	public interface IViewMono : IView, IDisposable
	{
		public RectTransform RectTransform { get; }
		public void SetActive(bool isActive);
		public void SetParent(Transform parent);
		public void SetStretch();
		public GameObject GetGameObject();
	}
}

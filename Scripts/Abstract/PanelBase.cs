using JetBrains.Annotations;
using UnityEngine;

namespace Itibsoft.PanelManager
{
	public abstract class PanelBase : MonoBehaviour, IPanel
	{
		public PanelState State { get; private set; } = PanelState.CLOSED;
		public PanelAttribute Meta { get; [UsedImplicitly] protected set; }
		public RectTransform RectTransform => _rectTransform;

		private RectTransform _rectTransform;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}

		public void SetActive(bool isActive)
		{
			gameObject.SetActive(isActive);

			State = isActive ? PanelState.OPENED : PanelState.CLOSED;
		}

		public void SetParent(Transform parent)
		{
			transform.SetParent(parent);
		}

		public void SetStretch()
		{
			_rectTransform.anchorMin = Vector2.zero;
			_rectTransform.anchorMax = Vector2.one;
			_rectTransform.offsetMin = Vector2.zero;
			_rectTransform.offsetMax = Vector2.zero;
			
			_rectTransform.localScale = Vector3.one;
		}

		public GameObject GetGameObject()
		{
			return gameObject;
		}

		public virtual void Dispose()
		{
			
		}
	}
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Itibsoft.PanelManager
{
    public class PanelDispatcher : MonoBehaviour
    {
        #region Fields

        #region Public Fields

        public Canvas Canvas { get; private set; }
        public CanvasScaler CanvasScaler { get; private set; }
        public GraphicRaycaster GraphicRaycaster { get; private set; }

        #endregion

        #region Serialize Fields

        [SerializeField] private RectTransform _windowContent;
        [SerializeField] private RectTransform _overlayContent;
        [SerializeField] private RectTransform _cashedContent;

        #endregion

        #region Private Fields

        private readonly Dictionary<PanelType, List<IPanel>> _contentsForPanels = new();

        #endregion

        #endregion

        #region Unity API

        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
            CanvasScaler = GetComponent<CanvasScaler>();
            GraphicRaycaster = GetComponent<GraphicRaycaster>();

            _windowContent ??= CreateContent("Windows [Content]");
            _overlayContent ??= CreateContent("Overlays [Content]");
            _cashedContent ??= CreateContent("Cached [Content]");
        }

        #endregion

        #region Public Methods

        public void SetWindow(IPanel panel) => SetPanelToContent(panel, true);
        public void SetOverlay(IPanel panel) => SetPanelToContent(panel, true);
        public void Cache(IPanel panel) => SetPanelToContent(panel, false);
        public void Release(IPanel panel) => _contentsForPanels[panel.Meta.PanelType].Remove(panel);

        #endregion

        #region Private Methods

        private void SetPanelToContent(IPanel panel, bool isOpen)
        {
            RectTransform content;
            var orderedPanels = new List<IPanel>();

            if (isOpen)
            {
                panel.SetActive(true);

                _contentsForPanels.GetOrCreateNew(PanelType.Cached).Remove(panel);
                _contentsForPanels.AddOrCreateNew(panel.Meta.PanelType, panel, out var panels);

                content = panel.Meta.PanelType switch
                {
                    PanelType.Window => _windowContent,
                    PanelType.Overlay => _overlayContent,
                    _ => default
                };

                orderedPanels = panels.OrderBy(panelOrdered => panelOrdered.Meta.Order).ToList();
            }
            else
            {
                _contentsForPanels.AddOrCreateNew(PanelType.Cached, panel);
                _contentsForPanels.GetOrCreateNew(panel.Meta.PanelType).Remove(panel);

                content = _cashedContent;
                panel.SetActive(false);
            }

            panel.SetParent(content);
            panel.SetStretch();

            for (int index = 0, count = orderedPanels.Count; index < count; index++)
            {
                var panelOrdered = orderedPanels[index];
                panelOrdered.RectTransform.SetSiblingIndex(index);
            }
        }

        private RectTransform CreateContent(string nameContent)
        {
            var group = new GameObject(nameContent, typeof(RectTransform));
            group.transform.SetParent(transform);

            var rectTransform = group.GetComponent<RectTransform>();

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            rectTransform.localScale = Vector3.one;

            return rectTransform;
        }

        #endregion
    }
}
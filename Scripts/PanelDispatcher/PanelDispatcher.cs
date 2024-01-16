using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Itibsoft.PanelManager
{
    public class PanelDispatcher : MonoBehaviour
    {
        [SerializeField] private RectTransform _windowContent;
        [SerializeField] private RectTransform _overlayContent;
        [SerializeField] private RectTransform _cashedContent;

        private readonly Dictionary<PanelType, List<IPanel>> _contentsForPanels = new();

        public static PanelDispatcher Create()
        {
            var components = new[]
            {
                typeof(PanelDispatcher),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster)
            };

            var instance = new GameObject("[PanelDispatcher]", components)
            {
                transform =
                {
                    position = Vector3.zero,
                    rotation = Quaternion.identity,
                    localScale = Vector3.zero
                }
            };

            var canvas = instance.GetComponent<Canvas>();
            var canvasScaler = instance.GetComponent<CanvasScaler>();
            var graphicRaycaster = instance.GetComponent<GraphicRaycaster>();
            var dispatcher = instance.GetComponent<PanelDispatcher>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = false;

            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0.5f;
            canvasScaler.referencePixelsPerUnit = 100;

            graphicRaycaster.ignoreReversedGraphics = true;
            graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

            CreateGroup("Windows [Content]", out dispatcher._windowContent);
            CreateGroup("Overlays [Content]", out dispatcher._overlayContent);
            CreateGroup("Cached [Content]", out dispatcher._cashedContent);

            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem.transform.SetParent(instance.transform);

            DontDestroyOnLoad(instance);

            return instance.GetComponent<PanelDispatcher>();

            void CreateGroup(string name, out RectTransform field)
            {
                var group = new GameObject(name, typeof(RectTransform));
                group.transform.SetParent(instance.transform);

                field = (RectTransform)group.transform;

                var rectTransform = group.GetComponent<RectTransform>();

                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;

                rectTransform.localScale = Vector3.one;
            }
        }

        public void SetWindow(IPanel panel) => PanelForContent(panel, true);
        public void SetOverlay(IPanel panel) => PanelForContent(panel, true);
        public void Cache(IPanel panel) => PanelForContent(panel, false);

        public void Release(IPanel panel) => _contentsForPanels[panel.Meta.PanelType].Remove(panel);

        private void PanelForContent(IPanel panel, bool isOpen)
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
    }
}
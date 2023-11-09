using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Itibsoft.PanelManager
{
    public class PanelDispatcher : MonoBehaviour
    {
        [SerializeField] private Transform _windowContent;
        [SerializeField] private Transform _overlayContent;
        [SerializeField] private Transform _cashedContent;

        private RectTransform _rectTransform;

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

            CreateGroup("Windows [Content]", instance.transform, out dispatcher._windowContent);
            CreateGroup("Overlays [Content]", instance.transform, out dispatcher._overlayContent);
            CreateGroup("Cached [Content]", instance.transform, out dispatcher._cashedContent);

            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem.transform.SetParent(instance.transform);

            DontDestroyOnLoad(instance);

            return instance.GetComponent<PanelDispatcher>();
        }

        private static void CreateGroup(string name, Component instance, out Transform field)
        {
            var group = new GameObject(name, typeof(RectTransform));
            group.transform.SetParent(instance.transform);

            field = group.transform;

            var rectTransform = group.GetComponent<RectTransform>();

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            rectTransform.localScale = Vector3.one;
        }

        public void SetWindow(IPanel panel)
        {
            panel.SetParent(_windowContent);
            panel.SetStretch();
            panel.SetActive(true);
            panel.SetOrder(panel.Meta.Order);
        }

        public void SetOverlay(IPanel panel)
        {
            panel.SetParent(_overlayContent);
            panel.SetStretch();
            panel.SetActive(true);
            panel.SetOrder(panel.Meta.Order);
        }

        public void Cache(IPanel panel)
        {
            panel.SetParent(_cashedContent);
            panel.SetActive(false);
        }
    }
}
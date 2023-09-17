using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Itibsoft.PanelManager
{
    public class PanelDispatcher : MonoBehaviour
    {
        private Transform _windowContent;
        private Transform _overlayContent;
        private Transform _cashedContent;

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

            CreateGroup("Windows [Content]", "_windowContent");
            CreateGroup("Overlays [Content]", "_overlayContent");
            CreateGroup("Cached [Content]", "_cashedContent");

            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem.transform.SetParent(instance.transform);
            
            DontDestroyOnLoad(instance);

            return instance.GetComponent<PanelDispatcher>();

            void CreateGroup(string name, string fieldName)
            {
                var group = new GameObject(name, typeof(RectTransform));
                group.transform.SetParent(instance.transform);

                var field = dispatcher.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null) field.SetValue(dispatcher, group.transform);

                var rectTransform = group.GetComponent<RectTransform>();
                
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
			
                rectTransform.localScale = Vector3.one;
            }
        }

        public void SetWindow(IPanel panel)
        {
            panel.SetParent(_windowContent);
            panel.SetStretch();
            panel.SetActive(true);
        }

        public void SetOverlay(IPanel panel)
        {
            panel.SetParent(_overlayContent);
            panel.SetStretch();
            panel.SetActive(true);
        }

        public void Cache(IPanel panel)
        {
            panel.SetParent(_cashedContent);
            panel.SetActive(false);
        }
    }
}
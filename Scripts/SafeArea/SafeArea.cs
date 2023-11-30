using UnityEngine;

namespace Itibsoft.PanelManager
{
    public class SafeArea : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Vector2Int _lastScreenSize = new(0, 0);
        private ScreenOrientation _lastOrientation = ScreenOrientation.AutoRotation;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (_rectTransform == null)
            {
                Debug.LogError("Cannot apply safe area - no RectTransform found on " + name);
                Destroy(gameObject);
            }

            Refresh();
        }

        private void Update()
        {
            var safeArea = Screen.safeArea;
            ApplySafeArea(Rect rect);
        }

        private void Refresh()
        {
           

            if (Screen.width != _lastScreenSize.x
                || Screen.height != _lastScreenSize.y
                || Screen.orientation != _lastOrientation)
            {
                // Fix for having auto-rotate off and manually forcing a screen orientation.
                // See https://forum.unity.com/threads/569236/#post-4473253 and https://forum.unity.com/threads/569236/page-2#post-5166467
                _lastScreenSize.x = Screen.width;
                _lastScreenSize.y = Screen.height;
                _lastOrientation = Screen.orientation;

                ApplySafeArea(safeArea);
            }
        }

        private void ApplySafeArea(Rect rect)
        {
            // Check for invalid screen startup state on some Samsung devices (see below)
            if (Screen.width > 0 && Screen.height > 0)
            {
                // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
                var anchorMin = rect.position;
                var anchorMax = rect.position + rect.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                // Fix for some Samsung devices (e.g. Note 10+, A71, S20) where Refresh gets called twice and the first time returns NaN anchor coordinates
                // See https://forum.unity.com/threads/569236/page-2#post-6199352
                if (anchorMin is { x: >= 0, y: >= 0 } && anchorMax is { x: >= 0, y: >= 0 })
                {
                    _rectTransform.anchorMin = anchorMin;
                    _rectTransform.anchorMax = anchorMax;
                }
            }
        }
    }
}
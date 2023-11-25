using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Itibsoft.PanelManager.Editor
{
    public static class MenuOptionsUtils
    {
        private const string K_UI_LAYER_NAME = "UI";

        public static Canvas CreateCanvas()
        {
            var root = ObjectFactory.CreateGameObject
            (
                "Canvas",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster)
            );

            root.layer = LayerMask.NameToLayer(K_UI_LAYER_NAME);

            var canvas = root.GetComponent<Canvas>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            StageUtility.PlaceGameObjectInCurrentStage(root);

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage != default)
            {
                Undo.SetTransformParent(root.transform, prefabStage.prefabContentsRoot.transform, "");
            }

            Undo.SetCurrentGroupName("Create " + root.name);

            return canvas;
        }
    }
}
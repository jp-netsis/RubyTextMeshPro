using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TMPro.EditorUtilities
{
    public static class RubyTMPro_CreateObjectMenu
    {
        private const string UI_LAYER_NAME = "UI";

        private const string STANDARD_SPRITE_PATH = "UI/Skin/UISprite.psd";
        private const string BACKGROUND_SPRITE_PATH = "UI/Skin/Background.psd";
        private const string INPUT_FIELD_BACKGROUND_PATH = "UI/Skin/InputFieldBackground.psd";
        private const string KNOB_PATH_PATH = "UI/Skin/Knob.psd";
        private const string CHECKMARK_PATH = "UI/Skin/Checkmark.psd";
        private const string DROPDOWN_ARROW_PATH = "UI/Skin/DropdownArrow.psd";
        private const string MASK_PATH = "UI/Skin/UIMask.psd";

        private static RubyTMP_DefaultControls.Resources standardResources;

        /// <summary>
        /// Create a TextMeshPro object that works with the Mesh Renderer
        /// </summary>
        /// <param name="command"></param>
        [MenuItem("GameObject/3D Object/Text - RubyTextMeshPro", false, 30)]
        private static void CreateTextMeshProObjectPerform(MenuCommand command)
        {
            GameObject go = new GameObject("Text (RubyTMP)");

            // Add support for new prefab mode
            StageUtility.PlaceGameObjectInCurrentStage(go);

            RubyTextMeshPro textMeshPro = go.AddComponent<RubyTextMeshPro>();
            textMeshPro.text = textMeshPro.uneditedText = "Sample text";
            textMeshPro.alignment = TextAlignmentOptions.TopLeft;

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

            GameObject contextObject = command.context as GameObject;

            if (contextObject != null)
            {
                GameObjectUtility.SetParentAndAlign(go, contextObject);
                Undo.SetTransformParent(go.transform, contextObject.transform, "Parent " + go.name);
            }

            Selection.activeGameObject = go;
        }

        /// <summary>
        /// Create a TextMeshPro object that works with the CanvasRenderer
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem("GameObject/UI/Text - RubyTextMeshPro", false, 2001)]
        private static void CreateTextMeshProGuiObjectPerform(MenuCommand menuCommand)
        {
            GameObject go = RubyTMP_DefaultControls.CreateText(RubyTMPro_CreateObjectMenu.GetStandardResources());

            // Override text color and font size
            TMP_Text textComponent = go.GetComponent<TMP_Text>();
            textComponent.color = Color.white;
//            if (textComponent.m_isWaitingOnResourceLoad == false)
            textComponent.fontSize = TMP_Settings.defaultFontSize;

            RubyTMPro_CreateObjectMenu.PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Button - RubyTextMeshPro", false, 2031)]
        public static void AddButton(MenuCommand menuCommand)
        {
            GameObject go = RubyTMP_DefaultControls.CreateButton(RubyTMPro_CreateObjectMenu.GetStandardResources());

            // Override font size
            TMP_Text textComponent = go.GetComponentInChildren<TMP_Text>();
            textComponent.fontSize = 24;

            RubyTMPro_CreateObjectMenu.PlaceUIElementRoot(go, menuCommand);
        }

        private static RubyTMP_DefaultControls.Resources GetStandardResources()
        {
            if (RubyTMPro_CreateObjectMenu.standardResources.standard == null)
            {
                RubyTMPro_CreateObjectMenu.standardResources.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>(RubyTMPro_CreateObjectMenu.STANDARD_SPRITE_PATH);
                RubyTMPro_CreateObjectMenu.standardResources.background = AssetDatabase.GetBuiltinExtraResource<Sprite>(RubyTMPro_CreateObjectMenu.BACKGROUND_SPRITE_PATH);
                RubyTMPro_CreateObjectMenu.standardResources.inputField = AssetDatabase.GetBuiltinExtraResource<Sprite>(RubyTMPro_CreateObjectMenu.INPUT_FIELD_BACKGROUND_PATH);
                RubyTMPro_CreateObjectMenu.standardResources.knob = AssetDatabase.GetBuiltinExtraResource<Sprite>(RubyTMPro_CreateObjectMenu.KNOB_PATH_PATH);
                RubyTMPro_CreateObjectMenu.standardResources.checkmark = AssetDatabase.GetBuiltinExtraResource<Sprite>(RubyTMPro_CreateObjectMenu.CHECKMARK_PATH);
                RubyTMPro_CreateObjectMenu.standardResources.dropdown = AssetDatabase.GetBuiltinExtraResource<Sprite>(RubyTMPro_CreateObjectMenu.DROPDOWN_ARROW_PATH);
                RubyTMPro_CreateObjectMenu.standardResources.mask = AssetDatabase.GetBuiltinExtraResource<Sprite>(RubyTMPro_CreateObjectMenu.MASK_PATH);
            }

            return RubyTMPro_CreateObjectMenu.standardResources;
        }

        private static void SetPositionVisibleInSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
        {
            // Find the best scene view
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView == null && SceneView.sceneViews.Count > 0)
            {
                sceneView = SceneView.sceneViews[0] as SceneView;
            }

            // Couldn't find a SceneView. Don't set position.
            if (sceneView == null || sceneView.camera == null)
            {
                return;
            }

            // Create world space Plane from canvas position.
            Camera camera = sceneView.camera;
            Vector3 position = Vector3.zero;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2f, camera.pixelHeight / 2f), camera, out Vector2 localPlanePosition))
            {
                // Adjust for canvas pivot
                localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
                localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

                localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
                localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

                // Adjust for anchoring
                Vector2 sizeDelta = canvasRTransform.sizeDelta;
                Vector2 anchorMin = itemTransform.anchorMin;
                position.x = localPlanePosition.x - sizeDelta.x * anchorMin.x;
                position.y = localPlanePosition.y - sizeDelta.y * anchorMin.y;

                Vector3 minLocalPosition;
                minLocalPosition.x = sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
                minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

                Vector3 maxLocalPosition;
                maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
                maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

                position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
                position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
            }

            itemTransform.anchoredPosition = position;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.localScale = Vector3.one;
        }

        private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
        {
            GameObject parent = menuCommand.context as GameObject;
            bool explicitParentChoice = true;

            if (parent == null)
            {
                parent = RubyTMPro_CreateObjectMenu.GetOrCreateCanvasGameObject();
                explicitParentChoice = false;

                // If in Prefab Mode, Canvas has to be part of Prefab contents,
                // otherwise use Prefab root instead.
                PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

                if (prefabStage != null && !prefabStage.IsPartOfPrefabContents(parent))
                {
                    parent = prefabStage.prefabContentsRoot;
                }
            }

            if (parent.GetComponentInParent<Canvas>() == null)
            {
                // Create canvas under context GameObject,
                // and make that be the parent which UI element is added under.
                GameObject canvas = RubyTMPro_CreateObjectMenu.CreateNewUI();
                canvas.transform.SetParent(parent.transform, false);
                parent = canvas;
            }

            // Setting the element to be a child of an element already in the scene should
            // be sufficient to also move the element to that scene.
            // However, it seems the element needs to be already in its destination scene when the
            // RegisterCreatedObjectUndo is performed; otherwise the scene it was created in is dirtied.
            SceneManager.MoveGameObjectToScene(element, parent.scene);

            if (element.transform.parent == null)
            {
                Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
            }

            GameObjectUtility.EnsureUniqueNameForSibling(element);

            // We have to fix up the undo name since the name of the object was only known after change parent it.
            Undo.SetCurrentGroupName("Create " + element.name);

            GameObjectUtility.SetParentAndAlign(element, parent);

            if (!explicitParentChoice) // not a context click, so center in SceneView
            {
                RubyTMPro_CreateObjectMenu.SetPositionVisibleInSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());
            }

            Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);

            Selection.activeGameObject = element;
        }

        private static GameObject CreateNewUI()
        {
            // Root for the UI
            GameObject root = new GameObject("Canvas")
            {
                layer = LayerMask.NameToLayer(RubyTMPro_CreateObjectMenu.UI_LAYER_NAME)
            };
            Canvas canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            root.AddComponent<CanvasScaler>();
            root.AddComponent<GraphicRaycaster>();

            // Works for all stages.
            StageUtility.PlaceGameObjectInCurrentStage(root);
            bool customScene = false;
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage != null)
            {
                root.transform.SetParent(prefabStage.prefabContentsRoot.transform, false);
                customScene = true;
            }

            Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

            // If there is no event system add one...
            // No need to place event system in custom scene as these are temporary anyway.
            // It can be argued for or against placing it in the user scenes,
            // but let's not modify scene user is not currently looking at.
            if (!customScene)
            {
                RubyTMPro_CreateObjectMenu.CreateEventSystem(false);
            }

            return root;
        }

        private static void CreateEventSystem(bool select, GameObject parent = null)
        {
            EventSystem eventSystem = Object.FindObjectOfType<EventSystem>();

            if (eventSystem == null)
            {
                GameObject goEventSystem = new GameObject("EventSystem");
                GameObjectUtility.SetParentAndAlign(goEventSystem, parent);
                eventSystem = goEventSystem.AddComponent<EventSystem>();
                goEventSystem.AddComponent<StandaloneInputModule>();

                Undo.RegisterCreatedObjectUndo(goEventSystem, "Create " + goEventSystem.name);
            }

            if (select && eventSystem != null)
            {
                Selection.activeGameObject = eventSystem.gameObject;
            }
        }

        // Helper function that returns a Canvas GameObject; preferably a parent of the selection, or other existing Canvas.
        private static GameObject GetOrCreateCanvasGameObject()
        {
            GameObject selectedGo = Selection.activeGameObject;

            // Try to find a gameObject that is the selected GO or one if its parents.
            Canvas canvas = selectedGo != null ? selectedGo.GetComponentInParent<Canvas>() : null;

            if (canvas == null)
            {
                return null;
            }

            if (RubyTMPro_CreateObjectMenu.IsValidCanvas(canvas))
            {
                return canvas.gameObject;
            }

            // No canvas in selection or its parents? Then use any valid canvas.
            // We have to find all loaded Canvases, not just the ones in main scenes.
            Canvas[] canvasArray = StageUtility.GetCurrentStageHandle().FindComponentsOfType<Canvas>();

            foreach (Canvas t in canvasArray)
            {
                if (RubyTMPro_CreateObjectMenu.IsValidCanvas(t))
                {
                    return t.gameObject;
                }
            }

            // No canvas in the scene at all? Then create a new one.
            return RubyTMPro_CreateObjectMenu.CreateNewUI();
        }

        private static bool IsValidCanvas(Canvas canvas)
        {
            if (canvas == null || !canvas.gameObject.activeInHierarchy)
            {
                return false;
            }

            // It's important that the non-editable canvas from a prefab scene won't be rejected,
            // but canvases not visible in the Hierarchy at all do. Don't check for HideAndDontSave.
            if (EditorUtility.IsPersistent(canvas) || (canvas.hideFlags & HideFlags.HideInHierarchy) != 0)
            {
                return false;
            }

            if (StageUtility.GetStageHandle(canvas.gameObject) != StageUtility.GetCurrentStageHandle())
            {
                return false;
            }

            return true;
        }
    }
}
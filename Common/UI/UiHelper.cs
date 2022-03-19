namespace Common.UI
{
    using System;
    using Bowser.Core;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    // Helpful discussion on transforms:
    // https://forum.unity.com/threads/whats-the-best-practice-for-moving-recttransforms-in-script.264495
    internal class UiHelper
    {
        public const int DefaultButtonFontSize = 7;
        public const int DefaultLabelFontSize = 5;
        public const int DefaultMenuHeaderFontSize = 10;
        public const float DefaultButtonZShift = -0.1f;
        public const float DefaultTextZShift = -0.2f;

        private const int VrUiCollisionLayer = 5;
        private const int HangoutsPointerCollisionLayer = 30;
        private const int HangoutsSceneIndex = 43;

        private static UiHelper _instance;

        public DemeoResource DemeoResource { get; }

        internal enum UiType
        {
            Vr,
            Hangouts,
        }

        public static UiHelper Instance()
        {
            if (_instance != null)
            {
                _instance.DemeoResource.Initialize();
                return _instance;
            }

            if (!CommonModule.IsInitialized)
            {
                throw new InvalidOperationException("Common module is not initialized.");
            }

            if (!IsReady())
            {
                throw new InvalidOperationException("UI dependencies not yet ready.");
            }

            _instance = new UiHelper(DemeoResource.Instance());
            return _instance;
        }

        private UiHelper(DemeoResource demeoResource)
        {
            DemeoResource = demeoResource;
        }

        /// <summary>
        /// Returns true when the helper is fully initialized with required Demeo resources.
        /// </summary>
        public static bool IsReady()
        {
            return CommonModule.IsInitialized && DemeoResource.IsReady() && IsReadyForHangouts();
        }

        private static bool IsReadyForHangouts()
        {
            if (GetCurrentUiType() != UiType.Hangouts)
            {
                return true;
            }

            return CommonModule.HangoutsButtonHandler != null;
        }

        public static UiType GetCurrentUiType()
        {
            if (SceneManager.GetActiveScene().buildIndex == HangoutsSceneIndex)
            {
                return UiType.Hangouts;
            }

            return UiType.Vr;
        }

        public GameObject CreateButtonText(string text)
        {
            return CreateText(text, DemeoResource.ColorBeige, fontSize: DefaultButtonFontSize);
        }

        public GameObject CreateLabelText(string text)
        {
            return CreateText(text, DemeoResource.ColorBrown, fontSize: DefaultLabelFontSize);
        }

        public GameObject CreateMenuHeaderText(string text)
        {
            return CreateText(text, DemeoResource.ColorBeige, fontSize: DefaultMenuHeaderFontSize);
        }

        /// <summary>
        /// Creates a text object.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     The text is pre-shifted <c>-0.2</c> along the Z-axis, so as to appear slightly in front of non-text
        ///     objects by default.
        ///     </para>
        ///     <para>
        ///     The <c>transform</c> of the returned object may be casted as a <see cref="RectTransform"/>.
        ///     </para>
        /// </remarks>
        /// <returns>The GameObject containing the text component.</returns>
        public GameObject CreateText(string text, Color color, int fontSize)
        {
            var textObject = new GameObject("Text");

            var textMeshPro = textObject.AddComponent<TextMeshPro>();
            textMeshPro.font = DemeoResource.Font;
            textMeshPro.fontStyle = FontStyles.Normal;
            textMeshPro.text = text;
            textMeshPro.color = color;
            textMeshPro.colorGradientPreset = DemeoResource.FontColorGradient;
            textMeshPro.alignment = TextAlignmentOptions.Center;
            textMeshPro.fontSize = fontSize;
            textMeshPro.fontSizeMax = fontSize;
            textMeshPro.fontSizeMin = 1;
            textMeshPro.enableAutoSizing = true;
            textMeshPro.transform.localPosition = new Vector3(0, 0, DefaultTextZShift);

            return textObject;
        }

        public GameObject CreateButton(Action callback)
        {
            if (GetCurrentUiType() == UiType.Hangouts)
            {
                return WrapObject(CreateHangoutsButton(callback));
            }

            return WrapObject(CreateVrUiButton(callback));
        }

        /// <summary>
        /// Creates a button to function in Demeo VR UI.
        /// </summary>
        private GameObject CreateVrUiButton(Action callback)
        {
            var buttonObject = new GameObject("Button");
            buttonObject.transform.localRotation = Quaternion.Euler(270, 0, 0); // Align object.
            buttonObject.layer = VrUiCollisionLayer;

            buttonObject.AddComponent<MeshFilter>().mesh = DemeoResource.ButtonMeshBlue;
            buttonObject.AddComponent<MeshRenderer>().material = DemeoResource.ButtonMaterial;

            var menuButtonHoverEffect = buttonObject.AddComponent<MenuButtonHoverEffect>();
            menuButtonHoverEffect.hoverMaterial = DemeoResource.ButtonHoverMaterial;
            menuButtonHoverEffect.Init();

            // Added after HoverMaterial to enable effect.
            buttonObject.AddComponent<ClickableButton>().InitButton(0, string.Empty, callback, false);

            // Added last to allow ray to hit full object.
            buttonObject.AddComponent<BoxCollider>();

            return buttonObject;
        }

        /// <summary>
        /// Creates a button to function in Demeo Hangouts.
        /// </summary>
        private GameObject CreateHangoutsButton(Action callback)
        {
            var buttonObject = CreateVrUiButton(callback);
            buttonObject.layer = HangoutsPointerCollisionLayer;

            var buttonData = buttonObject.AddComponent<BowserButtonData>();
            buttonData.buttonVisuals = buttonObject;
            buttonData.hoverMat = DemeoResource.ButtonHoverMaterial;
            buttonData.isLocalPress = true;
            buttonData.pointerOnly = true;
            buttonData.pressedPosition = buttonData.visualsIdlePosition;
            buttonData.pressEffect = BowserButtonData.PressEffect.HoverSize;
            buttonData.pressHaptic = BowserButtonData.HapticEffect.Mini;
            buttonData.pressSound = BowserButtonData.SoundEffect.Generic2d;

            CommonModule.HangoutsButtonHandler.RegisterBowserButton(buttonData, delegate { callback(); });
            return buttonObject;
        }

        /// <summary>
        /// Wraps the specified GameObject inside a new <see cref="GameObject"/>.
        /// </summary>
        /// <remarks>
        /// Useful for preserving the composition and layout of the original GameObject.
        /// </remarks>
        /// <returns>The GameObject whose child is the specified GameObject.</returns>
        private static GameObject WrapObject(GameObject child)
        {
            var container = new GameObject($"{child.name}Wrapper");
            child.transform.SetParent(container.transform);
            return container;
        }
    }
}

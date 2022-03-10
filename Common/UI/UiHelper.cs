namespace Common.UI
{
    using System;
    using TMPro;
    using UnityEngine;

    // Helpful discussion on transforms:
    // https://forum.unity.com/threads/whats-the-best-practice-for-moving-recttransforms-in-script.264495
    internal class UiHelper
    {
        public const int DefaultButtonFontSize = 7;
        public const int DefaultLabelFontSize = 5;
        public const int DefaultMenuHeaderFontSize = 10;
        public const float DefaultButtonZShift = -0.1f;
        public const float DefaultTextZShift = -0.2f;

        private static UiHelper _instance;

        public DemeoResource DemeoResource { get; }

        public static UiHelper Instance()
        {
            if (_instance != null)
            {
                _instance.DemeoResource.EnsureResourcesExists();
                return _instance;
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
            return DemeoResource.IsReady();
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

        public GameObject CreateButton(Action callback)
        {
            var buttonObject = new GameObject("Button");
            buttonObject.transform.localRotation = Quaternion.Euler(270, 0, 0); // Align object.
            buttonObject.layer = 5; // UI layer.

            buttonObject.AddComponent<MeshFilter>().mesh = DemeoResource.ButtonMeshBlue;
            buttonObject.AddComponent<MeshRenderer>().material = DemeoResource.ButtonMaterial;

            var menuButtonHoverEffect = buttonObject.AddComponent<MenuButtonHoverEffect>();
            menuButtonHoverEffect.hoverMaterial = DemeoResource.ButtonHoverMaterial;
            menuButtonHoverEffect.Init();

            // Added after HoverMaterial to enable effect.
            buttonObject.AddComponent<ClickableButton>().InitButton(0, string.Empty, callback, false);

            // Added last to allow ray to hit full object.
            buttonObject.AddComponent<BoxCollider>();

            return WrapObject(buttonObject);
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

        /// <summary>
        /// Wraps the specified GameObject inside a new <see cref="GameObject"/>.
        /// </summary>
        /// <remarks>
        /// Useful for preserving the composition and layout of the original GameObject.
        /// </remarks>
        /// <returns>The GameObject whose child is the specified GameObject.</returns>
        public static GameObject WrapObject(GameObject child)
        {
            var container = new GameObject($"{child.name}Wrapper");
            child.transform.SetParent(container.transform);
            return container;
        }
    }
}

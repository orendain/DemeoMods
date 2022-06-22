namespace Common.UI.Element
{
    using System;
    using TMPro;
    using UnityEngine;

    // Helpful discussion on transforms:
    // https://forum.unity.com/threads/whats-the-best-practice-for-moving-recttransforms-in-script.264495
    internal class VrElementCreator : IElementCreator
    {
        public const int DefaultLabelFontSize = 5;
        public const int DefaultMenuHeaderFontSize = 10;
        public const float DefaultButtonZShift = -0.1f;
        public const float DefaultTextZShift = -0.2f;
        private const int CollisionLayer = 5;

        private static VrElementCreator _instance;

        private readonly VrResourceTable _resourceTable;

        public static VrElementCreator Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (!IsReady())
            {
                throw new InvalidOperationException("UI dependencies not met.");
            }

            _instance = new VrElementCreator(VrResourceTable.Instance());
            return _instance;
        }

        private VrElementCreator(VrResourceTable resourceTable)
        {
            _resourceTable = resourceTable;
        }

        /// <summary>
        /// Returns true if the all UI dependencies are met.
        /// </summary>
        internal static bool IsReady()
        {
            return VrResourceTable.IsReady();
        }

        public int DefaultButtonFontSize() => 7;

        public GameObject CreateNormalText(string text)
        {
            return CreateText(text, VrResourceTable.ColorBrown, fontSize: DefaultLabelFontSize);
        }

        public GameObject CreateButtonText(string text)
        {
            return CreateText(text, VrResourceTable.ColorBeige, fontSize: DefaultButtonFontSize());
        }

        public GameObject CreateMenuHeaderText(string text)
        {
            return CreateText(text, VrResourceTable.ColorBeige, fontSize: DefaultMenuHeaderFontSize);
        }

        /// <summary>
        /// Creates a generic text element.
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
            var container = new GameObject("Text");

            var buttonText = container.AddComponent<TextMeshPro>();
            buttonText.font = _resourceTable.Font;
            buttonText.fontStyle = FontStyles.Normal;
            buttonText.text = text;
            buttonText.color = color;
            buttonText.colorGradientPreset = _resourceTable.FontColorGradient;
            buttonText.alignment = TextAlignmentOptions.Center;
            buttonText.fontSize = fontSize;
            buttonText.fontSizeMax = fontSize;
            buttonText.fontSizeMin = 1;
            buttonText.enableAutoSizing = true;
            buttonText.transform.localPosition = new Vector3(0, 0, DefaultTextZShift);

            return container;
        }

        public GameObject CreateButton(Action callback)
        {
            return WrapObject(CreateButtonRaw(callback));
        }

        /// <summary>
        /// Creates a button to function in Demeo VR UI.
        /// </summary>
        internal GameObject CreateButtonRaw(Action callback)
        {
            var button = new GameObject("Button");
            button.transform.localRotation = Quaternion.Euler(270, 0, 0); // Align object.
            button.layer = CollisionLayer;

            button.AddComponent<MeshFilter>().mesh = _resourceTable.ButtonMeshBlue;
            button.AddComponent<MeshRenderer>().material = _resourceTable.ButtonMaterial;

            var menuButtonHoverEffect = button.AddComponent<MenuButtonHoverEffect>();
            menuButtonHoverEffect.hoverMaterial = _resourceTable.ButtonHoverMaterial;
            menuButtonHoverEffect.Init();

            // Added after HoverMaterial to enable effect.
            button.AddComponent<ClickableButton>().InitButton(0, string.Empty, callback, false);

            // Added last to allow ray to hit full object.
            button.AddComponent<BoxCollider>();

            return button;
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

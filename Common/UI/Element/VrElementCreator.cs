namespace Common.UI.Element
{
    using System;
    using TMPro;
    using UnityEngine;

    // Helpful discussion on transforms:
    // https://forum.unity.com/threads/whats-the-best-practice-for-moving-recttransforms-in-script.264495
    internal class VrElementCreator : IElementCreator
    {
        public const int NormalFontSize = 5;
        public const int ButtonFontSize = 7;
        public const int HeaderFontSize = 10;
        public const float TextZShift = -0.2f;
        public const float ButtonZShift = -0.1f;
        private const int CollisionLayer = 5;

        public static readonly Color ColorBeige = new Color(0.878f, 0.752f, 0.384f, 1);
        public static readonly Color ColorBrown = new Color(0.0392f, 0.0157f, 0, 1);

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

        public GameObject CreateLeftText(string text)
        {
            return CreateListText(text);
        }

        public GameObject CreateNewText(string text)
        {
            return CreateMyText(text);
        }

        public GameObject CreateNormalText(string text)
        {
            return CreateText(text, ColorBrown, fontSize: NormalFontSize);
        }

        public GameObject CreateButtonText(string text)
        {
            return CreateText(text, ColorBeige, fontSize: ButtonFontSize);
        }

        public GameObject CreateMenuHeaderText(string text)
        {
            return CreateText(text, Color.cyan, fontSize: HeaderFontSize);
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

            var textComponent = container.AddComponent<TextMeshPro>();
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.color = color;
            textComponent.colorGradientPreset = _resourceTable.FontColorGradient;
            textComponent.enableAutoSizing = true;
            textComponent.font = _resourceTable.Font;
            textComponent.fontSize = fontSize;
            textComponent.fontSizeMax = fontSize;
            textComponent.fontSizeMin = 1;
            textComponent.fontStyle = FontStyles.Normal;
            textComponent.text = text;
            textComponent.transform.localPosition = new Vector3(0, 0, TextZShift);

            return container;
        }

        public GameObject CreateMyText(string text)
        {
            var container = new GameObject("Text");

            var textComponent = container.AddComponent<TextMeshPro>();
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.colorGradientPreset = _resourceTable.FontColorGradient;
            textComponent.enableAutoSizing = true;
            textComponent.font = _resourceTable.Font;
            textComponent.fontSize = 6;
            textComponent.fontSizeMax = 6;
            textComponent.fontSizeMin = 5;
            textComponent.fontStyle = FontStyles.Bold;
            textComponent.text = text;
            textComponent.transform.localPosition = new Vector3(0, 0, TextZShift);

            return container;
        }

        public GameObject CreateListText(string text)
        {
            var container = new GameObject("Text");

            var textComponent = container.AddComponent<TextMeshPro>();
            textComponent.alignment = TextAlignmentOptions.Left;
            textComponent.colorGradientPreset = _resourceTable.FontColorGradient;
            textComponent.enableAutoSizing = false;
            textComponent.font = _resourceTable.Font;
            textComponent.fontSize = 6;
            textComponent.fontSizeMax = 6;
            textComponent.fontSizeMin = 5;
            textComponent.fontStyle = FontStyles.Normal;
            textComponent.text = text;
            textComponent.transform.localPosition = new Vector3(0, 0, TextZShift);

            return container;
        }

        public GameObject CreateButton(Action callback)
        {
            return WrapObject(CreateButtonRaw(callback));
        }

        internal GameObject CreateButtonRaw(Action callback)
        {
            var button = new GameObject("Button");
            button.transform.localRotation = Quaternion.Euler(270, 0, 0); // Align object.
            button.layer = CollisionLayer;

            button.AddComponent<MeshFilter>().mesh = _resourceTable.ButtonMeshBlue;
            button.AddComponent<MeshRenderer>().material = _resourceTable.ButtonMaterial;

            var hoverEffect = button.AddComponent<MenuButtonHoverEffect>();
            hoverEffect.hoverMaterial = _resourceTable.ButtonMaterialHover;
            hoverEffect.Init();

            // Added after HoverMaterial to enable effect.
            var clickableButton = button.AddComponent<ClickableButton>();
            clickableButton.InitButton(0, string.Empty, callback, false);
            clickableButton.enabled = true;

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

namespace Common.UI.Element
{
    using System;
    using Boardgame.NonVR.Ui;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    internal class NonVrElementCreator : IElementCreator
    {
        public const int DefaultLabelFontSize = 25;
        public const int DefaultMenuHeaderFontSize = 50;
        private const int CollisionLayer = 5;

        private static NonVrElementCreator _instance;

        private readonly NonVrResourceTable _resourceTable;

        public static NonVrElementCreator Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (!IsReady())
            {
                throw new InvalidOperationException("UI dependencies not met.");
            }

            _instance = new NonVrElementCreator(NonVrResourceTable.Instance());
            return _instance;
        }

        private NonVrElementCreator(NonVrResourceTable nonVrResourceTable)
        {
            _resourceTable = nonVrResourceTable;
        }

        /// <summary>
        /// Returns true if the all UI dependencies are met.
        /// </summary>
        internal static bool IsReady()
        {
            return NonVrResourceTable.IsReady();
        }

        public int DefaultButtonFontSize() => 35;

        public GameObject CreateNormalText(string text)
        {
            return CreateText(text, _resourceTable.ColorBrown, fontSize: DefaultLabelFontSize);
        }

        public GameObject CreateButtonText(string text)
        {
            return CreateText(text, _resourceTable.ColorBeige, fontSize: DefaultButtonFontSize());
        }

        public GameObject CreateMenuHeaderText(string text)
        {
            return CreateText(text, _resourceTable.ColorBeige, fontSize: DefaultMenuHeaderFontSize);
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
            var container = new GameObject("Text");

            var buttonText = container.AddComponent<TextMeshProUGUI>();
            buttonText.alignment = TextAlignmentOptions.Center;
            buttonText.color = color;
            buttonText.enableAutoSizing = true;
            buttonText.fontSize = fontSize;
            buttonText.fontSizeMax = fontSize;
            buttonText.fontSizeMin = 1;
            buttonText.text = text;

            container.GetComponent<Graphic>().raycastTarget = false;

            return container;
        }

        public GameObject CreateButton(Action callback)
        {
            return WrapObject(CreateButtonRaw(callback));
        }

        /// <summary>
        /// Creates a button to function in Demeo VR UI.
        /// </summary>
        private GameObject CreateButtonRaw(Action callback)
        {
            var button = new GameObject("Button");
            button.layer = CollisionLayer;

            var nonVrButton = button.AddComponent<NonVrButton>();
            nonVrButton.AssignClickCallback(callback);
            nonVrButton.GetComponent<Button>().interactable = true;

            var normalContainer = new GameObject("Normal");
            normalContainer.transform.SetParent(button.transform, worldPositionStays: false);

            var buttonBackground = new GameObject("Image");
            buttonBackground.transform.SetParent(normalContainer.transform, worldPositionStays: false);

            var imageComponent = buttonBackground.AddComponent<Image>();
            imageComponent.sprite = _resourceTable.ButtonBlueNormal;

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

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

        public GameObject CreateText(string text, Color color, int fontSize)
        {
            var container = new GameObject("Text");

            var textComponent = container.AddComponent<TextMeshProUGUI>();
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.color = color;
            textComponent.enableAutoSizing = true;
            textComponent.fontSize = fontSize;
            textComponent.fontSizeMax = fontSize;
            textComponent.fontSizeMin = 1;
            textComponent.text = text;

            container.GetComponent<Graphic>().raycastTarget = false;

            return container;
        }

        public GameObject CreateButton(Action callback)
        {
            return WrapObject(CreateButtonRaw(callback));
        }

        private GameObject CreateButtonRaw(Action callback)
        {
            var container = new GameObject("Button");
            container.layer = CollisionLayer;

            var button = container.AddComponent<NonVrButton>();
            button.AssignClickCallback(callback);
            button.GetComponent<Button>().interactable = true;

            var normalContainer = new GameObject("Normal");
            normalContainer.transform.SetParent(container.transform, worldPositionStays: false);

            var normalImage = new GameObject("Image");
            normalImage.transform.SetParent(normalContainer.transform, worldPositionStays: false);
            normalImage.AddComponent<Image>().sprite = _resourceTable.ButtonBlueNormal;

            return container;
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

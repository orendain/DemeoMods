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

        private static UiHelper _instance;

        public DemeoResource DemeoResource { get; }

        public static UiHelper Instance()
        {
            if (_instance != null)
            {
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
            var buttonObject = new GameObject($"Button");
            buttonObject.transform.localRotation = Quaternion.Euler(0, 180, 0); // Un-reverse button from its default.
            buttonObject.layer = 5; // UI layer.

            buttonObject.AddComponent<MeshFilter>().mesh = DemeoResource.ButtonMesh;
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

        public GameObject CreateText(string text, Color color, int fontSize)
        {
            var textObject = new GameObject($"Text");

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
            textMeshPro.enableAutoSizing = false;
            textMeshPro.transform.localPosition = new Vector3(0, 0, -0.2f);

            return WrapObject(textObject);
        }

        public static GameObject WrapObject(GameObject child)
        {
            var container = new GameObject($"{child.name}Wrapper");
            child.transform.SetParent(container.transform);
            return container;
        }
    }
}

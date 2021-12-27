using System;
using TMPro;
using UnityEngine;

namespace Common.Ui
{
    // Helpful discussion on transforms: https://forum.unity.com/threads/whats-the-best-practice-for-moving-recttransforms-in-script.264495
    internal static class UiUtil
    {
        public static readonly int DefaultButtonFontSize = 7;
        public static readonly int DefaultLabelFontSize = 5;
        public static readonly int DefaultMenuHeaderFontSize = 10;
        
        public static GameObject CreateButtonText(string text)
        {
            return CreateText(text, DemeoUi.DemeoColorBeige, fontSize:DefaultButtonFontSize);
        }

        public static GameObject CreateLabelText(string text)
        {
            return CreateText(text, DemeoUi.DemeoColorBrown, fontSize:DefaultLabelFontSize);
        }

        public static GameObject CreateMenuHeaderText(string text)
        {
            return CreateText(text, DemeoUi.DemeoColorBeige, fontSize: DefaultMenuHeaderFontSize);
        }
        
        public static GameObject CreateButton(Action callback)
        {
            var buttonObject = new GameObject($"Button");
            buttonObject.transform.localRotation = Quaternion.Euler(0, 180, 0); // Un-reverse button from its default.
            buttonObject.layer = 5; // UI layer.
            
            buttonObject.AddComponent<MeshFilter>().mesh = DemeoUi.DemeoButtonMesh;
            buttonObject.AddComponent<MeshRenderer>().material = DemeoUi.DemeoButtonMaterial;
            
            var menuButtonHoverEffect = buttonObject.AddComponent<MenuButtonHoverEffect>();
            menuButtonHoverEffect.hoverMaterial = DemeoUi.DemeoButtonHoverMaterial;
            menuButtonHoverEffect.Init();
            
            // Added after HoverMaterial to enable effect.
            buttonObject.AddComponent<ClickableButton>().InitButton(0, "", callback, false);
            
            // Added last to allow ray to hit full object.
            buttonObject.AddComponent<BoxCollider>();

            return WrapObject(buttonObject);
        }
        
        public static GameObject CreateText(string text, Color color, int fontSize)
        {
            var textObject = new GameObject($"Text");
            
            var textMeshPro = textObject.AddComponent<TextMeshPro>();
            textMeshPro.font = DemeoUi.DemeoFont;
            textMeshPro.fontStyle = FontStyles.Normal;
            textMeshPro.text = text;
            textMeshPro.color = color;
            textMeshPro.colorGradientPreset = DemeoUi.DemeoFontColorGradient;
            textMeshPro.alignment = TextAlignmentOptions.Center;
            textMeshPro.fontSize = fontSize;
            textMeshPro.fontSizeMax = fontSize;
            textMeshPro.fontSizeMin = 1;
            textMeshPro.enableAutoSizing = true;
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

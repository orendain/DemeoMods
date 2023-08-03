namespace Common.UI.Element
{
    using System;
    using System.Text;
    using Boardgame.NonVR.Ui;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    internal class NonVrElementCreator : IElementCreator
    {
        public const int NormalFontSize = 22;
        public const int ButtonFontSize = 35;
        public const int HeaderFontSize = 36;
        private const int CollisionLayer = 5;

        public static readonly Color ColorBeige = new Color(0.878f, 0.752f, 0.384f, 1);
        public static readonly Color ColorBrown = new Color(0.0392f, 0.0157f, 0, 1);

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
            return CreateText(text, ColorBrown, fontSize: HeaderFontSize);
        }

        public GameObject CreateText(string text, Color color, int fontSize)
        {
            var setName = text;
            if (setName.Equals("None"))
            {
                setName = "<color=#B31106FF><b>None</b></color>";
            }
            else if (fontSize == ButtonFontSize)
            {
                fontSize = 18;
                if (setName.Contains("Demeo Revolutions"))
                {
                    if (setName.Contains("(EASY"))
                    {
                        setName = "<color=#00FF00><b>Demeo Revolutions</b></color>\n<color=#567FFD><b>(EASY)</b></color>";
                    }
                    else if (setName.Contains("(HARD"))
                    {
                        setName = "<color=#00FF00><b>Demeo Revolutions</b></color>\n<color=#8156FD><b>(HARD)</b></color>";
                    }
                    else if (setName.Contains("(LEGENDARY PROGRESSIVE"))
                    {
                        fontSize = 16;
                        setName = "<color=#00FF00><b>Demeo Revolutions</b></color>\n<color=#D456FD><b>(LEGENDARY PROGRESSIVE)</b></color>";
                    }
                    else if (setName.Contains("(LEGENDARY"))
                    {
                        setName = "<color=#00FF00><b>Demeo Revolutions</b></color>\n<color=#D456FD><b>(LEGENDARY)</b></color>";
                    }
                    else if (setName.Contains("(SMALL"))
                    {
                        fontSize = 16;
                        setName = "<color=#00FF00><b>Demeo Revolutions</b></color>\n<color=#567FFD><b>(SMALL PROGRESSIVE)</b></color>";
                    }
                    else if (setName.Contains("PROGRESSIVE"))
                    {
                        setName = "<color=#00FF00><b>Demeo Revolutions</b></color>\n<color=#FD56D3><b>(PROGRESSIVE)</b></color>";
                    }
                    else
                    {
                        fontSize = 22;
                        setName = "<color=#00FF00><b>Demeo Revolutions</b></color>";
                    }
                }
            }

            var container = new GameObject("Text");

            var textComponent = container.AddComponent<TextMeshProUGUI>();
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.color = color;
            textComponent.enableAutoSizing = true;
            textComponent.fontSize = fontSize;
            textComponent.fontSizeMax = fontSize;
            textComponent.fontSizeMin = 1;
            textComponent.text = setName;

            container.GetComponent<Graphic>().raycastTarget = false;

            return container;
        }

        public GameObject CreateMyText(string text)
        {
            var container = new GameObject("Text");

            var textComponent = container.AddComponent<TextMeshPro>();
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.enableAutoSizing = true;
            textComponent.fontSize = NormalFontSize;
            textComponent.fontSizeMax = NormalFontSize;
            textComponent.fontSizeMin = 1;
            textComponent.fontStyle = FontStyles.Normal;
            textComponent.text = text;

            container.GetComponent<Graphic>().raycastTarget = false;

            return container;
        }

        public GameObject CreateListText(string text)
        {
            var container = new GameObject("Text");

            var textComponent = container.AddComponent<TextMeshPro>();
            textComponent.alignment = TextAlignmentOptions.Left;
            textComponent.enableAutoSizing = false;
            textComponent.fontSize = 6;
            textComponent.fontSizeMax = 6;
            textComponent.fontSizeMin = 5;
            textComponent.fontStyle = FontStyles.Normal;
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

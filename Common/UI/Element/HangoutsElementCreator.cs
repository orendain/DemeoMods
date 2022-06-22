namespace Common.UI.Element
{
    using System;
    using Bowser.Core;
    using UnityEngine;

    internal class HangoutsElementCreator : IElementCreator
    {
        private const int CollisionLayer = 30;

        private static HangoutsElementCreator _instance;

        private readonly VrResourceTable _resourceTable;
        private readonly VrElementCreator _vrElementCreator;

        public static HangoutsElementCreator Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (!IsReady())
            {
                throw new InvalidOperationException("UI dependencies not met.");
            }

            _instance = new HangoutsElementCreator(VrResourceTable.Instance(), VrElementCreator.Instance());
            return _instance;
        }

        private HangoutsElementCreator(VrResourceTable resourceTable, VrElementCreator elementCreator)
        {
            _resourceTable = resourceTable;
            _vrElementCreator = elementCreator;
        }

        /// <summary>
        /// Returns true if the all UI dependencies are met.
        /// </summary>
        internal static bool IsReady()
        {
            CommonModule.Logger.Msg(VrElementCreator.IsReady());
            CommonModule.Logger.Msg(CommonModule.HangoutsButtonHandler != null);
            return VrElementCreator.IsReady()
                   && CommonModule.HangoutsButtonHandler != null;
        }

        public int DefaultButtonFontSize() => _vrElementCreator.DefaultButtonFontSize();

        public GameObject CreateNormalText(string text)
        {
            return _vrElementCreator.CreateNormalText(text);
        }

        public GameObject CreateButtonText(string text)
        {
            return _vrElementCreator.CreateButtonText(text);
        }

        public GameObject CreateMenuHeaderText(string text)
        {
            return _vrElementCreator.CreateMenuHeaderText(text);
        }

        public GameObject CreateText(string text, Color color, int fontSize)
        {
            return _vrElementCreator.CreateText(text, color, fontSize);
        }

        public GameObject CreateButton(Action callback)
        {
            return WrapObject(CreateButtonRaw(callback));
        }

        /// <summary>
        /// Creates a button to function in Demeo Hangouts.
        /// </summary>
        private GameObject CreateButtonRaw(Action callback)
        {
            var button = _vrElementCreator.CreateButtonRaw(callback);
            button.layer = CollisionLayer;

            var buttonData = button.AddComponent<BowserButtonData>();
            buttonData.buttonVisuals = button;
            buttonData.hoverMat = _resourceTable.ButtonHoverMaterial;
            buttonData.isLocalPress = true;
            buttonData.pointerOnly = true;
            buttonData.pressedPosition = buttonData.visualsIdlePosition;
            buttonData.pressEffect = BowserButtonData.PressEffect.HoverSize;
            buttonData.pressHaptic = BowserButtonData.HapticEffect.Mini;
            buttonData.pressSound = BowserButtonData.SoundEffect.Generic2d;

            CommonModule.HangoutsButtonHandler.RegisterBowserButton(buttonData, delegate { callback(); });
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

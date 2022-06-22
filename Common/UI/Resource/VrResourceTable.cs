namespace Common.UI
{
    using System;
    using System.Linq;
    using TMPro;
    using UnityEngine;

    internal class VrResourceTable
    {
        private static VrResourceTable _instance;

        internal static Color ColorBrown { get; } = new Color(0.0392f, 0.0157f, 0, 1);

        internal static Color ColorBeige { get; } = new Color(0.878f, 0.752f, 0.384f, 1);

        public TMP_FontAsset Font { get; private set; }

        public TMP_ColorGradient FontColorGradient { get; private set; }

        public Mesh ButtonMeshBlue { get; private set; }

        public Mesh ButtonMeshBrown { get; private set; }

        public Mesh ButtonMeshRed { get; private set; }

        public Material ButtonMaterial { get; private set; }

        public Material ButtonHoverMaterial { get; private set; }

        public Mesh MenuBoxMesh { get; private set; }

        public Material MenuBoxMaterial { get; private set; }

        public static VrResourceTable Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (!IsReady())
            {
                throw new InvalidOperationException("VR UI resources not yet available.");
            }

            _instance = new VrResourceTable();
            return _instance;
        }

        private VrResourceTable()
        {
            Refresh();
        }

        /// <summary>
        /// Returns true if the all UI dependencies are met.
        /// </summary>
        internal static bool IsReady()
        {
            return Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Any(x => x.name == "Demeo SDF")
                   && Resources.FindObjectsOfTypeAll<TMP_ColorGradient>().Any(x => x.name == "Demeo - Main Menu Buttons")
                   && Resources.FindObjectsOfTypeAll<Mesh>().Any(x => x.name == "UIMenuMainButton")
                   && Resources.FindObjectsOfTypeAll<Material>().Any(x => x.name == "MainMenuMat")
                   && Resources.FindObjectsOfTypeAll<Material>().Any(x => x.name == "MainMenuHover")
                   && Resources.FindObjectsOfTypeAll<Mesh>().Any(x => x.name == "MenuBox_SettingsButton")
                   && Resources.FindObjectsOfTypeAll<Material>().Any(x => x.name == "MainMenuMat (Instance)");
        }

        protected internal void Refresh()
        {
            Font = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First(x => x.name == "Demeo SDF");
            FontColorGradient = Resources
                .FindObjectsOfTypeAll<TMP_ColorGradient>()
                .First(x => x.name == "Demeo - Main Menu Buttons");
            ButtonMeshBlue = Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "UIMainButtonBlue");
            ButtonMeshBrown = Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "UIMainButtonBrown");
            ButtonMeshRed = Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "UIMainButtonRed");
            ButtonMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "MainMenuMat");
            ButtonHoverMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "MainMenuHover");
            MenuBoxMesh = Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "MenuBox_SettingsButton");
            MenuBoxMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "MainMenuMat (Instance)");
        }
    }
}

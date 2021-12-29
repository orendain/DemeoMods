namespace Common.UI
{
    using System;
    using System.Linq;
    using TMPro;
    using UnityEngine;

    internal class DemeoResource
    {
        private static DemeoResource _instance;

        public Color ColorBrown { get; } = new Color(0.0392f, 0.0157f, 0, 1);

        public Color ColorBeige { get; } = new Color(0.878f, 0.752f, 0.384f, 1);

        public Component LobbyAnchor { get; } = Resources.FindObjectsOfTypeAll<charactersoundlistener>()
            .First(x => x.name == "MenuBox_BindPose");

        public TMP_FontAsset Font { get; } =
            Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First(x => x.name == "Demeo SDF");

        public TMP_ColorGradient FontColorGradient { get; } = Resources
            .FindObjectsOfTypeAll<TMP_ColorGradient>()
            .First(x => x.name == "Demeo - Main Menu Buttons");

        public Mesh ButtonMesh { get; } =
            Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "UIMenuMainButton");

        public Material ButtonMaterial { get; } =
            Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "MainMenuMat");

        public Material ButtonHoverMaterial { get; } =
            Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "MainMenuHover");

        public Mesh MenuBoxMesh { get; } =
            Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "MenuBox_SettingsButton");

        public Material MenuBoxMaterial { get; } = Resources.FindObjectsOfTypeAll<Material>()
            .First(x => x.name == "MainMenuMat (Instance)");

        public static DemeoResource Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (!IsReady())
            {
                throw new InvalidOperationException("Demeo UI resources not yet available.");
            }

            _instance = new DemeoResource();
            return _instance;
        }

        private DemeoResource()
        {
        }

        public static bool IsReady()
        {
            return Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Any(x => x.name == "Demeo SDF")
                   && Resources.FindObjectsOfTypeAll<TMP_ColorGradient>().Any(x => x.name == "Demeo - Main Menu Buttons")
                   && Resources.FindObjectsOfTypeAll<Mesh>().Any(x => x.name == "UIMenuMainButton")
                   && Resources.FindObjectsOfTypeAll<Material>().Any(x => x.name == "MainMenuMat")
                   && Resources.FindObjectsOfTypeAll<Material>().Any(x => x.name == "MainMenuHover")
                   && Resources.FindObjectsOfTypeAll<Mesh>().Any(x => x.name == "MenuBox_SettingsButton")
                   && Resources.FindObjectsOfTypeAll<Material>().Any(x => x.name == "MainMenuMat (Instance)")
                   && Resources.FindObjectsOfTypeAll<charactersoundlistener>()
                       .Count(x => x.name == "MenuBox_BindPose") > 1;
        }
    }
}

namespace Common.Ui
{
    using System;
    using System.Linq;
    using TMPro;
    using UnityEngine;

    // TODO(orendain): Rename to DemeoResources.
    internal class DemeoUi
    {
        public readonly Color DemeoColorBrown = new Color(0.0392f, 0.0157f, 0, 1);
        public readonly Color DemeoColorBeige = new Color(0.878f, 0.752f, 0.384f, 1);

        public readonly Component DemeoLobbyAnchor = Resources.FindObjectsOfTypeAll<charactersoundlistener>()
            .First(x => x.name == "MenuBox_BindPose");

        public readonly TMP_FontAsset DemeoFont =
            Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First(x => x.name == "Demeo SDF");

        public readonly TMP_ColorGradient DemeoFontColorGradient = Resources
            .FindObjectsOfTypeAll<TMP_ColorGradient>()
            .First(x => x.name == "Demeo - Main Menu Buttons");

        public readonly Mesh DemeoButtonMesh =
            Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "UIMenuMainButton");

        public readonly Material DemeoButtonMaterial =
            Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "MainMenuMat");

        public readonly Material DemeoButtonHoverMaterial =
            Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "MainMenuHover");

        public readonly Mesh DemeoMenuBoxMesh =
            Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "MenuBox_SettingsButton");

        public readonly Material DemeoMenuBoxMaterial = Resources.FindObjectsOfTypeAll<Material>()
            .First(x => x.name == "MainMenuMat (Instance)");

        private static DemeoUi _instance;

        public static DemeoUi Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (!IsReady())
            {
                throw new InvalidOperationException("Demeo UI resources not yet available.");
            }

            _instance = new DemeoUi();
            return _instance;
        }

        private DemeoUi()
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

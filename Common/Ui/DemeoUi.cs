using System.Linq;
using TMPro;
using UnityEngine;

namespace Common.Ui
{
    internal static class DemeoUi
    {
        public static readonly Color DemeoColorBrown = new Color(0.0392f, 0.0157f, 0, 1);
        public static readonly Color DemeoColorBeige = new Color(0.878f, 0.752f, 0.384f, 1);

        public static readonly Component DemeoLobbyAnchor = Resources.FindObjectsOfTypeAll<charactersoundlistener>()
            .First(x => x.name == "MenuBox_BindPose");

        public static readonly TMP_FontAsset DemeoFont =
            Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First(x => x.name == "Demeo SDF");

        public static readonly TMP_ColorGradient DemeoFontColorGradient = Resources
            .FindObjectsOfTypeAll<TMP_ColorGradient>()
            .First(x => x.name == "Demeo - Main Menu Buttons");

        public static readonly Mesh DemeoButtonMesh =
            Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "UIMenuMainButton");

        public static readonly Material DemeoButtonMaterial =
            Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "MainMenuMat");

        public static readonly Material DemeoButtonHoverMaterial =
            Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "MainMenuHover");

        public static readonly Mesh DemeoMenuBoxMesh =
            Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "MenuBox_SettingsButton");

        public static readonly Material DemeoMenuBoxMaterial = Resources.FindObjectsOfTypeAll<Material>()
            .First(x => x.name == "MainMenuMat (Instance)");
    }
}
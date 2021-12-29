using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Common.Ui
{
    // TODO(orendain): Rename to DemeoResources.
    internal class DemeoUi
    {
        private static DemeoUi instance;

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

        public static DemeoUi Instance()
        {
            if (instance != null)
            {
                return instance;
            }

            if (!IsReady())
            {
                throw new InvalidOperationException("Demeo UI resources not yet available.");
            }

            instance = new DemeoUi();
            return instance;
        }

        private DemeoUi()
        {
        }

        public static bool IsReady()
        {
            return Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(x => x.name == "Demeo SDF").Count() > 0
                   && Resources
                       .FindObjectsOfTypeAll<TMP_ColorGradient>()
                       .Where(x => x.name == "Demeo - Main Menu Buttons").Count() > 0
                   && Resources.FindObjectsOfTypeAll<Mesh>().Where(x => x.name == "UIMenuMainButton").Count() > 0
                   && Resources.FindObjectsOfTypeAll<Material>().Where(x => x.name == "MainMenuMat").Count() > 0
                   && Resources.FindObjectsOfTypeAll<Material>().Where(x => x.name == "MainMenuHover").Count() > 0
                   && Resources.FindObjectsOfTypeAll<Mesh>().Where(x => x.name == "MenuBox_SettingsButton").Count() > 0
                   && Resources.FindObjectsOfTypeAll<Material>()
                       .Where(x => x.name == "MainMenuMat (Instance)").Count() > 0
                   && Resources.FindObjectsOfTypeAll<charactersoundlistener>()
                       .Where(x => x.name == "MenuBox_BindPose").Count() > 1;
        }
    }
}

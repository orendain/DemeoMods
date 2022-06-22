namespace Common.UI
{
    using System;
    using System.Linq;
    using UnityEngine;

    internal class NonVrResourceTable
    {
        private static NonVrResourceTable _instance;

        public Color ColorBrown { get; } = new Color(0.0392f, 0.0157f, 0, 1);

        public Color ColorBeige { get; } = new Color(0.878f, 0.752f, 0.384f, 1);

        public Sprite ButtonBlueNormal { get; private set; }

        public Sprite ButtonRedNormal { get; private set; }

        public Sprite PaperDecorated { get; private set; }

        public GameObject AnchorDesktopMainMenu { get; private set; }

        public GameObject AnchorDesktopPages { get; private set; }

        public GameObject AnchorNavigationBar { get; private set; }

        public static NonVrResourceTable Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (!IsReady())
            {
                throw new InvalidOperationException("NonVR UI resources not yet available.");
            }

            _instance = new NonVrResourceTable();
            return _instance;
        }

        private NonVrResourceTable()
        {
            Refresh();
        }

        /// <summary>
        /// Returns true if the all UI dependencies are met.
        /// </summary>
        internal static bool IsReady()
        {
            return Resources.FindObjectsOfTypeAll<Sprite>().Any(x => x.name == "ButtonMenuBlue")
                   && Resources.FindObjectsOfTypeAll<Sprite>().Any(x => x.name == "ButtonMenuRed")
                   && Resources.FindObjectsOfTypeAll<Sprite>().Any(x => x.name == "PaperDecorated")
                   && Resources.FindObjectsOfTypeAll<GameObject>().Any(x => x.name == "DesktopMainMenu(Clone)")
                   && Resources.FindObjectsOfTypeAll<GameObject>().Any(x => x.name == "Pages")
                   && Resources.FindObjectsOfTypeAll<GameObject>().Any(x => x.name == "NavigationBar");
        }

        private void Refresh()
        {
            ButtonBlueNormal = Resources.FindObjectsOfTypeAll<Sprite>().First(x => x.name == "ButtonMenuBlue");
            ButtonRedNormal = Resources.FindObjectsOfTypeAll<Sprite>().First(x => x.name == "ButtonMenuRed");
            PaperDecorated = Resources.FindObjectsOfTypeAll<Sprite>().First(x => x.name == "PaperDecorated");
            AnchorDesktopMainMenu = Resources.FindObjectsOfTypeAll<GameObject>()
                .First(x => x.name == "DesktopMainMenu(Clone)");
            AnchorDesktopPages = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "Pages");
            AnchorNavigationBar = Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == "NavigationBar");
        }
    }
}

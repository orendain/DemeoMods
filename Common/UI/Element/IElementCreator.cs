namespace Common.UI.Element
{
    using System;
    using System.Text;
    using UnityEngine;

    public interface IElementCreator
    {
        /// <summary>
        /// Creates a details text element.
        /// </summary>
        GameObject CreateLeftText(string text);

        /// <summary>
        /// Creates a details text element.
        /// </summary>
        GameObject CreateNewText(string text);

        /// <summary>
        /// Creates a normal text element.
        /// </summary>
        GameObject CreateNormalText(string text);

        /// <summary>
        /// Creates a button text element.
        /// </summary>
        GameObject CreateButtonText(string text);

        /// <summary>
        /// Creates a menu header element.
        /// </summary>
        GameObject CreateMenuHeaderText(string text);

        /// <summary>
        /// Creates a generic text element.
        /// </summary>
        GameObject CreateText(string text, Color color, int fontSize);

        /// <summary>
        /// Creates a my stringbuilder element.
        /// </summary>
        GameObject CreateMyText(string text);

        /// <summary>
        /// Creates a my stringbuilder element.
        /// </summary>
        GameObject CreateListText(string text);

        /// <summary>
        /// Creates a button with the specified callback.
        /// </summary>
        GameObject CreateButton(Action callback);
    }
}

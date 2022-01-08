using System;
using System.Collections.Generic;
using Common.UI;
using UnityEngine;

// TODO(orendain): Consider rending background as part of a stack.
// TODO(orendain): Consider not forcing a single sized stack frame.
internal class PageStack
{

    private GameObject _gameObject;
    private GameObject _background;

    private int currentPage;
    private List<GameObject> pages;

    private GameObject _stackPanel;
    private UiHelper _uiHelper;

    public static PageStack newInstance(string menuTitle, UiHelper uiHelper)
    {
        return new PageStack(menuTitle, uiHelper);
    }

    private PageStack(string menuTitle, UiHelper uiHelper)
    {
        _gameObject = new GameObject("PageStack");
        pages = new List<GameObject>();
        currentPage = 0;
        this._uiHelper = uiHelper;
    }

    private void Initialize()
    {
        _gameObject.transform.SetParent(_uiHelper.DemeoResource.LobbyAnchor.transform, worldPositionStays: true);
        _gameObject.transform.position = new Vector3(25, 30, 0);

        _background = new GameObject("PageStackBackground");
        _background.AddComponent<MeshFilter>().mesh = _uiHelper.DemeoResource.MenuBoxMesh;
        _background.AddComponent<MeshRenderer>().material = _uiHelper.DemeoResource.MenuBoxMaterial;

        _background.transform.SetParent(this.transform, worldPositionStays: false);
        _background.transform.localPosition = new Vector3(0, -3.6f, 0);
        _background.transform.localRotation =
            Quaternion.Euler(-90, 0, 0); // Un-flip card from it's default face-up position.
        _background.transform.localScale = new Vector3(2, 1, 2.5f);

        var menuTitle = _uiHelper.CreateMenuHeaderText("Public Rooms");
        menuTitle.transform.SetParent(this.transform, worldPositionStays: false);
        menuTitle.transform.localPosition = new Vector3(0, 2.375f, 0);

        var refreshButton = _uiHelper.CreateButton(RefreshRoomList);
        refreshButton.transform.SetParent(this.transform, worldPositionStays: false);
        refreshButton.transform.localPosition = new Vector3(0, 0.3f, 0);

        var refreshText = _uiHelper.CreateButtonText("Refresh");
        refreshText.transform.SetParent(this.transform, worldPositionStays: false);
        refreshText.transform.localPosition = new Vector3(0, 0.3f, 0);

        // TODO(orendain): Fix so that ray interacts with entire object.
        this.gameObject.AddComponent<BoxCollider>();

        _isInitialized = true;
    }

    /** Adds the specified page to the stack.
     * When adding, this method sets the argument's transform set parent with the argument worldPositionStays set to false.
     * As a result, any relative positioning set on the page before calling method is preserved.
     * The page's localRotation is reset.
     */
    public void AddPage(GameObject page)
    {
        pages.Add(page);
        page.transform.SetParent(_stackPanel.transform, worldPositionStays: false);
        page.transform.localRotation = Quaternion.Euler(0,0,0);
    }

    private void OnPreviousPageClick()
    {
        if (this.currentPage <= 1)
            return;

        --this.currentPage;
        SetActivePage(this.currentPage);
    }

    private void OnNextPageClick()
    {
        if (this.currentPage >= this.pages.Count)
            return;

        ++this.currentPage;
        SetActivePage(this.currentPage);
    }

    private void SetActivePage(int activePage)
    {
        this.currentPage = activePage;
        Render();
    }

    private void Render()
    {
        pages.ForEach(p => p.SetActive(false));
        pages[currentPage].SetActive(true);

        // TODO(orendain): Destroy and recreate.
        RenderNavigationButtons();
    }

    private void RenderNavigationButtons()
    {
        var nav = new GameObject("PageStackNavigation");
        nav.transform.SetParent(nav.transform, worldPositionStays: false);
        nav.transform.localPosition = new Vector3(0, 5, 0);

        var prevButton = _uiHelper.CreateButton(OnPreviousPageClick);
        prevButton.transform.SetParent(nav.transform, worldPositionStays: false);
        prevButton.transform.localPosition = new Vector3(0, 0, 0);

        var pageStatusText = string.Format("%s/%s", currentPage, pages.Count);
        var pageStatus = _uiHelper.CreateLabelText(pageStatusText);
        pageStatus.transform.SetParent(nav.transform, worldPositionStays:false);
        pageStatus.transform.localPosition = new Vector3(1, 0, 0);

        var nextButton = _uiHelper.CreateButton(OnNextPageClick);
        nextButton.transform.SetParent(nav.transform, worldPositionStays: false);
        nextButton.transform.localPosition = new Vector3(2, 0, 0);
    }
}

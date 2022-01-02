using System;
using System.Collections.Generic;
using Common.UI;
using UnityEngine;

// TODO(orendain): Consider rending background as part of a stack.
// TODO(orendain): Consider not forcing a single sized stack frame.
internal class PageStack
{

    private GameObject _gameObject;
    private int currentPage;
    private List<GameObject> pages;

    private GameObject _stackPanel;
    private UiHelper uiHelper;

    public static PageStack newInstance(UiHelper uiHelper)
    {
        return new PageStack(uiHelper);
    }

    private PageStack(UiHelper uiHelper)
    {
        _gameObject = new GameObject("PageStack");
        pages = new List<GameObject>();
        currentPage = 0;
        this.uiHelper = uiHelper;
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

        var prevButton = uiHelper.CreateButton(OnPreviousPageClick);
        prevButton.transform.SetParent(nav.transform, worldPositionStays: false);
        prevButton.transform.localPosition = new Vector3(0, 0, 0);

        var pageStatusText = string.Format("%s/%s", currentPage, pages.Count);
        var pageStatus = uiHelper.CreateLabelText(pageStatusText);
        pageStatus.transform.SetParent(nav.transform, worldPositionStays:false);
        pageStatus.transform.localPosition = new Vector3(1, 0, 0);

        var nextButton = uiHelper.CreateButton(OnNextPageClick);
        nextButton.transform.SetParent(nav.transform, worldPositionStays: false);
        nextButton.transform.localPosition = new Vector3(2, 0, 0);
    }
}

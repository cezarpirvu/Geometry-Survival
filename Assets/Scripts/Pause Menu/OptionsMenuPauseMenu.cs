using UnityEngine;

public class OptionsMenuPauseMenu : MonoBehaviour
{

    private Canvas optionsMenu;
    private CanvasGroup canvasGroup;

    // Use this for initialization
    void Start()
    {
        // the options menu canvas from the pause menu
        optionsMenu = GetComponent<Canvas>();
        canvasGroup = optionsMenu.GetComponent<CanvasGroup>();

        // hide the options menu
        optionsMenu.sortingOrder = 0;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }

    // check if the options menu from the pause menu should be shown
    public void enableOptionsMenu(bool check)
    {
        // the options menu canvas from the pause menu
        optionsMenu = GetComponent<Canvas>();
        canvasGroup = optionsMenu.GetComponent<CanvasGroup>();

        // get the parent
        GameObject parent = transform.parent.gameObject;

        // hide the pause menu if the options menu must be shown
        if (check)
        {
            // hide the pause menu
            parent.GetComponent<Canvas>().sortingOrder = 0;
            parent.GetComponent<CanvasGroup>().alpha = 0;
            parent.GetComponent<CanvasGroup>().interactable = false;

            // show the options menu
            optionsMenu.sortingOrder = 1;
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
        } else
        {
            // hide the options menu
            optionsMenu.sortingOrder = 0;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;

            // show the pause menu
            parent.GetComponent<Canvas>().sortingOrder = 1;
            parent.GetComponent<CanvasGroup>().alpha = 1;
            parent.GetComponent<CanvasGroup>().interactable = true;
        }
    }
}

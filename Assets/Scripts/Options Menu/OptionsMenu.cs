using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

    // Use this for initialization
    void Start ()
    {
        // the options menu
        Canvas canvas = GetComponent<Canvas>();
        CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();

        // set the order so that the options menu always show second
        canvas.sortingOrder = 0;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
}

using UnityEngine;
using System.Collections;

public class HighscoresMenu : MonoBehaviour {

    // instantiate the class so that it can be reffered to from other scripts
    public static HighscoresMenu instance;

    // when the script instance is awaken, store the instance class (used for references in scripts)
    void Awake ()
    {
        instance = this;
    }

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

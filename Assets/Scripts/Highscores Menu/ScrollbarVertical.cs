using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollbarVertical : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
        // reference to the scrollbar and the leaderboard gameobjects
        GameObject scrollbar = GameObject.FindGameObjectWithTag("Scrollbar");
        GameObject leaderboard = GameObject.FindGameObjectWithTag("Leaderboard");
        GameObject scrollrect = GameObject.FindGameObjectWithTag("Scroll Rect");

        // if the aren't enough entries in the leaderboard, then do not show the scrollbar
        if (scrollbar.GetComponent<Scrollbar>().value == 1)
        {
            // if the leaderboard has not been reset
            if (leaderboard != null)
            {
                // while there aren't sufficient entrie to be displayed, disable the scrollbar, it's components and the scrollable area
                if (leaderboard.transform.childCount <= 6)
                {
                    scrollbar.GetComponent<Image>().enabled = false;
                    scrollbar.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = false;
                    scrollrect.GetComponent<ScrollRect>().enabled = false;
                }

                // enable the scrollbar, it's components and the scrollable area
                if (leaderboard.transform.childCount > 6)
                {
                    scrollbar.GetComponent<Image>().enabled = true;
                    scrollbar.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = true;
                    scrollrect.GetComponent<ScrollRect>().enabled = true;
                }
            }
        }

        // if the leaderboard is reset then also hide the scrollbar
        if (leaderboard == null)
        {
            scrollbar.GetComponent<Image>().enabled = false;
            scrollbar.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
	}
}

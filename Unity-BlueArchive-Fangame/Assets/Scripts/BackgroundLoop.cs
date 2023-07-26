using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Unused Script for looping background because can't expect players climbing speed. But can be solved using bigger background image. */

public class BackgroundLoop : MonoBehaviour
{
    private float height;

    // Start is called before the first frame update
    private void Awake()
    {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        height = backgroundCollider.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         Call Reposition function when background's position is lower than background's height
         */

        if(transform.position.y <= -height)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        /*
         Replace background above to upper background to scroll down infinitely.
         */

        Vector2 offset = new Vector2(0, height * 4f);
        transform.position = (Vector2)transform.position + offset;
    }
}

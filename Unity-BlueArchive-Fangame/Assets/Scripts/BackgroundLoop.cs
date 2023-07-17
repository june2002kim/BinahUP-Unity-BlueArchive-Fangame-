using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(transform.position.y <= -height)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 offset = new Vector2(0, height * 4f);
        transform.position = (Vector2)transform.position + offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script for scrolling objects by Translate */

public class ScrollingObject : MonoBehaviour
{
    private float speed = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameover)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }
}

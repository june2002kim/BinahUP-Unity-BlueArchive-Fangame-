using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float xPos;

    private void OnEnable()
    {
        xPos = transform.position.x;
        if(xPos > 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameover && xPos < 0)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else if(!GameManager.instance.isGameover && xPos > 0)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
}

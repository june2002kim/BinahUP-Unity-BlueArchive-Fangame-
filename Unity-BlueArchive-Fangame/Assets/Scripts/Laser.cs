using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private PolygonCollider2D polyCollider;

    public Vector3 startPosition;
    public Vector3 endPosition;
    public Vector3 startPosition_;
    public Vector3 endPosition_;

    private float speed = 0.5f;
    Vector2[] laserCollider = new Vector2[4];
    private Vector3 offset = new Vector3(0.1f, 0.1f, 0);

    // Start is called before the first frame update
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        polyCollider = GetComponent<PolygonCollider2D>();

        lineRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        transform.position = Vector3.zero;

        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);

        laserCollider[0] = startPosition - offset;
        laserCollider[1] = startPosition + offset;
        laserCollider[2] = endPosition + offset;
        laserCollider[3] = endPosition - offset;
        
        polyCollider.points = laserCollider;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameover)
        {
            startPosition_ = startPosition + (Vector3.down * speed * Time.deltaTime);
            endPosition_ = endPosition + (Vector3.down * speed * Time.deltaTime);

            lineRenderer.SetPosition(0, startPosition_);
            lineRenderer.SetPosition(1, endPosition_);

            laserCollider[0] = startPosition_ - offset;
            laserCollider[1] = startPosition_ + offset;
            laserCollider[2] = endPosition_ + offset;
            laserCollider[3] = endPosition_ - offset;
            
            polyCollider.points = laserCollider;
        }
    }
}

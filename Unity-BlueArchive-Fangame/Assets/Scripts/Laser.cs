using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script for Binah's laser control */

public class Laser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private PolygonCollider2D polyCollider;

    public Vector3 startPosition;           // Laser's start position for LineRenderer
    public Vector3 endPosition;             // Laser's end position for LineRenderer
    public Vector3 startPosition_;          // Laser's new start position for scrolling down
    public Vector3 endPosition_;            // Laser's new end position for scrolling down

    private float speed = 0.5f;             // Laser's scrolling speed

    Vector2[] laserCollider = new Vector2[4];                           // Laser's four locations for PolygonCollier
    private Vector3 leftUpperOffset = new Vector3(0.1f, 0.1f, 0);       // left upper Laser's offset for its PolygonCollider
    private Vector3 rightUpperOffset = new Vector3(0.1f, -0.1f, 0);     // right upper Laser's offset for its PolygonCollider

    // Start is called before the first frame update
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        polyCollider = GetComponent<PolygonCollider2D>();

        lineRenderer.positionCount = 2;     // Laser has two position for LineRenderer (start position, end position)
    }

    private void OnEnable()
    {
        /*
         When Laser is OnEnabled, reset it's position because LineRenderer don't use WorldSpace
        And set LineRenderer's two positions with startPosition and endPosition
        And set PolygonCollider's position calculated by its shape and following offsets
         */

        transform.position = Vector3.zero;

        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);

        if(startPosition.x >= endPosition.x)
        {
            laserCollider[0] = startPosition - leftUpperOffset;
            laserCollider[1] = startPosition + leftUpperOffset;
            laserCollider[2] = endPosition + leftUpperOffset;
            laserCollider[3] = endPosition - leftUpperOffset;
        }
        else
        {
            laserCollider[0] = startPosition - rightUpperOffset;
            laserCollider[1] = startPosition + rightUpperOffset;
            laserCollider[2] = endPosition + rightUpperOffset;
            laserCollider[3] = endPosition - rightUpperOffset;
        }
        
        polyCollider.points = laserCollider;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         I don't know why but Laser draw with LineRenderer doesn't move with ScrollingObject script
        So using Update function, change LineRenderer and PolygonCollider's position 
         */

        if (!GameManager.instance.isGameover)
        {
            startPosition_ = startPosition + (Vector3.down * speed * Time.deltaTime);
            endPosition_ = endPosition + (Vector3.down * speed * Time.deltaTime);

            lineRenderer.SetPosition(0, startPosition_);
            lineRenderer.SetPosition(1, endPosition_);

            if (startPosition.x >= endPosition.x)
            {
                laserCollider[0] = startPosition_ - leftUpperOffset;
                laserCollider[1] = startPosition_ + leftUpperOffset;
                laserCollider[2] = endPosition_ + leftUpperOffset;
                laserCollider[3] = endPosition_ - leftUpperOffset;
            }
            else
            {
                laserCollider[0] = startPosition_ - rightUpperOffset;
                laserCollider[1] = startPosition_ + rightUpperOffset;
                laserCollider[2] = endPosition_ + rightUpperOffset;
                laserCollider[3] = endPosition_ - rightUpperOffset;
            }

            polyCollider.points = laserCollider;
        }
    }
}

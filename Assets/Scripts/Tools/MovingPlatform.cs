using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> points;
    public Transform platform;
    int goalPoint = 0;
    public float moveSpeed = 2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToNextPoint();
    }

    public void MoveToNextPoint()
    {
        // Change the position of platform 
        platform.position = Vector2.MoveTowards(platform.position, points[goalPoint].position, Time.deltaTime * moveSpeed); 
        // Check if we are in very close promixity of the next point
        if(Vector2.Distance(platform.position, points[goalPoint].position) < 0.1f)
        {
            // Check if we reached the last point, reset first point
            // If so change goal point to the next one
            if (goalPoint == points.Count - 1)
                goalPoint = 0;
            else
                goalPoint++;
        }
    }
}

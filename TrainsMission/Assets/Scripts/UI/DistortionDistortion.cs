using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistortionDistortion : MonoBehaviour
{

    public int max_Speed;

    private Vector3 start_Position;

    // Use this for initialization
    void Start()
    {
        start_Position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move_Vertical();
    }

    void Move_Vertical()
    {
        transform.position = new Vector3(transform.position.x, start_Position.y + Mathf.Sin(Time.time * max_Speed) * 15, transform.position.z);

        if (transform.position.y > -1.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.y < 1.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }
}

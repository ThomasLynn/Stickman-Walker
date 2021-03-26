using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{

    public Transform arena;

    private Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        foreach (Transform trans in arena)
        {
            if (trans.tag == "Stick")
            {
                Vector3 newPos = new Vector3(trans.GetComponent<StickAgent>().body.position.x, 0, -10);
                /*if (newPos.x < startingPos.x)
                {
                    newPos.x = startingPos.x;
                }*/
                transform.position = newPos;
                break;
            }
        }
    }
}

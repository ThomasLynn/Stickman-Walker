using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{

    public Transform arena;
    public bool doFollow;

    // Update is called once per frame
    void Update()
    {
        if (doFollow)
        {
            foreach (Transform trans in arena)
            {
                if (trans.tag == "Stick" && trans.gameObject.activeSelf)
                {
                    Vector3 newPos = new Vector3(trans.GetComponent<StickAgent>().body.position.x, 0, -10);
                    transform.position = newPos;
                    break;
                }
            }
        }
    }
}

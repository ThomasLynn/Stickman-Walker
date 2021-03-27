using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSnapper : MonoBehaviour
{
    public Rigidbody2D trackedObject;

    // Update is called once per frame
    void Update()
    {
        transform.position = trackedObject.position;
    }
}

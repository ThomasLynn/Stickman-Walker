using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnderScript : MonoBehaviour
{
    public StickAgent parentAgent;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            parentAgent.EndEpisodeAndRespawn(-1f);
        }
    }
}

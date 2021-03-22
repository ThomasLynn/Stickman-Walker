using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScript : MonoBehaviour
{

    public GameObject stickPrefab;
    public int maxSticks;

    private List<GameObject> stickList;

    void Start()
    {
        stickList = new List<GameObject>();
        Instantiate(stickPrefab, Vector2.zero, Quaternion.identity, transform);
    }

    public void EndEpisodeAndRespawn(GameObject go)
    {
        print("respawning "+go);
        if (go != null)
        {
            stickList.Remove(go);
            Destroy(go);
        }
        SpawnSticks();
        //Instantiate(stickPrefab, Vector2.zero, Quaternion.identity, transform);
        //Destroy(go);
    }

    private void SpawnSticks()
    {
        for (int i=stickList.Count;i< maxSticks; i++)
        {
            GameObject go = Instantiate(stickPrefab, Vector2.zero, Quaternion.identity, transform);
            stickList.Add(go);
        }
    }
}

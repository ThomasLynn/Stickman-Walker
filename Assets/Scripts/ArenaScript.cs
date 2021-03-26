using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScript : MonoBehaviour
{

    public GameObject stickPrefab;
    public int maxSticks;
    public bool doSpawn;

    private List<GameObject> stickList;

    void Start()
    {
        if (Application.isEditor)
        {
            maxSticks = 1;
        }
        stickList = new List<GameObject>();
        SpawnSticks();
    }

    public void EndEpisodeAndRespawn(GameObject go)
    {
        if (go != null)
        {
            stickList.Remove(go);
            Destroy(go);
        }
        SpawnSticks();
    }

    private void SpawnSticks()
    {
        if (doSpawn) {
            for (int i = stickList.Count; i < maxSticks; i++)
            {
                GameObject go = Instantiate(stickPrefab, transform.position, Quaternion.identity, transform);
                stickList.Add(go);
            }
        }
    }
}

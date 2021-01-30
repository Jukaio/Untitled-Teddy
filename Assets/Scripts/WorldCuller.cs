using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCuller : MonoBehaviour
{
    [SerializeField] private RoomGenerator RG;
    const int CURRENT = 0;
    const int PREVIOUS = 1;
    private Vector2Int[] position = { new Vector2Int(0, 0), new Vector2Int(0, 0) };

    void Start()
    {
        position[CURRENT] = RG.world.world_to_grid(transform.position);
        position[PREVIOUS] = position[CURRENT];
        RG.world.cull_world_around(position[CURRENT]);
    }

    void LateUpdate()
    {
        position[PREVIOUS] = position[CURRENT];
        position[CURRENT] = RG.world.world_to_grid(transform.position);
        if (position[PREVIOUS] != position[CURRENT])
        {
            var cur = RG.world.get(position[CURRENT].x, position[CURRENT].y);
            var pre = RG.world.get(position[PREVIOUS].x, position[PREVIOUS].y);
            if(cur != null) cur.parent.SetActive(true);
            if(pre != null) pre.parent.SetActive(false);
        }
    }
}

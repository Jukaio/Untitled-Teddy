using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCuller : MonoBehaviour
{
    [SerializeField] private RoomGenerator RG;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RG.world.cull_world_around(RG.world.world_to_grid(transform.position));
    }
}

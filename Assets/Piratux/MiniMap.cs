using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] RoomGenerator RG;
    public GameObject player;

    private int map_image_size = 20; // needs to be the same as all image sizes
    [SerializeField] GameObject Empty;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Room;
    [SerializeField] GameObject MapBorder;
    [SerializeField] Vector3 offset;
    GameObject[,] map_cell;
    private bool[,] room_discovered;

    Vector2Int player_cell_pos;

    private bool swapped_once = false;

    // Start is called before the first frame update
    void Start()
    {
        // // was trying to only show room when you discover it
        room_discovered = new bool[RG.world.Count.x, RG.world.Count.y];
        map_cell = new GameObject[RG.world.Count.x, RG.world.Count.y];
        for (int i = 0; i < RG.world.Count.x; i++)
        {
            for (int j = 0; j < RG.world.Count.y; j++)
            {
                room_discovered[i, j] = false;
                map_cell[i, j] = Instantiate(Empty);
                map_cell[i, j].transform.SetParent(transform, false);
                var RT = map_cell[i, j].GetComponent<RectTransform>();
                RT.anchoredPosition = new Vector3(map_image_size * i, -map_image_size * j, 0) + offset;
            }
        }
        var map_border = Instantiate(MapBorder);
        map_border.transform.SetParent(transform, false);
        var rt = map_border.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector3(0, 0, 0) + offset;
    }

    // Update is called once per frame
    void Update()
    {
        var new_player_cell_pos = RG.world.world_to_grid(player.transform.position);
        if (new_player_cell_pos != player_cell_pos)
        {
            UpdatePlayerMiniMapPosition(player_cell_pos, new_player_cell_pos);
            player_cell_pos = new_player_cell_pos;
        }
    }
    void UpdatePlayerMiniMapPosition(Vector2Int previous_pos, Vector2Int new_pos)
    {
        if (swapped_once == false)
        {
            swapped_once = true;
            map_cell[previous_pos.x, RG.world.Count.y - 1 - previous_pos.y].GetComponent<UnityEngine.UI.Image>().color = Empty.GetComponent<UnityEngine.UI.Image>().color;
            map_cell[new_pos.x, RG.world.Count.y - 1 - new_pos.y].GetComponent<UnityEngine.UI.Image>().color = Player.GetComponent<UnityEngine.UI.Image>().color;
        }
        else
        {
            room_discovered[new_pos.x, new_pos.y] = true;
            map_cell[previous_pos.x, RG.world.Count.y - 1 - previous_pos.y].GetComponent<UnityEngine.UI.Image>().color = Room.GetComponent<UnityEngine.UI.Image>().color;
            map_cell[new_pos.x, RG.world.Count.y - 1 - new_pos.y].GetComponent<UnityEngine.UI.Image>().color = Player.GetComponent<UnityEngine.UI.Image>().color;
        }
    }
}
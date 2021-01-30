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
    GameObject[,] map_cell;

    Vector2Int player_cell_pos;

    // Start is called before the first frame update
    void Start()
    {
        map_cell = new GameObject[RG.world.Count.x, RG.world.Count.y];
        player_cell_pos = RG.world.world_to_grid(player.transform.position);
        for (int i = 0; i < RG.world.Count.x; i++)
        {
            for (int j = 0; j < RG.world.Count.y; j++)
            {
                // (RG.world.Count.y - 1 - j), because camera angle is flipped...
                if (RG.world.get(i, RG.world.Count.y - 1 - j) == null)
                {
                    map_cell[i,j] = Instantiate(Empty);
                }
                else
                {
                    if (player_cell_pos.x == i && player_cell_pos.y == (RG.world.Count.y - 1 - j))
                    {
                        map_cell[i, j] = Instantiate(Player);
                    }
                    else
                    {
                        map_cell[i, j] = Instantiate(Room);
                    }
                }
                map_cell[i,j].transform.SetParent(transform, false);
                var RT = map_cell[i,j].GetComponent<RectTransform>();
                RT.anchoredPosition = new Vector3(map_image_size * i, -map_image_size * j, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var new_player_cell_pos = RG.world.world_to_grid(player.transform.position);
        if(new_player_cell_pos != player_cell_pos)
        {
            UpdatePlayerMiniMapPosition(player_cell_pos, new_player_cell_pos);
            player_cell_pos = new_player_cell_pos;
        }
    }
    void UpdatePlayerMiniMapPosition(Vector2Int previous_pos, Vector2Int new_pos)
    {
        map_cell[previous_pos.x, RG.world.Count.y - 1 - previous_pos.y].GetComponent<UnityEngine.UI.Image>().color = Room.GetComponent<UnityEngine.UI.Image>().color;
        map_cell[new_pos.x, RG.world.Count.y - 1 - new_pos.y].GetComponent<UnityEngine.UI.Image>().color = Player.GetComponent<UnityEngine.UI.Image>().color;
    }
}
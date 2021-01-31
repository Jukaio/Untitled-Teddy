using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class AStar
{
	private class Node
	{
		public Vector2Int position;
		public bool visited;
		public bool blocked;
		public float f;
		public float g;
		public Node parent;
		public List<Node> neighbours;
	}
	static float heuristic(Vector2Int from, Vector2Int to)
	{
		Vector2Int difference = to - from;
		if (difference.x < 0)
			difference.x = -difference.x;
		if (difference.y < 0)
			difference.y = -difference.y;

		return difference.x + difference.y;
	}
	private static bool in_bounds(Vector2Int that, Vector2Int within)
	{
		return that.x >= 0 && that.y >= 0 && that.x < within.x && that.y < within.y;
    }
	public static bool solvable(out List<Vector2Int> out_path, bool[,] grid, Vector2Int start, Vector2Int end)
	{
		out_path = new List<Vector2Int>();
		if (!in_bounds(start, new Vector2Int(grid.GetLength(0), grid.GetLength(1))))
			return false;
		if (!in_bounds(end, new Vector2Int(grid.GetLength(0), grid.GetLength(1))))
			return false;

		// Reset the grid and put the values on their "defaults"
		Node[,] nodes = new Node[grid.GetLength(0), grid.GetLength(1)];

		for (int x = 0; x < grid.GetLength(0); x++)
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				nodes[x, y] = new Node();
			}

		for (int x = 0; x < grid.GetLength(0); x++)
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				var use = nodes[x, y];
				use.position = new Vector2Int(x, y);
				use.visited = false;
				use.blocked = !grid[x, y];
				use.f = int.MaxValue;
				use.g = int.MaxValue;
				use.parent = null;
				use.neighbours = new List<Node>();
				if (x > 0)  /* filler */		{ use.neighbours.Add(nodes[x - 1, y]); };
				if (y > 0)  /* filler */		{ use.neighbours.Add(nodes[x, y - 1]); };
				if (x < grid.GetLength(0) - 1)  { use.neighbours.Add(nodes[x + 1, y]); };
				if (y < grid.GetLength(1) - 1)  { use.neighbours.Add(nodes[x, y + 1]); };
			}

		Node current = nodes[start.x, start.y];
		current.g = 0.0f;
		current.f = heuristic(start, end);

		List<Node> open_nodes = new List<Node>();
		open_nodes.Add(nodes[start.x, start.y]);

		while (!(open_nodes.Count == 0) && current != nodes[end.x, end.y])
		{
			open_nodes.Sort((a, b) => a.f.CompareTo(b.f));

			while (!(open_nodes.Count == 0) && open_nodes[0].visited)
			{
				open_nodes.RemoveAt(0);
			}

			if (open_nodes.Count == 0)
				break;


			current = open_nodes[0];
			current.visited = true;
			foreach (var neighbour in current.neighbours)
			{
				if (!neighbour.visited && 
					!neighbour.blocked)
					open_nodes.Add(neighbour);

				// if the movement cost g from the current one plus the target neighbour distace is smaller than
				// neighbours movement cost -> add current to the path
				float lower_cost = current.g + Vector2Int.Distance(current.position, neighbour.position);
				if (lower_cost < neighbour.g)
				{
					neighbour.parent = current;
					neighbour.g = lower_cost;
					neighbour.f = neighbour.g + heuristic(neighbour.position, end);
				}
			}
		}

		var temp = nodes[end.x, end.y];
		while (temp.parent != null)
		{
			out_path.Add(temp.position);
			temp = temp.parent;
		}

		if (current == nodes[end.x, end.y])
		{
			return true;
		}
		return false;
	}
}


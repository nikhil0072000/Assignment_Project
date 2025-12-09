using UnityEngine;
using System.Collections.Generic;

public class GridPathfinding : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;

    private class PathNode
    {
        public int x, z;
        public int gCost, hCost, fCost;
        public PathNode parent;
        public bool isWalkable;

        public PathNode(int x, int z)
        {
            this.x = x;
            this.z = z;
            this.isWalkable = true;
        }

        public void CalculateFCost() => fCost = gCost + hCost;
    }

    private PathNode[,] pathNodes;

    void Start()
    {
        InitializePathfindingGrid();
    }

    // Create pathfinding node grid
    private void InitializePathfindingGrid()
    {
        pathNodes = new PathNode[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                pathNodes[x, z] = new PathNode(x, z);
            }
        }

        UpdateWalkabilityFromTiles();
    }

    // Update node walkability from tile states
    public void UpdateWalkabilityFromTiles()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                GameObject tileObject = GameObject.Find($"Tile_{x}_{z}");
                if (tileObject != null)
                {
                    TileData tileData = tileObject.GetComponent<TileData>();
                    if (tileData != null)
                        pathNodes[x, z].isWalkable = tileData.IsWalkable();
                }
            }
        }
    }

    // Find path using A* algorithm
    public List<Vector2Int> FindPath(int startX, int startZ, int endX, int endZ)
    {
        if (!IsValidCoordinate(startX, startZ) || !IsValidCoordinate(endX, endZ))
            return null;

        UpdateWalkabilityFromTiles();

        if (!pathNodes[endX, endZ].isWalkable)
            return null;

        PathNode startNode = pathNodes[startX, startZ];
        PathNode endNode = pathNodes[endX, endZ];

        List<PathNode> openList = new List<PathNode>();
        HashSet<PathNode> closedList = new HashSet<PathNode>();

        ResetNodes();

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode)
                return ReconstructPath(endNode);

            foreach (PathNode neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.isWalkable || closedList.Contains(neighbor))
                    continue;

                int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighbor);

                if (tentativeGCost < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = CalculateDistance(neighbor, endNode);
                    neighbor.CalculateFCost();
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return null;
    }

    // Reset all nodes for fresh calculation
    private void ResetNodes()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                PathNode node = pathNodes[x, z];
                node.gCost = int.MaxValue;
                node.hCost = 0;
                node.fCost = 0;
                node.parent = null;
            }
        }
    }

    // Get valid neighbors (4-directional)
    private List<PathNode> GetNeighbors(PathNode node)
    {
        List<PathNode> neighbors = new List<PathNode>();
        int[] dx = { 0, 1, 0, -1 };
        int[] dz = { 1, 0, -1, 0 };

        for (int i = 0; i < 4; i++)
        {
            int checkX = node.x + dx[i];
            int checkZ = node.z + dz[i];

            if (IsValidCoordinate(checkX, checkZ))
                neighbors.Add(pathNodes[checkX, checkZ]);
        }

        return neighbors;
    }

    // Find node with lowest F cost
    private PathNode GetLowestFCostNode(List<PathNode> openList)
    {
        PathNode lowestFCostNode = openList[0];

        for (int i = 1; i < openList.Count; i++)
        {
            if (openList[i].fCost < lowestFCostNode.fCost)
                lowestFCostNode = openList[i];
        }

        return lowestFCostNode;
    }

    // Calculate Manhattan distance
    private int CalculateDistance(PathNode nodeA, PathNode nodeB)
    {
        int xDistance = Mathf.Abs(nodeA.x - nodeB.x);
        int zDistance = Mathf.Abs(nodeA.z - nodeB.z);
        return xDistance + zDistance;
    }

    // Reconstruct path from end to start
    private List<Vector2Int> ReconstructPath(PathNode endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        PathNode currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(new Vector2Int(currentNode.x, currentNode.z));
            currentNode = currentNode.parent;
        }

        path.Reverse();

        if (path.Count > 1)
            path.RemoveAt(0);

        return path;
    }

    // Check if coordinates are valid
    private bool IsValidCoordinate(int x, int z)
    {
        return x >= 0 && x < gridWidth && z >= 0 && z < gridHeight;
    }
}

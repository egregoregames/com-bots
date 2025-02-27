using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockMeshOptimizer : MonoBehaviour
{
    private struct Block
    {
        public Vector3 position;
        public bool isActive;
    }

    public List<GameObject> blocks;

    [ContextMenu("Do it")]
    public void Optimize()
    {
        var blockPositions = blocks.Select(b => b.transform.position).ToArray();
        InitializeBlocks(blockPositions);
        GenerateOptimizedMesh();
    }

    private Dictionary<Vector3, Block> blockGrid = new Dictionary<Vector3, Block>();
    
    private readonly Vector3[] directions = 
    {
        Vector3.forward, Vector3.back, Vector3.right, Vector3.left, Vector3.up, Vector3.down
    };

    public void GenerateOptimizedMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        foreach (var kvp in blockGrid)
        {
            Vector3 blockPos = kvp.Key;
            if (!kvp.Value.isActive) continue;

            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 neighborPos = blockPos + directions[i];

                if (blockGrid.ContainsKey(neighborPos) && blockGrid[neighborPos].isActive)
                    continue; // Skip if neighbor exists (hidden face)

                AddFace(blockPos, i, vertices, triangles, uvs);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    private void AddFace(Vector3 pos, int dir, List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
    {
        Vector3[] faceVerts = GetFaceVertices(pos, dir);
        int startIndex = vertices.Count;

        vertices.AddRange(faceVerts);
        triangles.AddRange(new int[] { startIndex, startIndex + 1, startIndex + 2, startIndex, startIndex + 2, startIndex + 3 });

        uvs.AddRange(new Vector2[]
        {
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)
        });
    }

    private Vector3[] GetFaceVertices(Vector3 pos, int dir)
    {
        switch (dir)
        {
            case 0: return new Vector3[] { pos + new Vector3(0, 0, 1), pos + new Vector3(1, 0, 1), pos + new Vector3(1, 1, 1), pos + new Vector3(0, 1, 1) }; // Front
            case 1: return new Vector3[] { pos + new Vector3(1, 0, 0), pos + new Vector3(0, 0, 0), pos + new Vector3(0, 1, 0), pos + new Vector3(1, 1, 0) }; // Back
            case 2: return new Vector3[] { pos + new Vector3(1, 0, 1), pos + new Vector3(1, 0, 0), pos + new Vector3(1, 1, 0), pos + new Vector3(1, 1, 1) }; // Right
            case 3: return new Vector3[] { pos + new Vector3(0, 0, 0), pos + new Vector3(0, 0, 1), pos + new Vector3(0, 1, 1), pos + new Vector3(0, 1, 0) }; // Left
            case 4: return new Vector3[] { pos + new Vector3(0, 1, 1), pos + new Vector3(1, 1, 1), pos + new Vector3(1, 1, 0), pos + new Vector3(0, 1, 0) }; // Top
            case 5: return new Vector3[] { pos + new Vector3(0, 0, 0), pos + new Vector3(1, 0, 0), pos + new Vector3(1, 0, 1), pos + new Vector3(0, 0, 1) }; // Bottom
        }
        return null;
    }

    public void InitializeBlocks(Vector3[] blockPositions)
    {
        blockGrid.Clear();
        foreach (var pos in blockPositions)
            blockGrid[pos] = new Block { position = pos, isActive = true };
    }
}

using UnityEngine;
using System.Collections.Generic;

public class CubeMerger : MonoBehaviour
{
    private static readonly Vector3[] FaceNormals =
    {
        Vector3.forward, Vector3.back, Vector3.right,
        Vector3.left, Vector3.up, Vector3.down
    };

    private static readonly int[,] FaceTriangles =
    {
        {0, 1, 2, 2, 3, 0},  // Front
        {4, 5, 6, 6, 7, 4},  // Back
        {1, 5, 6, 6, 2, 1},  // Right
        {0, 4, 7, 7, 3, 0},  // Left
        {3, 2, 6, 6, 7, 3},  // Top
        {0, 1, 5, 5, 4, 0}   // Bottom
    };

    private static readonly Vector3[] CubeVertices =
    {
        new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f),
        new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f),
        new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f),
        new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f)
    };

    private static readonly Vector2[] FaceUVs =
    {
        new Vector2(0, 0), new Vector2(1, 0),
        new Vector2(1, 1), new Vector2(0, 1)
    };

    [ContextMenu("Merge cubes")]
    public void MergeCubes()
    {
        Dictionary<Vector3, bool> cubePositions = new Dictionary<Vector3, bool>();
        List<Transform> cubes = new List<Transform>();

        // Find all cubes
        foreach (Transform child in transform)
        {
            
                cubePositions[child.position] = true;
                cubes.Add(child);
            
        }

        if (cubes.Count == 0) return;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        foreach (Transform cube in cubes)
        {
            Vector3 cubePos = cube.position;

            for (int face = 0; face < 6; face++)
            {
                Vector3 neighborPos = cubePos + FaceNormals[face];

                if (!cubePositions.ContainsKey(neighborPos))
                {
                    int vertexOffset = vertices.Count;

                    // Add face vertices & UVs
                    for (int i = 0; i < 4; i++)
                    {
                        vertices.Add(CubeVertices[FaceTriangles[face, i]] + cubePos);
                        uvs.Add(FaceUVs[i]);
                    }

                    // Add triangles
                    for (int i = 0; i < 6; i++)
                        triangles.Add(vertexOffset + FaceTriangles[face, i]);
                }
            }
        }

        // Create new mesh
        Mesh mergedMesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray()
        };
        mergedMesh.RecalculateNormals();

        // Create new GameObject for the merged mesh
        GameObject mergedCube = new GameObject("MergedMesh");
        mergedCube.transform.position = Vector3.zero;

        MeshFilter meshFilter = mergedCube.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = mergedCube.AddComponent<MeshRenderer>();
        meshFilter.mesh = mergedMesh;

        // Copy material from one of the original cubes (if available)
        if (cubes.Count > 0)
        {
            Renderer srcRenderer = cubes[0].GetComponent<Renderer>();
            if (srcRenderer)
                meshRenderer.material = srcRenderer.material;
        }

        // Destroy original cubes
        foreach (Transform cube in cubes)
        {
            Destroy(cube.gameObject);
        }
    }
}

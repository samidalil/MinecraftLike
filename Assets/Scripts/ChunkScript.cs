using System.Collections.Generic;
using UnityEngine;

public class ChunkScript : MonoBehaviour
{
    public static readonly int width = 16;
    public static readonly int height = WorldScript.height;

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;

    private WorldScript world;
    private Mesh mesh;
    private readonly List<Vector3> vertices = new List<Vector3>();
    private readonly List<int> triangles = new List<int>();

    private void Awake()
    {
        mesh = new Mesh();
    }

    public void SetWorld(WorldScript w)
    {
        world = w;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Refresh()
    {
        BuildLayout();
        RefreshMesh();
    }

    public void BuildLayout()
    {
        vertices.Clear();
        triangles.Clear();

        int x = (int)transform.position.x;
        int z = (int)transform.position.z;

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                for (int k = 0; k < width; k++)
                    if (world.GetBlock(x + i, j, z + k) != BlockType.Empty)
                        CreateCube(new Vector3(i, j, k), x + i, j, z + k);
    }

    private void CreateCube(Vector3 localPos, int x, int y, int z)
    {
        int numFaces = 0;

        if (world.GetBlock(x, y + 1, z) == BlockType.Empty)
        {
            vertices.Add(localPos + new Vector3(0, 1, 0));
            vertices.Add(localPos + new Vector3(0, 1, 1));
            vertices.Add(localPos + new Vector3(1, 1, 1));
            vertices.Add(localPos + new Vector3(1, 1, 0));
            numFaces++;
        }

        if (world.GetBlock(x, y - 1, z) == BlockType.Empty)
        {
            vertices.Add(localPos + new Vector3(0, 0, 0));
            vertices.Add(localPos + new Vector3(1, 0, 0));
            vertices.Add(localPos + new Vector3(1, 0, 1));
            vertices.Add(localPos + new Vector3(0, 0, 1));
            numFaces++;
        }

        if (world.GetBlock(x, y, z - 1) == BlockType.Empty)
        {
            vertices.Add(localPos + new Vector3(0, 0, 0));
            vertices.Add(localPos + new Vector3(0, 1, 0));
            vertices.Add(localPos + new Vector3(1, 1, 0));
            vertices.Add(localPos + new Vector3(1, 0, 0));
            numFaces++;
        }

        if (world.GetBlock(x + 1, y, z) == BlockType.Empty)
        {
            vertices.Add(localPos + new Vector3(1, 0, 0));
            vertices.Add(localPos + new Vector3(1, 1, 0));
            vertices.Add(localPos + new Vector3(1, 1, 1));
            vertices.Add(localPos + new Vector3(1, 0, 1));
            numFaces++;
        }

        if (world.GetBlock(x, y, z + 1) == BlockType.Empty)
        {
            vertices.Add(localPos + new Vector3(1, 0, 1));
            vertices.Add(localPos + new Vector3(1, 1, 1));
            vertices.Add(localPos + new Vector3(0, 1, 1));
            vertices.Add(localPos + new Vector3(0, 0, 1));
            numFaces++;
        }

        if (world.GetBlock(x - 1, y, z) == BlockType.Empty)
        {
            vertices.Add(localPos + new Vector3(0, 0, 1));
            vertices.Add(localPos + new Vector3(0, 1, 1));
            vertices.Add(localPos + new Vector3(0, 1, 0));
            vertices.Add(localPos + new Vector3(0, 0, 0));
            numFaces++;
        }

        int tl = vertices.Count - 4 * numFaces;
        for (int i = 0; i < numFaces; i++)
            triangles.AddRange(new int[] { tl + i * 4, tl + i * 4 + 1, tl + i * 4 + 2, tl + i * 4, tl + i * 4 + 2, tl + i * 4 + 3 });
    }

    private void RefreshMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
}

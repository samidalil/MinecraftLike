using UnityEngine;

public enum BlockType
{
    Empty,
    Base
}

public class WorldScript : MonoBehaviour
{
    public static readonly int height = 128;

    [SerializeField] public int width;
    [SerializeField] private int seed;

    private BlockType[] blocks;

    private void Awake()
    {
        if (seed == 0)
            seed = Random.Range(int.MinValue, int.MaxValue) / int.MaxValue;
        blocks = new BlockType[width * height * width];
    }

    public void BuildMap()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                for (int z = 0; z < width; z++)
                    blocks[x + width * (y + height * z)] = ComputeNoise(x, y, z);
    }

    public BlockType GetBlock(int x, int y, int z)
    {
        if (
            x >= 0 && x < width &&
            y >= 0 && y < height &&
            z >= 0 && z < width
        )
            return blocks[x + width * (y + height * z)];
        return BlockType.Empty;
    }

    private BlockType ComputeNoise(int x, int y, int z)
    {
        float noiseValue = Mathf.PerlinNoise(x * z * .03f + seed, z * .03f + seed) * Mathf.PerlinNoise(x * .001f, z * x * .005f) * (1.5f + seed);
        float baseLandHeight = height * .3f + noiseValue;

        return y <= baseLandHeight ? BlockType.Base : BlockType.Empty;
    }
}

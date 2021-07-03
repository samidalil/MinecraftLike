using UnityEngine;

enum Axis { X, Z }

public class TerrainGeneratorScript : MonoBehaviour
{
    private int chunkMovingValue;
    private Vector3 forward;
    private Vector3 back;
    private Vector3 right;
    private Vector3 left;

    [SerializeField]
    [Tooltip("The world to build")]
    private WorldScript world;

    [SerializeField]
    [Tooltip("Position to check")]
    private Transform characterTransform;

    [SerializeField]
    [Tooltip("Chunk to instantiate")]
    private GameObject chunkObject;

    [SerializeField] private int chunkDistance;

    private Vector3 generatedPos;
    private int length;
    private ChunkScript[] chunks;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        float movedOnX = characterTransform.position.x - generatedPos.x;
        float movedOnZ = characterTransform.position.z - generatedPos.z;

        if (Mathf.Abs(movedOnX) >= ChunkScript.width)
            UpdateChunks(Axis.X);
        if (Mathf.Abs(movedOnZ) >= ChunkScript.width)
            UpdateChunks(Axis.Z);
    }

    private void Initialize()
    {

        transform.position = new Vector3(
            (world.width - world.transform.position.x) / 2,
            0,
            (world.width - world.transform.position.x) / 2
        );
        length = chunkDistance * 2 - 1;
        chunks = new ChunkScript[length * length];

        chunkMovingValue = ChunkScript.width * length;
        forward = Vector3.forward * chunkMovingValue;
        back = Vector3.back * chunkMovingValue;
        right = Vector3.right * chunkMovingValue;
        left = Vector3.left * chunkMovingValue;

        UpdatePos();
        InitializeChunks();
    }

    private void InitializeChunks()
    {
        world.BuildMap();

        for (int i = 0; i < length; i++)
            for (int j = 0; j < length; j++)
            {
                int index = i * length + j;

                chunks[index] = GameObject.Instantiate(chunkObject).GetComponent<ChunkScript>();
                chunks[index].SetWorld(world);

                UpdateChunk(index, generatedPos + new Vector3(
                    (i - chunkDistance) * ChunkScript.width,
                    0,
                    (j - chunkDistance) * ChunkScript.width
                ));
            }
    }

    private void UpdateChunks(Axis axis)
    {
        UpdatePos();
        MoveChunks(axis);
    }

    private void UpdateChunk(int index, Vector3 newPos)
    {
        chunks[index].SetPosition(newPos);
        chunks[index].Refresh();
    }

    private void MoveChunks(Axis axis)
    {
        for (int i = 0; i < length; i++)
            for (int j = 0; j < length; j++)
            {
                int index = i * length + j;
                Vector3 chunkPos = chunks[index].transform.position;

                float value;

                if (axis == Axis.X)
                    value = generatedPos.x - chunkPos.x;
                else
                    value = generatedPos.z - chunkPos.z;

                if (Mathf.Abs(value) >= ChunkScript.width * chunkDistance)
                {
                    Vector3 pos;

                    if (axis == Axis.X)
                        pos = (value < 0 ? left : right);
                    else
                        pos = (value < 0 ? back : forward);

                    Debug.Log(chunkPos);
                    UpdateChunk(index, chunkPos + pos);
                }
            }


    }

    private void UpdatePos()
    {
        generatedPos = new Vector3(
            (int)(characterTransform.position.x - characterTransform.position.x % ChunkScript.width),
            0,
            (int)(characterTransform.position.z - characterTransform.position.z % ChunkScript.width)
        );
    }
}

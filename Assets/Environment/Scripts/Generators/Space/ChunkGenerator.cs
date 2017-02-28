using UnityEngine;
using System.Collections.Generic;

public class ChunkGenerator : MonoBehaviour {

    const float sqrMoveUpdateThreshold = 25f * 25f;

    static float maxViewDst;
    public Transform viewer;
    public Material mapMaterial;

    public static Vector3 viewerPosition;
    public static Vector3 viewerPositionOld;

    static MapGenerator mapGenerator;
    public int chunkSize;
    public int chunksVisibleInViewDst;

    Dictionary<Vector3 , Chunk> ChunkDictionary = new Dictionary<Vector3 , Chunk>();
    static List<Chunk> ChunksVisibleLastUpdate = new List<Chunk>();

    void Start () {
        mapGenerator = FindObjectOfType<MapGenerator>();
        maxViewDst = chunkSize * chunksVisibleInViewDst;


        UpdateVisibleChunks();
    }

    void Update () {
        viewerPosition = viewer.position;
        if ( ( viewerPositionOld - viewerPosition ).sqrMagnitude > sqrMoveUpdateThreshold ) {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks () {

        for ( int i = 0; i < ChunksVisibleLastUpdate.Count; i++ ) {
            ChunksVisibleLastUpdate [i].SetVisible(false);
        }
        ChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);
        int currentChunkCoordZ = Mathf.RoundToInt(viewerPosition.z / chunkSize);

        for ( int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++ ) {
            for ( int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++ ) {
                for ( int zOffset = -chunksVisibleInViewDst; zOffset <= chunksVisibleInViewDst; zOffset++ ) {
                    Vector3 viewedChunkCoord = new Vector3(currentChunkCoordX + xOffset , currentChunkCoordY + yOffset , currentChunkCoordZ + zOffset);

                    if ( ChunkDictionary.ContainsKey(viewedChunkCoord) )
                        ChunkDictionary [viewedChunkCoord].UpdateChunk();
                    else
                        ChunkDictionary.Add(viewedChunkCoord , new Chunk(viewedChunkCoord , chunkSize , transform, mapMaterial));
                }
            }
        }
    }

    public class Chunk {

        GameObject meshObject;
        Vector3 position;
        Bounds bounds;

        //MeshData meshData;

        MeshFilter meshFilter;
        MeshRenderer meshRenderer;

        public Chunk (Vector3 coord , int size , Transform parent, Material material) {
            position = coord * size;
            bounds = new Bounds(position , Vector3.one * size);

            //meshObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            meshObject = new GameObject("Chunk");
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;

            meshObject.transform.position = position;

            meshObject.transform.parent = parent;
            SetVisible(false);

            Debug.Log("Mesh Data Requested");
            mapGenerator.RequestMeshData(onMeshDataReceived);
        }

        void onMeshDataReceived (MeshData meshData) {
            Debug.Log("Mesh Data received");
            meshFilter.mesh = meshData.CreateMesh(true);

            //This may be innefficient
            UpdateChunk();
        }

        public void UpdateChunk () {
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDstFromNearestEdge <= maxViewDst;

            if ( visible )
                ChunksVisibleLastUpdate.Add(this);

            SetVisible(visible);
        }

        public void SetVisible (bool visible) {
            meshObject.SetActive(visible);
        }

        public bool IsVisible () {
            return meshObject.activeSelf;
        }

    }
}

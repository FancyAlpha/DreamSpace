using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkGenerator : MonoBehaviour {

    public const float maxViewDist = 300;
    public Transform viewer;
    public Material figureMaterial;

    public static Vector3 viewerPosition;
    Vector3 viewerPositionOld;
    //static map
    public int chunkSize;
    int ChunksVisibleInViewDist;

    const float updateThreshhold = 50f;
    const float sqrUpdateThreshhold = updateThreshhold * updateThreshhold;

    Dictionary<Vector3 , chunk> chunkDictionary = new Dictionary<Vector3 , chunk>();
    List<chunk> visibleChunks = new List<chunk>();

    // Use this for initialization
    void Start () {
        ChunksVisibleInViewDist = Mathf.RoundToInt(maxViewDist / chunkSize);

        viewerPosition = viewer.position;
        viewerPositionOld = viewerPosition;
        UpdateVisibleChunks();
    }

    // Update is called once per frame
    void Update () {
        viewerPosition = viewer.position;

        if ( ( viewerPositionOld - viewerPosition ).sqrMagnitude > sqrUpdateThreshhold ) {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks () {

        for ( int i = 0; i < visibleChunks.Count; i++ ) {
            visibleChunks [i].setVisible(false);
        }
        visibleChunks.Clear();

        Vector3 currentChunkCoord = new Vector3(Mathf.RoundToInt(viewerPosition.x / chunkSize) ,
                                                 Mathf.RoundToInt(viewerPosition.y / chunkSize) ,
                                                 Mathf.RoundToInt(viewerPosition.z / chunkSize));

        for ( int xOffset = -ChunksVisibleInViewDist; xOffset <= ChunksVisibleInViewDist; xOffset++ ) {
            for ( int yOffset = -ChunksVisibleInViewDist; yOffset <= ChunksVisibleInViewDist; yOffset++ ) {
                for ( int zOffset = -ChunksVisibleInViewDist; zOffset <= ChunksVisibleInViewDist; zOffset++ ) {
                    Vector3 viewedChunkCoord = new Vector3(currentChunkCoord.x + xOffset ,
                                                           currentChunkCoord.y + yOffset ,
                                                           currentChunkCoord.z + zOffset);

                    if ( chunkDictionary.ContainsKey(viewedChunkCoord) ) {
                        chunkDictionary [viewedChunkCoord].updateChunk();
                        if ( chunkDictionary [viewedChunkCoord].isVisible() ) {
                            visibleChunks.Add(chunkDictionary [viewedChunkCoord]);
                        }
                    } else {
                        chunkDictionary.Add(viewedChunkCoord , new chunk(viewedChunkCoord , chunkSize , transform , figureMaterial));
                    }
                }
            }
        }
    }

    public class chunk {
        GameObject meshObject;
        Vector3 position;
        Bounds bounds;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        public chunk (Vector3 coord , int size , Transform parent , Material material) {
            //position = (coord * size) + size/2;
            position = new Vector3(( coord.x * size ) + size / 2 , ( coord.y * size ) + size / 2 , ( coord.z * size ) + size / 2);
            bounds = new Bounds(position , Vector3.one * size);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            meshObject.transform.position = position;
            meshObject.transform.parent = parent;
            meshObject.transform.localScale = new Vector3(size/5, size/5, size/5);
            setVisible(false);
        }

        public void updateChunk () {
            float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDistanceFromNearestEdge <= maxViewDist;
            setVisible(visible);
        }

        public void setVisible (bool visible) {
            meshObject.SetActive(visible);
        }

        public bool isVisible () {
            return meshObject.activeSelf;
        }
    }
}

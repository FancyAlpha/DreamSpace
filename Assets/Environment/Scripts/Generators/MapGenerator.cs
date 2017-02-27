using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class MapGenerator : MonoBehaviour {
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public void RequestMeshData (Action<MeshData> callback) {
        ThreadStart threadStart = delegate {
            MeshDataThread(callback);
        };

        new Thread(threadStart).Start();
    }

    void MeshDataThread (Action<MeshData> callback) {
        MeshData meshData = PolyhedronGenerator.Create(0 , Vector3.one , Vector3.zero);
        lock ( meshDataThreadInfoQueue ) {
            Debug.Log("locked");
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback , meshData));
        }
    }

    void Update () {
        if ( meshDataThreadInfoQueue.Count > 0 ) {
            for ( int i = 0; i < meshDataThreadInfoQueue.Count; i++ ) {
                Debug.Log("Queued");
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    struct MapThreadInfo<T> {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo (Action<T> callback , T parameter) {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}
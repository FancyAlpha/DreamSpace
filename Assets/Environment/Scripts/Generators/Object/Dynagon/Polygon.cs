using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Dynagon {

	public abstract class Polygon {

        public MeshData mesh;

		public List<Vector3> vertices;
		public List<int> indexes;

		public Polygon(List<Vector3> vertices) {
			// todo: check a number of vertices is multiple of 3
			this.vertices = vertices;
			indexes = Enumerable.Range(0, vertices.Count).ToList<int>();

            mesh = new MeshData();
		}

		protected Vector3 GetCenterOfTriangle(int firstIndexOfTriangle) {
			var i = firstIndexOfTriangle;
			return (vertices[i] + vertices[i + 1] + vertices[i + 2]) / 3;
		}
		
		protected Vector3 GetNormalOfTriangle(int firstIndexOfTriangle) {
			var i = firstIndexOfTriangle;
			return Vector3.Cross(vertices[i + 1] - vertices[i], vertices[i + 2] - vertices[i]);
		}

		protected void ReverseSurface(int firstIndexOfTriangle) {
			var i = firstIndexOfTriangle;
			int temp = indexes[i + 1];
			indexes[i + 1] = indexes[i + 2];
			indexes[i + 2] = temp;
		}

		protected abstract void OptimizeIndexes();
		
		protected MeshData BuildMesh() {
			mesh.vertices = vertices.ToArray();
			mesh.triangles = indexes.ToArray();
			mesh.uvs = new Vector2[mesh.vertices.Length];

            return mesh;
		}

		public MeshData Build() {
			OptimizeIndexes();
			BuildMesh();
			return mesh;
		}
	}

}

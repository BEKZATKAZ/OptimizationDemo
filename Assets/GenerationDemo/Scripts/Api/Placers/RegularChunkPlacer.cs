using GenerationDemo.Core;
using GenerationDemo.Core.DTOs;
using UnityEngine;

namespace GenerationDemo.Api.Placers
{
    public sealed class StandardChunkPlacer : ChunkPlacerBase
    {
        private ChunkObject _prefab;

        public StandardChunkPlacer WithPrefab(ChunkObject prefab)
        {
            _prefab = prefab;
            return this;
        }

        protected override void OnChunkGenerated(VertexDTO[] vertices, int[] indices)
        {
            ChunkObject instance = Instantiate(_prefab);
            Mesh mesh = instance.Mesh;

            Vector3[] positions = new Vector3[vertices.Length];
            Vector2[] uvs = new Vector2[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                positions[i] = vertices[i].Position;
                uvs[i] = vertices[i].UV;
            }

            mesh.SetVertices(positions);
            mesh.SetUVs(0, uvs);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);

            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
        }
    }
}

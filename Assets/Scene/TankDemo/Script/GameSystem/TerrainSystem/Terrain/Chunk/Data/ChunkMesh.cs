using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WaterBox
{
    public class ChunkSubMesh
    {
        public List<List<Vector3Int>> m_Faces { get; set; }         //  这个Mesh的所有面
        public short m_BlockTypeValue { get; set; }
        public ChunkSubMesh(short blockType)
        {
            m_BlockTypeValue = blockType;
            m_Faces = new List<List<Vector3Int>>();
            m_Faces.Add(new List<Vector3Int>());
            m_Faces.Add(new List<Vector3Int>());
            m_Faces.Add(new List<Vector3Int>());
            m_Faces.Add(new List<Vector3Int>());
            m_Faces.Add(new List<Vector3Int>());
            m_Faces.Add(new List<Vector3Int>());
        }

        public static bool operator ==(ChunkSubMesh b, ChunkSubMesh c)
        {
            if (b.m_BlockTypeValue == c.m_BlockTypeValue)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(ChunkSubMesh b, ChunkSubMesh c)
        {
            if (b.m_BlockTypeValue != c.m_BlockTypeValue)
            {
                return true;
            }
            return false;
        }
    };
    public class ChunkMesh
    {
        enum FaceType
        {
            FACE_TYPE_BOTTOM = 0,
            FACE_TYPE_TOP,
            FACE_TYPE_LEFT,
            FACE_TYPE_RIGHT,
            FACE_TYPE_FRONT,
            FACE_TYPE_BACK
        };

        private List<List<Vector3>> m_VertexOffset;

        int                 m_SubMeshCount;
        List<Vector3>       m_Vertices ;
        List<Vector2>       m_Uv;
        List< List<int>>    m_Index;
        List<short>         m_TypeValue;

        bool m_IsHaveData;
        public bool IsHaveData { get { return m_IsHaveData; } }


        public ChunkMesh()
        {
            m_VertexOffset = new List<List<Vector3>>();
            m_Vertices = new List<Vector3>();
            m_Uv = new List<Vector2>();
            m_Index = new List<List<int>>();
            m_TypeValue = new List<short>();
            m_SubMeshCount = 0;
            m_IsHaveData = false;
            generateOffset();
        }
        public short getTypeValue(int index)
        {
            return m_TypeValue[index];
        }

        private void generateOffset()
        {
            m_VertexOffset.Add(new List<Vector3>());
            m_VertexOffset.Add(new List<Vector3>());
            m_VertexOffset.Add(new List<Vector3>());
            m_VertexOffset.Add(new List<Vector3>());
            m_VertexOffset.Add(new List<Vector3>());
            m_VertexOffset.Add(new List<Vector3>());
            m_VertexOffset[(int)FaceType.FACE_TYPE_BOTTOM].Add(new Vector3(0, 0, 0));
            m_VertexOffset[(int)FaceType.FACE_TYPE_BOTTOM].Add(new Vector3(1, 0, 0));
            m_VertexOffset[(int)FaceType.FACE_TYPE_BOTTOM].Add(new Vector3(1, 0, 1));
            m_VertexOffset[(int)FaceType.FACE_TYPE_BOTTOM].Add(new Vector3(0, 0, 1));

            m_VertexOffset[(int)FaceType.FACE_TYPE_TOP].Add(new Vector3(0, 1, 0));
            m_VertexOffset[(int)FaceType.FACE_TYPE_TOP].Add(new Vector3(0, 1, 1));
            m_VertexOffset[(int)FaceType.FACE_TYPE_TOP].Add(new Vector3(1, 1, 1));
            m_VertexOffset[(int)FaceType.FACE_TYPE_TOP].Add(new Vector3(1, 1, 0));

            m_VertexOffset[(int)FaceType.FACE_TYPE_LEFT].Add(new Vector3(0, 0, 0));
            m_VertexOffset[(int)FaceType.FACE_TYPE_LEFT].Add(new Vector3(0, 0, 1));
            m_VertexOffset[(int)FaceType.FACE_TYPE_LEFT].Add(new Vector3(0, 1, 1));
            m_VertexOffset[(int)FaceType.FACE_TYPE_LEFT].Add(new Vector3(0, 1, 0));

            m_VertexOffset[(int)FaceType.FACE_TYPE_RIGHT].Add(new Vector3(1, 0, 0));
            m_VertexOffset[(int)FaceType.FACE_TYPE_RIGHT].Add(new Vector3(1, 1, 0));
            m_VertexOffset[(int)FaceType.FACE_TYPE_RIGHT].Add(new Vector3(1, 1, 1));
            m_VertexOffset[(int)FaceType.FACE_TYPE_RIGHT].Add(new Vector3(1, 0, 1));

            m_VertexOffset[(int)FaceType.FACE_TYPE_FRONT].Add(new Vector3(0, 0, 0));
            m_VertexOffset[(int)FaceType.FACE_TYPE_FRONT].Add(new Vector3(0, 1, 0));
            m_VertexOffset[(int)FaceType.FACE_TYPE_FRONT].Add(new Vector3(1, 1, 0));
            m_VertexOffset[(int)FaceType.FACE_TYPE_FRONT].Add(new Vector3(1, 0, 0));

            m_VertexOffset[(int)FaceType.FACE_TYPE_BACK].Add(new Vector3(0, 0, 1));
            m_VertexOffset[(int)FaceType.FACE_TYPE_BACK].Add(new Vector3(1, 0, 1));
            m_VertexOffset[(int)FaceType.FACE_TYPE_BACK].Add(new Vector3(1, 1, 1));
            m_VertexOffset[(int)FaceType.FACE_TYPE_BACK].Add(new Vector3(0, 1, 1));
        }

        public void parseChunkData(ChunkData chunkData)
        {
            List<ChunkSubMesh> meshes = new List<ChunkSubMesh>();
            for (int x = 0; x < TerrainSystem.CHUNK_SIZE; ++x)
            {
                for (int y = 0; y < TerrainSystem.CHUNK_SIZE; ++y)
                {
                    for (int z = 0; z < TerrainSystem.CHUNK_SIZE; ++z)
                    {
                        short value = chunkData.getValue(x, y, z);
                        if (0 != value)
                        {
                            ChunkSubMesh mesh = new ChunkSubMesh(value);
                            int meshIndex = meshes.FindIndex(item => { if (mesh == item) return true; return false; });
                            if (meshIndex == -1)
                            {
                                meshes.Add(mesh);
                            }
                            else
                            {
                                mesh = meshes[meshIndex];
                            }
                            //  底面
                            if (y - 1 >= 0 && chunkData.getValue(x, y - 1, z) == 0 || 
                                (y == 0 && chunkData.OutSideData != null && chunkData.OutSideData.getDataValue(ChunkOutSideData.DataType.TYPE_BOTTOM, x, z) == 0))
                            {
                                mesh.m_Faces[(int)FaceType.FACE_TYPE_BOTTOM].Add(new Vector3Int(x, y, z));
                            }
                            //  顶面
                            if (y + 1 < TerrainSystem.CHUNK_SIZE && chunkData.getValue(x, y + 1, z) == 0 ||
                                (y + 1 == TerrainSystem.CHUNK_SIZE && chunkData.OutSideData != null && chunkData.OutSideData.getDataValue(ChunkOutSideData.DataType.TYPE_TOP, x, z) == 0))
                            {
                                mesh.m_Faces[(int)FaceType.FACE_TYPE_TOP].Add(new Vector3Int(x, y, z));
                            } 

                            //  左面
                            if (x - 1 >= 0 && chunkData.getValue(x - 1, y, z) == 0 ||
                                (x == 0 && chunkData.OutSideData != null && chunkData.OutSideData.getDataValue(ChunkOutSideData.DataType.TYPE_LEFT, y, z) == 0))
                            {
                                mesh.m_Faces[(int)FaceType.FACE_TYPE_LEFT].Add(new Vector3Int(x, y, z));
                            }
                            //  右面
                            if (x + 1 < TerrainSystem.CHUNK_SIZE && chunkData.getValue(x + 1, y, z) == 0 ||
                                (x + 1 == TerrainSystem.CHUNK_SIZE && chunkData.OutSideData != null && chunkData.OutSideData.getDataValue(ChunkOutSideData.DataType.TYPE_RIGHT, y, z) == 0))
                            {
                                mesh.m_Faces[(int)FaceType.FACE_TYPE_RIGHT].Add(new Vector3Int(x, y, z));
                            }
                            //  前面
                            if (z - 1 >= 0 && chunkData.getValue(x, y, z - 1) == 0 || 
                                (z == 0 && chunkData.OutSideData != null && chunkData.OutSideData.getDataValue(ChunkOutSideData.DataType.TYPE_FRONT, x, y) == 0))
                            {
                                mesh.m_Faces[(int)FaceType.FACE_TYPE_FRONT].Add(new Vector3Int(x, y, z));
                            }
                            //  后面
                            if (z + 1 < TerrainSystem.CHUNK_SIZE && chunkData.getValue(x, y, z + 1) == 0  ||
                                (z + 1 == TerrainSystem.CHUNK_SIZE && chunkData.OutSideData != null && chunkData.OutSideData.getDataValue(ChunkOutSideData.DataType.TYPE_BACK, x, y) == 0))
                            {
                                mesh.m_Faces[(int)FaceType.FACE_TYPE_BACK].Add(new Vector3Int(x, y, z));
                            }
                        }
                    }
                }
            }
            m_Vertices = new List<Vector3>();
            m_Uv = new List<Vector2>();
            m_Index = new List<List<int>> ();

            int vertexNumber = 0;
            m_SubMeshCount = meshes.Count();
            for (int i = 0; i < m_SubMeshCount; ++i)
            {
                m_IsHaveData = true;
                ChunkSubMesh mesh = meshes[i];
                List<List<Vector3Int>> faces = mesh.m_Faces;
                m_Index.Add(new List<int>());
                m_TypeValue.Add(mesh.m_BlockTypeValue);

                //  subMesh的几个面处理
                for (int faceIndex = 0; faceIndex < faces.Count(); ++faceIndex)
                {
                    List<Vector3Int> face = faces[faceIndex];
                    List<Vector3> vertexOffset = m_VertexOffset[faceIndex];

                    //  一个面的所有顶点处理
                    for (int fv = 0; fv < face.Count(); ++fv)
                    {
                        //  添加顶点
                        Vector3Int boxPos = face[fv];
                        for (int v = 0; v < 4; ++v)
                        {
                            m_Vertices.Add(new Vector3(boxPos.x + vertexOffset[v].x, boxPos.y + vertexOffset[v].y, boxPos.z + vertexOffset[v].z));
                        }

                        //  添加UV
                        m_Uv.Add(new Vector2(0, 1));
                        m_Uv.Add(new Vector2(0, 0));
                        m_Uv.Add(new Vector2(1, 1));
                        m_Uv.Add(new Vector2(1, 0));

                        //  添加索引
                        m_Index[i].Add(vertexNumber + 0);
                        m_Index[i].Add(vertexNumber + 1);
                        m_Index[i].Add(vertexNumber + 2);

                        m_Index[i].Add(vertexNumber + 2);
                        m_Index[i].Add(vertexNumber + 3);
                        m_Index[i].Add(vertexNumber + 0);
                        vertexNumber += 4;
                    }
                }
            }
        }

        public Mesh generateMesh()
        {
            if(m_SubMeshCount == 0)
            {
                return null;
            }

            Mesh mesh = new Mesh();
            mesh.subMeshCount = m_SubMeshCount;

            mesh.vertices = m_Vertices.ToArray();
            mesh.uv = m_Uv.ToArray();
            for (int i=0; i<m_SubMeshCount; ++i)
            {
                mesh.SetTriangles(m_Index[i], i);//triangles = index;
            }
            mesh.RecalculateNormals();      //  自动计算法线
            mesh.RecalculateBounds();       //  自动计算BoundBox

            m_SubMeshCount = 0;
            m_Vertices.Clear();
            m_Uv.Clear();
            m_Index.Clear();
            return mesh;
        }
    }
}

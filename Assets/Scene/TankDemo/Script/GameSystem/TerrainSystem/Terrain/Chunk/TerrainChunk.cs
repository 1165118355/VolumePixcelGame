using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WaterBox
{
    [Serializable]
    public class TerrainChunk
    {
        public enum TerrainChunkState
        {
        STATE_NULL,
        STATE_LOADING,
        STATE_COMPLETE,
        }
        public Terrain Terrain { get { return m_Terrain; } set { m_Terrain = value; } }
        public GameObject gameObject { get { return m_Chunk; } }
        public TerrainChunkState State{ get { return m_State; } set { m_State = value; } }

        public TerrainChunk(Terrain terrain)
        {
            init(terrain);
        }
        ~TerrainChunk()
        {
        }
        public void init(Terrain terrain)
        {
            m_Terrain = terrain;
            m_ChunkMesh = new ChunkMesh();
        }

        /// <summary>
        ///   
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="type"></param>
        public void setBlock(IVec3 pos, int type)
        {
            m_ChunkData.setValue(pos, type);
            m_ChunkData.m_IsChange = true;
        }
        public int getBlock(IVec3 pos)
        {
            return m_ChunkData.getValue(pos.x, pos.y, pos.z);
        }

        public void setAllBlock(int type)
        {
            for(int i=0; i<16; ++i)
            {
                for(int j=0; j<16; ++j)
                {
                    for(int k=0; k<16; ++k)
                    {
                        m_ChunkData.setValue(i, j, k, type);
                    }
                }
            }
        }
        /// <summary>
        /// 销毁chunk
        /// </summary>
        public void destory()
        {
            GameObject.Destroy(m_Chunk);
            m_ChunkData = null;
        }

        /// <summary>
        /// 是否有数据
        /// </summary>
        /// <returns></returns>
        public bool isHaveChunkData()
        {
            return m_ChunkData != null;
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="chunkData"></param>
        public void setChunkData(ChunkData chunkData)
        {
            m_ChunkData = chunkData;
            if(!m_ChunkData.m_IsNull)
            {
                m_ChunkMesh.parseChunkData(m_ChunkData);
            }
        }
        public ChunkData getChunkData()
        {
            return m_ChunkData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        public void setTilePosition(IVec3 pos)
        {
            m_TilePosition = pos;

        }
        public IVec3 getTilePosition()
        {
            return m_TilePosition;
        }

        public void setTerrainPosition(IVec3 pos)
        {
            m_TerrainPos = pos;
        }

        public IVec3 getTerrainPosition()
        {
            return m_TerrainPos;
        }

        /// <summary>
        /// 生成chunk
        /// </summary>
        public void generateChunk()
        {
            Mesh mesh = m_ChunkMesh.generateMesh();
            if(mesh == null || mesh.subMeshCount <= 0)
            {
                return;
            }
            if(m_Chunk == null)
            {
                m_Chunk = new GameObject();

                m_Chunk.AddComponent<MeshCollider>();
                m_Chunk.AddComponent<MeshFilter>();
                m_Chunk.AddComponent<MeshRenderer>();
                m_Chunk.transform.parent = m_Terrain.GetGameObject().transform;
                m_Chunk.name = "x" + m_TilePosition.x + "_y" + m_TilePosition.y + "_z" + m_TilePosition.z;
                Vector3 transformPosition = new Vector3(m_TilePosition.x * 16, m_TilePosition.y * 16, m_TilePosition.z * 16);
                m_Chunk.transform.localPosition = transformPosition;
            }
            List<Material> materialsList = new List<Material>();
            for (int i = 0; i < mesh.subMeshCount; ++i)
            {
                int typeValue = m_ChunkMesh.getTypeValue(i);
                Material material = TerrainCreator.get().getMaterial(typeValue);
                materialsList.Add(material);
            }
            m_Chunk.GetComponent<MeshCollider>().sharedMesh = mesh;
            m_Chunk.GetComponent<MeshFilter>().mesh = mesh;
            m_Chunk.GetComponent<MeshRenderer>().materials = materialsList.ToArray();
            testValue++;
        }

        public void updateChunk()
        {
            m_ChunkMesh.parseChunkData(m_ChunkData);
            generateChunk();
        }


        private GameObject m_Chunk = null;
        private IVec3 m_TilePosition = new IVec3();
        private IVec3 m_TerrainPos = new IVec3();
        private ChunkMesh m_ChunkMesh;
        private ChunkData m_ChunkData;
        Terrain m_Terrain;
        TerrainChunkState m_State = TerrainChunkState.STATE_NULL;

        int testValue = 0;

    }

}

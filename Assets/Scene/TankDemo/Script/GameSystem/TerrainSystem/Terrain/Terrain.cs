using UnityEngine;
using System.Collections;
using System.Linq;
using LitJson;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace WaterBox
{
    [Serializable]
    public class Terrain
    {
        [NonSerialized]
        private GameObject m_GameObject = null;
        private TerrainChunk[,,] m_Chunks = new TerrainChunk[16, 16, 16];
        private IVec3 m_TerrainSize = new IVec3(256, 256, 256);
        private IVec3 m_TerrainPos;
        private byte[,] m_HeightMap;
        FastNoise noiseNew;

        public Terrain()
        {
            noiseNew = new FastNoise();
            m_GameObject = new GameObject();
            start();
        }
        public Terrain(IVec3 terrainPos)
        {
            noiseNew = new FastNoise();
            m_GameObject = new GameObject();
            m_TerrainPos = terrainPos;

            m_GameObject.transform.position = (terrainPos * TerrainSystem.TERRAIN_SIZE_X).toVector3();
            m_GameObject.transform.name = "" + terrainPos;


            m_HeightMap = new byte[TerrainSystem.TERRAIN_SIZE, TerrainSystem.TERRAIN_SIZE];
            for(int i=0; i<TerrainSystem.TERRAIN_SIZE; ++i)
            {
                for(int j=0; j<TerrainSystem.TERRAIN_SIZE; ++j)
                {
                    int x = i + terrainPos.x * TerrainSystem.TERRAIN_SIZE;
                    int z = j + terrainPos.y * TerrainSystem.TERRAIN_SIZE;
                    float value = noiseNew.GetPerlin(x * 64f, z * 64f) * 0.5f + 0.5f;
                    m_HeightMap[i, j] = (byte)value;
                }
            }
        }
        // Use this for initialization
        public void start()
        {
            m_Chunks = new TerrainChunk[16, 16, 16];
        }

        // Update is called once per frame
        public void update()
        {
        }

        GameObject getGameObject()
        {
            if(m_GameObject == null)
            {
                m_GameObject = new GameObject();
            }
            return m_GameObject;
        }

        public void setChunk(IVec3 pos, TerrainChunk chunk)
        {
            m_Chunks[pos.x, pos.y, pos.z] = chunk;
        }
        public TerrainChunk getChunk(IVec3 pos)
        {
            if(pos.x < 0 || pos.x >= TerrainSystem.CHUNK_SIZE_X ||
                pos.y < 0 || pos.y >= TerrainSystem.CHUNK_SIZE_Y ||
                pos.z < 0 || pos.z >= TerrainSystem.CHUNK_SIZE_Z)
            {
                return null;
            }
            return m_Chunks[pos.x, pos.y, pos.z];
        }

        public void setBlock(IVec3 chunkPos, IVec3 blockPos, int value, bool updateMesh = true)
        {
            TerrainChunk oringChunk = getChunk(chunkPos);
            TerrainChunk xChunk =null;
            TerrainChunk yChunk =null;
            TerrainChunk zChunk = null;
            oringChunk.setBlock(blockPos, value);

            //  更新周边Chunk的边缘面
            //  XXX
            if (blockPos.x == 0 && chunkPos.x > 0 ||
                blockPos.x == TerrainSystem.CHUNK_SIZE_X-1 && chunkPos.x < TerrainSystem.TERRAIN_SIZE_X-1)
            {
                IVec3 tmpBlockPos = blockPos;
                IVec3 tmpChunkPos = chunkPos;
                ChunkOutSideData.DataType outSideType = ChunkOutSideData.DataType.TYPE_LEFT;
                if (blockPos.x == 0)
                {
                    tmpBlockPos.x = TerrainSystem.CHUNK_SIZE_X - 1;
                    tmpChunkPos.x -= 1;
                    outSideType = ChunkOutSideData.DataType.TYPE_RIGHT;
                }
                if(blockPos.x == TerrainSystem.CHUNK_SIZE_X - 1)
                {
                    tmpBlockPos.x = 0;
                    tmpChunkPos.x += 1;
                    outSideType = ChunkOutSideData.DataType.TYPE_LEFT;
                }
                xChunk = getChunk(tmpChunkPos);
                if(xChunk != null)
                {
                    bool outSideValue = (value == 0 ? false : true);
                    var chunkData = xChunk.getChunkData();
                    chunkData.OutSideData.setDataValue(outSideType, tmpBlockPos.y, tmpBlockPos.z, outSideValue);
                    xChunk.updateChunk();
                }
            }

            //  YYYYYYY
            if (blockPos.y == 0 && chunkPos.y > 0 ||
                blockPos.y == TerrainSystem.CHUNK_SIZE_Y - 1 && chunkPos.y < TerrainSystem.TERRAIN_SIZE_Y - 1)
            {
                IVec3 tmpBlockPos = blockPos;
                IVec3 tmpChunkPos = chunkPos;
                ChunkOutSideData.DataType outSideType = ChunkOutSideData.DataType.TYPE_TOP;
                if (blockPos.y == 0)
                {
                    tmpBlockPos.y = TerrainSystem.CHUNK_SIZE_Y - 1;
                    tmpChunkPos.y -= 1;
                    outSideType = ChunkOutSideData.DataType.TYPE_TOP;
                }
                if (blockPos.y == TerrainSystem.CHUNK_SIZE_Y - 1)
                {
                    tmpBlockPos.y = 0;
                    tmpChunkPos.y += 1;
                    outSideType = ChunkOutSideData.DataType.TYPE_BOTTOM;
                }
                yChunk = getChunk(tmpChunkPos);
                if(yChunk != null)
                {
                    bool outSideValue = (value == 0 ? false : true);
                    var chunkData = yChunk.getChunkData();
                    chunkData.OutSideData.setDataValue(outSideType, tmpBlockPos.x, tmpBlockPos.z, outSideValue);
                    yChunk.updateChunk();
                }
            }

            //  ZZZZZ
            if (blockPos.z == 0 && chunkPos.z > 0 ||
                blockPos.z == TerrainSystem.CHUNK_SIZE_Z - 1 && chunkPos.z < TerrainSystem.TERRAIN_SIZE_Z - 1)
            {
                IVec3 tmpBlockPos = blockPos;
                IVec3 tmpChunkPos = chunkPos;
                ChunkOutSideData.DataType outSideType = ChunkOutSideData.DataType.TYPE_BACK;
                if (blockPos.z == 0)
                {
                    tmpBlockPos.z = TerrainSystem.CHUNK_SIZE_Z - 1;
                    tmpChunkPos.z -= 1;
                    outSideType = ChunkOutSideData.DataType.TYPE_BACK;
                }
                if (blockPos.z == TerrainSystem.CHUNK_SIZE_Z - 1)
                {
                    tmpBlockPos.z = 0;
                    tmpChunkPos.z += 1;
                    outSideType = ChunkOutSideData.DataType.TYPE_FRONT;
                }
                zChunk = getChunk(tmpChunkPos);
                if(zChunk != null)
                {
                    bool outSideValue = (value == 0 ? false : true);
                    var chunkData = zChunk.getChunkData();
                    chunkData.OutSideData.setDataValue(outSideType, tmpBlockPos.x, tmpBlockPos.y, outSideValue);
                }
            }
            if(updateMesh)
            {
                if(zChunk != null)
                {
                    zChunk.updateChunk();
                }
                if (yChunk != null)
                {
                    yChunk.updateChunk();
                }
                if (xChunk != null)
                {
                    xChunk.updateChunk();
                }
                oringChunk.updateChunk();
            }
        }

        void setOutsideData()
        {

        }
        public IVec3 getTerrainPos()
        {
            return m_TerrainPos;
        }

        public void generateChunkData()
        {


        }

        public GameObject GetGameObject()
        {
            return m_GameObject;
        }

        public bool save()
        {
            string terrainFilePath = "./TerrainData/";
            string terrainName = "x" + m_TerrainPos.x + "_z" + m_TerrainPos.z + ".ter";
            string filePath = terrainFilePath + terrainName;
            Directory.CreateDirectory(terrainFilePath);

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                for (int i = 0; i < TerrainSystem.CHUNK_SIZE; ++i)
                {

                    for (int j = 0; j < TerrainSystem.CHUNK_SIZE; ++j)
                    {
                        for (int k = 0; k < TerrainSystem.CHUNK_SIZE; ++k)
                        {
                            if (m_Chunks[i, j, k] == null)
                                continue;
                            ChunkData chunkData = m_Chunks[i, j, k].getChunkData();
                            if (chunkData != null && chunkData.m_IsChange)
                            {
                                byte [] data = chunkData.saveToData();
                                fs.Write(data, 0, data.Length);
                            }
                        }
                    }
                }
                fs.Close();
            }
            return true;
        }

        public static Terrain load(IVec3 pos)
        {
            string terrainFilePath = "./TerrainData/";
            string terrainName = "x" + pos.x + "_z" + pos.z + ".ter";
            string fileName = terrainFilePath + terrainName;

            if (!File.Exists(fileName))
            {
                return null;
            }
            Terrain ter = new Terrain(pos);
            
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] data = new byte[ChunkData.AllDataSize];
                long length = fs.Length;
                long numChunkData = length / ChunkData.AllDataSize;
                for(int i=0; i< numChunkData; ++i)
                {
                    TerrainChunk chunk = new TerrainChunk(ter);
                    ChunkData chunkData = new ChunkData();
                    fs.Read(data, 0, ChunkData.AllDataSize);
                    chunkData.loadByData(data);
                    chunk.setTilePosition(chunkData.m_ChunkPos);
                    chunk.setChunkData(chunkData);
                    ter.setChunk(chunkData.m_ChunkPos, chunk);
                }
            }
            ter.getGameObject().transform.position = (ter.getTerrainPos() * TerrainSystem.TERRAIN_SIZE_X).toVector3();
            ter.getGameObject().transform.name = "" + ter.getTerrainPos();

            //for(int i=0; i< TerrainSystem.CHUNK_SIZE; ++i)
            //{
            //    for(int j=0; j< TerrainSystem.CHUNK_SIZE; ++j)
            //    {
            //        for (int k = 0; k < TerrainSystem.CHUNK_SIZE; ++k)
            //        {
            //            if (ter.m_Chunks[i, j, k] != null)
            //            {
            //            }
            //        }
            //    }
            //}
            return ter;
        }
    }
}
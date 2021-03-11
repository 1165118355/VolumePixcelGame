using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using LitJson;
using System;

namespace WaterBox
{
    public class TerrainCreator
    {
        private static TerrainCreator m_Instance = m_Instance = new TerrainCreator();
        public const int TERRAIN_MOUNTAIN_HEIGHT = 40;
        public const int TERRAIN_GROUND_HEIGHT = 2;
        private GameObject actorMaster;

        BerlinNoise noiseX1;
        BerlinNoise noiseY1;
        BerlinNoise noiseZ1;
        FastNoise noiseNew;

        BerlinNoise3D noise = new BerlinNoise3D();

        List<TerrainChunk> m_ProductionQueueChunk;
        List<TerrainChunk> m_ChunkDeleteBuffer;
        Queue<TerrainChunk> m_ChunkBuildQueue;

        private Thread m_GenerateChunkThread;
        private object m_ProductionQueueLocker = new object();
        private object m_BuildQueueLocker = new object();
        TerrainCreator()
        {
            noiseNew = new FastNoise();
            m_ProductionQueueChunk = new List<TerrainChunk>();
            m_ChunkDeleteBuffer = new List<TerrainChunk>();
            m_ChunkBuildQueue = new Queue<TerrainChunk>();
            m_GenerateChunkThread = new Thread(this.loadChunk);
            Start();
        }
        ~TerrainCreator()
        {
            m_GenerateChunkThread.Abort();
        }

        public static TerrainCreator get()
        {
            return m_Instance;
        }
        void Start()
        {
            noiseX1 = new BerlinNoise(0.01f, 2, 18);
            noiseY1 = new BerlinNoise(0.1f, 12, 18);
            noiseZ1 = new BerlinNoise(0.01f, 1, 18);
            //noiseX2 = new BerlinNoise(1.4f      , 2, 4);
            //noiseX3 = new BerlinNoise(0.18f     , 0.7f, 6);
            //noiseZ2 = new BerlinNoise(0.001f    , 6, 25);
            //noiseZ3 = new BerlinNoise(4         , 0.5f, 2);

            //noiseX1.addBerlinNoiseLayer(noiseX2);
            //noiseX1.addBerlinNoiseLayer(noiseX3);
            //noiseZ1.addBerlinNoiseLayer(noiseZ2);
            //noiseZ1.addBerlinNoiseLayer(noiseZ3);

            noise.setNoiseX(noiseX1);
            noise.setNoiseY(noiseY1);
            noise.setNoiseZ(noiseZ1);
        }

        public void shutdown()
        {
            m_GenerateChunkThread.Abort();
            lock (m_ProductionQueueLocker)
            {
                m_ProductionQueueChunk.Clear();
            }
            lock (m_BuildQueueLocker)
            {
                m_ChunkDeleteBuffer.Clear();
            }
            m_ChunkBuildQueue.Clear();
        }


        /// <summary>
        /// 在主线程中的更新函数，可能用来删除Unity的Mesh对象什么的
        /// </summary>
        public void updateMaster()
        {  
            //  删除需要删除的Chunk
            if(m_ChunkDeleteBuffer.Count > 1)
            {
                TerrainChunk deleteChunk = m_ChunkDeleteBuffer[0];
                m_ChunkDeleteBuffer.Remove(deleteChunk);
                deleteChunk.destory();
            }

            //  主线程中构建chunk
            TerrainChunk chunk =null;
            lock (m_BuildQueueLocker)
            {
                if(m_ChunkBuildQueue.Count>0)
                {
                    chunk = m_ChunkBuildQueue.Dequeue();
                }
            }
            if (chunk != null)
            {
                chunk.State = TerrainChunk.TerrainChunkState.STATE_COMPLETE;
                chunk.generateChunk();
            }
        }


        public void addBuildQueue(TerrainChunk chunk)
        {
            lock (m_BuildQueueLocker)
            {
                m_ChunkBuildQueue.Enqueue(chunk);
            }
        }

        public void addProductionQueue(TerrainChunk chunk)
        {
            lock (m_ProductionQueueLocker)
            {
                m_ProductionQueueChunk.Add(chunk);
            }
        }


        /// <summary>
        /// 寻找Chunk，如果没有找到则将该Chunk放入待加载缓存区
        /// </summary>
        /// <param name="chunkTer1"></param>
        /// <param name="chunkTile1"></param>
        /// <returns></returns>
        public TerrainChunk getChunk(Terrain terrain, IVec3 chunkTile1)
        {
            //  去正在加载的队列中找下
            IVec3 terrainPos = terrain.getTerrainPos();
            lock (m_ProductionQueueLocker)
            {
                for (int i = 0; i < m_ProductionQueueChunk.Count; ++i)
                {
                    TerrainChunk chunk = m_ProductionQueueChunk[i];
                    IVec3 chunkTer2 = chunk.getTerrainPosition();
                    IVec3 chunkTile2 = chunk.getTilePosition();
                    if (terrainPos == chunkTer2 &&
                        chunkTile1 == chunkTile2)
                    {
                        return chunk;
                    }
                }
            }
        
            //  加载列表都没有该chunk，那么我们就要将该Chunk放入加载列表中
            TerrainChunk newChunk = new TerrainChunk(terrain);
            newChunk.setTerrainPosition(terrainPos);
            newChunk.setTilePosition(chunkTile1);
            terrain.setChunk(chunkTile1, newChunk);
            addProductionQueue(newChunk);

            return newChunk;
        }

        public Material getMaterial(int value)
        {
            Material material;
            switch (value )
            {
                case 1:
                    material = Resources.Load("Materials/Box/Materials/Soil") as Material;
                    break;
                case 2:
                    material = Resources.Load("Materials/Box/Materials/Stone") as Material;
                    break;
                default:
                    material = new Material(Shader.Find("Standard"));
                    material.color = Color.red;
                    //material = Resources.Load("Scene/TankDemo/Materials/Red") as Material;
                    break;
            }
            return material;
        }

        /// <summary>
        ///     删除一个CHunk
        /// </summary>
        /// <param name="chunk"></param>
        public void recoverChunk(TerrainChunk chunk)
        {
            m_ChunkDeleteBuffer.Add(chunk);
        }

        /// <summary>
        /// 唤醒加载线程
        /// </summary>
        public void awakenLoad()
        {
            if (!m_GenerateChunkThread.IsAlive)
            {
                if (m_ProductionQueueChunk.Count > 0)
                {
                    if (m_GenerateChunkThread.ThreadState == ThreadState.Unstarted)
                    {
                        m_GenerateChunkThread.Start();
                    }
                }
            }
        }
        void loadChunk()
        {
            while (true)
            {
                if (m_ProductionQueueChunk.Count == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                TerrainChunk chunk = null;
                lock (m_ProductionQueueLocker)
                {
                    if (m_ProductionQueueChunk.Count > 0)
                    {
                        chunk = m_ProductionQueueChunk[0];
                    }
                }
                if(chunk != null)
                {
                    generateChunk(chunk);
                    lock (m_ProductionQueueLocker)
                    {
                        m_ProductionQueueChunk.Remove(chunk);
                    }
                    addBuildQueue(chunk);
                }
            }
        }

        /// <summary>
        /// 创建一个地形
        /// </summary>
        /// <param name="x">地形的x</param>
        /// <param name="y">地形的y</param>
        ChunkData createChunkData(IVec3 terrainPos, IVec3 chunkPos)
        {
            int x = terrainPos.x * TerrainSystem.TERRAIN_SIZE_X + chunkPos.x * TerrainSystem.CHUNK_SIZE_X;
            int z = terrainPos.z * TerrainSystem.TERRAIN_SIZE_Z + chunkPos.z * TerrainSystem.CHUNK_SIZE_Z;
            ChunkData data = new ChunkData();
            data.OutSideData = new ChunkOutSideData();
            if (data.load(getTerrainPath(terrainPos, chunkPos)))
            {
                return data;
            }
            data.m_TerrainIndexX = terrainPos.x;
            data.m_TerrainIndexZ = terrainPos.z;
            data.m_ChunkPos = chunkPos;

            bool isChunkHaveBlock = false;
            for (int horizontalIndex = 0; horizontalIndex < TerrainSystem.CHUNK_SIZE_X; ++horizontalIndex)
            {
                for (int heightIndex = 0; heightIndex < TerrainSystem.CHUNK_SIZE_Y; ++heightIndex)
                {
                    for (int verticalIndex = 0; verticalIndex < TerrainSystem.CHUNK_SIZE_Z; ++verticalIndex)
                    {
                        IVec3 blockPos = new IVec3(horizontalIndex, heightIndex, verticalIndex);
                        int value = calcChunkData(terrainPos, chunkPos, blockPos);
                        data.setValue(blockPos.x, blockPos.y, blockPos.z, value);
                        if(value != 0)
                        {
                            isChunkHaveBlock = true;
                        }

                        ////  检查并设置边界数据
                        //if (verticalIndex == TerrainSystem.CHUNK_SIZE_Z - 1)
                        //{
                        //    blockPos.z += 1;
                        //    bool isHaveBlock = calcChunkData(terrainPos, chunkPos, blockPos) != 0;
                        //    data.OutSideData.setDataValue(ChunkOutSideData.DataType.TYPE_BACK, blockPos.x, blockPos.y, isHaveBlock);
                        //}
                        //else if (verticalIndex == 0)
                        //{
                        //    blockPos.z -= 1;
                        //    bool isHaveBlock = calcChunkData(terrainPos, chunkPos, blockPos) != 0;
                        //    data.OutSideData.setDataValue(ChunkOutSideData.DataType.TYPE_FRONT, blockPos.x, blockPos.y, isHaveBlock);
                        //}

                        //blockPos = new IVec3(horizontalIndex, heightIndex, verticalIndex);
                        //if (heightIndex == TerrainSystem.CHUNK_SIZE_Y - 1)
                        //{
                        //    blockPos.y += 1;
                        //    bool isHaveBlock = calcChunkData(terrainPos, chunkPos, blockPos) != 0;
                        //    data.OutSideData.setDataValue(ChunkOutSideData.DataType.TYPE_TOP, blockPos.x, blockPos.z, isHaveBlock);
                        //}
                        //else if (heightIndex == 0)
                        //{
                        //    blockPos.y -= 1;
                        //    bool isHaveBlock = calcChunkData(terrainPos, chunkPos, blockPos) != 0;
                        //    data.OutSideData.setDataValue(ChunkOutSideData.DataType.TYPE_BOTTOM, blockPos.x, blockPos.z, isHaveBlock);
                        //}

                        //blockPos = new IVec3(horizontalIndex, heightIndex, verticalIndex);
                        //if (horizontalIndex == TerrainSystem.CHUNK_SIZE_X - 1)
                        //{
                        //    blockPos.x += 1;
                        //    bool isHaveBlock = calcChunkData(terrainPos, chunkPos, blockPos) != 0;
                        //    data.OutSideData.setDataValue(ChunkOutSideData.DataType.TYPE_RIGHT, blockPos.y, blockPos.z, isHaveBlock);
                        //}
                        //else if (horizontalIndex == 0)
                        //{
                        //    blockPos.x -= 1;
                        //    bool isHaveBlock = calcChunkData(terrainPos, chunkPos, blockPos) != 0;
                        //    data.OutSideData.setDataValue(ChunkOutSideData.DataType.TYPE_LEFT, blockPos.y, blockPos.z, isHaveBlock);
                        //}
                    }
                }
            }
            //if(isChunkHaveBlock)
            //{
            //    data.save();
            //}
            return data;
        }

        int calcChunkData(IVec3 terrainPos, IVec3 chunkPos, IVec3 blockPos)
        {
            int value = 0;

            int x = terrainPos.x * TerrainSystem.TERRAIN_SIZE_X + chunkPos.x * TerrainSystem.CHUNK_SIZE_X;
            int y = terrainPos.y * TerrainSystem.TERRAIN_SIZE_Y + chunkPos.y * TerrainSystem.CHUNK_SIZE_Y;
            int z = terrainPos.z * TerrainSystem.TERRAIN_SIZE_Z + chunkPos.z * TerrainSystem.CHUNK_SIZE_Z;

            //data.m_terrainArray[horizontalIndex, heightIndex, verticalIndex] = 1;
            float noiseValue = createBerlinNoise(x + blockPos.x, y + blockPos.y, z + blockPos.z);
            //height = height * TERRAIN_MOUNTAIN_HEIGHT+ TERRAIN_GROUND_HEIGHT;

            float blockHeight = blockPos.y + chunkPos.y * TerrainSystem.CHUNK_SIZE_Y;
            blockHeight = blockHeight / TerrainSystem.TERRAIN_SIZE_Y;
            blockHeight = (noiseValue + blockHeight) / 2.0f;
            //blockHeight = noiseValue;
            if (blockHeight < 0.5)
            {
                if (blockHeight > 0.45)
                {
                    value = 1;
                }
                else 
                {
                    value = 2;
                }
            }
            else
            {
                value = 0;
            }
            if(chunkPos.y == 0 && blockPos.y == 0)
            {
                value = 2;
            }
            return value;
        }
    

        public void generateChunk(TerrainChunk chunk)
        {
            IVec3 terrainPos = chunk.getTerrainPosition();
            IVec3 chunkPos = chunk.getTilePosition();
            ChunkData data = chunk.getChunkData();

            var firstTime = DateTime.Now;
            if (data == null)
                data = createChunkData(terrainPos, chunkPos);
            var lastTime = DateTime.Now;
            Debug.Log("consume time =" + (lastTime - firstTime).TotalMilliseconds);
            //if (!isHaveTerrain(terrainPos, chunkPos))//   没想到读文件会造成卡顿，这里先注释掉了
            //{
            //    UtilsCommon.log("create Chunk");
            //    data = createChunk(terrainPos, chunkPos);
            //}
            //else
            //{
            //    data = new ChunkData(getTerrainPath(terrainPos, chunkPos));
            //}
            chunk.setChunkData(data);
        }

        bool isHaveTerrain(IVec3 terrainPos, IVec3 chunkPos)
        {
            string terrainPath = getTerrainPath(terrainPos, chunkPos);
            if (File.Exists(terrainPath))
            {
                return true;
            }
            return false;
        }

        string getTerrainPath(IVec3 terrainPos, IVec3 chunkPos)
        {
            string terrainDir = "./TerrainData/";
            string terrainName = "x" + terrainPos.x + "_z" + terrainPos.y + "/";
            string chunkName = "chunk_x" + chunkPos.x + "_y" + chunkPos.y + "_z" + chunkPos.z + ".ter";
            string terrainPath = terrainDir + terrainName + chunkName;
            return terrainPath;
        }

        public void GetMessage(string message)
        {
        }

        protected float createBerlinNoise(int x, int y, int z)
        {
            x = Mathf.Abs(x+1000000);
            y = Mathf.Abs(y + 1000000);
            z = Mathf.Abs(z+1000000);
            float randomValue = 0;
            randomValue += noiseX1.berlinNoise(x) * 0.5f;
            randomValue += noiseZ1.berlinNoise(z) * 0.5f;
            
            return noise.berlinNoise(x, y, z);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WaterBox
{
    public class TerrainSystem :GameSystem
    {
        static public int TERRAIN_SIZE = 256;
        static public int TERRAIN_SIZE_X = 256;
        static public int TERRAIN_SIZE_Y = 256;
        static public int TERRAIN_SIZE_Z = 256;

        static public int CHUNK_SIZE = 16;
        static public int CHUNK_SIZE_X = 16;
        static public int CHUNK_SIZE_Y = 16;
        static public int CHUNK_SIZE_Z = 16;

        Terrain m_CurrentTerrain;
        private List<Terrain> m_AroundTerrain;
        private float m_LoadDistance;
        private float m_UnloadDistance;
        public TerrainSystem(GameSystemMG gameSystemMG):
            base(gameSystemMG)
        {
            m_LoadDistance = 128;
            m_UnloadDistance = 32;
            m_AroundTerrain = new List<Terrain> ();
    }

        public override void init()
        {
            EventSystem.get().registrationEvent<Varible, Varible>(EventEnum.EventEnumType.TERRAIN_BLOCK_REMOVE, removeBlock);
            EventSystem.get().registrationEvent<Varible, Varible>(EventEnum.EventEnumType.TERRAIN_BLOCK_ADD, addBlock);
            EventSystem.get().registrationEvent(EventEnum.EventEnumType.TERRAIN_SAVE, save);
        }
	
	    // Update is called once per frame
	    public override void update()
        {
            foreach(var i in m_AroundTerrain)
            {
                i.update();
            }
            autoGenerateTerrain();
            TerrainCreator.get().updateMaster();
        }

        /// <summary>
        /// 生成地形
        /// </summary>
        /// <param name="terrainPos"> 地形的位置 </param>
        /// <returns>返回地形</returns>
        Terrain generateTerrain(IVec3 terrainPos)
        {
            Terrain ter = new Terrain(terrainPos);
            return ter;
        }
        void generateChunk(Terrain terrain, IVec3 chunkPos)
        {
            if(!(chunkPos.x >= 0 && chunkPos.x < CHUNK_SIZE_X && 
                chunkPos.y >= 0 && chunkPos.y < CHUNK_SIZE_Y && 
                chunkPos.z >= 0 && chunkPos.z < CHUNK_SIZE_Z))
            {
                return;
            }
            TerrainChunk chunk = terrain.getChunk(chunkPos);

            if (chunk == null || 
                chunk.State == TerrainChunk.TerrainChunkState.STATE_NULL)
            {
                if(chunk == null)
                {
                    chunk = TerrainCreator.get().getChunk(terrain, chunkPos);
                }
                else
                {
                    TerrainCreator.get().addProductionQueue(chunk);
                }
                chunk.State = TerrainChunk.TerrainChunkState.STATE_LOADING;

                TerrainCreator.get().awakenLoad();
            }
        }
          

        float m_CheckTerrainTime = 0;
        /// <summary>
        /// 自动生成地形                        
        /// </summary>
        void autoGenerateTerrain()
        {
            m_CheckTerrainTime += Time.deltaTime;
            if(m_CheckTerrainTime < 0.7)
            {
                //  0.7秒检查一次
                return;
            }
            m_CheckTerrainTime = 0;

            //  拿到主角
            GameObject mainCamera = GameObject.Find("ActorMaster");
            ActorMaster actorMaster = mainCamera.GetComponent<ActorMaster>();
            //  判断主角的位置，并选择加载或卸载地形
            IVec3 actorPos = new IVec3(actorMaster.tilePosX, actorMaster.tilePosY, actorMaster.tilePosZ);
            IVec3 terrainPos = new IVec3(actorPos.x / TERRAIN_SIZE_X, 0, actorPos.z / TERRAIN_SIZE_Z);
            actorPos.x -= (int)terrainPos[0] * TERRAIN_SIZE_X;
            actorPos.z -= (int)terrainPos[2] * TERRAIN_SIZE_Z;

            int loadCakeRadius = (int)(m_LoadDistance / 16);
            int loadCakeDiameter = loadCakeRadius * 2 + 1;
            int loadNumber = loadCakeDiameter * CHUNK_SIZE_Y * loadCakeDiameter;
            List<IVec3> loadPos = new List<IVec3>();
            int x = 0;
            int y = 0;
            int z = 0;
            int yStart = 0;
            for(int i =0; i< loadCakeRadius; ++i)
            {
                for(int j=0; j<=i*2; ++j)
                {
                    x = j - i;
                    int yAdd = j < i ? 1 : -1;
                    for(int l=0; l <=yStart*2; ++l)
                    {
                        int zAdd = l < yStart ? 1 : -1;
                        y = l - yStart;
                        if (j != 0 && j != j * 2 &&
                            l != 0 && l != yStart*2)
                        {
                            IVec3 l2 = new IVec3(actorPos.x + x, actorPos.y + y, actorPos.z + z);
                            IVec3 l3 = new IVec3(actorPos.x + x, actorPos.y + y, actorPos.z - z);
                            loadPos.Add(l2);
                            loadPos.Add(l3);
                        }
                        else
                        {
                            IVec3 l1 = new IVec3(actorPos.x + x, actorPos.y + y, actorPos.z);
                            loadPos.Add(l1);
                        }
                        z += zAdd;
                    }
                    yStart += yAdd;
                    z = 0;
                }
                yStart = 0;
                x = 0;
            }

            int loadTileNumber = loadPos.Count;
            for (int i = 0; i < loadTileNumber; ++i)
            {
                IVec3 worldPos = new IVec3(loadPos[i].x * CHUNK_SIZE_X, loadPos[i].y * CHUNK_SIZE_X, loadPos[i].z * CHUNK_SIZE_X);
                worldPos.y = Math.Max(0, worldPos.y);
                worldPos.y = Math.Min(TERRAIN_SIZE_Y-1, worldPos.y);
                terrainPos = calcTerrain(worldPos);
                IVec3 chunkPos = calcChunk(worldPos);
                Terrain terrain = getTerrain(terrainPos);
                generateChunk(terrain, chunkPos);
            }
        }

        /// <summary>
        /// 判断是否已经加载
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        bool isLoaded(IVec3 pos)
        {
            return false;
        }

        Terrain getTerrain(IVec3 pos)
        {
            Terrain terrain;
            foreach (var i in m_AroundTerrain)
            {
                var terPos = i.getTerrainPos();
                if (terPos.x == pos.x &&
                    terPos.z == pos.z)
                {
                    return i;
                }
            }
            terrain = Terrain.load(pos);
            if (terrain == null)
            {
                terrain = new Terrain(pos);
                terrain.start();
            }
            m_AroundTerrain.Add(terrain);
            return terrain;
        }

        public IVec3 calcChunk(Vector3 pos)
        {
            //  得到chunk和block的坐标
            int chunkX = (((int)Math.Floor(pos.x ) % TERRAIN_SIZE_Y) + TERRAIN_SIZE_Y) % TERRAIN_SIZE_Y / CHUNK_SIZE_X;
            int chunkY = (((int)Math.Floor(pos.y ) % TERRAIN_SIZE_Y) + TERRAIN_SIZE_Y) % TERRAIN_SIZE_Y / CHUNK_SIZE_Y;
            int chunkZ = (((int)Math.Floor(pos.z ) % TERRAIN_SIZE_Y) + TERRAIN_SIZE_Y) % TERRAIN_SIZE_Y / CHUNK_SIZE_Z;
            return new IVec3(chunkX, chunkY, chunkZ);

        }
        public IVec3 calcTerrain(Vector3 pos)
        {
            //  得到chunk和block的坐标
            int chunkX = (int)Math.Floor(pos.x / TERRAIN_SIZE_X);
            int chunkY = (int)Math.Floor(pos.y / TERRAIN_SIZE_Y);
            int chunkZ = (int)Math.Floor(pos.z / TERRAIN_SIZE_Z);
            return new IVec3(chunkX, chunkY, chunkZ);
        }
        public IVec3 calcBlock(Vector3 pos)
        {
            //  得到chunk和block的坐标
            int chunkX = (int)((pos.x % CHUNK_SIZE_X) + CHUNK_SIZE_X)% CHUNK_SIZE_X;
            int chunkY = (int)((pos.y % CHUNK_SIZE_Y) + CHUNK_SIZE_Y)% CHUNK_SIZE_Y;
            int chunkZ = (int)((pos.z % CHUNK_SIZE_Z) + CHUNK_SIZE_Z)% CHUNK_SIZE_Z;
            return new IVec3(chunkX, chunkY, chunkZ);
        }

        /// <summary>
        /// 移除一个方块
        /// </summary>
        /// <param name="pos"></param>
        public void removeBlock(Varible pos, Varible normal)
        {
            Vector3 wpos = pos.getVec3();
            Vector3 wnormal = normal.getVec3();
            wpos = wpos - wnormal * 0.1f;

            IVec3 terrainPos = calcTerrain(wpos);

            IVec3 chunkPos = calcChunk(wpos);
            IVec3 blockPos = calcBlock(wpos);
            Terrain terrian = getTerrain(terrainPos);

            if(terrian != null)
            {
                terrian.setBlock(chunkPos, blockPos, 0);
            }
        }

        /// <summary>
        /// 添加一个方块
        /// </summary>
        /// <param name="pos"></param>
        public void addBlock(Varible pos, Varible normal)
        {
            Vector3 wpos = pos.getVec3();
            Vector3 wnormal = normal.getVec3();
            wpos = wpos + wnormal * 0.1f;

            IVec3 terrainPos = calcTerrain(wpos);

            IVec3 chunkPos = calcChunk(wpos);
            IVec3 blockPos = calcBlock(wpos);
            Terrain terrian = getTerrain(terrainPos);

            if (terrian != null)
            {
                terrian.setBlock(chunkPos, blockPos, 1);
            }
        }
        public void save()
        {
            foreach (var i in m_AroundTerrain)
            {
                i.save();
            }
        }

        public override void shutdown()
        {
            base.shutdown();
            TerrainCreator.get().shutdown();
        }
    }
}
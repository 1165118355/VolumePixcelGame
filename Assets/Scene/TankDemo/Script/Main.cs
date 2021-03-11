using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WaterBox
{
    public class Main : MonoBehaviour {

        GameSystemMG m_GameSystemMG;

        void Awake()
        {
            m_GameSystemMG = GameSystemMG.Instance;
            m_GameSystemMG.init();
        }
        void Start()
        {
            //Terrain terrain = new Terrain();
            //TerrainChunk chunk = new TerrainChunk(terrain);
            //chunk.setTilePosition(new IVec3(0,1, 0));
            //TerrainCreator.get().generateChunk(chunk);
            //ChunkData data = chunk.getChunkData();
            //byte [] sdata = data.saveToData();

            //using (FileStream fs = new FileStream("./testFile", FileMode.Create, FileAccess.Write))
            //{
            //    fs.Write(sdata, 0, ChunkData.DataSize);
            //    fs.Close();
            //}
            //sdata = new byte[ChunkData.AllDataSize];
            //using (FileStream fs = new FileStream("./testFile", FileMode.Open, FileAccess.Read))
            //{
            //    fs.Read(sdata, 0, ChunkData.DataSize);
            //    fs.Close();
            //}

            //TerrainChunk newChunk = new TerrainChunk(terrain);
            //ChunkData newChunkdata = new ChunkData();
            //newChunkdata.loadByData(sdata);
            //newChunk.setChunkData(newChunkdata);
            //newChunk.generateChunk();
        }

        // Update is called once per frame
        void Update()
        {
            m_GameSystemMG.update();
        }
    }
}

/*
 
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            double milliseconds = stopwatch.ElapsedMilliseconds;
            UtilsCommon.log("Genaerate Chunkss ElapsedTime = " + milliseconds);
            stopwatch.Stop();
     
     */

using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using System;

namespace WaterBox
{
    [Serializable]
    public class ChunkData
    {
        public static int HeaderSize = 3;
        public static int DataSize =TerrainSystem.CHUNK_SIZE * TerrainSystem.CHUNK_SIZE * TerrainSystem.CHUNK_SIZE;
        public static int AllDataSize = 3 + DataSize;
        /// <summary>
        /// 地形块的数据和地形块数据的尺寸
        /// </summary>
        public byte[] m_DataArray { set; get; }

        /// <summary>
        /// 索引，像是(0, 0)(0, 1)这样的，他代表这个地形块数据的位置
        /// </summary>
        public int m_TerrainIndexX { set; get; }
        public int m_TerrainIndexZ { set; get; }

        public IVec3 m_ChunkPos = new IVec3();

        public bool m_IsNull = false;
        public bool m_IsChange = false;


        ChunkOutSideData m_OutSideData;
        public ChunkOutSideData OutSideData 
        {
            get { return m_OutSideData; }
            set { m_OutSideData = value; }
        }

        /// <summary>
        ///     构造函数耶耶耶
        /// </summary>
        public ChunkData()
        {
            m_DataArray = new byte[DataSize] ;
        }
        ~ChunkData()
        {
            m_DataArray = null;
        }

        /// <summary>
        ///     构造一个地形数据对象并加载一个本地的地图数据
        /// </summary>
        /// <param name="fileName">地图数据的名字</param>
        public ChunkData(string fileName)
        {
            load(fileName);
        }

        /// <summary>
        ///     保存到本地（根据xz坐标命名）
        /// </summary>
        /// <returns></returns>
        public byte[] saveToData()
        {
            byte[] data = new byte[AllDataSize];
            data[0] = (byte)m_ChunkPos.x;
            data[1] = (byte)m_ChunkPos.y;
            data[2] = (byte)m_ChunkPos.z;
            Array.Copy(m_DataArray, 0, data, 3, DataSize);
            return data;
        }

        public bool loadByData(byte[] data)
        {
            if(data.Length < ChunkData.AllDataSize)
            {
                return false;
            }
            m_ChunkPos = new IVec3(data[0], data[1], data[2]);

            Array.Copy(data, 3, m_DataArray, 0, DataSize);
            m_IsChange = true;
            return true;
        }

        /// <summary>
        ///     本地加载一个地图数据
        /// </summary>
        /// <param name="fileName">地图数据的路径文件名</param>
        /// <returns>是否加载成功</returns>
        public bool load(string fileName)
        {
            return false;
        }

        public int getOffset(int x, int y, int z)
        {
            return x *(TerrainSystem.CHUNK_SIZE * TerrainSystem.CHUNK_SIZE) + y * TerrainSystem.CHUNK_SIZE + z;
        }

        public void setValue(int x, int y, int z, int value)
        {
            m_DataArray[getOffset(x, y, z)] = (byte)value;
        }
        public void setValue(IVec3 pos, int value)
        {
            m_DataArray[getOffset(pos.x, pos.y, pos.z)] = (byte)value;
        }
        public short getValue(int x, int y, int z)
        {
            return m_DataArray[getOffset(x,y,z)];
        }
    }

}
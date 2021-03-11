using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// 用于判断Chunk边上的面是否需要渲染出来，典型的用空间换时间
/// </summary>
namespace WaterBox
{
    [Serializable]
    public class ChunkOutSideData
    {
        public enum DataType
        {
            TYPE_TOP,
            TYPE_BOTTOM,
            TYPE_FRONT,
            TYPE_BACK,
            TYPE_LEFT,
            TYPE_RIGHT,
        }
        public byte[] m_TopOutSide;
        public byte[] m_BottomOutSide;
        public byte[] m_FrontOutSide;
        public byte[] m_BackOutSide;
        public byte[] m_LeftOutSide;
        public byte[] m_RightOutSide;

        static ushort ArraySize = 32;
        public void load(JsonData data)
        {
            loadArray(DataType.TYPE_TOP, data["m_TopOutSide"]);
            loadArray(DataType.TYPE_BOTTOM, data["m_BottomOutSide"]);
            loadArray(DataType.TYPE_FRONT, data["m_FrontOutSide"]);
            loadArray(DataType.TYPE_BACK, data["m_BackOutSide"]);
            loadArray(DataType.TYPE_LEFT, data["m_LeftOutSide"]);
            loadArray(DataType.TYPE_RIGHT, data["m_RightOutSide"]);
        }
        void loadArray(DataType type, JsonData data)
        {
            byte[]  array = getArray(type);
            if(array == null || data == null)
            {
                return;
            }
            for(int i=0; i< ArraySize; ++i)
            {
                array[i] = byte.Parse(data[i].ToString());
            }

        }

        public void setDataValue(DataType type, int x, int y, bool value)
        {
            byte[] array = getArray(type); 
            setArrayValue(array, x, y, value);
        }
        public int getDataValue(DataType type, int x, int y)
        {
            byte[] array = getArray(type);
            int value = getArrayValue(array, x, y);
            return value;
        }


        int getArrayValue(byte[] array, int x, int y)
        {
            if (array == null)
            {
                return -1;
            }
            int value = 0;

            int byteIndex = y * 2;
            int bitOffset = x;
            if (x > 7)
            {
                bitOffset -= 8;
                byteIndex += 1;
            }

            int tempValue = array[byteIndex];
            bitOffset = 1 << bitOffset;
            tempValue = (byte)(tempValue & bitOffset);
            return tempValue;
        }

        void setArrayValue(byte []array, int x, int y, bool value)
        {
            if (array == null)
            {
                return;
            }

            int byteIndex = y * 2;
            int bitOffset = x;
            if (x > 7)
            {
                bitOffset -= 8;
                byteIndex += 1;
            }
            int baseBitValue = 1;
            if(byteIndex > 31)
            {
                return;
            }

            int tempValue = array[byteIndex];
            bitOffset = baseBitValue << bitOffset;
            if(value)
            {
                tempValue = (tempValue | bitOffset);
            }
            else
            {
                tempValue = (tempValue & (~bitOffset));
            }
            array[byteIndex] = (byte)tempValue;
        }
        byte[] getArray(DataType type)
        {
            byte[] array = null;
            switch (type)
            {
                case DataType.TYPE_TOP:
                    if (m_TopOutSide == null)
                        m_TopOutSide = new byte[ArraySize];
                    array = m_TopOutSide;
                    break;
                case DataType.TYPE_BOTTOM:
                    if (m_BottomOutSide == null)
                        m_BottomOutSide = new byte[ArraySize];
                    array = m_BottomOutSide;
                    break;
                case DataType.TYPE_FRONT:
                    if (m_FrontOutSide == null)
                        m_FrontOutSide = new byte[ArraySize];
                    array = m_FrontOutSide;
                    break;
                case DataType.TYPE_BACK:
                    if (m_BackOutSide == null)
                        m_BackOutSide = new byte[ArraySize];
                    array = m_BackOutSide;
                    break;
                case DataType.TYPE_LEFT:
                    if (m_LeftOutSide == null)
                        m_LeftOutSide = new byte[ArraySize];
                    array = m_LeftOutSide;
                    break;
                case DataType.TYPE_RIGHT:
                    if (m_RightOutSide == null)
                        m_RightOutSide = new byte[ArraySize];
                    array = m_RightOutSide;
                    break;
            }
            return array;

        }
    }
}

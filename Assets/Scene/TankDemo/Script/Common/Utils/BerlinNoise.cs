using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class BerlinNoise
    {
        public BerlinNoise(float frequency = 1, float amplitude = 1, float weight = 1)
        {
            m_Weight = weight;
            m_Frequency = frequency;
            m_Amplitude = amplitude;
            m_BerlinNoiseLayers = new List<BerlinNoise>();
            m_Seed = m_SeedIndex++;
        }
        public BerlinNoise(BerlinNoise clone)
        {
            m_Weight = clone.m_Weight;
            m_Frequency = clone.m_Frequency;
            m_Amplitude = clone.m_Amplitude;
            m_BerlinNoiseLayers = clone.m_BerlinNoiseLayers;
            m_Seed = clone.m_Seed;
        }

        ///	\brief	设置发生器种子
        public void setSeed(int seed)
        {
            m_Seed = seed;
        }
        public int getSeed()
        {
            return m_Seed;
        }

        ///	\brief	输入一个值，返回一个噪声值，噪声值为0~1
        public float berlinNoise(float seed)
        {
            float theSeed = seed * m_Frequency;
            float priorValue = UtilsMath.rand(m_Seed, (int)theSeed);
            float nextValue = UtilsMath.rand(m_Seed, (int)Math.Ceiling(theSeed));
            float randomValue = UtilsMath.lerp(priorValue, nextValue, theSeed - (int)theSeed);    //	用随机数得出来的
            randomValue *= (1/m_Amplitude);

            //	权重叠加
            float weightMax = m_Weight;
            for (int i = 0; i < m_BerlinNoiseLayers.Count; ++i)
            {
                weightMax += m_BerlinNoiseLayers[i].getWeight();
            }

            //	噪点融合
            randomValue = randomValue * (m_Weight / weightMax);
            for (int i = 0; i < m_BerlinNoiseLayers.Count(); ++i)
            {
                float layerRandomValue = m_BerlinNoiseLayers[i].berlinNoise(seed);
                float layerWeightValue = m_BerlinNoiseLayers[i].getWeight();
                randomValue += layerRandomValue * (layerWeightValue / weightMax);
            }

            randomValue = (randomValue + 1.0f) / 2;
            return randomValue;
        }

        ///	\brief	设置频率
        public void setFrequency(float frequency)
        {
            m_Frequency = frequency;
        }

        public float getFrequency()
        {
            return m_Frequency;
        }

        ///	\brief	设置振幅
        public void setAmplitude(float amplitude)
        {
            m_Amplitude = amplitude;
        }
        public float getAmplitude()
        {
            return m_Amplitude;
        }

        ///	\brief	设置权重
        public void setWeight(float weight)
        {
            m_Weight = weight;
        }
        public float getWeight()
        {
            return m_Weight;
        }

        ///	\brief	添加一个组合的柏林噪声
        public void addBerlinNoiseLayer(BerlinNoise berlin)
        {
            m_BerlinNoiseLayers.Add(berlin);
        }

        private int m_Seed;         //	随机数发生器种子
        private float m_Frequency;  //	频率(注意：如果频率过高但采样周期低的话会导致数据丢失，例如：频率是10则采样周期应是0.1，也就是berlinNoise最低应该以0.1递增，同理，如果频率是5，则berlinNoise最低应该以0.5递增，否则就会造成数据丢失
        private float m_Amplitude;  //	振幅(值在0~1之间
        private float m_Weight;     //	权重（噪声叠加融合的时候会用到，他越大，噪声波的样子越像他）
        private List<BerlinNoise> m_BerlinNoiseLayers;

        static int m_SeedIndex;
    }
    class BerlinNoise2D
    {
        public void setNoiseX(BerlinNoise noise)
        {
            m_XNoise = noise;
            m_XNoiseSeed = new BerlinNoise(noise);
        }
        public void setNoiseY(BerlinNoise noise)
        {
            m_YNoise = noise;
            m_YNoiseSeed = new BerlinNoise(noise);
        }

        public void setSeed(int seed)
        {
            m_YNoiseSeed.setSeed(seed);
        }

        public float berlinNoise(int ix, int iy)
        {
            float y = iy * m_YNoise.getFrequency();
            m_XNoise.setSeed((int)(1000000 * m_YNoiseSeed.berlinNoise((int)y)));
            float randomValue1 = m_XNoise.berlinNoise(ix);
            m_XNoise.setSeed((int)(1000000 * m_YNoiseSeed.berlinNoise((int)y+1)));
            float randomValue2 = m_XNoise.berlinNoise(ix);

            float lerpValue = y - (int)y;
            float randomValue = UtilsMath.lerp(randomValue1, randomValue2, lerpValue);

            return randomValue;
        }
        BerlinNoise m_XNoise;
        BerlinNoise m_YNoise;
        BerlinNoise m_XNoiseSeed;
        BerlinNoise m_YNoiseSeed;
    }

    class BerlinNoise3D
    {
        public void setNoiseX(BerlinNoise noise)
        {
            m_XZNoise.setNoiseX(noise);
        }
        public void setNoiseY(BerlinNoise noise)
        {
            m_YNoise = noise;
        }
        public void setNoiseZ(BerlinNoise noise)
        {
            m_XZNoise.setNoiseY(noise);
        }

        public float berlinNoise(int ix, int iy, int iz)
        {
            float y = iy * m_YNoise.getFrequency();
            float lerpValueY = y - (int)y;

            m_XZNoise.setSeed((int)y);
            float randomValue1 = m_XZNoise.berlinNoise(ix, iz);
            m_XZNoise.setSeed((int)y + 1);
            float randomValue2 = m_XZNoise.berlinNoise(ix, iz);

            float randomValue = UtilsMath.lerp(randomValue1, randomValue2, lerpValueY);
            //float randomValue = m_XZNoise.berlinNoise(ix, iz);
            return randomValue;
        }


        BerlinNoise2D m_XZNoise = new BerlinNoise2D();
        BerlinNoise m_YNoise;
    }
}

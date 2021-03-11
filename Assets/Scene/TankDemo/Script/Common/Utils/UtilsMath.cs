using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class UtilsMath
    {        ///	\brief	随机函数，输入一个正整数，返回一个0~1的伪随机数
        static public float rand(int seed, int x)
        {
            x = (x << 13) ^ x;
            //float randomVlaue = (1.0 - ((x * (x * x * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
            float randomVlaue = (1.0f - ((x * (x * x * seed + 3) + 1376312589) & 0x7fffffff) / 1073741824.0f);
            return randomVlaue;
        }

        ///	\brief	插值算法
        static public float lerp(float a, float b, float x)
        {
            float ft = x * 3.1415927f;
            float f = (1 - (float)Math.Cos(ft)) * 0.5f;
            return a * (1 - f) + b * f;
        }

        static public int pow(int who, int pow)
        {
            int result = who;
            for(int i=1; i<pow; ++i)
            {
                result *= who;
            }
            return result;
        }
    }
}

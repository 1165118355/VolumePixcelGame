using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WaterBox
{
    [Serializable]
    public class Vec3
    {
        public double x,y,z;
        public Vec3(double x,double y,double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [Serializable]
    public class IVec3
    {
        public int x, y, z;
        public IVec3()
        {

        }
        public IVec3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3 toVector3()
        {
            return new Vector3(x,y,z);
        }
        public int this[int index]
        { 
            get {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                }
                return 0;
            }
            set{
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                }
            }
        }
        public static IVec3 operator *(IVec3 b, IVec3 c)
        {
            IVec3 ret = new IVec3();
            ret.x = b.x * c.x;
            ret.y = b.y * c.y;
            ret.z = b.z * c.z;
            return ret;
        }
        public static IVec3 operator +(IVec3 b, IVec3 c)
        {
            IVec3 ret = new IVec3();
            ret.x = b.x + c.x;
            ret.y = b.y + c.y;
            ret.z = b.z + c.z;
            return ret;
        }
        public static IVec3 operator -(IVec3 b, IVec3 c)
        {
            IVec3 ret = new IVec3();
            ret.x = b.x - c.x;
            ret.y = b.y - c.y;
            ret.z = b.z - c.z;
            return ret;
        }
        public static Vector3Int operator *(IVec3 b,  Vector3Int c)
        {
            Vector3Int ret = new Vector3Int();
            ret.x = b.x * c.x;
            ret.y = b.y * c.y;
            ret.z = b.z * c.z;
            return ret;
        }
        public static Vector3Int operator +(IVec3 b, Vector3Int c)
        {
            Vector3Int ret = new Vector3Int();
            ret.x = b.x + c.x;
            ret.y = b.y + c.y;
            ret.z = b.z + c.z;
            return ret;
        }
        public static Vector3Int operator -(IVec3 b, Vector3Int c)
        {
            Vector3Int ret = new Vector3Int();
            ret.x = b.x - c.x;
            ret.y = b.y - c.y;
            ret.z = b.z - c.z;
            return ret;
        }
        public static IVec3 operator *(IVec3 b, int c)
        {
            IVec3 ret = new IVec3();
            ret.x = b.x * c;
            ret.y = b.y * c;
            ret.z = b.z * c;
            return ret;
        }
        public static IVec3 operator /(IVec3 b, int c)
        {
            IVec3 ret = new IVec3();
            ret.x = b.x / c;
            ret.y = b.y / c;
            ret.z = b.z / c;
            return ret;
        }
        public static IVec3 operator +(IVec3 b, int c)
        {
            IVec3 ret = new IVec3();
            ret.x = b.x + c;
            ret.y = b.y + c;
            ret.z = b.z + c;
            return ret;
        }
        public static IVec3 operator -(IVec3 b, int c)
        {
            IVec3 ret = new IVec3();
            ret.x = b.x - c;
            ret.y = b.y - c;
            ret.z = b.z - c;
            return ret;
        }
        public static implicit operator Vector3(IVec3 v)
        {
            Vector3 ret = new Vector3(v.x, v.y, v.z);
            return ret;
        }
        public static implicit operator Vector3Int(IVec3 v)
        {
            Vector3Int ret = new Vector3Int(v.x, v.y, v.z);
            return ret;
        }
        public static implicit operator Vec3(IVec3 v)
        {
            Vec3 ret = new Vec3(v.x, v.y, v.z);
            return ret;
        }
    }
}

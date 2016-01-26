using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Physarealm
{
    public class Index3f
    {
        public float x;
        public float y;
        public float z;
        public Index3f(float a, float b, float c) 
        {
            x = a; y = b; z = c;
        }
        public Index3f(int index, int xmax, int ymax, int zmax)
        {
            z = (int)(index / (xmax * ymax));
            y = (int)((index - (int)(z * xmax * ymax)) / xmax);
            x = index - (int)y * xmax - (int)z * xmax * ymax;
        }
        public Index3f(Index3f anoind):this(anoind.x, anoind.y, anoind.z){}
        public int convertToIndex(int xmax, int ymax, int zmax)
        {
            constraint(xmax, ymax, zmax);
            return (int)x + (int)y * xmax + (int)z * xmax * ymax;
        }
        private void constraint(int xmax, int ymax, int zmax) 
        {
            x = x > 0 ? x : 0;
            y = y > 0 ? y : 0;
            z = z > 0 ? z : 0;
            x = x < xmax ? x : xmax - 1;
            y = y < ymax ? y : ymax - 1;
            z = z < zmax ? z : zmax - 1;
        }
        public void convertFromIndex(int index, int xmax, int ymax, int zmax)
        {
            z = (int)(index / (xmax * ymax));
            y = (int)((index - (int)(z * xmax * ymax)) / xmax);
            x = index - (int)y * xmax - (int)z * xmax * ymax;
        }
        public Index3f getRandNearbyPos( int radius, int xmax, int ymax, int zmax) 
        {
            int start_x = (int)x - radius > 0 ? (int)x - radius : 0;
            int start_y = (int)y - radius > 0 ? (int)y - radius : 0;
            int start_z = (int)z - radius > 0 ? (int)z - radius : 0;
            int end_x = (int)x + radius < xmax ? (int)x + radius : xmax - 1;
            int end_y = (int)y + radius < ymax ? (int)y + radius : ymax - 1;
            int end_z = (int)z + radius < zmax ? (int)z + radius : zmax - 1;
            return new Index3f( Libutility.getRand(start_x, end_x + 1),
                    Libutility.getRand(start_y, end_y + 1),
                    Libutility.getRand(start_z, end_z + 1));
        }
        public override string ToString()
        {
            return x + " " + y + " " + z;
        }
        public Point3d getPos() 
        {
            return new Point3d(x, y, z);
        }
    }
}

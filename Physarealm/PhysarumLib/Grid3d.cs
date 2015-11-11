using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Rhino;
using Rhino.Geometry;

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Physarealm
{
    public class Grid3d
    {
        public int _x { private set; get; }
        public int _y { private set; get; }
        public int _z { private set; get; }
        public float[, ,] trail { get; set; }
        private float[, ,] temptrail;
        private int[, ,] particle_ids;
        public int[, ,] griddata { get; set; }//0 for default, 1 for food, 2 for inside obstacle or outside container
        public int[, ,] agedata;
        public float diffdamp { get; set; }
        public float projectvalue { get; set; }
        private List<Point3d> _origins;
        private int grid_age;
        public bool age_flag { get; set; }
        //bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public Grid3d(int x, int y, int z) //initialize data
        {
            _x = x; _y = y; _z = z;
            trail = new float[_x, _y, _z];
            temptrail = new float[_x, _y, _z];
            particle_ids = new int[_x, _y, _z];
            griddata = new int[_x, _y, _z];
            agedata = new int[_x, _y, _z];
            grid_age = 0;
            for (int i = 0; i < _x; i++)
            {
                for (int j = 0; j < _y; j++)
                {
                    for (int k = 0; k < _z; k++)
                    {
                        trail[i, j, k] = 0;
                        temptrail[i, j, k] = 0;
                        particle_ids[i, j, k] = -2;
                        griddata[i, j, k] = 0;
                        agedata[i, j, k] = 0;
                    }
                }
            }
            projectvalue = 50;
            diffdamp = 0.1F;
            age_flag = false;
        }
        public bool isOccupidByParticle(int x, int y, int z)
        {
            if (particle_ids[x, y, z] == -1)
                return false;
            return true;
        }
        public void occupyGridCell(int x, int y, int z, int id)
        {
            particle_ids[x, y, z] = id;
        }
        public void clearGridCell(int x, int y, int z)
        {
            particle_ids[x, y, z] = -1;
        }
        public void increaseTrail(int x, int y, int z, float val)
        {
            trail[x, y, z] += val;
        }
        public void setGridCellValue(int xpos, int ypos, int zpos, int radius, int val)
        {
            if (radius < 1)
                griddata[xpos, ypos, zpos] = val;
            int start_x = xpos - radius > 0 ? xpos - radius : 0;
            int start_y = ypos - radius > 0 ? ypos - radius : 0;
            int start_z = zpos - radius > 0 ? zpos - radius : 0;
            int end_x = xpos + radius < _x ? xpos + radius : _x - 1;
            int end_y = ypos + radius < _x ? ypos + radius : _y - 1;
            int end_z = zpos + radius < _x ? zpos + radius : _z - 1;
            for (int i = start_x; i <= end_x; i++)
            {
                for (int j = start_y; j <= end_y; j++)
                {
                    for (int k = start_z; k <= end_z; k++)
                    {
                        griddata[i, j, k] = val;
                    }
                }
            }

        }
        public float getTrailAndCheckBounds(int x, int y, int z)
        {
            /*if (wrap)
            {
                x = x < 0 ? _x - 1 : x;//wrap x if x < 0
                x = x > _x - 1 ? 0 : x;//wrap x if x > _x
                y = y < 0 ? _y - 1 : y;
                y = y > _y - 1 ? 0 : y;
                z = z < 0 ? _z - 1 : z;
                z = z > _z - 1 ? 0 : z;
            }*/
            return trail[x, y, z];
        }
        public float getAverageNeighbourhood(int x, int y, int z, int radius) //return avg of 3 * 3 * 3 grid if radius = 1
        {
            float total = 0;
            if (radius < 1)
                return trail[x, y, z];
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            int start_z = z - radius > 0 ? z - radius : 0;
            int end_x = x + radius < _x - 1 ? x + radius : _x - 1;
            int end_y = y + radius < _y - 1 ? y + radius : _y - 1;
            int end_z = z + radius < _z - 1 ? z + radius : _z - 1;
            for (int i = start_x; i <= end_x; i++)
            {
                for (int j = start_y; j <= end_y; j++)
                {
                    for (int k = start_z; k <= end_z; k++)
                    {
                        total += trail[i, j, k];
                    }
                }
            }
            int num = (int)Math.Pow(radius * 2 + 1, 3);
            return total / num;
        }
        public void diffuseTrails()
        {
            //float ave = 0;
            grid_age++;
            System.Threading.Tasks.Parallel.For(0, _x, (i) =>
            {
                System.Threading.Tasks.Parallel.For(0, _y, (j) =>
                {
                    System.Threading.Tasks.Parallel.For(0, _z, (k) =>
                    {
                        float ave = getAverageNeighbourhood(i, j, k, 1);
                        temptrail[i, j, k] = ave * (1 - diffdamp);
                        if (agedata[i, j, k] != 0)
                            agedata[i, j, k]++;
                    });
                });
            });


            /*
            for (int i = 0; i < _x; i++) {
              for (int j = 0; j < _y; j++) {
                for (int k = 0; k < _z; k++) {
                  ave = getAverageNeighbourhood(i, j, k, 1);
                  temptrail[i, j, k] = ave * (1 - diffdamp);
                }
              }
            }
            */
            System.Threading.Tasks.Parallel.For(0, _x, (i) =>
            {
                System.Threading.Tasks.Parallel.For(0, _y, (j) =>
                {
                    System.Threading.Tasks.Parallel.For(0, _z, (k) =>
                    {
                        trail[i, j, k] = temptrail[i, j, k];
                        if (trail[i, j, k] < 0 || particle_ids[i, j, k] == -2)
                            trail[i, j, k] = 0;
                        if (age_flag == true && agedata[i, j, k] != 0 && agedata[i, j, k] > 5)
                            trail[i, j, k] = 0;

                    });
                });
            });
            /*
            for (int i = 0; i < _x; i++)
            {
              for (int j = 0; j < _y; j++)
              {
                for (int k = 0; k < _z; k++)
                {
                  trail[i, j, k] = temptrail[i, j, k];
                  if (trail[i, j, k] < 0 || griddata[i, j, k] == 2 )
                    trail[i, j, k] = 0;
                }
              }
            }*/
        }
        public void projectToTrail()//project food to increase trail
        {
            System.Threading.Tasks.Parallel.For(0, _x, (i) =>
            {
                System.Threading.Tasks.Parallel.For(0, _y, (j) =>
                {
                    System.Threading.Tasks.Parallel.For(0, _z, (k) =>
                    {
                        if (griddata[i, j, k] == 1)
                        {
                            increaseTrail(i, j, k, projectvalue);
                        }
                    });
                });
            });
            /*
          for (int i = 0; i < _x; i++)
          {
            for (int j = 0; j < _y; j++)
            {
              for (int k = 0; k < _z; k++)
              {
                if (griddata[i, j, k] == 1) {
                  increaseTrail(i, j, k, projectvalue);
                }
              }
            }*/

        }
        public int countNumberOfParticlesPresent(int x, int y, int z, int radius)
        {
            int total = 0;
            //if (radius < 1)
            //  return isOccupidByParticle(x, y, z) == true ? 1 : 0;
            int countNow = 0;
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            int start_z = z - radius > 0 ? z - radius : 0;
            int end_x = x + radius <= _x - 1 ? x + radius : _x - 1;
            int end_y = y + radius <= _y - 1 ? y + radius : _y - 1;
            int end_z = z + radius <= _z - 1 ? z + radius : _z - 1;
            for (int i = start_x; i <= end_x; i++)
            {
                for (int j = start_y; j <= end_y; j++)
                {
                    for (int k = start_z; k <= end_z; k++)
                    {
                        if (isOccupidByParticle(i, j, k) == true)
                            total++;
                        countNow++;
                    }
                }
            }
            int totalShouldBe = (int)Math.Pow(radius * 2 + 1, 3) - 1;
            total += totalShouldBe - countNow;
            return total;
        }
        public List<Point3d> findNeighborParticle(Amoeba agent, int radius)
        {
            List<Point3d> nei = new List<Point3d>();
            int x = agent.curx;
            int y = agent.cury;
            int z = agent.curz;
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            int start_z = z - radius > 0 ? z - radius : 0;
            int end_x = x + radius <= _x - 1 ? x + radius : _x - 1;
            int end_y = y + radius <= _y - 1 ? y + radius : _y - 1;
            int end_z = z + radius <= _z - 1 ? z + radius : _z - 1;
            for (int i = start_x; i <= end_x; i++)
            {
                for (int j = start_y; j <= end_y; j++)
                {
                    for (int k = start_z; k <= end_z; k++)
                    {
                        if (isOccupidByParticle(i, j, k) == true)
                            nei.Add(new Point3d(i, j, k));
                    }
                }
            }

            return nei;
        }
        public List<Point3d> findNeighborParticle(int x, int y, int z, int radius)
        {
            List<Point3d> nei = new List<Point3d>();
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            int start_z = z - radius > 0 ? z - radius : 0;
            int end_x = x + radius <= _x - 1 ? x + radius : _x - 1;
            int end_y = y + radius <= _y - 1 ? y + radius : _y - 1;
            int end_z = z + radius <= _z - 1 ? z + radius : _z - 1;
            for (int i = start_x; i <= end_x; i++)
            {
                for (int j = start_y; j <= end_y; j++)
                {
                    for (int k = start_z; k <= end_z; k++)
                    {
                        if (isOccupidByParticle(i, j, k) == true)
                            nei.Add(new Point3d(i, j, k));
                    }
                }
            }

            return nei;
        }
        public float getMaxTrailValue()
        {
            float max = 0;
            for (int i = 0; i < _x; i++)
            {
                for (int j = 0; j < _y; j++)
                {
                    for (int k = 0; k < _z; k++)
                    {
                        if (trail[i, j, k] > max)
                            max = trail[i, j, k];
                    }
                }
            }
            return max;
        }
        public float getOffsetTrailValue(int x, int y, int z, Vector3d orient, float viewangle, float offsetangle, float offsetsteps, Libutility util)
        {
            Point3f origin = new Point3f(x, y, z);
            Plane oriplane = new Plane(origin, orient);
            float u = offsetsteps * util.sinlut[(int)viewangle] * util.coslut[(int)offsetangle];
            float v = offsetsteps * util.sinlut[(int)viewangle] * util.sinlut[(int)offsetangle];
            float w = offsetsteps * util.coslut[(int)viewangle];
            //float u = (double) offsetsteps * Math.Sin((double) viewangle * 3.1416F / 180) * Math.Cos((double) offsetangle * 3.1416F / 180);
            //float v = (double)offsetsteps * Math.Sin((double) viewangle * 3.1416F / 180) * Math.Sin((double) offsetangle * 3.1416F / 180);
            //float w = (double)offsetsteps * Math.Cos((double) viewangle * 3.1416F / 180);
            Point3d target = oriplane.PointAt(u, v, w);
            int tx = (int)Math.Round(target.X);
            int ty = (int)Math.Round(target.Y);
            int tz = (int)Math.Round(target.Z);
            if (tx < 0 || tx >= _x || ty < 0 || ty >= _y || tz < 0 || tz >= _z)
                return -1;
            return trail[tx, ty, tz];
        }
        /*
        public trailInfo getOffsetTrailValue(int x, int y, int z, Vector3d orient, float viewangle, float offsetangle, float offsetsteps, Libutility util)
        {
          Point3f origin = new Point3f(x, y, z);
          Plane oriplane = new Plane(origin, orient);
          float u = offsetsteps * util.sinlut[(int) viewangle] * util.coslut[(int) offsetangle];
          float v = offsetsteps * util.sinlut[(int) viewangle] * util.sinlut[(int) offsetangle];
          float w = offsetsteps * util.coslut[(int) viewangle];
          Point3d target = oriplane.PointAt(u, v, w);
          int tx = (int) Math.Round(target.X);
          int ty = (int) Math.Round(target.Y);
          int tz = (int) Math.Round(target.Z);
          float tr = 0;
          if (tx < 0 || tx >= _x || ty < 0 || ty >= _y || tz < 0 || tz >= _z)
            tr = -1;
          else
            tr = trail[tx, ty, tz];
          trailInfo newinfo = new trailInfo(new Point3d(tx, ty, tz), tr);
          return newinfo;
        }*/

        public Point3d getNeighbourhoodFreePos(int x, int y, int z, int radius, Libutility util)
        {
            Point3d retpt = new Point3d(-1, -1, -1);
            if (radius < 1)
            {
                return retpt;
            }
            /*
            int times = 0;
            int tpz;
            int tpy;
            int tpx;
            do
            {
              tpx = util.getRand(x - radius, x + radius + 1);
              tpy = util.getRand(y - radius, y + radius + 1);
              tpz = util.getRand(z - radius, z + radius + 1);
              times++;
            }
            while (particle_ids[tpx, tpy, tpz] != -1 && times < 20);
            if (particle_ids[tpx, tpy, tpz] != -1)
              return retpt;
            else
              return new Point3d(tpx, tpy, tpz);
      */

            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            int start_z = z - radius > 0 ? z - radius : 0;
            int end_x = x + radius < _x ? x + radius : _x - 1;
            int end_y = y + radius < _y ? y + radius : _y - 1;
            int end_z = z + radius < _z ? z + radius : _z - 1;
            int count = (end_x - start_x + 1) * (end_y - start_y + 1) * (end_z - start_z + 1); //
            //int[] ids = new int[count];
            List<Point3d> freePos = new List<Point3d>();
            for (int i = end_x; i >= start_x; i--)
            {
                for (int j = end_y; j >= start_y; j--)
                {
                    for (int k = end_z; k >= start_z; k--)
                    {
                        if (particle_ids[i, j, k] == -1)
                        {
                            //retpt = new Point3d(i, j, k);
                            freePos.Add(new Point3d(i, j, k));
                            //return retpt;
                        }
                    }
                }
            }
            int lens = freePos.Count;
            if (lens == 0)
                return retpt;
            retpt = freePos[util.getRand(lens)];
            return retpt;
        }
        public Point3d getPositionWhenSuppliedWithAngleAndRadius(int x, int y, int z, Vector3d orient, float anglephy, float angletheta, float radius)
        {
            Point3d origin = new Point3d(x, y, z);
            Plane oriplane = new Plane(origin, orient);
            double u = radius * Math.Sin(anglephy) * Math.Cos(angletheta);
            double v = radius * Math.Sin(anglephy) * Math.Sin(angletheta);
            double w = radius * Math.Cos(anglephy);
            Point3d target = oriplane.PointAt(u, v, w);
            return target;
        }
        public void setObstacles(List<Brep> obs)
        {
            if (obs.Count == 0)
                return;
            System.Threading.Tasks.Parallel.ForEach(obs, (br) =>
            {
                Curve[] intersectCrv;
                Point3d[] intersectPt;
                for (int i = 0; i < _x; i++)
                {
                    for (int j = 0; j < _y; j++)
                    {
                        Line thisCrv = new Line(new Point3d(i, j, 0), new Point3d(i, j, _z));
                        Curve crv = thisCrv.ToNurbsCurve();
                        Rhino.Geometry.Intersect.Intersection.CurveBrep(crv, br, 1, out intersectCrv, out intersectPt);
                        if (intersectPt == null)
                            continue;
                        int len = intersectPt.Length;
                        if (len <= 1)
                            continue;
                        Point3d ptS = intersectPt[0];
                        Point3d ptE = intersectPt[1];
                        if (ptS.Z > ptE.Z)
                        {
                            double swap = ptS.Z;
                            ptS.Z = ptE.Z;
                            ptE.Z = swap;
                        }

                        for (int k = (int)ptS.Z; k < (int)ptE.Z; k++)
                        {
                            particle_ids[i, j, k] = -2;
                        }
                    }
                }
            });

        }
        public void setContainer(List<Brep> cont)
        {
            if (cont.Count == 0)
            {
                for (int i = 0; i < _x; i++)
                {
                    for (int j = 0; j < _y; j++)
                    {
                        for (int k = -0; k < _z; k++)
                        {
                            particle_ids[i, j, k] = -1;
                        }
                    }
                }
                return;
            }
            System.Threading.Tasks.Parallel.ForEach(cont, (br) =>
            {
                Curve[] intersectCrv;
                Point3d[] intersectPt;
                for (int i = 0; i < _x; i++)
                {
                    for (int j = 0; j < _y; j++)
                    {
                        Line thisCrv = new Line(new Point3d(i, j, 0), new Point3d(i, j, _z));
                        Curve crv = thisCrv.ToNurbsCurve();
                        Rhino.Geometry.Intersect.Intersection.CurveBrep(crv, br, 1, out intersectCrv, out intersectPt);
                        if (intersectPt == null)
                            continue;
                        int len = intersectPt.Length;
                        if (len <= 1)
                            continue;
                        Point3d ptS = intersectPt[0];
                        Point3d ptE = intersectPt[1];
                        if (ptS.Z > ptE.Z)
                        {
                            double swap = ptS.Z;
                            ptS.Z = ptE.Z;
                            ptE.Z = swap;
                        }

                        for (int k = (int)ptS.Z; k < (int)ptE.Z; k++)
                        {
                            particle_ids[i, j, k] = -1;
                        }
                    }
                }
            });
        }
        public void setFood(List<Point3d> food)
        {
            foreach (Point3d pt in food)
            {
                setGridCellValue((int)pt.X, (int)pt.Y, (int)pt.Z, 1, 1);
            }
            projectToTrail();
        }
        public void setBirthPlace(List<Point3d> origin)
        {
            _origins = origin;
            //foreach (Point3d pt in origin)
            //{
            //  setGridCellValue((int) pt.X, (int) pt.Y, (int) pt.Z, 1, 1);
            //}
        }
        public Point3d getRandomBirthPlace(Libutility util)
        {
            int length = _origins.Count;
            return _origins[util.getRand(length)];
        }
        public Mesh getTrailEvaMesh(int z)
        {
            Mesh evaMesh = new Mesh();
            Plane worldXY = new Plane(new Point3d(0, 0, z), new Point3d(1, 0, z), new Point3d(0, 1, z));
            evaMesh = Mesh.CreateFromPlane(worldXY, new Interval(0, _x), new Interval(0, _y), _x, _y);
            for (int i = 0; i < evaMesh.Vertices.Count; i++)
            {
                Point3f vert = evaMesh.Vertices[i];
                int x = (int)vert.X;
                int y = (int)vert.Y;
                float thisTrail = 0;
                if (x < _x - 1 && y < _y - 1)
                    thisTrail = Math.Abs(trail[x, y, z]);
                if (thisTrail > 255)
                    thisTrail = 255;
                evaMesh.VertexColors.SetColor(i, (int)thisTrail, 0, 0);
            }
            return evaMesh;

        }

        public float[] getTrailV()
        {
            float[] ret = new float[_x * _y * _z];
            int item = 0;
            for (int k = 0; k < _z; k++)
            {
                for (int j = 0; j < _y; j++)
                {
                    for (int i = 0; i < _x; i++)
                    {
                        ret[item] = trail[i, j, k];
                        item++;
                    }
                }
            }
            return ret;

        }


    }//end of Grid3d
}

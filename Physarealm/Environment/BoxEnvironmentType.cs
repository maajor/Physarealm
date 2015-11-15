using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Physarealm.Environment
{
    public class BoxEnvironmentType:AbstractEnvironmentType, IDisposable
    {
        public Point3d[ , , ] positions;
        public float[, ,] trail { get; set; }
        private float[, ,] temptrail;
        private int[, ,] particle_ids;
        public int[, ,] griddata { get; set; }//0 for default, 1 for food
        public int[, ,] agedata;
        public float diffdamp { get; set; }
        public float projectvalue { get; set; }
        private List<Point3d> _origins;
        private int grid_age;
        public bool age_flag { get; set; }
        private BoundingBox _box;
        double u_interval;
        double v_interval;
        double w_interval;
        public double UMin { get; private set; }
        public double UMax { get; private set; }
        public double VMin { get; private set; }
        public double VMax { get; private set; }
        public double WMin { get; private set; }
        public double WMax { get; private set; }
        //bool disposed = false;

        /*public BoxEnvironmentType(int x, int y, int z):base(x, y, z) //initialize data
        {
            //_x = x; _y = y; _z = z;
            positions = new Point3d[u, v, w];
            trail = new float[u, v, w];
            temptrail = new float[u, v, w];
            particle_ids = new int[u, v, w];
            griddata = new int[u, v, w];
            agedata = new int[u, v, w];
            grid_age = 0;
            for (int i = 0; i < u; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    for (int k = 0; k < w; k++)
                    {
                        positions[i, j, k] = new Point3d(i, j, k);
                        trail[i, j, k] = 0;
                        temptrail[i, j, k] = 0;
                        particle_ids[i, j, k] = -1;
                        griddata[i, j, k] = 0;
                        agedata[i, j, k] = 0;
                    }
                }
            }
            projectvalue = 50;
            diffdamp = 0.1F;
            age_flag = false;
        }*/
        public BoxEnvironmentType(BoundingBox box, int x, int y, int z)
            : base(x, y, z) //initialize data
        {
            _box = box;
            UMin = _box.Min.X;
            UMax = _box.Max.X;
            VMin = _box.Min.Y;
            VMax = _box.Max.Y;
            WMin = _box.Min.Z;
            WMax = _box.Max.Z;
            positions = new Point3d[u,v,w];
            u_interval = (UMax - UMin) / u;
            v_interval = (VMax - VMin) / v;
            w_interval = (WMax - WMin) / w;
            if (w_interval < 0)
                throw new Exception("negative interval");
            trail = new float[u, v, w];
            temptrail = new float[u, v, w];
            particle_ids = new int[u, v, w];
            griddata = new int[u, v, w];
            agedata = new int[u, v, w];
            grid_age = 0;
            for (int i = 0; i < u; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    for (int k = 0; k < w; k++)
                    {
                        positions[i, j, k] = new Point3d(UMin + i * u_interval + u_interval / 2, VMin + j * v_interval + v_interval / 2, WMin + k * w_interval + w_interval / 2);
                        trail[i, j, k] = 0;
                        temptrail[i, j, k] = 0;
                        particle_ids[i, j, k] = -1;
                        griddata[i, j, k] = 0;
                        agedata[i, j, k] = 0;
                    }
                }
            }
            projectvalue = 100;
            diffdamp = 0.1F;
            age_flag = false;
        }
        public BoxEnvironmentType(BoxEnvironmentType boxenv) : this(boxenv._box, boxenv.u, boxenv.v, boxenv.w) { }
        public override bool isOccupidByParticle(int x, int y, int z)
        {
            if (particle_ids[x, y, z] == -1)
                return false;
            return true;
        }
        public override void occupyGridCell(int x, int y, int z, int id)
        {
            particle_ids[x, y, z] = id;
        }
        public override void clearGridCell(int x, int y, int z)
        {
            particle_ids[x, y, z] = -1;
        }
        public override void increaseTrail(int x, int y, int z, float val)
        {
            trail[x, y, z] += val;
        }
        public override int getGriddata(int u, int v, int w) 
        {
            return griddata[u, v, w];
        }
        public override void setGridCellValue(int xpos, int ypos, int zpos, int radius, int val)
        {
            if (radius < 1)
                griddata[xpos, ypos, zpos] = val;
            int start_x = xpos - radius > 0 ? xpos - radius : 0;
            int start_y = ypos - radius > 0 ? ypos - radius : 0;
            int start_z = zpos - radius > 0 ? zpos - radius : 0;
            int end_x = xpos + radius < u ? xpos + radius : u - 1;
            int end_y = ypos + radius < v ? ypos + radius : v - 1;
            int end_z = zpos + radius < w ? zpos + radius : w - 1;
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
        public override float getAverageNeighbourhood(int x, int y, int z, int radius) //return avg of 3 * 3 * 3 grid if radius = 1
        {
            float total = 0;
            if (radius < 1)
                return trail[x, y, z];
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            int start_z = z - radius > 0 ? z - radius : 0;
            int end_x = x + radius < u - 1 ? x + radius : u - 1;
            int end_y = y + radius < v - 1 ? y + radius : v - 1;
            int end_z = z + radius < w - 1 ? z + radius : w - 1;
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
        public override  void diffuseTrails()
        {
            //float ave = 0;
            grid_age++;
            System.Threading.Tasks.Parallel.For(0, u, (i) =>
            {
                System.Threading.Tasks.Parallel.For(0, v, (j) =>
                {
                    System.Threading.Tasks.Parallel.For(0, w, (k) =>
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
            System.Threading.Tasks.Parallel.For(0, u, (i) =>
            {
                System.Threading.Tasks.Parallel.For(0, v, (j) =>
                {
                    System.Threading.Tasks.Parallel.For(0, w, (k) =>
                    {
                        trail[i, j, k] = temptrail[i, j, k];
                        if (trail[i, j, k] < 0 || particle_ids[i, j, k] == -2)
                            trail[i, j, k] = 0;
                        //if (age_flag == true && agedata[i, j, k] != 0 && agedata[i, j, k] > 5)
                         //   trail[i, j, k] = 0;

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
        public override void projectToTrail()//project food to increase trail
        {
            System.Threading.Tasks.Parallel.For(0, u, (i) =>
            {
                System.Threading.Tasks.Parallel.For(0, v, (j) =>
                {
                    System.Threading.Tasks.Parallel.For(0, w, (k) =>
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
        public override int countNumberOfParticlesPresent(int x, int y, int z, int radius)
        {
            int total = 0;
            //if (radius < 1)
            //  return isOccupidByParticle(x, y, z) == true ? 1 : 0;
            int countNow = 0;
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            int start_z = z - radius > 0 ? z - radius : 0;
            int end_x = x + radius <= u - 1 ? x + radius : u - 1;
            int end_y = y + radius <= v - 1 ? y + radius : v - 1;
            int end_z = z + radius <= w - 1 ? z + radius : w - 1;
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
        public override List<Point3d> findNeighborParticle(Amoeba agent, int radius)
        {
            List<Point3d> nei = new List<Point3d>();
            int x = agent.curx;
            int y = agent.cury;
            int z = agent.curz;
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            int start_z = z - radius > 0 ? z - radius : 0;
            int end_x = x + radius <= u - 1 ? x + radius : u - 1;
            int end_y = y + radius <= v - 1 ? y + radius : v - 1;
            int end_z = z + radius <= w - 1 ? z + radius : w - 1;
            for (int i = start_x; i <= end_x; i++)
            {
                for (int j = start_y; j <= end_y; j++)
                {
                    for (int k = start_z; k <= end_z; k++)
                    {
                        if (isOccupidByParticle(i, j, k) == true)
                        {
                            //nei.Add(new Point3d(i, j, k));
                            nei.Add(positions[i,j,k]);
                        }
                    }
                }
            }

            return nei;
        }
        public override List<Point3d> findNeighborParticle(int x, int y, int z, int radius)
        {
            List<Point3d> nei = new List<Point3d>();
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            int start_z = z - radius > 0 ? z - radius : 0;
            int end_x = x + radius <= u - 1 ? x + radius : u - 1;
            int end_y = y + radius <= v - 1 ? y + radius : v - 1;
            int end_z = z + radius <= w - 1 ? z + radius : w - 1;
            for (int i = start_x; i <= end_x; i++)
            {
                for (int j = start_y; j <= end_y; j++)
                {
                    for (int k = start_z; k <= end_z; k++)
                    {
                        if (isOccupidByParticle(i, j, k) == true)
                        {
                            //nei.Add(new Point3d(i, j, k));
                            nei.Add(positions[i, j, k]);
                        }
                    }
                }
            }

            return nei;
        }
        public override float getMaxTrailValue()
        {
            float max = 0;
            for (int i = 0; i < u; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    for (int k = 0; k < w; k++)
                    {
                        if (trail[i, j, k] > max)
                            max = trail[i, j, k];
                    }
                }
            }
            return max;
        }
        public override float getOffsetTrailValue(int x, int y, int z, Vector3d orient, float viewangle, float offsetangle, float offsetsteps, Libutility util)
        {
            //Point3f origin = new Point3f(x, y, z);
            Point3d origin = positions[x, y, z];
            Plane oriplane = new Plane(origin, orient);
            float _u = offsetsteps * util.sinlut[(int)viewangle] * util.coslut[(int)offsetangle];
            float _v = offsetsteps * util.sinlut[(int)viewangle] * util.sinlut[(int)offsetangle];
            float _w = offsetsteps * util.coslut[(int)viewangle];
            //float u = (double) offsetsteps * Math.Sin((double) viewangle * 3.1416F / 180) * Math.Cos((double) offsetangle * 3.1416F / 180);
            //float v = (double)offsetsteps * Math.Sin((double) viewangle * 3.1416F / 180) * Math.Sin((double) offsetangle * 3.1416F / 180);
            //float w = (double)offsetsteps * Math.Cos((double) viewangle * 3.1416F / 180);
            Point3d target = oriplane.PointAt(_u, _v, _w);
            //int tx = (int)Math.Round(target.X);
            //int ty = (int)Math.Round(target.Y);
            //int tz = (int)Math.Round(target.Z);
            int tx = getUIndex(target.X);
            int ty = getVIndex(target.Y);
            int tz = getWIndex(target.Z);
            if (tx < 0 || tx >= u || ty < 0 || ty >= v || tz < 0 || tz >= w)
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

        public override Point3d getNeighbourhoodFreePos(int x, int y, int z, int radius, Libutility util)
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
            int end_x = x + radius < u ? x + radius : u - 1;
            int end_y = y + radius < v ? y + radius : v - 1;
            int end_z = z + radius < w ? z + radius : w - 1;
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
                            freePos.Add(positions[i,j,k]);
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
        /*public Point3d getPositionWhenSuppliedWithAngleAndRadius(int x, int y, int z, Vector3d orient, float anglephy, float angletheta, float radius)
        {
            Point3d origin = new Point3d(x, y, z);
            Plane oriplane = new Plane(origin, orient);
            double u = radius * Math.Sin(anglephy) * Math.Cos(angletheta);
            double v = radius * Math.Sin(anglephy) * Math.Sin(angletheta);
            double w = radius * Math.Cos(anglephy);
            Point3d target = oriplane.PointAt(u, v, w);
            return target;
        }*/
        public override void setObstacles(List<Brep> obs)
        {
            if (obs.Count == 0)
                return;
            System.Threading.Tasks.Parallel.ForEach(obs, (br) =>
            {
                Curve[] intersectCrv;
                Point3d[] intersectPt;
                for (int i = 0; i < u; i++)
                {
                    for (int j = 0; j < v; j++)
                    {
                        Line thisCrv = new Line(new Point3d(i, j, 0), new Point3d(i, j, w));
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
        public override void setContainer(List<Brep> cont)
        {
            if (cont.Count == 0)
            {
                for (int i = 0; i < u; i++)
                {
                    for (int j = 0; j < v; j++)
                    {
                        for (int k = -0; k < w; k++)
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
                for (int i = 0; i < u; i++)
                {
                    for (int j = 0; j < v; j++)
                    {
                        Line thisCrv = new Line(new Point3d(i, j, 0), new Point3d(i, j, w));
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
        public override void setFood(List<Point3d> food)
        {
            foreach (Point3d pt in food)
            {
                setGridCellValue((int)pt.X, (int)pt.Y, (int)pt.Z, 1, 1);
            }
            projectToTrail();
        }
        public override void setBirthPlace(List<Point3d> origin)
        {
            _origins = origin;
            //foreach (Point3d pt in origin)
            //{
            //  setGridCellValue((int) pt.X, (int) pt.Y, (int) pt.Z, 1, 1);
            //}
        }
        public override Point3d getRandomBirthPlace(Libutility util)
        {
            int length = _origins.Count;
            return _origins[util.getRand(length)];
        }
        public override  Mesh getTrailEvaMesh(int z)
        {
            Mesh evaMesh = new Mesh();
            Plane worldXY = new Plane(new Point3d(0, 0, z), new Point3d(1, 0, z), new Point3d(0, 1, z));
            evaMesh = Mesh.CreateFromPlane(worldXY, new Interval(0, u), new Interval(0, v), u, v);
            for (int i = 0; i < evaMesh.Vertices.Count; i++)
            {
                Point3f vert = evaMesh.Vertices[i];
                int x = (int)vert.X;
                int y = (int)vert.Y;
                float thisTrail = 0;
                if (x < u - 1 && y < v - 1)
                    thisTrail = Math.Abs(trail[x, y, z]);
                if (thisTrail > 255)
                    thisTrail = 255;
                evaMesh.VertexColors.SetColor(i, (int)thisTrail, 0, 0);
            }
            return evaMesh;

        }

        public override float[] getTrailV()
        {
            float[] ret = new float[u * v * w];
            int item = 0;
            for (int k = 0; k < w; k++)
            {
                for (int j = 0; j < v; j++)
                {
                    for (int i = 0; i < u; i++)
                    {
                        ret[item] = trail[i, j, k];
                        item++;
                    }
                }
            }
            return ret;

        }

        private int getUIndex(double x) 
        {
            int uid = (int)((x - UMin) / u_interval);
            return uid;
        }
        private int getVIndex(double y) 
        {
            int vid = (int)((y - VMin) / v_interval);
            return vid;
        }
        private int getWIndex(double z)
        {
            int wid = (int)((z - WMin) / w_interval);
            return wid;
        }
        public override Point3d getIndexByPosition(double x, double y, double z) 
        {
            return new Point3d(getUIndex(x), getVIndex(y), getWIndex(z));
        }
        public override Point3d getPositionByIndex(int u, int v, int w) 
        {
            return positions[u, v, w];
        }
        public override double getUMin() { return UMin; }
        public override double getVMin() { return VMin; }
        public override double getWMin() { return WMin; }
        public override double getUMax() { return UMax; }
        public override double getVMax() { return VMax; }
        public override double getWMax() { return WMax; }
        public override bool constrainPos(ref float x, ref float y, ref float z) 
        {
            bool flag = false;
            if (x < getUMin())
            {
                x = -x;
                flag = true;
            }
            if (x > getUMax())
            {
                x = (float)(getUMax() - u_interval);
                flag = true;
            }
            if (y < 0)
            {
                y = -y;
                flag = true;
            }
            if (y >  getVMax())
            {
                y = (float) (getVMax() - v_interval);
                flag = true;
            }
            if (z < 0)
            {
                z = -z;
                flag = true;
            }
            if (z > getWMax())
            {
                z = (float) (getWMax() - w_interval);
                flag = true;
            }
            return flag;
        }
        public override void Clear() 
        {
            griddata.Initialize();
            temptrail.Initialize();
            particle_ids.Initialize();
            agedata.Initialize();
            _origins.Clear();
        }

        public void Dispose()
        {
            Clear();
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new BoxEnvironmentType(this);
        }
        public override string ToString()
        {
            return this.TypeName;
        }
        public override string TypeName
        {
            get
            {
                string typename = "Physarealm.BoxEnvironmentType";
                return typename;
            }
        }
    }
}

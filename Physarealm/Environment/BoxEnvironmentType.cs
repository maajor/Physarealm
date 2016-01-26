using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Physarealm.Environment
{
    public class BoxEnvironmentType:AbstractEnvironmentType, IDisposable
    {
        /*
        public Point3d[ , , ] positions;
        public float[, ,] trail { get; set; }
        private float[, ,] temptrail;
        private int[, ,] particle_ids;
        public int[, ,] griddata { get; set; }//0 for default, 1 for food, 2 for outside
        public int[, ,] agedata;*/
        public float diffdamp { get; set; }
        public float projectvalue { get; set; }
        //private List<Point3d> _origins;
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
            projectvalue = 10;
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
            positions = new Point3d[u * v * w];
            u_interval = (UMax - UMin) / u;
            v_interval = (VMax - VMin) / v;
            w_interval = (WMax - WMin) / w;
            trail = new float[u * v * w];
            temptrail = new float[u * v * w];
            particle_ids = new int[u * v * w];
            griddata = new int[u * v * w];
            agedata = new int[u * v * w];
            grid_age = 0;
            for (int i = 0; i < u; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    for (int k = 0; k < w; k++)
                    {
                        int thisid = i + j * u + k * u * v;
                        positions[thisid] = new Point3d(UMin + i * u_interval + u_interval / 2, VMin + j * v_interval + v_interval / 2, WMin + k * w_interval + w_interval / 2);
                        trail[thisid] = 0;
                        temptrail[thisid] = 0;
                        particle_ids[thisid] = -2;
                        griddata[thisid] = 2;
                        agedata[thisid] = 0;
                    }
                }
            }
            projectvalue = 5;
            diffdamp = 0.1F;
            age_flag = false;
            _escape_p = 0;
        }
        public BoxEnvironmentType(BoxEnvironmentType boxenv) : this(boxenv._box, boxenv.u, boxenv.v, boxenv.w) { }
        public override bool isOccupidByParticleByIndex(int idx)
        {
            //if (particle_ids[x, y, z] == -1 || particle_ids[x, y, z] == -2)
            if (particle_ids[idx] < 0)
                return false;
            return true;
        }
        public override bool isWithinObstacleByIndex(int idx)
        {
            if (particle_ids[idx] == -2)
                return true;
            return false;
        }
        public override void occupyGridCellByIndex(int idx, int id)
        {
            particle_ids[idx] = id;
        }
        public override void clearGridCellByIndex(int idx)
        {
            if (griddata[idx] == 2)
                particle_ids[idx] = -2;
            else
                particle_ids[idx] = -1;
        }
        public override void increaseTrailByIndex(int idx, float val)
        {
            trail[idx] += val;
        }
        public override int getGriddataByIndex(int idx) 
        {
            return griddata[idx];
        }
        public override void setGridCellValueByIndex(int idx, int radius, int val)
        {
            if (radius < 1)
                griddata[idx] = val;
            List<int> radIndic = radiusIndices(idx, radius);
            foreach (int i in radIndic)
            {
                griddata[i] = val;
            }
        }
        public override float getAverageNeighbourhoodByIndex(int idx, int radius) //return avg of 3 * 3 * 3 grid if radius = 1
        {
            float total = 0;
            if (radius < 1)
                return trail[idx];
            List<int> radIndic = radiusIndices(idx, radius);
            foreach (int i in radIndic)
            {
                total += trail[i];
            }
            int num = (radius * 2 + 1) * (radius * 2 + 1) * (radius * 2 + 1);
            //int num = (int)Math.Pow(radius * 2 + 1, 3);
            return total / num;
        }
        public override  void diffuseTrails()
        {
            //float ave = 0;
            grid_age++;
            for(int i = 0; i < u * v * w; i++)
            {
                float ave = getAverageNeighbourhoodByIndex(i, 1);
                temptrail[i] = ave * (1 - diffdamp);
                //if (agedata[i] != 0)
                //    agedata[i]++;
            }
            for (int i = 0; i < u * v * w; i++)
            {
                trail[i] = temptrail[i];
                //      if (trail[i] < 0 )
                //          trail[i] = 0;
                //      if (griddata[i] == 2)
                //          trail[i] *= (float)_escape_p;

            }
        }
        public override void projectToTrail()//project food to increase trail
        {
            for (int i = 0; i < u * v * w; i++)
            {
                if (griddata[i] == 1)
                {
                    increaseTrailByIndex(i, projectvalue);
                }

            }
        }
        public override int countNumberOfParticlesPresentByIndex(int idx, int radius)
        {
            int total = 0;
            //if (radius < 1)
            //  return isOccupidByParticleByIndex(x, y, z) == true ? 1 : 0;
            int countNow = 0;
            List<int> radIndic = radiusIndices(idx, radius);
            foreach (int i in radIndic)
            {
                if (isOccupidByParticleByIndex(i) == true)
                    total++;
                countNow++;
            }
            int totalShouldBe = (int)Math.Pow(radius * 2 + 1, 3) - 1;
            total += totalShouldBe - countNow;
            return total;
        }
        public override List<Point3d> findNeighborParticle(Amoeba agent, int radius)
        {
            List<Point3d> nei = new List<Point3d>();
            Index3f thispos = agent.indexPos;
            List<int> radIndic = radiusIndices(thispos.convertToIndex(u,v,w), radius);
            foreach (int i in radIndic)
            {
                if (isOccupidByParticleByIndex(i) == true)
                {
                    //nei.Add(new Point3d(i, j, k));
                    nei.Add(positions[i]);
                }
            } 
            return nei;
        }
        public override List<Point3d> findNeighborParticleByIndex(int idx, int radius)
        {
            List<Point3d> nei = new List<Point3d>();
            List<int> radIndic = radiusIndices(idx, radius);
            foreach (int i in radIndic)
            {
                if (isOccupidByParticleByIndex(i) == true)
                {
                    nei.Add(positions[i]);
                }
            }
            return nei;
        }
        public override List<Point3d> getNeighbourTrailClosePosByIndex(int idx, int radius, double near_level)
        {
            double maxv = getMaxTrailValue();
            List<Point3d> nei = new List<Point3d>();
            List<int> radIndic = radiusIndices(idx, radius);
            foreach (int i in radIndic)
            {
                if (Math.Abs(trail[idx] - trail[i]) / maxv < near_level)
                {
                    nei.Add(positions[i]);
                }
            }
            return nei;
        }
        public override float getMaxTrailValue()
        {
            float max = 0;
            for (int i = 0; i < u * v * w; i++)
            {
                if (trail[i] > max)
                    max = trail[i];
            }
            return max;
        }
        public override float getOffsetTrailValueByIndex(int idx, Vector3d orient, float viewangle, float offsetangle, float offsetsteps)
        {
            //Point3f origin = new Point3f(x, y, z);
            //Point3d _origin = positions[idx];
            Index3f originidx = new Index3f(idx, u, v, w);
            Point3d _origin = new Point3d(originidx.x, originidx.y, originidx.z);
            Plane oriplane = new Plane(_origin, orient);
            float _u = offsetsteps * Libutility.sinlut[(int)viewangle] * Libutility.coslut[(int)offsetangle];
            float _v = offsetsteps * Libutility.sinlut[(int)viewangle] * Libutility.sinlut[(int)offsetangle];
            float _w = offsetsteps * Libutility.coslut[(int)viewangle];
            //float u = (double) offsetsteps * Math.Sin((double) viewangle * 3.1416F / 180) * Math.Cos((double) offsetangle * 3.1416F / 180);
            //float v = (double)offsetsteps * Math.Sin((double) viewangle * 3.1416F / 180) * Math.Sin((double) offsetangle * 3.1416F / 180);
            //float w = (double)offsetsteps * Math.Cos((double) viewangle * 3.1416F / 180);
            Point3d target = oriplane.PointAt(_u, _v, _w);
            //int tx = (int)Math.Round(target.X);
            //int ty = (int)Math.Round(target.Y);
            //int tz = (int)Math.Round(target.Z);
            if (target.X < 0 || target.X >= u || target.Y < 0 || target.Y >= v || target.Z < 0 || target.Z >= w)
                return -1;
            Index3f thispos = new Index3f((float)target.X, (float)target.Y, (float)target.Z);
            /*int tx = getUIndex(target.X);
            int ty = getVIndex(target.Y);
            int tz = getWIndex(target.Z);*/
            return trail[thispos.convertToIndex(u, v, w)];
        }
        public override Point3d getNeighbourhoodFreePosByIndex(int idx, int radius)
        {
            Point3d retpt = new Point3d(-1, -1, -1);
            if (radius < 1)
            {
                return retpt;
            }
            Index3f idpos = new Index3f(idx, u, v, w);
            int start_x = (int)idpos.x - radius > 0 ? (int)idpos.x - radius : 0;
            int start_y = (int)idpos.y - radius > 0 ? (int)idpos.y - radius : 0;
            int start_z = (int)idpos.z - radius > 0 ? (int)idpos.z - radius : 0;
            int end_x = (int)idpos.x + radius < u ? (int)idpos.x + radius : u - 1;
            int end_y = (int)idpos.y + radius < v ? (int)idpos.y + radius : v - 1;
            int end_z = (int)idpos.z + radius < w ? (int)idpos.z + radius : w - 1;
            int count = (end_x - start_x + 1) * (end_y - start_y + 1) * (end_z - start_z + 1); //
            //int[] ids = new int[count];
            List<Point3d> freePos = new List<Point3d>();
            for (int i = end_x; i >= start_x; i--)
            {
                for (int j = end_y; j >= start_y; j--)
                {
                    for (int k = end_z; k >= start_z; k--)
                    {
                        if (particle_ids[i + j * u + k * u * v] == -1)
                        {
                            //retpt = new Point3d(i, j, k);
                            freePos.Add(positions[i + j * u + k * u * v]);
                            //return retpt;
                        }
                    }
                }
            }
            int lens = freePos.Count;
            if (lens == 0)
                return retpt;
            retpt = freePos[Libutility.getRand(lens)];
            return retpt;
        }
        private List<int> radiusIndices(int idx, int radius) 
        {
            List<int> indices = new List<int>();
            Index3f thispos = new Index3f(idx, u, v, w);
            int x = (int)thispos.x;
            int y = (int)thispos.y;
            int z = (int)thispos.z;
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
                        indices.Add(i + j * u + k * v * u);
                    }
                }
            }
            return indices;
        
        }
        public override Vector3d getAcuOrientation(Index3f pos, Vector3d idOri)
        {
            Index3f nexpos = new Index3f(pos.x + (float)idOri.X, pos.y + (float)idOri.Y, pos.z + (float)idOri.Z);
            Point3d lastpt = getPositionByIndex(pos.convertToIndex(u, v, w));
            Point3d newpt = getPositionByIndex(nexpos.convertToIndex(u, v, w));
            return Point3d.Subtract(newpt, lastpt);
        } 
        public override void setObstacles(List<Brep> obs)
        {
            if (obs.Count == 0 || obs == null)
                return;
            System.Threading.Tasks.Parallel.ForEach(obs, (br) =>
            {
                Curve[] intersectCrv;
                Point3d[] intersectPt;
                for (int i = 0; i < u; i++)
                {
                    for (int j = 0; j < v; j++)
                    {
                        Line thisCrv = new Line(getPositionByIndex(i + j * u), getPositionByIndex(i + j * u + (w -1) * u * v));
                        thisCrv.Extend(w_interval * 10, w_interval * 10);
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

                        for (int k = (int)getWIndex(ptS.Z); k < (int)getWIndex(ptE.Z); k++)
                        {
                            particle_ids[i + j * u + k * u * v] = -2;
                            griddata[i + j * u + k * u * v] = 2;
                        }
                    }
                }
            });

        }
        public void setContainer() 
        {
            for (int i = 0; i < u; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    for (int k = 0; k < w; k++)
                    {
                        particle_ids[i + j * u + k * u * v] = -1;
                        griddata[i + j * u + k * u * v] = 0;
                    }
                }
            }
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
                            particle_ids[i + j * u + k * u * v] = -1;
                            griddata[i + j * u + k * u * v] = 0;
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
                        Line thisCrv = new Line(getPositionByIndex(i + j * u), getPositionByIndex(i + j * u + (w - 1) * u * v));
                        thisCrv.Extend(w_interval * 10, w_interval * 10);
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

                        for (int k = (int)getWIndex(ptS.Z); k < (int)getWIndex(ptE.Z); k++)
                        {
                            particle_ids[i + j * u + k * u * v] = -1;
                            griddata[i + j * u + k * u * v] = 0;
                        }
                    }
                }
            });
        }
        public override void setFood(List<Point3d> food)
        {
            foreach (Point3d pt in food)
            {
                Index3f thispos = new Index3f(getUIndex(pt.X), getVIndex(pt.Y), getWIndex(pt.Z));
                setGridCellValueByIndex(thispos.convertToIndex(u,v,w), 1, 1);
            }
            projectToTrail();
        }
        /*public override void setBirthPlace(List<Point3d> origin)
        {
            _origins = origin;
            //foreach (Point3d pt in origin)
            //{
            //  setGridCellValueByIndex((int) pt.X, (int) pt.Y, (int) pt.Z, 1, 1);
            //}
        }*/
        public override Point3d getRandomBirthPlace()
        {
            //int length = _origins.Count;
            return emitter.getRandEmitPos();
            //return _origins[util.getRand(length)];
        }

        public override  Mesh getTrailEvaMesh(double zpos)
        {
            int z = (int)getWIndex(zpos);
            double trailmax = getMaxTrailValue();
            Mesh evaMesh = new Mesh();
            Plane worldXY = new Plane(new Point3d(0, 0, z), new Point3d(1, 0, z), new Point3d(0, 1, z));
            Plane baseplane = new Plane(new Point3d(0, 0, getWMin()), new Vector3d(0, 0, 1));
            evaMesh = Mesh.CreateFromPlane(baseplane, new Interval(getUMin(), getUMax()), new Interval(getVMin(), getVMax()), u, v);
            for (int i = 0; i < evaMesh.Vertices.Count; i++)
            {
                Point3f vert = evaMesh.Vertices[i];
                int x = (int)getUIndex(vert.X);
                int y = (int)getVIndex(vert.Y);
                float thisTrail = 0;
                if (x < u - 1 && y < v - 1)
                    thisTrail = (float)(trail[x + y * u + z * u * v] * 255.0 / trailmax);
                if (thisTrail > 255)
                    thisTrail = 255;
      
                evaMesh.VertexColors.SetColor(i, (int)thisTrail, 0, 0);
            }
            return evaMesh;

        }

        public override float[] getTrails()
        {
            return trail;

        }

        private float getUIndex(double x) 
        {
            float uid = (float)((x - UMin) / u_interval);
            uid = uid < u ? uid : u - 1;
            uid = uid > 0 ? uid : 0;
            return uid;
        }
        private float getVIndex(double y) 
        {
            float vid = (float)((y - VMin) / v_interval);
            vid = vid < v ? vid : v - 1;
            vid = vid > 0 ? vid : 0;
            return vid;
        }
        private float getWIndex(double z)
        {
            float wid = (float)((z - WMin) / w_interval);
            wid = wid < w ? wid : w - 1;
            wid = wid > 0 ? wid : 0;
            return wid;
        }
        public override Index3f getIndexByPosition(double x, double y, double z) 
        {
            return new Index3f(getUIndex(x), getVIndex(y), getWIndex(z));
        }
        public override Index3f getIndexByPosition(Point3d pos)
        {
            return new Index3f(getUIndex(pos.X), getVIndex(pos.Y), getWIndex(pos.Z));
        }
        public override Point3d getPositionByIndex(int idx) 
        {
            return positions[idx];
        }
        public override double getUMin() { return UMin; }
        public override double getVMin() { return VMin; }
        public override double getWMin() { return WMin; }
        public override double getUMax() { return UMax; }
        public override double getVMax() { return VMax; }
        public override double getWMax() { return WMax; }
        public override double getEnvAccu() 
        {
            double minuv = Math.Min(u_interval, v_interval);
            return Math.Min(minuv, w_interval);
        }
        public override bool constrainPos(Index3f pos, int type) 
        {
            bool flag = false;
            if (type == 0)
            {
                if (pos.x < getUMin())
                {
                    pos.x = 2 * (float)(getUMin()) - pos.x;
                    flag = true;
                }
                if (pos.x > getUMax())
                {
                    pos.x = (float)(getUMax() - u_interval);
                    flag = true;
                }
                if (pos.y < getVMin())
                {
                    pos.y = 2 * (float)(getVMin()) - pos.y;
                    flag = true;
                }
                if (pos.y > getVMax())
                {
                    pos.y = (float)(getVMax() - v_interval);
                    flag = true;
                }
                if (pos.z < getWMin())
                {
                    pos.z = 2 * (float)(getWMin()) - pos.z;
                    flag = true;
                }
                if (pos.z > getWMax())
                {
                    pos.z = (float)(getWMax() - w_interval);
                    flag = true;
                }
            }
            else 
            {
                if (pos.x < getUMin())
                {
                    pos.x = (float)(pos.x + getUMax());
                    flag = true;
                }
                if (pos.x > getUMax())
                {
                    pos.x = (float)(pos.x - getUMax());
                    flag = true;
                }
                if (pos.y < getVMin())
                {
                    pos.y = (float)(pos.y + getVMax());
                    flag = true;
                }
                if (pos.y > getVMax())
                {
                    pos.y = (float)(pos.y - getVMax());
                    flag = true;
                }
                if (pos.z < getWMin())
                {
                    pos.z = (float)(pos.z + getWMax());
                    flag = true;
                }
                if (pos.z > getWMax())
                {
                    pos.z = (float)(pos.z - getWMax());
                    flag = true;
                }
            }
            return flag;
        }
        public override Vector3d bounceOrientation(Point3d pos, Vector3d vel) 
        {
            Vector3d retVec = vel;
            if (pos.X < getUMin() || pos.X > getUMin())
                vel.X = -vel.X;
            if (pos.Y < getVMin() || pos.Y > getVMin())
                vel.Y = -vel.Y;
            if (pos.Z < getWMin() || pos.Z > getWMin())
                vel.Z = -vel.Z;
            return retVec;

        }
        public override Point3d[] getPoints() 
        {
            return positions;
        }
        public override bool isOutsideBorderRangeByIndex(int idx)
        {
            Index3f thispos = new Index3f(idx, u, v, w);
            if (thispos.x < 2 || thispos.x > (u - 3))
                return true;
            else if (thispos.y < 2 || thispos.y > (v - 3))
                return true;
            else if (thispos.z < 2 || thispos.z > (w - 3))
                return true;
            else return false;
        }
        public override void Clear() 
        {
            positions.Initialize();
            trail.Initialize();
            griddata.Initialize();
            temptrail.Initialize();
            particle_ids.Initialize();
            agedata.Initialize();
            //_origins.Clear();
        }
        private void resetParticleIds() 
        {

            for (int i = 0; i < u * v * w; i++) 
            {
                if (griddata[i] == 2)
                   particle_ids[i] = -2;
                else
                   particle_ids[i] = -1;
            }
        }
        public override void Reset()
        {
            resetParticleIds();
            for (int i = 0; i < u * v * w; i++)
            {
                trail[i] = 0;
                temptrail[i] = 0;
                agedata[i] = 0;
            }
        }
        public override Vector3d projectOrientationToEnv(Point3d pos, Vector3d vel)
        {
            return vel;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Physarealm.Environment
{
    class SurfaceEnvironmentType : AbstractEnvironmentType, IDisposable
    {
        public Point3d[, ,] uv_positions;
        private Surface _srf;
        public float[, ,] trail { get; set; }
        private float[, ,] temptrail;
        private int[, ,] particle_ids;
        public int[, ,] griddata { get; set; }//0 for default, 1 for food, 2 for outside
        public int[, ,] agedata;
        public float diffdamp { get; set; }
        public float projectvalue { get; set; }
        //private List<Point3d> _origins;
        private int grid_age;
        public bool age_flag { get; set; }
        double u_interval;
        double v_interval;
        //double w_interval;
        //public double UMin { get; private set; }
        //public double UMax { get; private set; }
        //public double VMin { get; private set; }
        //public double VMax { get; private set; }
        //public double WMin { get; private set; }
        //public double WMax { get; private set; }
                
        public SurfaceEnvironmentType(Surface srf, int x, int y, int z)
            : base(x, y, 1) //initialize data
        {
            _srf = srf;
            //UMin = _box.Min.X;
            //UMax = _box.Max.X;
            //VMin = _box.Min.Y;
            //VMax = _box.Max.Y;
           // WMin = _box.Min.Z;
            //WMax = _box.Max.Z;
            uv_positions = new Point3d[u,v,1];
            u_interval = _srf.Domain(0).Length / u;
            v_interval = _srf.Domain(1).Length / v;
            //w_interval = (WMax - WMin) / w;
            trail = new float[u, v, 1];
            temptrail = new float[u, v, 1];
            particle_ids = new int[u, v, 1];
            griddata = new int[u, v, 1];
            agedata = new int[u, v, 1];
            grid_age = 0;
            for (int i = 0; i < u; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    //for (int k = 0; k < w; k++)
                    //{
                        uv_positions[i, j, 0] = _srf.PointAt(i * u_interval + u_interval / 2, j * v_interval + v_interval / 2);
                        trail[i, j, 0] = 0;
                        temptrail[i, j, 0] = 0;
                        particle_ids[i, j, 0] = -2;
                        griddata[i, j, 0] = 2;
                        agedata[i, j, 0] = 0;
                    //}
                }
            }
            projectvalue = 5;
            diffdamp = 0.1F;
            age_flag = false;
            _escape_p = 0;
            env_type = 2;
        }
        public SurfaceEnvironmentType(SurfaceEnvironmentType srfenv) : this(srfenv._srf, srfenv.u, srfenv.v, srfenv.w) { }
        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new SurfaceEnvironmentType(this);
        }

        public override int getGriddataByIndex(int x, int y, int z)
        {
            return griddata[x, y, 0];
        }
        public override bool isOccupidByParticleByIndex(int x, int y, int z)
        {
            if (particle_ids[x, y,0] == -1 || particle_ids[x, y, 0] == -2)
                return false;
            return true;
        }
        public override bool isWithinObstacleByIndex(int x, int y, int z)
        {
            if (particle_ids[x, y, 0] == -2)
                return true;
            return false;
        }
        public override void occupyGridCellByIndex(int x, int y, int z, int id)
        {
            particle_ids[x, y, 0] = id;
        }
        public override void clearGridCellByIndex(int x, int y, int z)
        {
            if (griddata[x, y, 0] == 2)
                particle_ids[x, y, 0] = -2;
            else
                particle_ids[x, y, 0] = -1;
        }
        public override void increaseTrailByIndex(int x, int y, int z, float val)
        {
            trail[x, y, z] += val;
        }
        public override void setGridCellValueByIndex(int x, int y, int z, int radius, int val)
        {
            if (radius < 1)
                griddata[x, y, z] = val;
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            //int start_z = w - radius > 0 ? w - radius : 0;
            int end_x = x + radius < u ? x + radius : u - 1;
            int end_y = y + radius < v ? y + radius : v - 1;
            //int end_z = w + radius < w ? w + radius : w - 1;
            for (int i = start_x; i <= end_x; i++)
            {
                for (int j = start_y; j <= end_y; j++)
                {
                    //for (int k = start_z; k <= end_z; k++)
                    //{
                        griddata[i, j, 0] = val;
                    //}
                }
            }
        }
        public override float getAverageNeighbourhoodByIndex(int x, int y, int z, int radius)
        {
            float total = 0;
            if (radius < 1)
                return trail[x, y, z];
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            //int start_z = w - radius > 0 ? w - radius : 0;
            int end_x = x + radius < u - 1 ? x + radius : u - 1;
            int end_y = y + radius < v - 1 ? y + radius : v - 1;
            //int end_z = w + radius < w - 1 ? w + radius : w - 1;
            for (int i = start_x; i <= end_x; i++)
            {
                for (int j = start_y; j <= end_y; j++)
                {
                    //for (int k = start_z; k <= end_z; k++)
                    //{
                        total += trail[i, j, 0];
                    //}
                }
            }
            int num = (radius * 2 + 1) * (radius * 2 + 1);
            //int num = (int)Math.Pow(radius * 2 + 1, 2);
            return total / num;
        }
        public override void diffuseTrails()
        {
            grid_age++;
            /*System.Threading.Tasks.Parallel.For(0, u, (i) =>
            {
                System.Threading.Tasks.Parallel.For(0, v, (j) =>
                {
                    //int k = 0;
                    //System.Threading.Tasks.Parallel.For(0, w, (k) =>
                    //{
                        float ave = getAverageNeighbourhoodByIndex(i, j, 0, 1);
                        temptrail[i, j, 0] = ave * (1 - diffdamp);
                        if (agedata[i, j, 0] != 0)
                            agedata[i, j, 0]++;
                    //});
                });
            });*/
            for (int i = 0; i < u; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    float ave = getAverageNeighbourhoodByIndex(i, j, 0, 1);
                    temptrail[i, j, 0] = ave * (1 - diffdamp);
                    if (agedata[i, j, 0] != 0)
                        agedata[i, j, 0]++;
                }
            }
            for (int i = 0; i < u; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    trail[i, j, 0] = temptrail[i, j, 0];
                    if (trail[i, j, 0] < 0)
                        trail[i, j, 0] = 0;
                    if (griddata[i, j, 0] == 2)
                        trail[i, j, 0] *= (float)_escape_p;
                }
            }
            /*System.Threading.Tasks.Parallel.For(0, u, (i) =>
            {
                System.Threading.Tasks.Parallel.For(0, v, (j) =>
                {
                    //int k = 0;
                    //System.Threading.Tasks.Parallel.For(0, w, (k) =>
                    //{
                        trail[i, j, 0] = temptrail[i, j, 0];
                        if (trail[i, j, 0] < 0)
                            trail[i, j, 0] = 0;
                        if (griddata[i, j, 0] == 2)
                            trail[i, j, 0] *= (float)_escape_p;
                    //});
                });
            });*/
            
        }

        public override void projectToTrail()
        {
            /*System.Threading.Tasks.Parallel.For(0, u, (i) =>
            {
                System.Threading.Tasks.Parallel.For(0, v, (j) =>
                {
                    //System.Threading.Tasks.Parallel.For(0, w, (k) =>
                    //{
                        if (griddata[i, j, 0] == 1)
                        {
                            increaseTrailByIndex(i, j, 0, projectvalue);
                        }
                    //});
                });
            });*/
            for (int i = 0; i < u; i++) 
            {
                for (int j = 0; j < v; j++) 
                {
                    if (griddata[i, j, 0] == 1)
                    {
                        increaseTrailByIndex(i, j, 0, projectvalue);
                    }
                }
            }
        }

        public override int countNumberOfParticlesPresentByIndex(int x, int y, int z, int radius)
        {
            int total = 0;
            int countNow = 0;
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            //int start_z = w - radius > 0 ? w - radius : 0;
            int end_x = x + radius <= u - 1 ? x + radius : u - 1;
            int end_y = y + radius <= v - 1 ? y + radius : v - 1;
            //int end_z = w + radius <= w - 1 ? w + radius : w - 1;
            for (int i = start_x; i <= end_x; i++)
            {
                for (int j = start_y; j <= end_y; j++)
                {
                    //for (int k = start_z; k <= end_z; k++)
                    //{
                        if (isOccupidByParticleByIndex(i, j, 0) == true)
                            total++;
                        countNow++;
                    //}
                }
            }
            int totalShouldBe = (int)Math.Pow(radius * 2 + 1, 2) - 1;
            total += totalShouldBe - countNow;
            return total;
        }

        public override List<Point3d> findNeighborParticle(Amoeba agent, int radius)
        {
            List<Point3d> nei = new List<Point3d>();
            int x = agent.curx;
            int y = agent.cury;
            //int z = agent.curz;
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            //int start_z = z - radius > 0 ? z - radius : 0;
            int end_x = x + radius <= u - 1 ? x + radius : u - 1;
            int end_y = y + radius <= v - 1 ? y + radius : v - 1;
            //int end_z = z + radius <= w - 1 ? z + radius : w - 1;
            for (int i = start_x; i <= end_x; i++)
            {
                for (int j = start_y; j <= end_y; j++)
                {
                    //for (int k = start_z; k <= end_z; k++)
                    //{
                        if (isOccupidByParticleByIndex(i, j, 0) == true)
                        {
                            //nei.Add(new Point3d(i, j, k));
                            nei.Add(uv_positions[i, j, 0]);
                        }
                    //}
                }
            }

            return nei; 
        }

        public override List<Point3d> findNeighborParticleByIndex(int x, int y, int z, int radius)
        {
            List<Point3d> nei = new List<Point3d>();
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            //int start_z = w - radius > 0 ? w - radius : 0;
            int end_x = x + radius <= u - 1 ? x + radius : u - 1;
            int end_y = y + radius <= v - 1 ? y + radius : v - 1;
            //int end_z = w + radius <= w - 1 ? w + radius : w - 1;
            for (int i = start_x; i <= end_x; i++)
            {
                for (int j = start_y; j <= end_y; j++)
                {
                    int k = 0;
                    //for (int k = start_z; k <= end_z; k++)
                    //{
                        if (isOccupidByParticleByIndex(i, j, k) == true)
                        {
                            //nei.Add(new Point3d(i, j, k));
                            nei.Add(uv_positions[i, j, k]);
                        }
                    //}
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
                    int k = 0;
                    //for (int k = 0; k < w; k++)
                    //{
                        if (trail[i, j, k] > max)
                            max = trail[i, j, k];
                    //}
                }
            }
            return max;
        }

        public override float getOffsetTrailValue(int x, int y, int z, Vector3d orient, float viewangle, float offsetangle, float offsetsteps, Libutility util)
        {
            /*Point3d pt;
            Vector3d[] vec;
            _srf.Evaluate((x + 0.5) * u_interval, (y + 0.5) * v_interval, 1, out pt, out vec);
            vec[0].Unitize();
            Vector3d oricopy = new Vector3d(orient.X, orient.Y, orient.Z);
            oricopy.Unitize();
            double dotproduc = Vector3d.Multiply(vec[0], oricopy);
            if (dotproduc > 0.3)
                return -1;
            Point3d _origin = uv_positions[x, y, z];
            Plane oriplane = new Plane(_origin, orient);
            float _u = offsetsteps * util.sinlut[(int)viewangle] * util.coslut[(int)offsetangle];
            float _v = offsetsteps * util.sinlut[(int)viewangle] * util.sinlut[(int)offsetangle];
            float _w = offsetsteps * util.coslut[(int)viewangle];
            Point3d target = oriplane.PointAt(_u, _v, _w);
            Point3d targetid = getIndexByPosition(target.X, target.Y, target.Z);
            int tx = (int)targetid.X;
            int ty = (int)targetid.Y;
            int tz = getWIndex(target.Z);
            if (tx < 0 || tx >= u || ty < 0 || ty >= v || tz < 0 || tz >= w)
                return -1;
            return trail[tx, ty, tz];*/
            float viewRad = viewangle * 3.1416F / 180;
            Point3d _ori = new Point3d(x, y, 0);
            Vector3d _tarOri = orient;
            _tarOri.Transform(Transform.Rotation(viewRad, Plane.WorldXY.ZAxis, _ori));
            _tarOri.Unitize();
            _tarOri *= offsetsteps;
            Point3d _tar = _ori + _tarOri;
            //Point3d _tar = Point3d.Add(_ori, _tarOri);
            int tx = (int)_tar.X;
            int ty = (int)_tar.Y;
            if (tx < 0 || tx >= u || ty < 0 || ty >= v)
                return -1;
            return trail[tx, ty, 0];

        }

        public override Point3d getNeighbourhoodFreePosByIndex(int x, int y, int z, int radius, Libutility util)
        {
            Point3d retpt = new Point3d(-1, -1, -1);
            if (radius < 1)
            {
                return retpt;
            }
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
                    //for (int k = end_z; k >= start_z; k--)
                    //{
                        if (particle_ids[i, j, 0] == -1)
                        {
                            //retpt = new Point3d(i, j, k);
                            freePos.Add(uv_positions[i, j, 0]);
                            //return retpt;
                        }
                    //}
                }
            }
            int lens = freePos.Count;
            if (lens == 0)
                return retpt;
            retpt = freePos[util.getRand(lens)];
            return retpt;
        }

        public override void setObstacles(List<Brep> obs)
        {
            Interval u_intv = _srf.Domain(0);
            double u_step = u_intv.Length / u;
            System.Threading.Tasks.Parallel.ForEach(obs, (br) =>
            {
                
                for(int i = 0; i < u; i++)
                {
                    Curve thiscrv = _srf.IsoCurve(1, u_intv.Min + i * u_step + u_step / 2);
                    Curve[] intersectCrv;
                    Point3d[] intersectPt;
                    Rhino.Geometry.Intersect.Intersection.CurveBrep(thiscrv, br, 1, out intersectCrv, out intersectPt);
                    if (intersectPt == null)
                        continue;
                    int len = intersectPt.Length;
                    if (len <= 1)
                        continue;
                    Point3d ptS = intersectPt[0];
                    Point3d ptE = intersectPt[1];
                    double v1;
                    double v2;
                    double u1;
                    double u2;
                    _srf.ClosestPoint(ptS, out  u1, out v1);
                    _srf.ClosestPoint(ptE, out u2, out v2);
                    if (v1 > v2)
                    {
                        double swap = v1;
                        v2 = v1;
                        v1 = swap;
                    }

                    for (int k = (int)v1; k < v2; k++)
                    {
                        particle_ids[i, k, 0] = -2;
                        griddata[i, k, 0] = 2;
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
                    //for (int k = 0; k < w; k++)
                    //{
                        particle_ids[i, j, 0] = -1;
                        griddata[i, j, 0] = 0;
                    //}
                }
            }
        }
        public override void setContainer(List<Brep> cont)
        {
            Interval u_intv = _srf.Domain(0);
            double u_step = u_intv.Length / u;
            if (cont.Count == 0)
            {
                for (int i = 0; i < u; i++)
                {
                    for (int j = 0; j < v; j++)
                    {
                        //for (int k = -0; k < w; k++)
                        //{
                            particle_ids[i, j, 0] = -1;
                            griddata[i, j, 0] = 0;
                        //}
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
                    Curve thiscrv = _srf.IsoCurve(1, u_intv.Min + i * u_step + u_step / 2);
                    Rhino.Geometry.Intersect.Intersection.CurveBrep(thiscrv, br, 1, out intersectCrv, out intersectPt);
                    if (intersectPt == null)
                        continue;
                    int len = intersectPt.Length;
                    if (len <= 1)
                        continue;
                    Point3d ptS = intersectPt[0];
                    Point3d ptE = intersectPt[1];
                    double v1;
                    double v2;
                    double u1;
                    double u2;
                    _srf.ClosestPoint(ptS, out  u1, out v1);
                    _srf.ClosestPoint(ptE, out u2, out v2);
                    if (v1 > v2)
                    {
                        double swap = v1;
                        v2 = v1;
                        v1 = swap;
                    }

                    for (int k = (int)v1; k < v2; k++)
                    {
                        particle_ids[i, k, 0] = -1;
                        griddata[i, k, 0] = 0;
                    }
                    
                }
            });
        }

        public override void setFood(List<Point3d> food)
        {
            foreach (Point3d pt in food)
            {
                Point3d indexpt = getIndexByPosition(pt.X, pt.Y,pt.Z);
                setGridCellValueByIndex((int)indexpt.X, (int)(indexpt.Y), (int)(indexpt.Z), 1, 1);
            }
            projectToTrail();
        }


        public override Point3d getRandomBirthPlace(Libutility util)
        {
            /*Point3d retpos = emitter.getRandEmitPos();
            double u;
            double v;
            _srf.ClosestPoint(retpos, out u, out v);
            return _srf.PointAt(u, v);*/
            return emitter.getRandEmitPos();

        }

        public override Mesh getTrailEvaMesh(double z)
        {
            /*
            int z = getWIndex(zpos);
            double trailmax = getMaxTrailValue();
            Mesh evaMesh = new Mesh();
            Plane worldXY = new Plane(new Point3d(0, 0, z), new Point3d(1, 0, z), new Point3d(0, 1, z));
            Plane baseplane = new Plane(new Point3d(0, 0, getWMin()), new Vector3d(0, 0, 1));
            evaMesh = Mesh.CreateFromPlane(baseplane, new Interval(getUMin(), getUMax()), new Interval(getVMin(), getVMax()), u, v);
            for (int i = 0; i < evaMesh.Vertices.Count; i++)
            {
                Point3f vert = evaMesh.Vertices[i];
                int x = getUIndex(vert.X);
                int y = getVIndex(vert.Y);
                float thisTrail = 0;
                if (x < u - 1 && y < v - 1)
                    thisTrail = (float)(trail[x, y, z] * 255.0 / trailmax);
                if (thisTrail > 255)
                    thisTrail = 255;

                evaMesh.VertexColors.SetColor(i, (int)thisTrail, 0, 0);
            }
            return evaMesh;*/
            return null;
        }

        public override Point3d getIndexByPosition(double x, double y, double z)
        {
            Point3d retpos = new Point3d(x,y,z);
            double i;
            double j;
            _srf.ClosestPoint(retpos, out i, out j);
            return new Point3d(getUIndex(i), getVIndex(j), 0); ;
        }

        public override Point3d getPositionByIndex(int x, int y, int z)
        {
            return uv_positions[x, y, 0];
        }
        public override Vector3d getOrientationFromUv(Point3d pos, Vector3d uv_orit) 
        {
            /*double i, j;
            _srf.ClosestPoint(pos,out i,out j);
            double i2 = i + uv_orit.X;
            double j2 = j + uv_orit.Y;
            Point3d tarPt;
            Vector3d[] deris;
            if (!_srf.Evaluate(i2, j2, 1, out tarPt, out deris))
            {
                return Point3d.Subtract(tarPt, pos);
            }
            else
                return new Vector3d(0, 0, 0);  */
            Point3d _ori = getIndexByPosition(pos.X, pos.Y, pos.Z);
            Point3d _tarIdx = Point3d.Add(_ori, uv_orit);
            int _x = (int)_tarIdx.X < u ? (int)_tarIdx.X : u - 1;
            int _y = (int)_tarIdx.Y < v ? (int)_tarIdx.Y : v - 1;
            _x = _x > 0 ? _x : 0;
            _y = _y > 0 ? _y : 0;
            Point3d _tar = getPositionByIndex(_x, _y, 0);
            return Point3d.Subtract(_tar, pos);
        }
        public override double getUMin()
        {
            throw new NotImplementedException();
        }

        public override double getVMin()
        {
            throw new NotImplementedException();
        }

        public override double getWMin()
        {
            throw new NotImplementedException();
        }

        public override double getUMax()
        {
            throw new NotImplementedException();
        }

        public override double getVMax()
        {
            throw new NotImplementedException();
        }

        public override double getWMax()
        {
            throw new NotImplementedException();
        }

        public override double getEnvAccu()
        {
            Interval u_intv = _srf.Domain(0);
            Interval v_intv = _srf.Domain(1);
            double u_step = u_intv.Length / u;
            double v_step = v_intv.Length / v;
            Point3d orig = _srf.PointAt(0, 0);
            double dist1 = orig.DistanceTo(_srf.PointAt(u_step, 0));
            double dist2 = orig.DistanceTo(_srf.PointAt(0, v_step));
            return Math.Min(dist1, dist2) ;
        }

        public override bool constrainPos(ref float x, ref float y, ref float z, int type)
        {
            //bool flag = false;
            double i;
            double j;
            _srf.ClosestPoint(new Point3d(x, y, z), out i, out j);
            if (isOutsideBorderRangeByIndex(getUIndex(i),getVIndex(j), 0))
            {
                Point3d thispt = _srf.PointAt(i, j);
                x = (float)thispt.X;
                y = (float)thispt.Y;
                z = (float)thispt.Z;
            
                return true;
            }
            else
                return false;
        }
        public override bool isOutsideBorderRangeByIndex(int x, int y, int z)
        {
            if (x < 2 || x > (u - 2))
                return true;
            else if (y < 2 || y > (v - 2))
                return true;
            //else if (z < 2 || z > (w - 2))
                //return true;
            else return false;
        }

        public override List<float> getTrailV()
        {
            List<float> ret = new List<float>();
            //int item = 0;
            for (int k = 0; k < w; k++)
            {
                for (int j = 0; j < v; j++)
                {
                    for (int i = 0; i < u; i++)
                    {
                        //ret[item] = trail[i, j, k];
                        //item++;
                        //ret.Add(trail[i, j, 0]);
                        ret.Add(trail[i, j, 0]);
                    }
                }
            }
            return ret;
        }

        public override void Clear()
        {
            uv_positions.Initialize();
            trail.Initialize();
            griddata.Initialize();
            temptrail.Initialize();
            particle_ids.Initialize();
            agedata.Initialize();
            //_origins.Clear();
        }

        public override void Reset()
        {
            resetParticleIds();

            for (int i = 0; i < u; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    //for (int k = 0; k < w; k++)
                    //{
                        trail[i, j, 0] = 0;
                        temptrail[i, j, 0] = 0;
                        agedata[i, j, 0] = 0;
                    //}
                }
            }
        }
        private void resetParticleIds()
        {
            for (int k = 0; k < w; k++)
            {
                for (int j = 0; j < v; j++)
                {
                    for (int i = 0; i < u; i++)
                    {
                        if (griddata[i, j, k] == 2)
                            particle_ids[i, j, k] = -2;
                        else
                            particle_ids[i, j, k] = -1;
                    }
                }
            }
        }

        public override float[, ,] getTrails() { return trail; }
        public override Point3d[, ,] getPosition() { return uv_positions; }
        public override Vector3d bounceOrientation(Point3d pos, Vector3d vel) 
        {
            return new Vector3d(0, 0, 0);
        }
        public override List<Point3d> getNeighbourTrailClosePos(int x, int y, int z, int radius, double near_level)
        {
            double maxv = getMaxTrailValue();
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
                        if (Math.Abs(trail[x, y, z] - trail[i, j, k]) / maxv < near_level)
                        {
                            nei.Add(uv_positions[i, j, k]);
                        }
                    }
                }
            }

            return nei;
        }

        public override string ToString()
        {
            return TypeName;
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.SurfaceEnvironmentType";
            }
        }
        private int getUIndex(double x)
        {
            int uid = (int)(x / u_interval);
            uid = uid < u ? uid : u - 1;
            uid = uid > 0 ? uid : 0;
            return uid;
        }
        private int getVIndex(double y)
        {
            int vid = (int)(y / v_interval);
            vid = vid < v ? vid : v - 1;
            vid = vid > 0 ? vid : 0;
            return vid;
        }
        private int getWIndex(double z)
        {
            //int wid = (int)((z - WMin) / w_interval);
            return 0;
        }
        public override Vector3d projectOrientationToEnv(Point3d pos, Vector3d vel) 
        {
            double i;
            double j;
            Plane frame;
            _srf.ClosestPoint(pos, out i, out j);
            _srf.FrameAt(i, j, out frame);
            Point3d ptframe;
            frame.RemapToPlaneSpace(Point3d.Add(pos, vel), out ptframe);
            frame.PointAt(ptframe.X, ptframe.Y);
            return new Vector3d(Point3d.Subtract( frame.PointAt(ptframe.X, ptframe.Y), pos));
        }
        public void Dispose()
        {
            Clear();
        }
    }
}

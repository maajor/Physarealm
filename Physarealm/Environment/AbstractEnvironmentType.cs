using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;
using Grasshopper.Kernel.Types;
using Physarealm.Emitter;

namespace Physarealm.Environment
{
    public abstract class AbstractEnvironmentType:GH_Goo<object>
    {

        public int u;
        public int v;
        public int w;
        public static Point3d[] positions;
        public static float[] trail;
        protected static float[] temptrail;
        protected static int[] particle_ids;
        public static int[] griddata;//0 for default, 1 for food, 2 for outside
        public static int[] agedata;

        public double _escape_p { get; set; }
        public AbstractEmitterType emitter;
        abstract public override IGH_Goo Duplicate();

        public abstract int getGriddataByIndex(int idx);//by index
        public abstract bool isOccupidByParticleByIndex(int idx);//by index
        public abstract bool isWithinObstacleByIndex(int idx);//by index
        public abstract void occupyGridCellByIndex(int idx, int id);//by index
        public abstract void clearGridCellByIndex(int idx);//by index
        public abstract void increaseTrailByIndex(int idx, float val);//by index
        public abstract void setGridCellValueByIndex(int idx, int radius, int val);//by index
        public abstract float getAverageNeighbourhoodByIndex(int idx, int radius);//by index
        public abstract void diffuseTrails();
        public abstract void projectToTrail();
        public abstract int countNumberOfParticlesPresentByIndex(int idx, int radius);//by index
        public abstract List<Point3d> findNeighborParticle(Amoeba agent, int radius);
        public abstract List<Point3d> findNeighborParticleByIndex(int idx, int radius);//by index
        public abstract float getMaxTrailValue();
        public abstract float getOffsetTrailValueByIndex(int idx, Vector3d orient, float viewangle, float offsetangle, float offsetsteps);
        public abstract Point3d getNeighbourhoodFreePosByIndex(int idx, int radius);//by index
        public abstract void setObstacles(List<Brep> obs);
        public abstract void setContainer(List<Brep> cont);
        public abstract void setFood(List<Point3d> food);
        //public abstract void setBirthPlace(List<Point3d> origin);
        public abstract Point3d getRandomBirthPlace();
        public abstract Mesh getTrailEvaMesh(double z);
        public abstract Index3f getIndexByPosition(double x, double y, double z);
        //public abstract Index3f getIndexByPosition(Index3f idx);
        public abstract Index3f getIndexByPosition(Point3d pos);
        public abstract Point3d getPositionByIndex(int idx);
        public abstract double getUMin();
        public abstract double getVMin();
        public abstract double getWMin();
        public abstract double getUMax();
        public abstract double getVMax();
        public abstract double getWMax();
        public abstract double getEnvAccu();
        public abstract bool constrainPos(Index3f pos, int type);
        public abstract float[] getTrails();
        public abstract void Clear();
        public abstract void Reset();
        public abstract List<Point3d> getNeighbourTrailClosePosByIndex(int idx, int radius, double near_level);
        public abstract bool isOutsideBorderRangeByIndex(int idx);
        public abstract Vector3d projectOrientationToEnv(Point3d pos, Vector3d vel);
        public abstract Vector3d bounceOrientation(Point3d pos, Vector3d vel);
        public abstract Point3d[] getPoints();
        public abstract Vector3d getAcuOrientation(Index3f pos, Vector3d idOri);

        public AbstractEnvironmentType(int x, int y, int z) 
        {
            u = x;
            v = y;
            w = z;
        }

        public override bool IsValid
        {
            get { return true; }
        }

        public abstract override string ToString();

        public override string TypeDescription
        {
            get { return "abstract environment type description"; }
        }

        public override string TypeName
        {
            get { return "abstract environment type name"; }
        }
    }
}

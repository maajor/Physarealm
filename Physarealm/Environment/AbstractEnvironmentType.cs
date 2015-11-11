using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;
using Grasshopper.Kernel.Types;

namespace Physarealm.Environment
{
    public abstract class AbstractEnvironmentType:GH_Goo<object>
    {

        public int u;
        public int v;
        public int w;
        abstract public override IGH_Goo Duplicate();

        public abstract int getGriddata(int u, int v, int w);
        public abstract bool isOccupidByParticle(int u, int v, int w);
        public abstract void occupyGridCell(int u, int v, int w, int id);
        public abstract void clearGridCell(int u, int v, int w);
        public abstract void increaseTrail(int u, int v, int w, float val);
        public abstract void setGridCellValue(int u, int v, int w, int radius, int val);
        public abstract float getAverageNeighbourhood(int u, int v, int w, int radius);
        public abstract void diffuseTrails();
        public abstract void projectToTrail();
        public abstract int countNumberOfParticlesPresent(int u, int v, int w, int radius);
        public abstract List<Point3d> findNeighborParticle(Amoeba agent, int radius);
        public abstract List<Point3d> findNeighborParticle(int u, int v, int w, int radius);
        public abstract float getMaxTrailValue();
        public abstract float getOffsetTrailValue(int u, int v, int w, Vector3d orient, float viewangle, float offsetangle, float offsetsteps, Libutility util);
        public abstract Point3d getNeighbourhoodFreePos(int u, int v, int w, int radius, Libutility util);
        public abstract void setObstacles(List<Brep> obs);
        public abstract void setContainer(List<Brep> cont);
        public abstract void setFood(List<Point3d> food);
        public abstract void setBirthPlace(List<Point3d> origin);
        public abstract Point3d getRandomBirthPlace(Libutility util);
        public abstract Mesh getTrailEvaMesh(int z);
        public abstract float[] getTrailV();
        public abstract void Clear();

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

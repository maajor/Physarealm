﻿using System;
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
        public int env_type;//1:box environment, 2: nurbs surface, 3: mesh
        public double _escape_p { get; set; }
        public AbstractEmitterType emitter;
        abstract public override IGH_Goo Duplicate();

        public abstract int getGriddataByIndex(int u, int v, int w);//by index
        public abstract bool isOccupidByParticleByIndex(int u, int v, int w);//by index
        public abstract bool isWithinObstacleByIndex(int x, int y, int z);//by index
        public abstract void occupyGridCellByIndex(int u, int v, int w, int id);//by index
        public abstract void clearGridCellByIndex(int u, int v, int w);//by index
        public abstract void increaseTrailByIndex(int u, int v, int w, float val);//by index
        public abstract void setGridCellValueByIndex(int u, int v, int w, int radius, int val);//by index
        public abstract float getAverageNeighbourhoodByIndex(int u, int v, int w, int radius);//by index
        public abstract void diffuseTrails();
        public abstract void projectToTrail();
        public abstract int countNumberOfParticlesPresentByIndex(int u, int v, int w, int radius);//by index
        public abstract List<Point3d> findNeighborParticle(Amoeba agent, int radius);
        public abstract List<Point3d> findNeighborParticleByIndex(int u, int v, int w, int radius);//by index
        public abstract float getMaxTrailValue();
        public abstract float getOffsetTrailValue(int u, int v, int w, Vector3d orient, float viewangle, float offsetangle, float offsetsteps, Libutility util);
        public abstract Point3d getNeighbourhoodFreePosByIndex(int u, int v, int w, int radius, Libutility util);//by index
        public abstract void setObstacles(List<Brep> obs);
        public abstract void setContainer(List<Brep> cont);
        public abstract void setFood(List<Point3d> food);
        //public abstract void setBirthPlace(List<Point3d> origin);
        public abstract Point3d getRandomBirthPlace(Libutility util);
        public abstract Mesh getTrailEvaMesh(double z);
        public abstract Point3d getIndexByPosition(double x, double y, double z);
        public abstract Point3d getPositionByIndex(int u, int v, int w);
        public abstract double getUMin();
        public abstract double getVMin();
        public abstract double getWMin();
        public abstract double getUMax();
        public abstract double getVMax();
        public abstract double getWMax();
        public abstract double getEnvAccu();
        public abstract bool constrainPos(ref float x, ref float y, ref float z, int type);
        public abstract List<float> getTrailV();
        public abstract void Clear();
        public abstract void Reset();
        public abstract float[, ,] getTrails();
        public abstract Point3d[, ,] getPosition();
        public abstract List<Point3d> getNeighbourTrailClosePos(int u, int v, int w, int radius, double near_level);
        public abstract bool isOutsideBorderRangeByIndex(int u, int v, int w);
        public abstract Vector3d projectOrientationToEnv(Point3d pos, Vector3d vel);
        public abstract Point3d projectLocationToEnv(Point3d pos);
        public abstract Vector3d bounceOrientation(Point3d pos, Vector3d vel);
        public abstract Vector3d getOrientationFromUv(Point3d pos, Vector3d uv_orit);

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

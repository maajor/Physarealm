using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Analysis
{
    public class FieldInterconnectComponent :AbstractFieldAnalysisComponent
    {
        private int radius;
        private double possib;
        private List<Line> connect;
        private List<double> weight;
        private double near_level;
        /// <summary>
        /// Initializes a new instance of the FieldInterconnectComponent class.
        /// </summary>
        public FieldInterconnectComponent()
            : base("Field Interconnect", "FInter",
                "Field Interconnect",
                null, "6833EB95-06B4-43B3-B53A-A59AAE2D8149")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddIntegerParameter("radius", "r", "radius", GH_ParamAccess.item);
            pManager.AddNumberParameter("possibility", "p", "possibility", GH_ParamAccess.item);
            pManager.AddNumberParameter("near level", "nl", "near level", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Connect Line", "cL", "Connect Line", GH_ParamAccess.list);
            pManager.AddNumberParameter("Connect Weight", "cW", "Connect Weight", GH_ParamAccess.list);
        }
        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref env)) return false;
            if (!da.GetData(1, ref radius)) return false;
            if (!da.GetData(2, ref possib)) return false;
            if (!da.GetData(3, ref near_level)) return false;
            if (radius < 1)
                radius = 1;
            if (possib > 1)
                possib = 1;
            else if (possib < 0)
                possib = 0;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            da.SetDataList(0, connect);
            da.SetDataList(1, weight);
        }
        protected override void SolveInstance(IGH_DataAccess da)
        {
            if (!GetInputs(da)) return;
            connect = new List<Line>();
            weight = new List<double>();
            Random rand = new Random(DateTime.Now.Millisecond);
            float[,,] value = env.getTrails();
            float maxv = env.getMaxTrailValue();
            Point3d[,,] pos = env.getPosition();
            for (int i = 0; i < env.u; i++) 
            {
                for (int j = 0; j < env.v; j++) 
                {
                    for (int k = 0; k < env.w; k++) 
                    {
                        if (rand.NextDouble() > (value[i, j, k]  / maxv) * possib)
                            continue;
                        Point3d thispt = pos[i,j,k];
                        List<Point3d> neighbor = env.getNeighbourTrailClosePos(i,j,k,radius, near_level);
                        foreach (Point3d pt in neighbor)
                        {
                            if (rand.NextDouble() < (value[i, j, k] / maxv)  * 0.5)
                            {
                                connect.Add(new Line(pt, thispt));
                                Point3d indexpos = env.getIndexByPosition(pt.X, pt.Y, pt.Z);
                                weight.Add(value[i, j, k] + value[(int)indexpos.X, (int)indexpos.Y, (int)indexpos.Z]);
                            }
                        }
                    }
                }
            }
            SetOutputs(da);
        }
    }
}
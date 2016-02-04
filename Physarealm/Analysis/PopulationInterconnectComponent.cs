using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Environment;

namespace Physarealm.Analysis
{
    public class PopulationInterconnectComponent :AbstractPopulationAnalysisComponent
    {
        private Physarum p;
        private AbstractEnvironmentType env;
        private int radius;
        private double possib;
        /// <summary>
        /// Initializes a new instance of the PopulationInterconnectComponent class.
        /// </summary>
        public PopulationInterconnectComponent()
            : base("Population Interconnect", "PopInter",
                "This componets connect pairs of agents locations below a certain distance.",
               null, "51167445-C5B7-42A4-AE33-FBA3C575C993")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddGenericParameter("Environment", "Env", "Environment", GH_ParamAccess.item);
            pManager.AddIntegerParameter("detect radius", "detr", "Below this value, pairs of agents locations will be connected at a possibility.", GH_ParamAccess.item);
            pManager.AddNumberParameter("select possibility", "selp", "The possibility of a pair of agents locations are connected", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Connect Line", "CL", "Connect Line", GH_ParamAccess.list);
        }
        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref p)) return false;
            if (!da.GetData(1, ref env)) return false;
            if (!da.GetData(2, ref radius)) return false;
            if (!da.GetData(3, ref possib)) return false;
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
            List<Point3d> pos = new List<Point3d>();
            List<Line> connect = new List<Line>();
            Random rand = new Random(DateTime.Now.Millisecond);
            foreach (Amoeba amo in p.population)
            {
                List<Point3d> nei = env.findNeighborParticle(amo, radius);
                foreach (Point3d pt_nei in nei)
                {
                    if(pt_nei.X > amo.Location.X && rand.NextDouble() < possib)
                        connect.Add(new Line(pt_nei, amo.Location));
                }
            }
            da.SetDataList(0, connect);

        }

    }
}
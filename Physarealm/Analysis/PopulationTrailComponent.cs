using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using Grasshopper;

namespace Physarealm.Analysis
{
    public class PopulationTrailComponent :AbstractPopulationAnalysisComponent
    {
        private Physarum p;
        private DataTree<Point3d> trailTree;
        private Boolean reset;
        private int iter;
        private int history;
        private int maxid;
        /// <summary>
        /// Initializes a new instance of the PopulationTrailComponent class.
        /// </summary>
        public PopulationTrailComponent()
            : base("Population Trail", "popTrail",
                "This component gives trails of each agent.",
                null, "5CD2F008-2787-4235-8712-6839AEA5BB6D")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddIntegerParameter("History Step", "HisStp", "History steps recorded", GH_ParamAccess.item, 10);
            pManager.AddBooleanParameter("reset", "r", "If reset if false, this component start to record. Otherwise clear records. ", GH_ParamAccess.item, true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Trail Points", "TrPts", "Trail Points, use curve or polyline to connect these points", GH_ParamAccess.tree);
        }
        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref p)) return false;
            if (!da.GetData(1, ref history)) return false;
            if (!da.GetData(2, ref reset)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            da.SetDataTree(0, trailTree);
        }
        protected override void SolveInstance(IGH_DataAccess da)
        {
            if (!GetInputs(da)) return;
            if (reset == true)
            {
                trailTree = new DataTree<Point3d>();
                iter = 0;
                maxid = 0;
            }
            else 
            {
                foreach (Amoeba amo in p.population) 
                {
                    GH_Path thispath = new GH_Path(amo.ID);
                    if (amo.ID > maxid)
                    {
                        trailTree.Add(amo.prev_loc, thispath);
                        maxid = amo.ID;
                    }
                    trailTree.Add(amo.Location, thispath);
                    if (trailTree.Branch(thispath).Count > history) 
                    {
                        trailTree.Branch(thispath).RemoveAt(0);
                    }
                }
                foreach (int id in p._todie_id) 
                {
                    GH_Path thispath = new GH_Path(id);
                    trailTree.Branch(thispath).Clear();
                }
                iter++;
            }
            SetOutputs(da);
        }
    }
}
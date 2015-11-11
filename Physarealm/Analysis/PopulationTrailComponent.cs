using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Analysis
{
    public class PopulationTrailComponent :AbstractPopulationAnalysisComponent
    {
        /// <summary>
        /// Initializes a new instance of the PopulationTrailComponent class.
        /// </summary>
        public PopulationTrailComponent()
            : base("Population Trail", "popTrail",
                "Population Trail",
                null, "5CD2F008-2787-4235-8712-6839AEA5BB6D")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddIntegerParameter("History Step", "HisStp", "History Step", GH_ParamAccess.item, 10);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Trail Points", "TrPts", "Trail Points", GH_ParamAccess.tree);
        }
        protected override bool GetInputs(IGH_DataAccess da)
        {
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {

        }
    }
}
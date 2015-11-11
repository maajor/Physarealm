using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Analysis
{
    public class PopulationInterconnectComponent :AbstractPopulationAnalysisComponent
    {
        /// <summary>
        /// Initializes a new instance of the PopulationInterconnectComponent class.
        /// </summary>
        public PopulationInterconnectComponent()
            : base("Population Interconnect", "PopInter",
                "Description",
               null, "51167445-C5B7-42A4-AE33-FBA3C575C993")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Connect Line", "CL", "Connect Line", GH_ParamAccess.list);
            pManager.AddNumberParameter("Connect Weight", "CW", "Connect Weight", GH_ParamAccess.list);
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
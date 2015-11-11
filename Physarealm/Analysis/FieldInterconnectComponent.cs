using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Analysis
{
    public class FieldInterconnectComponent :AbstractFieldAnalysisComponent
    {
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
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Connect Line", "cL", "Connect Line", GH_ParamAccess.list);
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
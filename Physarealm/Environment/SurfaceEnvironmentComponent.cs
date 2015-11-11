using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Environment
{
    public class SurfaceEnvironmentComponent :AbstractEnvironmentComponent
    {
        /// <summary>
        /// Initializes a new instance of the SurfaceEnvironmentComponent class.
        /// </summary>
        public SurfaceEnvironmentComponent()
            : base("Surface Environment", "SurfEnv",
                "Description"
                , null, "68B8A101-5678-4406-AAE0-6D702C9930BA")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "srf", "Surface", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
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
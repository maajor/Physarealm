using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Emitter
{
    public class SurfaceEmitterComponent :AbstractEmitterComponent
    {
        /// <summary>
        /// Initializes a new instance of the SurfaceEmitterComponent class.
        /// </summary>
        public SurfaceEmitterComponent()
            : base("Surface Emitter", "SrfEmi",
                "Description",
                null, "47075063-D9D7-4F6E-9A0C-1173884F9376")
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
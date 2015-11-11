﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Food
{
    public class SurfaceFoodComponent :AbstractFoodComponent
    {
        /// <summary>
        /// Initializes a new instance of the SurfaceFoodComponent class.
        /// </summary>
        public SurfaceFoodComponent()
            : base("Surface Food", "SrfF",
                "Description",
                null, "413990CA-FCD6-4979-8A8A-A299811EB7B4")
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
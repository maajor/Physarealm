using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    public class PcdSettingComponent :AbstractSettingComponent
    {
        private double pcd;
        /// <summary>
        /// Initializes a new instance of the PcdSettingComponent class.
        /// </summary>
        public PcdSettingComponent()
            : base("Pcd Setting", "PcdSet",
                "Description",
                null, "C64A5382-EFB1-4AC4-A184-C0EAA351356A")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("PCD", "pcd", "Possibility Changing Direction, range: 0~1", GH_ParamAccess.item, 0.1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Pcd Setting", "PSet", "Pcd Setting", GH_ParamAccess.item);
        }

        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref pcd)) return false;
            return true;
        }

        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractSettingType pset = new PcdSettingType(pcd);
            da.SetData(0, pset);
        }
    }
}
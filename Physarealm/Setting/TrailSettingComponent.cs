using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    public class TrailSettingComponent :AbstractSettingComponent
    {
        private double trail_ratio;
        /// <summary>
        /// Initializes a new instance of the TrailSettingComponent class.
        /// </summary>
        public TrailSettingComponent()
            : base("Trail Setting", "TrSet",
                "Description",
                null, "4F7C1186-3185-42B3-881A-82B1D9E0590E")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Agent-Food Trail Ratio", "TrRat", "Ratio of deployed chemoattractor amounts, agent to food. As number", GH_ParamAccess.item, 0.2);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TrailSetting", "TSet", "TrailSetting", GH_ParamAccess.item);
        }

        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref trail_ratio)) return false;
            return true;
        }

        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractSettingType tset = new TrailSettingType(trail_ratio);
            da.SetData(0, tset);
        }
    }
}
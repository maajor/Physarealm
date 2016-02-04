using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    public class SpeedSettingComponent :AbstractSettingComponent
    {
        private double max_speed;
        /// <summary>
        /// Initializes a new instance of the SpeedSettingComponent class.
        /// </summary>
        public SpeedSettingComponent()
            : base("Speed Setting", "SpSet",
                "Description",
                null, "5C38034E-9302-4A39-B67D-BB7B1AE388AF")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Max Speed", "MS", "Agents' speed in index space. As number", GH_ParamAccess.item, 10);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SpeedSetting", "SSet", "Agent Max Speed Setting", GH_ParamAccess.item);
        }

        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref max_speed)) return false;
            return true;
        }

        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractSettingType speset = new SpeedSettingType((float)max_speed);
            da.SetData(0, speset);
        }
    }
}
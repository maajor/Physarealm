using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    public class AgentSettingComponent : AbstractSettingComponent
    {
        private double sensor_angle;
        private double rotate_angle;
        private double sensor_offset;
        private int detect_dir;
        private int death_distance;
        private double max_speed;
        private double depT;
        /// <summary>
        /// Initializes a new instance of the AgentSettingComponent class.
        /// </summary>
        public AgentSettingComponent()
            : base("Agent Setting", "AS",
                "AgentSetting",
                null, "D70A2A74-F96A-4000-871C-9748F314500A")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("sensor angle", "sa", "sensor angle", GH_ParamAccess.item, 22.5);
            pManager.AddNumberParameter("rotate angle", "ra", "rotate angle", GH_ParamAccess.item, 45);
            pManager.AddNumberParameter("sensor offset", "so", "sensor offset", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("detect direction", "ddir", "detect direction", GH_ParamAccess.item, 4);
            pManager.AddIntegerParameter("death distance", "ddis", "death distance", GH_ParamAccess.item, 100);
            pManager.AddNumberParameter("max speed", "ms", "max speed", GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("deploy trace once", "dept", "deploy trace once", GH_ParamAccess.item, 10);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AgtSetting", "AS", "Agent Setting", GH_ParamAccess.item);
        }
        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(nextInputIndex++, ref sensor_angle)) return false;
            if (!da.GetData(nextInputIndex++, ref rotate_angle)) return false;
            if (!da.GetData(nextInputIndex++, ref sensor_offset)) return false;
            if (!da.GetData(nextInputIndex++, ref detect_dir)) return false;
            if (!da.GetData(nextInputIndex++, ref death_distance)) return false;
            if (!da.GetData(nextInputIndex++, ref max_speed)) return false;
            if (!da.GetData(nextInputIndex++, ref depT)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            AgentSettingType agtset = new AgentSettingType((float)sensor_angle, (float)rotate_angle, (float)sensor_offset, detect_dir,death_distance, (float)max_speed, (float)depT);
            da.SetData(nextOutputIndex++, agtset);
        }
    }
}
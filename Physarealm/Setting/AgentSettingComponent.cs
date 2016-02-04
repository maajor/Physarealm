using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    abstract public class AgentSettingComponent : AbstractSettingComponent
    {
        private double sensor_angle;
        private double rotate_angle;
        private double sensor_offset;
        private int detect_dir_r;
        private int detect_dir_phy;
        private int death_distance;
        private double max_speed;
        private double depT;
        /// <summary>
        /// Initializes a new instance of the AgentSettingComponent class.
        /// </summary>
        public AgentSettingComponent()
            : base("Agent Setting", "ASet",
                "AgentSetting",
                null, "D70A2A74-F96A-4000-871C-9748F314500A")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Sensor Angle", "SA", "Agent sense a cone space with apenture equals 2*thisvalue. As number in degree, range 0~180", GH_ParamAccess.item, 22.5);
            pManager.AddNumberParameter("Sotate Angle", "RA", "Agent does not rotate to a sensed angle with max chemoattractor, it rotate that angle multiplies rotateangle/senseangle. As number in degree, range 0~180", GH_ParamAccess.item, 45);
            pManager.AddNumberParameter("Sensor Offset", "SO", "Agent's sensor's max range.(height of sense cone). As number", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("Detect Direction R", "DDirR", "Subdivide of sense points on perimeter of sensecone's bottom circle. As integer.", GH_ParamAccess.item, 4);
            pManager.AddIntegerParameter("Detect Direction Phy", "DDirP", "Subdivide of sense points on radius of sensecone's bottom circle. As integer.", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("Death Distance", "DDis", "Agents die after travel such steps", GH_ParamAccess.item, 100);
            pManager.AddNumberParameter("Max Speed", "MS", "Agents' speed in index space. As number.", GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("Deploy Trace Once", "DepT", "Agent deploys such amount of chemoattractor once step. As number.", GH_ParamAccess.item, 10);
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
            if (!da.GetData(nextInputIndex++, ref detect_dir_r)) return false;
            if (!da.GetData(nextInputIndex++, ref detect_dir_phy)) return false;
            if (!da.GetData(nextInputIndex++, ref death_distance)) return false;
            if (!da.GetData(nextInputIndex++, ref max_speed)) return false;
            if (!da.GetData(nextInputIndex++, ref depT)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            AgentSettingType agtset = new AgentSettingType((float)sensor_angle, (float)rotate_angle, (float)sensor_offset, detect_dir_r, detect_dir_phy,death_distance, (float)max_speed, (float)depT);
            da.SetData(nextOutputIndex++, agtset);
        }
    }
}
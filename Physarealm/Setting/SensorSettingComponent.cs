using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Properties;

namespace Physarealm.Setting
{
    public class SensorSettingComponent : AbstractSettingComponent
    {
        private double sensor_angle;
        private double rotate_angle;
        private double sensor_offset;
        /// <summary>
        /// Initializes a new instance of the SensorSettingComponent class.
        /// </summary>
        public SensorSettingComponent()
            : base("Sensor Setting", "SenSet",
                "Sensor Setting",
                Resources.icon_sensor_setting, "9CCAE233-00BA-416D-8501-542E6B843CE4")
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
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SensorSetting", "SSet", "Agent Sense Setting", GH_ParamAccess.item);
        }


        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref sensor_angle)) return false;
            if (!da.GetData(1, ref rotate_angle)) return false;
            if (!da.GetData(2, ref sensor_offset)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractSettingType agtset = new SensorSettingType((float)sensor_angle, (float)rotate_angle, (float)sensor_offset);
            da.SetData(0, agtset);
        }
    }
}
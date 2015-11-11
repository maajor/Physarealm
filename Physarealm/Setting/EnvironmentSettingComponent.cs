using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    public class EnvironmentSettingComponent : AbstractSettingComponent
    {
        private double pcd;
        private int div_radius;
        private int die_radius;
        private int div_max;
        private int div_min;
        private int die_min;
        private int die_max;
        private double guide_factor;
        /// <summary>
        /// Initializes a new instance of the DivAndDieSetting class.
        /// </summary>
        public EnvironmentSettingComponent()
            : base("Environment Setting", "EnvSetting",
                "Description",
                null, "2F3156E2-F636-43D2-B06F-00A7D495CA9D")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("possibility changing direction", "pcd", "possibility changing direction", GH_ParamAccess.item, 0.1);
            pManager.AddNumberParameter("verticle guide factor", "vgf", "verticle guide factor", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("division detect radius", "dvr", "division detect radius", GH_ParamAccess.item, 3);
            pManager.AddIntegerParameter("division min", "dvmin", "division if neighborhood agents count above", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("division max", "dvmax", "division if neighborhood agents count below", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("death detect radius", "der", "death detect radius", GH_ParamAccess.item, 2);
            pManager.AddIntegerParameter("death min", "demin", "death if neighborhood agents count below", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("death max", "demax", "death if neighborhood agents count above", GH_ParamAccess.item, 123);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("EnvSetting", "ES", "Environment Setting", GH_ParamAccess.item);
        }
        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref pcd)) return false;
            if (!da.GetData(1, ref guide_factor)) return false;
            if (!da.GetData(2, ref div_radius)) return false;
            if (!da.GetData(3, ref div_min)) return false;
            if (!da.GetData(4, ref div_max)) return false;
            if (!da.GetData(5, ref die_radius)) return false;
            if (!da.GetData(6, ref die_min)) return false;
            if (!da.GetData(7, ref die_max)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            EnvironmentSettingType env = new EnvironmentSettingType((float)pcd, div_radius, die_radius, div_max, div_min, die_max, die_min, guide_factor);
            da.SetData(0, env);
        }
    }
}
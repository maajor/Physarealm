using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Properties;

namespace Physarealm.Setting
{
    public class BirthDeathSettingComponent : AbstractSettingComponent
    {
        private int div_radius;
        private int die_radius;
        private int div_max;
        private int div_min;
        private int die_min;
        private int die_max;
        /// <summary>
        /// Initializes a new instance of the BirthDeathSettingComponent class.
        /// </summary>
        public BirthDeathSettingComponent()
            : base("Birth Death Setting", "BDSet",
                "Birth and death condition of agents. Program examine agents at present in a near radius. For example, for a brep environment, radius = 1 means program checks nearby 3*3*3 = 27 space with the examinied agent at center. Radius = 2 means 5*5*5 = 125. But for surface environment, radius = 1 means 3*3 = 9, etc.",
                Resources.icon_birth_death_setting, "02BFACA5-88DA-4351-9E0C-3B5EC4863CE9")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Division Detect Radius", "DVR", "Division detect radius. As integer", GH_ParamAccess.item, 2);
            pManager.AddIntegerParameter("Division Min", "DvMin", "Divide if neighborhood agents count above or equal. As integer.", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("Division Max", "DvMax", "Divide if neighborhood agents count below.  As integer, below (DVR*2 + 1)^3 for brep env and below (DVR*2 + 1)^2 for srf env.", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("Death Detect Radius", "DER", "Death detect radius", GH_ParamAccess.item, 3);
            pManager.AddIntegerParameter("Death Min", "DeMin", "Die if neighborhood agents count below or equal. As integer.", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("Death Max", "DeMax", "Die if neighborhood agents count above. As integer, below (DVR*2 + 1)^3 for brep env and below (DVR*2 + 1)^2 for srf env.", GH_ParamAccess.item, 123);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BDSetting", "BDS", "Birth Death Setting", GH_ParamAccess.item);
        }



        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref div_radius)) return false;
            if (!da.GetData(1, ref div_min)) return false;
            if (!da.GetData(2, ref div_max)) return false;
            if (!da.GetData(3, ref die_radius)) return false;
            if (!da.GetData(4, ref die_min)) return false;
            if (!da.GetData(5, ref die_max)) return false;
            return true;
        }

        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractSettingType env = new BirthDeathSettingType(div_radius, die_radius, div_max, div_min, die_max, die_min);
            da.SetData(0, env);
        }
    }
}
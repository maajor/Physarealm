using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

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
                "Description",
                null, "02BFACA5-88DA-4351-9E0C-3B5EC4863CE9")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("division detect radius", "dvr", "division detect radius", GH_ParamAccess.item, 2);
            pManager.AddIntegerParameter("division min", "dvmin", "division if neighborhood agents count above", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("division max", "dvmax", "division if neighborhood agents count below", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("death detect radius", "der", "death detect radius", GH_ParamAccess.item, 3);
            pManager.AddIntegerParameter("death min", "demin", "death if neighborhood agents count below", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("death max", "demax", "death if neighborhood agents count above", GH_ParamAccess.item, 123);
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
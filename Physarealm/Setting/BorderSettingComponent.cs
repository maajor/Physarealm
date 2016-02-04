using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    public class BorderSettingComponent : AbstractSettingComponent
    {
        private int bdtype;
        /// <summary>
        /// Initializes a new instance of the PcdSettingComponent class.
        /// </summary>
        public BorderSettingComponent()
            : base("Border Setting", "BdSet",
                "Setting for border type.",
                null, "5972853B-7C9C-4818-AFDE-DC7D5A2DD7E6")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //pManager.AddNumberParameter("PCD", "pcd", "Possibility Changing Direction, range: 0~1", GH_ParamAccess.item, 0.1);
            pManager.AddIntegerParameter("Border Type", "BdType", "Border type, 0 for normal, 1 for wrap, 2 for border bounce", GH_ParamAccess.item, 0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BorderTypeSetting", "BTSet", "BorderTypeSetting", GH_ParamAccess.item);
        }

        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref bdtype)) return false;
            return true;
        }

        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractSettingType bset = new BorderSettingType(bdtype);
            da.SetData(0, bset);
        }
    }
}
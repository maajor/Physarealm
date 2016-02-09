using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Properties;

namespace Physarealm.Setting
{
    public class DetectDirectionSettingComponent :AbstractSettingComponent
    {
        private int det_dir_r;
        private int det_dir_phy;
        /// <summary>
        /// Initializes a new instance of the DetectDirectionSettingComponent class.
        /// </summary>
        public DetectDirectionSettingComponent()
            : base("Detect Direction Setting", "DDirSets",
                "Detect Direction Setting",
                Resources.icon_detect_direction_setting, "D865B6E0-E0F3-4BB6-A00C-9DAA72A000E0")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Detect Direction R", "DDirR", "Subdivide of sense points on perimeter of sensecone's bottom circle. As integer.", GH_ParamAccess.item, 4);
            pManager.AddIntegerParameter("Detect Direction Phy", "DDirP", "Subdivide of sense points on radius of sensecone's bottom circle. As integer.", GH_ParamAccess.item, 1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Detect Direction Setting", "DDSet", "Detect Direction Setting", GH_ParamAccess.item); 
        }



        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref det_dir_r)) return false;
            if (!da.GetData(1, ref det_dir_phy)) return false;
            return true;
        }

        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractSettingType ddset = new DetectDirectionSettingType(det_dir_r, det_dir_phy);
            da.SetData(0, ddset);
        }
    }
}
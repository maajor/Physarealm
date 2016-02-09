using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Properties;

namespace Physarealm.Setting
{
    public class EscapeSettingComponent :AbstractSettingComponent
    {
        private double esc_p;
        /// <summary>
        /// Initializes a new instance of the EscapeSettingComponent class.
        /// </summary>
        public EscapeSettingComponent()
            : base("Escape Setting", "EscSet",
                "Escape Setting",
                Resources.icon_escape_setting, "84457914-DE3F-4D5D-B435-D5950525C142")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Escape Possibility", "EscP", "Possibility agent will escape the constraints or obstacles.", GH_ParamAccess.item, 0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("EscSetting", "ES", "Escape Setting", GH_ParamAccess.item);
        }


        protected override bool GetInputs(IGH_DataAccess da)
        {
            esc_p = 0;
            if(! da.GetData(0, ref esc_p)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {

            AbstractSettingType esset = new EscapeSettingType(esc_p);
            da.SetData(0, esset);
        }
    }
}
using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    public class InitialVelocitySettingComponent : AbstractSettingComponent
    {
        private Vector3d ori;
        /// <summary>
        /// Initializes a new instance of the SpeedSettingComponent class.
        /// </summary>
        public InitialVelocitySettingComponent()
            : base("Initial Velocity Setting", "IniOSet",
                "Initial velocity of agents.",
                null, "727459EA-B6C2-4576-B222-21893324D2D9")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddVectorParameter("Initial Velocity", "IniVel", "Initial Velocity as vector", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("InitialVelocitySetting", "IVSet", "Agent initial velocity Setting", GH_ParamAccess.item);
        }

        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref ori)) return false;
            return true;
        }

        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractSettingType speset = new InitialVelocitySettingType(ori);
            da.SetData(0, speset);
        }
    }
}
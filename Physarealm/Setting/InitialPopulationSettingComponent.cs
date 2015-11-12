using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    public class InitialPopulationSettingComponent :AbstractSettingComponent
    {
        private int popsize;
        /// <summary>
        /// Initializes a new instance of the InitialPopulationSettingComponent class.
        /// </summary>
        public InitialPopulationSettingComponent()
            : base("Initial Population Setting", "InPSet",
                "Description",
                null, "2B7F549B-E39E-4E8F-8976-5A60EFB340AB")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Population Size", "PopS", "Initial Population Size", GH_ParamAccess.item, 200);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PopuSetting", "PopSet", "Population Setting", GH_ParamAccess.item);
        }

        protected override bool GetInputs(IGH_DataAccess da)
        {
            if(!da.GetData(0, ref popsize)) return false;
            return true;
        }

        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractSettingType popset = new InitialPopulationSettingType(popsize);
            da.SetData(0, popset);
        }
    }
}
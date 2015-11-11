using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Analysis
{
    public abstract class AbstractPopulationAnalysisComponent :AbstractComponent
    {
        /// <summary>
        /// Initializes a new instance of the AbstractPopulationAnalysisComponent class.
        /// </summary>
        public AbstractPopulationAnalysisComponent(string name, string nickname, string description,
                                    Bitmap icon, string componentGuid)
            : base(name, nickname,
                description,
                "Physarealm", "Analysis", icon, componentGuid)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Population", "P", "Population", GH_ParamAccess.item);
        }
        protected abstract override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager);
        protected abstract override void SetOutputs(IGH_DataAccess da);
        protected abstract override bool GetInputs(IGH_DataAccess da);

    }
}
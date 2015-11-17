using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Environment;

namespace Physarealm.Analysis
{
    public abstract class AbstractFieldAnalysisComponent :AbstractComponent
    {
        protected AbstractEnvironmentType env;
        /// <summary>
        /// Initializes a new instance of the AbstractFieldAnalysisComponent class.
        /// </summary>
        public AbstractFieldAnalysisComponent(string name, string nickname, string description,
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
            pManager.AddGenericParameter("Field", "F", "Field", GH_ParamAccess.item);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Environment
{
    public abstract class AbstractEnvironmentComponent : AbstractComponent
    {
        /// <summary>
        /// Initializes a new instance of the AbstractEnvironmentComponent class.
        /// </summary>
        protected AbstractEnvironmentComponent(string name, string nickname, string description,
                                    Bitmap icon, string componentGuid)
            : base(name, nickname,
                description,
                "Physarealm", "Environment", icon, componentGuid)
        {
        }


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("environment", "env", "environment description", GH_ParamAccess.item);
        }

    }
}
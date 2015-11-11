using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Emitter
{
    public abstract class AbstractEmitterComponent : AbstractComponent
    {
        /// <summary>
        /// Initializes a new instance of the AbstractEmitterComponent class.
        /// </summary>
        protected AbstractEmitterComponent(string name, string nickname, string description,
                                    Bitmap icon, string componentGuid)
            : base(name, nickname,
                description,
                "Physarealm", "Emitter", icon, componentGuid)
        {
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Emitter", "E", "agents emitter", GH_ParamAccess.item);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Food
{
    public abstract class AbstractFoodComponent : AbstractComponent
    {
        /// <summary>
        /// Initializes a new instance of the AbstractFoodComponent class.
        /// </summary>
        public AbstractFoodComponent(string name, string nickname, string description,
                                    Bitmap icon, string componentGuid)
            : base(name, nickname,
                description,
                "Physarealm", "Food", icon, componentGuid)
        {
        }


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Food", "F", "Food for agents to search", GH_ParamAccess.item);
        }


    }
}
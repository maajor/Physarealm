using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    public class BirthDeathSettingComponent : AbstractSettingComponent
    {
        /// <summary>
        /// Initializes a new instance of the BirthDeathSettingComponent class.
        /// </summary>
        public BirthDeathSettingComponent()
            : base("Birth Death Setting", "BDSet",
                "Description",
                null, "02BFACA5-88DA-4351-9E0C-3B5EC4863CE9")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }
    }
}
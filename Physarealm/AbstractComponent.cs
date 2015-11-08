using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm
{
    public abstract class AbstractComponent : GH_Component
    {
        private readonly Bitmap icon;
        private readonly Guid componentGuid;

        protected int nextInputIndex, nextOutputIndex;
        /// <summary>
        /// Initializes a new instance of the AbstractComponent class.
        /// </summary>
        protected AbstractComponent(string name, string nickname, string description,
                                    string category, string subcategory, Bitmap icon,
                                    string componentGuid)
            : base(name, nickname, description, category, subcategory)
        {
            this.icon = icon;
            this.componentGuid = new Guid("{" + componentGuid + "}");
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected abstract override void RegisterInputParams(GH_InputParamManager pManager);

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected abstract override void RegisterOutputParams(GH_OutputParamManager pManager);

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess da)
        {
            nextInputIndex = nextOutputIndex = 0;
            if (!GetInputs(da)) return;
            SetOutputs(da);
        }
        /// <summary>
        /// Gets the data from da and checks that the input data is valid.
        /// </summary>
        /// <param name="da">The object that gives access to inputs and outputs.</param>
        /// <returns>true iff the all the inputs are supplied and valid.</returns>
        protected abstract bool GetInputs(IGH_DataAccess da);

        /// <summary>
        /// Solves for and sets the output data based on the inputs.
        /// </summary>
        /// <param name="da">The object that gives access to inputs and outputs.</param>
        protected abstract void SetOutputs(IGH_DataAccess da);
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return icon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return componentGuid; }
        }
    }
}
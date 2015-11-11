using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Emitter;
using Physarealm.Environment;
using Physarealm.Food;
using Physarealm.Setting;

namespace Physarealm
{
    public class PhysarealmComponent : GH_Component
    {
        private Physarum popu;
        private AbstractEnvironmentType env;
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PhysarealmComponent()
            : base("Physarealm", "PRealm",
                "agent-based modelling of physarum polycephalum",
                "Physarealm", "Core")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Environment", "Env", "Environment", GH_ParamAccess.item);
            pManager.AddGenericParameter("Emitter", "Emi", "Emitter", GH_ParamAccess.item);
            pManager.AddGenericParameter("Food", "F", "Food", GH_ParamAccess.item);
            pManager.AddGenericParameter("Environment Setting", "EnvS", "Environment Setting", GH_ParamAccess.item);
            pManager.AddGenericParameter("Agent Setting", "AgtS", "Agent Setting", GH_ParamAccess.item);
            pManager.AddBooleanParameter("reset", "r", "reset", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ChemoAttractor Field", "CAF", "ChemoAttractor Field", GH_ParamAccess.item);
            pManager.AddGenericParameter("Population", "pop", "Population", GH_ParamAccess.item);
            pManager.AddIntegerParameter("iteration", "i", "Iteration", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{00e8eaa0-aad5-4f38-bb1a-7ca5ef1581c4}"); }
        }
    }
}

using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Environment;
namespace Physarealm.Analysis
{
    public class FieldPointComponent : AbstractFieldAnalysisComponent
    {
        //private List<float> value;
        /// <summary>
        /// Initializes a new instance of the FieldValueComponent class.
        /// </summary>
        public FieldPointComponent()
            : base("Field Point", "FPt",
                "Description",
                null, "8617148B-F5C6-40C7-94CD-E16A0C0FB3FD")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Pts", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref env)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            da.SetDataList(0, env.getPoints());
        }
        protected override void SolveInstance(IGH_DataAccess da)
        {
            if (!GetInputs(da)) return;
            //value = new List<float>();
            //value = env.getTrailV();

            SetOutputs(da);
        }

    }
}
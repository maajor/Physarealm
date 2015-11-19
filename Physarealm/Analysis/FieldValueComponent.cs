using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Environment;
namespace Physarealm.Analysis
{
    public class FieldValueComponent :AbstractFieldAnalysisComponent
    {
        private List<float> value;
        /// <summary>
        /// Initializes a new instance of the FieldValueComponent class.
        /// </summary>
        public FieldValueComponent()
            : base("Field Value", "FV",
                "Description",
                null, "7963C564-60CC-4E2D-94F5-D48D8CBBD428")
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
            pManager.AddNumberParameter("Values", "V", "Values", GH_ParamAccess.list);
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
            da.SetDataList(0, env.getTrailV());
        }
        protected override void SolveInstance(IGH_DataAccess da)
        {
            if (!GetInputs(da)) return;
            value = new List<float>();
            //value = env.getTrailV();

            SetOutputs(da);
        }

    }
}
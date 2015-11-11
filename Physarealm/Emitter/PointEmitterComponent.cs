using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Emitter
{
    public class PointEmitterComponent :AbstractEmitterComponent
    {
        private List<Point3d> emit;
        /// <summary>
        /// Initializes a new instance of the PointEmitterComponent class.
        /// </summary>
        public PointEmitterComponent()
            : base("PointEmitter", "PtEmi",
                "Description",
                null, "05CBA783-3A74-4253-B8C3-B894D9715A01")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Pts", "points emitters", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }
        protected override bool GetInputs(IGH_DataAccess da)
        {
            if(!da.GetData(nextInputIndex++, ref emit)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractEmitterType emitter = new PointEmitterType(emit);
            da.SetData(nextOutputIndex++, emitter);
        }
    }
}
using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Emitter
{
    public class PointEmitterComponent :AbstractEmitterComponent
    {
        private List<Point3d> emit = new List<Point3d>();
        //private Point3d pt;
        /// <summary>
        /// Initializes a new instance of the PointEmitterComponent class.
        /// </summary>
        public PointEmitterComponent()
            : base("Point Emitter", "PtEmi",
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

        protected override bool GetInputs(IGH_DataAccess da)
        {
            emit = new List<Point3d>();
            if(!da.GetDataList(nextInputIndex++, emit)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractEmitterType emitter = new PointEmitterType(emit);
            da.SetData(nextOutputIndex++, emitter);
        }
    }
}
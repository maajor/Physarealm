using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Emitter
{
    public class CurveEmitterComponent :AbstractEmitterComponent
    {
        private List<Curve> curves;
        /// <summary>
        /// Initializes a new instance of the CurveEmitterComponent class.
        /// </summary>
        public CurveEmitterComponent()
            : base("Curve Emitter", "CrvEmi",
                "Description",
                null, "63B3C8C7-B14B-4D7A-9BC3-FBF408F0D8ED")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Crv", "curve emitter", GH_ParamAccess.list);
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
            curves = new List<Curve>();
            if (!da.GetDataList(nextInputIndex++, curves)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            CurveEmitterType crvemit = new CurveEmitterType(curves);
            da.SetData(nextOutputIndex++, crvemit);
            return;
        }

    }
}
using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Food
{
    public class PointFoodComponent :AbstractFoodComponent
    {
        private List<Point3d> pts;
        /// <summary>
        /// Initializes a new instance of the PointFoodComponent class.
        /// </summary>
        public PointFoodComponent()
            : base("PointFoodComponent", "Nickname",
                "Description"
                , null, "A365E1B3-4149-4C20-B6B2-47A356D6D7D1")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Pts", "Point food", GH_ParamAccess.list);
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
            if (!da.GetData(nextInputIndex++, ref pts)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractFoodType food = new PointFoodType(pts);
            da.SetData(nextOutputIndex++, food);
        }
    }
}
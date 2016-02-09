using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Properties;

namespace Physarealm.Analysis
{
    public class PopulationVelocityComponent :AbstractPopulationAnalysisComponent
    {
        private List<Vector3d> vel;
        private Physarum p;
        /// <summary>
        /// Initializes a new instance of the PopulationPositionComponent class.
        /// </summary>
        public PopulationVelocityComponent()
            : base("Population Velocity", "PopVel",
                "This component give velocity of each agents.",
                Resources.icon_population_velocity, "04895848-99AD-46AD-93D2-0B64E5382ACF")
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
            pManager.AddVectorParameter("Velocities", "Vel", "Velocities as Vector3d", GH_ParamAccess.list);
        }

        protected override bool GetInputs(IGH_DataAccess da)
        {
            if(!da.GetData(0, ref p)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            da.SetDataList(0, vel);
        }
        protected override void SolveInstance(IGH_DataAccess da)
        {
            if (!GetInputs(da)) return;
            vel = new List<Vector3d>();
            foreach (Amoeba amo in p.population)
                vel.Add(amo.orientation);

            SetOutputs(da);
        }
    }
}
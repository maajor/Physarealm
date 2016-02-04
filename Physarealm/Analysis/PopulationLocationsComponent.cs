using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Analysis
{
    public class PopulationLocationsComponent : AbstractPopulationAnalysisComponent
    {
        private List<Point3d> pos = new List<Point3d>();
        private Physarum p;
        /// <summary>
        /// Initializes a new instance of the PopulationPositionComponent class.
        /// </summary>
        public PopulationLocationsComponent()
            : base("Population Locations", "PopLoc",
                "This component gives all agents locations in population.",
                null, "25EBEFEA-5C51-448F-8D8B-222D424ED2BD")
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
            pManager.AddPointParameter("Locations", "Loc", "Agent Locations", GH_ParamAccess.list);
        }

        protected override bool GetInputs(IGH_DataAccess da)
        {
            if(!da.GetData(0, ref p)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            da.SetDataList(0, pos);
        }
        protected override void SolveInstance(IGH_DataAccess da)
        {
            if (!GetInputs(da)) return;
            pos.Clear();
            foreach (Amoeba amo in p.population)
                pos.Add(amo.Location);

            SetOutputs(da);
        }
    }
}
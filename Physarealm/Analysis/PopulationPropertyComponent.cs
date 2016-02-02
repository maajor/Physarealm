using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Analysis
{
    public abstract class PopulationPropertyComponent : AbstractPopulationAnalysisComponent
    {
        private List<double> prop;
        private Physarum p;
        /// <summary>
        /// Initializes a new instance of the PopulationPositionComponent class.
        /// </summary>
        public PopulationPropertyComponent()
            : base("Population Property", "PopProp",
                "Population Property",
                null, "DC5CFD04-7FA2-4946-9790-FE50968085FB")
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
            pManager.AddNumberParameter("Property", "Prop", "Property", GH_ParamAccess.list);
        }

        protected override bool GetInputs(IGH_DataAccess da)
        {
            if(!da.GetData(0, ref p)) return false;
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            da.SetDataList(0, prop);
        }
        protected override void SolveInstance(IGH_DataAccess da)
        {
            if (!GetInputs(da)) return;
            prop = new List<double>();
            foreach (Amoeba amo in p.population)
                prop.Add(amo.tempValue);

            SetOutputs(da);
        }
    }
}
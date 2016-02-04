using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Physarealm.Analysis
{
    public class PopulationPropertyComponent : AbstractPopulationAnalysisComponent
    {
        private DataTree<double> prop;
        private Physarum p;
        /// <summary>
        /// Initializes a new instance of the PopulationPositionComponent class.
        /// </summary>
        private PopulationPropertyComponent()//this component is purely for debug, I diable it in a release version. 
            : base("Population Property", "PopProp",
                "",
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
            da.SetDataTree(0, prop);
        }
        protected override void SolveInstance(IGH_DataAccess da)
        {
            if (!GetInputs(da)) return;
            prop = new DataTree<double>();
            int index = 0;
            foreach (Amoeba amo in p.population) 
            { 
                prop.Add(amo.tempValue,new GH_Path(index));
                prop.Add(PhysaSetting.gmin, new GH_Path(index));
                prop.Add(PhysaSetting.gmax, new GH_Path(index));
                index++;
            }

            SetOutputs(da);
        }
    }
}
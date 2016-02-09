using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Properties;

namespace Physarealm.Analysis
{
    public class FieldAnalysisMeshComponent :AbstractFieldAnalysisComponent
    {
        private double z;
        /// <summary>
        /// Initializes a new instance of the FieldAnalysisMeshComponent class.
        /// </summary>
        public FieldAnalysisMeshComponent()
            : base("Field Analysis Mesh", "FAMesh",
                "when this component applies to brep or box environment, it display a pseudo-color mesh illustrating chemoattrator(co) value field density. Higher density is reprented by red while lower density is black.",
                Resources.icon_field_analysis_mesh, "E1068216-44E9-4A4A-861E-E978A60378B1")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddNumberParameter("Z", "z", "Z coordinate of this analysis mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Analysis Mesh", "AMesh", "Analysis Mesh", GH_ParamAccess.item);
        }
        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref env)) return false;
            if (!da.GetData(1, ref z)) return false;
            if (z < env.getWMin())
                z = env.getWMin();
            else if (z > env.getWMax())
                z = env.getWMax();
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            Mesh mesh = env.getTrailEvaMesh(z);
            da.SetData(0, mesh);
        }
    }
}
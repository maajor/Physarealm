using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Environment
{
    public class BoxEnvironmentComponent : AbstractEnvironmentComponent
    {
        private int x_count;
        private int y_count;
        private int z_count;
        private Box box;
        private List<Brep> obs = new List<Brep>();
        /// <summary>
        /// Initializes a new instance of the BoxEnvironmentComponent class.
        /// </summary>
        public BoxEnvironmentComponent()
            : base("Box Environment", "BoxEnvir",
                "A box environment, agents run on uv-space", null
                , "CBE2B1BA-1C6E-473E-8D4A-F724290A9BED")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("XResolution", "xres", "Subdivision on x-axis", GH_ParamAccess.item, 100);
            pManager.AddIntegerParameter("YResolution", "yres", "Subdivision on y-axis", GH_ParamAccess.item, 100);
            pManager.AddIntegerParameter("ZResolution", "zres", "Subdivision on z-axis", GH_ParamAccess.item, 100);
            pManager.AddBoxParameter("Box", "B", "Box", GH_ParamAccess.item);
            pManager.AddBrepParameter("Obstacles", "Obs", "A list of breps represent obstacles", GH_ParamAccess.list);
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
            //pManager.AddGenericParameter("BoxEnvironment", "BoxEnv", "Box Environment", GH_ParamAccess.item);
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>

        protected override bool GetInputs(IGH_DataAccess da)
        {
            obs = new List<Brep>();
            if (!da.GetData(nextInputIndex++, ref x_count)) return false;
            if (!da.GetData(nextInputIndex++, ref y_count)) return false;
            if (!da.GetData(nextInputIndex++, ref z_count)) return false;
            if (!da.GetData(nextInputIndex++, ref box)) return false;
            da.GetDataList(nextInputIndex++, obs);
            return true;
        }

        protected override void SetOutputs(IGH_DataAccess da)
        {
            BoxEnvironmentType environment = new BoxEnvironmentType(box.BoundingBox, x_count, y_count, z_count);
            environment.setContainer();
            if (obs != null)
                environment.setObstacles(obs);
            //environment.setContainer(null);
            da.SetData(nextOutputIndex++, environment);
        }
    }
}
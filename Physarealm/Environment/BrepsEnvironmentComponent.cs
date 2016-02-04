using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Environment
{
    public class BrepsEnvironmentComponent : AbstractEnvironmentComponent
    {
        private int x_count;
        private int y_count;
        private int z_count;
        private List<Brep> breps = new List<Brep>();
        private List<Brep> obs = new List<Brep>();
        //private int[,,] pos;
        /// <summary>
        /// Initializes a new instance of the BrepEnvironmentComponent class.
        /// </summary>
        public BrepsEnvironmentComponent()
            : base("Brep Environment", "BrepEnvir",
                "A brep environment, agents run on uv-space",
                null, "5BA44E57-DDE9-44B9-A990-9CD35445AB3F")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("XResolution", "xres", "Subdivision on x-axis", GH_ParamAccess.item, 100);
            pManager.AddIntegerParameter("YResolution", "yres", "Subdivision on y-axis", GH_ParamAccess.item, 100);
            pManager.AddIntegerParameter("ZResolution", "zres", "Subdivision on z-axis", GH_ParamAccess.item, 100);
            pManager.AddBrepParameter("Brep", "B", "Brep", GH_ParamAccess.list);
            pManager.AddBrepParameter("Obstacles", "Obs", "A list of breps represent obstacles", GH_ParamAccess.list);
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
            //pManager.AddIntegerParameter("pts", "pts", "pts", GH_ParamAccess.list);
            //pManager.AddGenericParameter("BoxEnvironment", "BoxEnv", "Box Environment", GH_ParamAccess.item);
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>

        protected override bool GetInputs(IGH_DataAccess da)
        {
            breps = new List<Brep>();
            obs = new List<Brep>();
            if (!da.GetData(nextInputIndex++, ref x_count)) return false;
            if (!da.GetData(nextInputIndex++, ref y_count)) return false;
            if (!da.GetData(nextInputIndex++, ref z_count)) return false;
            if (!da.GetDataList(nextInputIndex++, breps)) return false;
            da.GetDataList(4, obs);
            return true;
        }

        protected override void SetOutputs(IGH_DataAccess da)
        {
            BoundingBox box = breps[0].GetBoundingBox(Plane.WorldXY);
            foreach (Brep brep in breps)
            {
                BoundingBox thisbox = brep.GetBoundingBox(Plane.WorldXY);
                box.Union(thisbox);
            }
            BoxEnvironmentType environment = new BoxEnvironmentType(box, x_count, y_count, z_count);
            environment.setContainer(breps);
            //pos = environment.griddata;
            if (obs != null)
                environment.setObstacles(obs);
            //environment.setContainer(null);
            da.SetData(nextOutputIndex++, environment);
            //da.SetDataList(1, pos);
        }
    }
}
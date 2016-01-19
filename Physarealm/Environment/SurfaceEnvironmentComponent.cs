using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Environment
{
    public class SurfaceEnvironmentComponent :AbstractEnvironmentComponent
    {
        private int u_count;
        private int v_count;
        private Surface srf;
        private List<Brep> obs;
        private List<Brep> cont;
        /// <summary>
        /// Initializes a new instance of the SurfaceEnvironmentComponent class.
        /// </summary>
        public SurfaceEnvironmentComponent()
            : base("Surface Environment", "SurfEnv",
                "Description"
                , null, "68B8A101-5678-4406-AAE0-6D702C9930BA")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("UResolution", "ures", "subdivision on u-axis", GH_ParamAccess.item, 100);
            pManager.AddIntegerParameter("VResolution", "vres", "subdivision on v-axis", GH_ParamAccess.item, 100);
            pManager.AddSurfaceParameter("Surface", "srf", "Surface", GH_ParamAccess.item);
            pManager.AddBrepParameter("Constraint", "Cons", "Constraint for a list of breps", GH_ParamAccess.list);
            pManager.AddBrepParameter("Obstacles", "Obs", "Obstacles for a list of breps", GH_ParamAccess.list);
            pManager[3].Optional = true;
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            base.RegisterOutputParams(pManager);
            //pManager.AddPointParameter("g", "g", "g", GH_ParamAccess.list);

        }
        protected override bool GetInputs(IGH_DataAccess da)
        {
            obs = new List<Brep>();
            cont = new List<Brep>();
            if (!da.GetData(0, ref u_count)) return false;
            if (!da.GetData(1, ref v_count)) return false;
            if (!da.GetData(2, ref srf)) return false;
            da.GetDataList(3, cont);
            da.GetDataList(4, obs);
            return true;
        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            SurfaceEnvironmentType environment = new SurfaceEnvironmentType(srf, u_count, v_count, 1);
            //if (obs != null)
            //    environment.setContainer(cont);
            //else
            environment.setContainer();
            if (obs != null)
                environment.setObstacles(obs);
            da.SetData(0, environment);
            //da.SetDataList(1, environment.getPosition());
        }
    }
}
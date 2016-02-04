using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    public class DeathDistanceSettingComponent :AbstractSettingComponent
    {
        private int death_distance;
        /// <summary>
        /// Initializes a new instance of the DeathDistanceSettingComponent class.
        /// </summary>
        public DeathDistanceSettingComponent()
            : base("Death Distance Setting", "DDSet",
                "Death Distance Setting",
                null, "44522DCA-970C-4693-9C99-B348533BD9F8")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Death Distance", "DDis", "An positive interger as input represent death distance. As interger.", GH_ParamAccess.item, 100);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Death Distance Setting", "DDisSet", "Agents die after travel such steps", GH_ParamAccess.item);
        }



        protected override bool GetInputs(IGH_DataAccess da)
        {
            if(! da.GetData(0, ref death_distance)) return false;
            return true;
        }

        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractSettingType ddset = new DeathDistanceSettingType(death_distance);
            da.SetData(0, ddset);
        }
    }
}
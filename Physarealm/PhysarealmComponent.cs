using System;
using System.Collections.Generic;
using System.Reflection;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Emitter;
using Physarealm.Environment;
using Physarealm.Food;
using Physarealm.Setting;
using Physarealm.Properties;

namespace Physarealm
{
    public class PhysarealmComponent :AbstractComponent
    {
        private Physarum popu = new Physarum();
        private AbstractEnvironmentType env;
        private List<AbstractSettingType> setList = new List<AbstractSettingType>();
        private AbstractFoodType food;
        private AbstractEmitterType emit;
        private Boolean rst;
        private int iter;
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PhysarealmComponent()
            : base("Physarealm", "PRealm",
                "agent-based modelling of physarum polycephalum",
                "Physarealm", "Core", Resources.icon_core, "94F0DDD9-40C6-4F07-B6A8-BB33A6B5C978")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Environment", "Env", "Environment", GH_ParamAccess.item);
            pManager.AddGenericParameter("Emitter", "Emi", "Emitter", GH_ParamAccess.item);
            pManager.AddGenericParameter("Food", "F", "Food", GH_ParamAccess.item);
            pManager.AddGenericParameter("Settings", "Set", "Settings", GH_ParamAccess.list);
            pManager.AddBooleanParameter("reset", "r", "reset", GH_ParamAccess.item, true);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ChemoAttractor Field", "CAF", "ChemoAttractor Field", GH_ParamAccess.item);
            pManager.AddGenericParameter("Population", "pop", "Population", GH_ParamAccess.item);
            pManager.AddIntegerParameter("iteration", "i", "Iteration", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!GetInputs(DA)) return;

            if (rst == true)
            {
                iter = 0;
                popu.Clear();
                env.Reset();
                popu.initParameters();
                //env.setBirthPlace(emit.getEmitPts());
                env.emitter = emit;
                env.setFood(food.getFoodPts(env.getEnvAccu()));
                foreach (AbstractSettingType sett in setList)
                    sett.setParameter(popu);
                popu.initPopulation(env);
            }

            else
            {
                popu.Update(env);
                iter++;
            }

            SetOutputs(DA);
        }
        protected override bool GetInputs(IGH_DataAccess da)
        {
            if(! da.GetData(0, ref env)) return false;
            if (!da.GetData(1, ref emit)) return false;
            if (!da.GetData(2, ref food)) return false;
            da.GetDataList(3, setList);
            if (!da.GetData(4, ref rst)) return false;
            return true;

        }
        protected override void SetOutputs(IGH_DataAccess da)
        {
            da.SetData(0, env);
            da.SetData(1, popu);
            da.SetData(2, iter);
        }
        public override string Description
        {
            get
            {
                return base.Description;
            }
            set
            {
                base.Description = value;
            }
        }
    }
}

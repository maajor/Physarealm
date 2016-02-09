using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Physarealm.Properties;

namespace Physarealm.Setting
{
    public class TestFrequencySettingComponent :AbstractSettingComponent
    {
        private int div_freq;
        private int die_freq;
        /// <summary>
        /// Initializes a new instance of the TestFrequencySettingComponent class.
        /// </summary>
        public TestFrequencySettingComponent()
            : base("Test Frequency Setting", "TFSet",
                "Test Frequency Setting",
                Resources.icon_test_frequency_setting, "D09089B0-9926-440D-86F4-F8E4D2AC07C7")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("DivisionTestFreq", "DiTFr", "Frequency to test division, as integer.", GH_ParamAccess.item, 3);
            pManager.AddIntegerParameter("DeathTestFreq", "DeTFr", "Frequency to test death, as integer", GH_ParamAccess.item, 3);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TestFrequencySetting", "TFSet", "TestFrequencySetting", GH_ParamAccess.item);
        }


        protected override bool GetInputs(IGH_DataAccess da)
        {
            if (!da.GetData(0, ref div_freq)) return false;
            if (!da.GetData(1, ref die_freq)) return false;
            return true;
        }

        protected override void SetOutputs(IGH_DataAccess da)
        {
            AbstractSettingType tfset = new TestFrequencySettingType(div_freq, die_freq);
            da.SetData(0, tfset);
        }
    }
}
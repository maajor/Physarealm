using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class TestFrequencySettingType : AbstractSettingType
    {
        private int div_freq;
        private int die_freq;
        public TestFrequencySettingType(int divf, int dief) 
        {
            div_freq = divf > 1 ? divf : 1;
            die_freq = dief > 1 ? dief : 1;
        }
        public TestFrequencySettingType(TestFrequencySettingType t) : this(t.div_freq, t.die_freq) { }
        public override void setParameter(Physarum p)
        {
            PhysaSetting._division_frequency_test = div_freq;
            PhysaSetting._death_frequency_test = die_freq;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new TestFrequencySettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.TestFrequencySettingType";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "Physarealm.TestFrequencySettingType";
            }
        }
        public override string ToString()
        {
            return TypeName + "\ndeath test frequency: " + die_freq
                + "\ndivision test frequency: " + div_freq;
        }
    }
}

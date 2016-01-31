using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class GuideSettingType:AbstractSettingType
    {
        private double guide_factor;
        private bool both_direction;
        public GuideSettingType(double gf, bool bdir) { guide_factor = gf > 0 ? gf : 0; both_direction = bdir; }
        public GuideSettingType(GuideSettingType g) : this(g.guide_factor, g.both_direction) { }
        public override void setParameter(Physarum p)
        {
            PhysaSetting.guide_factor = guide_factor;
            PhysaSetting.both_dir_flag = both_direction;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new GuideSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.GuideSettingType";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "Physarealm.GuideSettingType";
            }
        }
        public override string ToString()
        {
            return TypeName + "\nguide factor: " + guide_factor;
        }
    }
}

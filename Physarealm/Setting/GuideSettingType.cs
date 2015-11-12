using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class GuideSettingType:AbstractSettingType
    {
        private double guide_factor;
        public GuideSettingType(double gf) { guide_factor = gf > 0 ? gf : 0; }
        public GuideSettingType(GuideSettingType g) : this(g.guide_factor) { }
        public override void setParameter(Physarum p)
        {
            p.guide_factor = guide_factor;
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

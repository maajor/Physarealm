using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class TrailSettingType : AbstractSettingType
    {
        private double trail_ratio;
        public TrailSettingType(double rat) 
        {
            trail_ratio = rat > 0.01 ? rat : 0.01;
        }
        public TrailSettingType(TrailSettingType t) : this(t.trail_ratio) { }
        public override void setParameter(Physarum p)
        {
            PhysaSetting._depT = (float)(5 * trail_ratio);
            

        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new TrailSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.TrailSettingType";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "Physarealm.TrailSettingType";
            }
        }
        public override string ToString()
        {
            return TypeName + "\ntrail ratio: " + trail_ratio;
        }
    }
}

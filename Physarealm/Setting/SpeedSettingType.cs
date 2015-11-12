using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class SpeedSettingType:AbstractSettingType
    {
        private double speed;
        public SpeedSettingType(double s) { speed = s > 1 ? s : 1; }
        public SpeedSettingType(SpeedSettingType s) : this(s.speed) { }
        public override void setParameter(Physarum p)
        {
            p._speed = (float)speed;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new SpeedSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.SpeedSettingType";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "Physarealm.SpeedSettingType";
            }
        }
        public override string ToString()
        {
            return TypeName + "\nspeed: " + speed;
        }
    }
}

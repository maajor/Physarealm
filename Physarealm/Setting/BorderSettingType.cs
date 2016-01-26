using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class BorderSettingType:AbstractSettingType
    {
        private int bdtype;

        public BorderSettingType(int b) 
        {
            if (b < 0)
                bdtype = 0;
            else if (b > 2)
                bdtype = 2;
            else
                bdtype = b;
        }
        public BorderSettingType(BorderSettingType p) : this(p.bdtype) { }
        public override void setParameter(Physarum p)
        {
            PhysaSetting.border_type = bdtype;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new BorderSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.BorderSettingType";
            }
        }
        public override string ToString()
        {
            return TypeName + " border type = " + bdtype;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class EscapeSettingType : AbstractSettingType
    {
        private double esc_p;
        public EscapeSettingType(double ep) 
        {
            if (ep > 1)
                esc_p = 1;
            else if (ep < 0)
                esc_p = 0;
            else esc_p = ep;
        }
        public EscapeSettingType(EscapeSettingType es) : this(es.esc_p) { }
        public override void setParameter(Physarum p)
        {
            p.escape_p = esc_p;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new EscapeSettingType(this);
        }
    }
}

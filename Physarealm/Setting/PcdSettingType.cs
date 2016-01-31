using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class PcdSettingType:AbstractSettingType
    {
        private double pcd;

        public PcdSettingType(double p) 
        {
            if (p > 1)
                pcd = 1;
            else if (p < 0)
                p = 0;
            else
                pcd = p;
        }
        public PcdSettingType(PcdSettingType p) : this(p.pcd) { }
        public override void setParameter(Physarum p)
        {
            PhysaSetting._pcd =(float) pcd;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new PcdSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.PcdSettingType";
            }
        }
        public override string ToString()
        {
            return TypeName + " pcd = " + pcd;
        }
    }
}

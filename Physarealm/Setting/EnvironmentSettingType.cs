using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class EnvironmentSettingType:AbstractSettingType
    {
        private float pcd;
        private int div_radius;
        private int die_radius;
        private int div_max;
        private int div_min;
        private int die_min;
        private int die_max;
        private double guide_factor;

        public EnvironmentSettingType() 
        {
            pcd = 0.1F;
            div_radius = 3;
            die_radius = 2;
            div_max = 10;
            div_min = 0;
            die_min = 0;
            die_max = 123;
            guide_factor = 0;
        }
        public EnvironmentSettingType(float pc_d, int dvr, int der, int dvmax, int dvmin, int demax, int demin, double gd) 
        {
            if (pc_d > 1)
                pcd = 1;
            else if (pc_d < 0)
                pcd = 0;
            else 
                pcd = pc_d;
            div_radius = dvr;
            die_radius = der;
            if (dvmax < dvmin)
            {
                int temp = dvmin;
                dvmin = dvmax;
                dvmax = temp;
            }
            if (demax < demin)
            {
                int temp = demin;
                demin = demax;
                demax = temp;
            }
            div_max = dvmax < Math.Pow(dvr * 2 + 1, 3) - 1 ? dvmax : (int)Math.Pow(dvr * 2 + 1, 3) - 1;
            div_min = dvmin >= 0 ? dvmin : 0;
            die_max = demax < Math.Pow(der * 2 + 1, 3) - 1 ? demax : (int)Math.Pow(der * 2 + 1, 3) - 1;
            die_min = demin >= 0 ? demin : 0;
            guide_factor = gd > 0 ? gd:0;
        }
        public EnvironmentSettingType(EnvironmentSettingType a)
            : this( a.pcd, a.div_radius, a.die_radius, a.div_max, a.div_min, a.die_max, a.die_min, a.guide_factor) { }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new EnvironmentSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.EnvironmentSetting";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "Physarealm.EnvironmentSetting";
            }
        }
    }
}

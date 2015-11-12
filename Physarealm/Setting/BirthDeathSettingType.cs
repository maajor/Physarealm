using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class BirthDeathSettingType:AbstractSettingType
    {
        private int div_radius;
        private int die_radius;
        private int div_max;
        private int div_min;
        private int die_min;
        private int die_max;

        public BirthDeathSettingType() 
        {
            div_radius = 3;
            die_radius = 2;
            div_max = 10;
            div_min = 0;
            die_min = 0;
            die_max = 123;
        }
        public BirthDeathSettingType(int dvr, int der, int dvmax, int dvmin, int demax, int demin) 
        {
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
        }
        public BirthDeathSettingType(BirthDeathSettingType a)
            : this(a.div_radius, a.die_radius, a.div_max, a.div_min, a.die_max, a.die_min) { }
        public override void setParameter(Physarum p) 
        {
            List<int> cond = new List<int>();
            cond.Add(div_radius);
            cond.Add(div_min);
            cond.Add(div_max);
            cond.Add(die_radius);
            cond.Add(die_min);
            cond.Add(die_max);
            p.setBirthDeathCondition(cond);
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new BirthDeathSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.BirthDeathSettingType";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "Physarealm.BirthDeathSettingType";
            }
        }
        public override string ToString()
        {
            return TypeName + "\ndivision radius: " + div_radius
                + "\ndivision min: " + div_min
                + "\ndivision max: " + div_max
                + "\ndeath radius: " + die_radius
                + "\ndeath min: " + die_min
                + "\ndeath max: " + die_max;
        }
    }
}

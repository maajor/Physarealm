using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class DeathDistanceSettingType :AbstractSettingType
    {
        private int _death_distance;
        public DeathDistanceSettingType(int dd) 
        {
            _death_distance = dd > 1 ? dd : 1;
        }
        public DeathDistanceSettingType(DeathDistanceSettingType d) : this(d._death_distance) { }
        public override void setParameter(Physarum p)
        {
            PhysaSetting._death_distance = _death_distance;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new DeathDistanceSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.DeathDistanceSettingType";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "Physarealm.DeathDistanceSettingType";
            }
        }
        public override string ToString()
        {
            return TypeName + "\ndeath distance: " + _death_distance;
        }
    }
}

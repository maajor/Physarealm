using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class InitialPopulationSettingType :AbstractSettingType
    {
        private int pop_size;

        public InitialPopulationSettingType(int psize) 
        {
            pop_size = psize; 
        }
        public InitialPopulationSettingType(InitialPopulationSettingType i) : this(i.pop_size) { }
        public override void setParameter(Physarum p)
        {
            PhysaSetting._popsize = pop_size;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new InitialPopulationSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.InitialPopulationType";
            }
        }
        public override string ToString()
        {
            return TypeName + " popsize = " + pop_size;
        }
    }
}

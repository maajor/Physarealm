using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.Kernel.Types;

namespace Physarealm.Food
{
    abstract class AbstractFoodType:GH_Goo<object>
    {

        public override bool IsValid
        {
            get { return true; }
        }

        public override string ToString()
        {
            return TypeName;
        }

        public override string TypeDescription
        {
            get { return "AbstractFoodType Description"; }
        }

        public override string TypeName
        {
            get { return "AbstractFoodType"; }
        }
    }
}

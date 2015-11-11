using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Physarealm.Food
{
    abstract class AbstractFoodType:GH_Goo<object>
    {
        public abstract List<Point3d> getFoodPts();
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

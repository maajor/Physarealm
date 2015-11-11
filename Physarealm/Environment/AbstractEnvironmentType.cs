using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;
using Grasshopper.Kernel.Types;

namespace Physarealm
{
    public abstract class AbstractEnvironmentType:GH_Goo<object>
    {


        abstract public override IGH_Goo Duplicate();

        public override bool IsValid
        {
            get { return true; }
        }

        public abstract override string ToString();

        public override string TypeDescription
        {
            get { return "abstract environment type description"; }
        }

        public override string TypeName
        {
            get { return "abstract environment type name"; }
        }
    }
}

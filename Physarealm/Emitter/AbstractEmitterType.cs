using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;
using Grasshopper.Kernel.Types;

namespace Physarealm.Emitter
{
    public abstract class AbstractEmitterType:GH_Goo<object>
    {

        abstract public override IGH_Goo Duplicate();

       public abstract Point3d getRandEmitPos();

        public override bool IsValid
        {
            get { return true; }
        }

        abstract public override string ToString();

        public override string TypeDescription
        {
            get { return "a emitter"; }
        }

        public override string TypeName
        {
            get { return "AbstractEmitterType"; }
        }
    }
}

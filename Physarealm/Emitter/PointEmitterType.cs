using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;

namespace Physarealm.Emitter
{
    class PointEmitterType:AbstractEmitterType
    {
        public List<Point3d> origins = new List<Point3d>();
        private Random rand;

        public PointEmitterType(List<Point3d> pts) { origins = pts; rand = new Random(DateTime.Now.Millisecond); }
        public PointEmitterType(PointEmitterType p) : this(p.origins) { rand = new Random(DateTime.Now.Millisecond); }

        public override Point3d getRandEmitPos()
        {
            int length = origins.Count;
            return origins[rand.Next(length)];
        }
        public override string ToString()
        {
            return this.TypeName;
        }
        public override IGH_Goo Duplicate()
        {
            return new PointEmitterType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.PointEmitterType";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "A agents Emitter";
            }
        }

    }
}

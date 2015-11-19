using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Geometry;

namespace Physarealm.Emitter
{
    class SurfaceEmitterType : AbstractEmitterType
    {
        protected List<Surface> _surf;
        private Random rand;
        private int count;

        public SurfaceEmitterType(List<Surface> srf) 
        { 
            _surf = srf;
            count = srf.Count;
            rand = new Random(DateTime.Now.Millisecond);
        }
        public SurfaceEmitterType(SurfaceEmitterType srfemi) : this(srfemi._surf) { rand = new Random(DateTime.Now.Millisecond); }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new SurfaceEmitterType(this);
        }

        public override Rhino.Geometry.Point3d getRandEmitPos()
        {
            int id = rand.Next(count);
            Surface thissrf = _surf[id];
            Interval uintv = thissrf.Domain(0);
            Interval vintv = thissrf.Domain(1);
            double umin = uintv.Min;
            double umax = uintv.Max;
            double vmin = vintv.Min;
            double vmax = vintv.Max;
            double randposu = rand.NextDouble() * (umax - umin) + umin;
            double randposv = rand.NextDouble() * (vmax - vmin) + vmin;
            return thissrf.PointAt(randposu, randposv);
        }

        public override string ToString()
        {
            return this.TypeName;
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.SurfaceEmitterType";
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

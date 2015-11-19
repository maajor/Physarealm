using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;
namespace Physarealm.Emitter
{
    class CurveEmitterType : AbstractEmitterType
    {
        private List<Curve> _crvs;
        private Random rand;
        private int count;

        public CurveEmitterType(List<Curve> crvs) 
        {
            _crvs = crvs;
            count = crvs.Count;
            rand = new Random(DateTime.Now.Millisecond);
        }
        public CurveEmitterType(CurveEmitterType crvemit) :this(crvemit._crvs)
        {
            rand = new Random(DateTime.Now.Millisecond);
        }
        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new CurveEmitterType(this);
        }

        public override Rhino.Geometry.Point3d getRandEmitPos()
        {
            int id = rand.Next(count);
            Curve thiscrv = _crvs[id];
            Interval intv = thiscrv.Domain;
            double para = rand.NextDouble() * (intv.Max - intv.Min) + intv.Min;
            return thiscrv.PointAt(para);
        }

        public override string ToString()
        {
            return this.TypeName;
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.CurveEmitterType";
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

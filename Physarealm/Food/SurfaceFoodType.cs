using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Physarealm.Food
{
    class SurfaceFoodType : AbstractFoodType
    {
        private List<Surface> _srfs;
        public SurfaceFoodType(List<Surface> srfs) { _srfs = srfs; }
        public SurfaceFoodType(SurfaceFoodType sftype) : this(sftype._srfs) { }
        public override List<Rhino.Geometry.Point3d> getFoodPts(double accu)
        {
            List<Point3d> retlist = new List<Point3d>();
            foreach (Surface srf in _srfs) 
            {
                double wid;
                double hei;
                srf.GetSurfaceSize(out wid, out hei);
                Interval uintv = srf.Domain(0);
                Interval vintv = srf.Domain(1);
                double ustep = uintv.Length / (wid / accu);
                double vstep = vintv.Length / (hei / accu);
                for (double countu = ustep / 2; countu < uintv.Max; countu += ustep) 
                {
                    for (double countv = vstep / 2; countv < vintv.Max; countv += vstep) 
                    {
                        retlist.Add(srf.PointAt(countu, countv));
                    }
                }
            }
            return retlist;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new SurfaceFoodType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.SurfaceFood";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "surface location of foods";
            }
        }
        public override string ToString()
        {
            return this.TypeName;
        }
    }
}

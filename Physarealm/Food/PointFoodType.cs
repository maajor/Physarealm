using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Physarealm.Food
{
    class PointFoodType:AbstractFoodType
    {
        public List<Point3d> positions;
        public PointFoodType() { positions = new List<Point3d>(); }
        public PointFoodType(List<Point3d> l){ positions = l;}
        public PointFoodType(PointFoodType p) : this(p.positions) { }
        public override List<Point3d> getFoodPts() 
        {
            return positions;
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.PointFood";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "points location of foods";
            }
        }
        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new PointFoodType(this);
        }
    }
}

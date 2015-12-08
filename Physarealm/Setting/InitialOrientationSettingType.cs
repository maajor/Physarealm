using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    class InitialOrientationSettingType:AbstractSettingType
    {
        private Vector3d orit;
        public InitialOrientationSettingType(Vector3d s) { orit = s; }
        public InitialOrientationSettingType(InitialOrientationSettingType s) : this(s.orit) { }
        public override void setParameter(Physarum p)
        {
            p.initOrient = orit;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new InitialOrientationSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.InitialOrientationSettingType";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "Physarealm.InitialOrientationSettingType";
            }
        }
        public override string ToString()
        {
            return TypeName + "\norientation: " + orit;
        }
    }
}

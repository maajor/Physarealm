using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    class InitialVelocitySettingType:AbstractSettingType
    {
        private Vector3d orit;
        public InitialVelocitySettingType(Vector3d s) { orit = s; }
        public InitialVelocitySettingType(InitialVelocitySettingType s) : this(s.orit) { }
        public override void setParameter(Physarum p)
        {
            p.initOrient = orit;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new InitialVelocitySettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.InitialVelocitySettingType";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "Physarealm.InitialVelocitySettingType";
            }
        }
        public override string ToString()
        {
            return TypeName + "\nvelocity: " + orit;
        }
    }
}

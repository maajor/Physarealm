using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class DetectDirectionSettingType :AbstractSettingType
    {
        private int detect_direction;
        public DetectDirectionSettingType(int det) 
        {
            detect_direction = det >= 4 ? det : 4;
        }
        public DetectDirectionSettingType(DetectDirectionSettingType d) : this(d.detect_direction) { }
        public override void setParameter(Physarum p)
        {
            p._detectDir = detect_direction;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new DetectDirectionSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.DetectDirectionSettingType";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "Physarealm.DetectDirectionSettingType";
            }
        }
        public override string ToString()
        {
            return TypeName + "\ndetect direction: " + detect_direction;
        }
    }
}

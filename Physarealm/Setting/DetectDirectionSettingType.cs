using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class DetectDirectionSettingType :AbstractSettingType
    {
        private int detect_direction_r;
        private int detect_direction_phy;
        public DetectDirectionSettingType(int detr, int detphy) 
        {
            detect_direction_r = detr >= 4 ? detr : 4;
            detect_direction_phy = detphy >= 1 ? detphy : 1;

        }
        public DetectDirectionSettingType(DetectDirectionSettingType d) : this(d.detect_direction_r, d.detect_direction_phy) { }
        public override void setParameter(Physarum p)
        {
            PhysaSetting.DetectDirRSubd = detect_direction_r;
            PhysaSetting.DetectDirPhySubd = detect_direction_phy;
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
            return TypeName + "\ndetect direction r: " + detect_direction_r + "\ndetect direction phy: " + detect_direction_phy;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Physarealm;

namespace Physarealm.Setting
{
    class AgentSettingType:AbstractSettingType
    {
        private float sensor_angle;
        private float rotate_angle;
        private float sensor_offset;
        private int detect_dir_r;
        private int detect_dir_phy;
        private int death_distance;
        private float max_speed;
        private float depT;

        public AgentSettingType() 
        {
            sensor_angle = 22.5F;
            rotate_angle = 45;
            sensor_offset = 10;
            detect_dir_r = 4;
            detect_dir_phy = 1;
            death_distance = 100;
            max_speed = 5;
            depT = 10;
        }
        public AgentSettingType(float sa, float ra, float so, int det_dr, int det_dphy, int dea_d, float ms, float dept) 
        {
            if (sa >= 180)
                sensor_angle = 180;
            else if (sa < 0)
                sensor_angle = 0;
            else
                sensor_angle = sa;
            if (so >= 180)
                sensor_offset = 180;
            else if (so < 0)
                sensor_offset = 0;
            else
                sensor_offset = so;
            if (det_dr < 4)
                det_dr = 4;
            else
                detect_dir_r = det_dr;
            if (det_dphy < 1)
                det_dphy = 1;
            else
                detect_dir_phy = det_dphy;
            death_distance = dea_d > 0 ? dea_d:1;
            max_speed = ms > 1 ? ms:1;
            depT = dept > 1 ? dept:1;
        }
        public AgentSettingType(AgentSettingType a)
            : this(a.sensor_angle, a.rotate_angle, a.sensor_offset,
                a.detect_dir_r, a.detect_dir_phy, a.death_distance, a.max_speed, a.depT) { }
        public override void setParameter(Physarum p) 
        {
            PhysaSetting._sense_angle = sensor_angle;
            PhysaSetting._rotate_angle = rotate_angle;
            PhysaSetting._sense_offset = sensor_offset;
            PhysaSetting._death_distance = death_distance;
            PhysaSetting.DetectDirRSubd = detect_dir_r;
            PhysaSetting.DetectDirPhySubd = detect_dir_phy;
            PhysaSetting._speed = max_speed;
            PhysaSetting._depT = depT;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new AgentSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.AgentSetting";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "Physarealm.AgentSetting";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class AgentSettingType:AbstractSettingType
    {
        private float sensor_angle;
        private float rotate_angle;
        private float sensor_offset;
        private int detect_dir;
        private int death_distance;
        private float max_speed;
        private float depT;

        public AgentSettingType() 
        {
            sensor_angle = 22.5F;
            rotate_angle = 45;
            sensor_offset = 10;
            detect_dir = 4;
            death_distance = 100;
            max_speed = 5;
            depT = 10;
        }
        public AgentSettingType(float sa, float ra, float so, int det_d, int dea_d, float ms, float dept) 
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
            if (det_d < 4)
                detect_dir = 4;
            else
                detect_dir = det_d;
            death_distance = dea_d > 0 ? dea_d:1;
            max_speed = ms > 1 ? ms:1;
            depT = dept > 1 ? dept:1;
        }
        public AgentSettingType(AgentSettingType a)
            : this(a.sensor_angle, a.rotate_angle, a.sensor_offset,
                a.detect_dir, a.death_distance, a.max_speed, a.depT) { }
        public override void setParameter(Physarum p) 
        {
            p._sense_angle = sensor_angle;
            p._rotate_angle = rotate_angle;
            p._sense_offset = sensor_offset;
            p._death_distance = death_distance;
            p._detectDir = detect_dir;
            p._speed = max_speed;
            p._depT = depT;
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

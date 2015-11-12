using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm.Setting
{
    class SensorSettingType:AbstractSettingType
    {
        private float sensor_angle;
        private float rotate_angle;
        private float sensor_offset;

        public SensorSettingType(float sa, float ra, float so) 
        {
            if (sa >= 180)
                sensor_angle = 180;
            else if (sa < 0)
                sensor_angle = 0;
            else
                sensor_angle = sa;
            if (ra >= 180)
                rotate_angle = 180;
            else if (ra < 0)
                rotate_angle = 0;
            else
                rotate_angle = ra;
            sensor_offset = so > 1 ? so : 1;
        }
        public SensorSettingType(SensorSettingType a)
            : this(a.sensor_angle, a.rotate_angle, a.sensor_offset
                ) { }
        public override void setParameter(Physarum p)
        {
            p._sense_angle = sensor_angle;
            p._rotate_angle = rotate_angle;
            p._sense_offset = sensor_offset;
        }

        public override Grasshopper.Kernel.Types.IGH_Goo Duplicate()
        {
            return new SensorSettingType(this);
        }
        public override string TypeName
        {
            get
            {
                return "Physarealm.SensorSettingType";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "Physarealm.SensorSettingType";
            }
        }
        public override string ToString()
        {
            return TypeName + "\nsensor offset: " + sensor_offset
                + "\nsensor angle: " + sensor_angle
                + "\nrotate angle: " + rotate_angle;
        }
    }
}

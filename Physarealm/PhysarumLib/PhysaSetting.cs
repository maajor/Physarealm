using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physarealm
{
    public static class PhysaSetting
    {
        public static int _popsize { get; set; }
        public static float _sense_offset { get; set; }
        public static float _sense_angle { get; set; }
        public static float _rotate_angle { get; set; }
        public static float _pcd { get; set; }
        public static float _depT { get; set; }
        public static float _speed { get; set; }
        private static int _detectDirRSubd { get; set; }
        private static int _detectDirPhySubd { get; set; }
        public static int DetectDirRSubd
        {
            get { return _detectDirRSubd; }
            set
            { 
                _detectDirRSubd = value;
                _sensor_theta_step_angle = 360 / _detectDirRSubd;
            }
        }
        public static int DetectDirPhySubd
        { 
            get { return _detectDirPhySubd;} 
            set 
            {
                _detectDirPhySubd = value;
                _sensor_phy_step_angle = _sense_angle / _detectDirPhySubd;
            } 
        }
        public static float _sensor_theta_step_angle { get;private set; }
        public static float _sensor_phy_step_angle { get; private set; }
        public static int _death_distance { get; set; }
        public static int _division_frequency_test { get; set; }
        public static int _death_frequency_test { get; set; }
        public static int gw { get; set; }//division
        public static int gmin { get; set; }
        public static int gmax { get; set; }
        public static int sw { get; set; }//die
        public static int smax { get; set; }
        public static int smin { get; set; }
        public static double guide_factor { get; set; }
        public static double escape_p { get; set; }
        public static bool both_dir_flag;
        public static int border_type;//0:border die; 1:wrap; 2:bounce

        static PhysaSetting()
        {
            _sense_angle = 22.5F;
            _rotate_angle = 45F;
            _sense_offset = 7F;
            DetectDirRSubd = 4;
            DetectDirPhySubd = 1;
            _death_distance = 100;
            _speed = 3;
            _pcd = 0.1F;
            _depT = 3F;
            _popsize = 50;
            escape_p = 0;
            guide_factor = 0;
            gw = 3;
            gmin = 0;
            gmax = 10;
            sw = 2;
            smax = 123;
            smin = 0;
            _division_frequency_test = 4;
            _death_frequency_test = 4;
            guide_factor = 0;
            escape_p = 0;
            both_dir_flag = true;
            border_type = 0;
        }
        public static void setBirthDeathCondition(List<int> cond)
        {
            gw = cond[0];
            gmin = cond[1];
            gmax = cond[2];
            sw = cond[3];
            smin = cond[4];
            smax = cond[5];
        }
    }

}

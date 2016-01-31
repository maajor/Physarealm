using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Rhino;
using Rhino.Geometry;
using Physarealm.Environment;

namespace Physarealm 
{
    public class Amoeba : Particle, IDisposable
    {
        public Vector3d orientation { get; set; }
        private float tempfloatx;//a temporary accurate position x
        private float tempfloaty;//a temporary accurate position z
        private float tempfloatz;//a temporary accurate position z
        public int ID { get; private set; }
        public int curx { get; private set; }// u index
        public int cury { get; private set; }// v index
        public int curz { get; private set; }// w index
        private int tempx; // temporary u index
        private int tempy; // temporary v index
        private int tempz; // temporary v index
        public int _distance_traveled;
        private float _cur_speed;
        public bool _moved_successfully;
        public bool _divide { set; get; }
        public bool _die { set; get; }
        private float _ca_torealease;
        public float[] sensor_data;
        public Point3d prev_loc;

        public Amoeba() : base() { }
        public Amoeba(int id)
            : base()
        {
            ID = id;
            _ca_torealease = 3;
        }
        public bool initializeAmoeba(AbstractEnvironmentType env, Libutility util)
        {
            do
            {
                tempfloatx = (float)util.getDoubleRand(env.getUMin(), env.getUMax());
                tempfloaty = (float)util.getDoubleRand(env.getVMin(), env.getVMax());
                tempfloatz = (float)util.getDoubleRand(env.getWMin(), env.getWMax());
                Point3d indexPos = env.getIndexByPosition(tempfloatx, tempfloaty, tempfloatz);
                tempx = (int)indexPos.X;
                tempy = (int)indexPos.Y;
                tempz = (int)indexPos.Z;
            }
            while (!iniSuccess(tempx, tempy, tempz, env, util));
            curx = tempx;
            cury = tempy;
            curz = tempz;
            //Location = new Point3d(curx, cury, curz);
            Location = env.getPositionByIndex(curx, cury, curz);
            occupyCell(curx, cury, curz, env);
            selectRandomDirection(util);
            prev_loc = Location;
            return true;
        }
        public bool initializeAmoeba(double x, double y, double z, AbstractEnvironmentType env, Libutility util)
        {
            Point3d indexLoca = env.getIndexByPosition(x, y, z);
            tempx = (int)indexLoca.X;
            tempy = (int)indexLoca.Y;
            tempz = (int)indexLoca.Z;
            curx = tempx;
            cury = tempy;
            curz = tempz;
            //Location = new Point3d(curx, cury, curz);
            Location = env.getPositionByIndex(curx, cury, curz);
            occupyCell(curx, cury, curz, env);
            selectRandomDirection(util);
            prev_loc = Location;
            return true;
        }
        public bool initializeAmoeba(double x, double y, double z,  int radius, AbstractEnvironmentType env, Libutility util)
        {
            int try_count = 0;
            Point3d IniIndexLoca = env.getIndexByPosition(x, y, z);
            int start_x = (int)IniIndexLoca.X - radius > 0 ? (int)IniIndexLoca.X - radius : 0;
            int start_y = (int)IniIndexLoca.Y - radius > 0 ? (int)IniIndexLoca.Y - radius : 0;
            int start_z = (int)IniIndexLoca.Z - radius > 0 ? (int)IniIndexLoca.Z - radius : 0;
            int end_x = (int)IniIndexLoca.X + radius < env.u ? (int)IniIndexLoca.X + radius : env.u - 1;
            int end_y = (int)IniIndexLoca.Y + radius < env.v ? (int)IniIndexLoca.Y + radius : env.v - 1;
            int end_z = (int)IniIndexLoca.Z + radius < env.w ? (int)IniIndexLoca.Z + radius : env.w - 1;
            do
            {
                tempx = util.getRand(start_x, end_x + 1);
                tempy = util.getRand(start_y, end_y + 1);
                tempz = util.getRand(start_z, end_z + 1);
                try_count++;
            }
            while (!iniSuccess(tempx, tempy, tempz, env, util) && try_count<10);
            curx = tempx;
            cury = tempy;
            curz = tempz;
            //Location = new Point3d(curx, cury, curz);
            Location = env.getPositionByIndex(curx, cury, curz);
            occupyCell(curx, cury, curz, env);
            selectRandomDirection(util);
            prev_loc = new Point3d(x, y, z);
            return true;
        }
        public bool iniSuccess(int x, int y, int z, AbstractEnvironmentType env, Libutility util)
        {
            if (env.isOccupidByParticleByIndex(x, y, z) == true)
                return false;
            if (env.isWithinObstacleByIndex(x, y, z) && util.getRandDouble() > PhysaSetting.escape_p)
                return false;
            return true;
        }
        public void occupyCell(int x, int y, int z, AbstractEnvironmentType env)
        {
            env.clearGridCellByIndex(curx, cury, curz);
            env.occupyGridCellByIndex(tempx, tempy, tempz, ID);
            curx = tempx;
            cury = tempy;
            curz = tempz;
            resetFloatingPointPosition(env);
            if (_moved_successfully)
                env.increaseTrailByIndex(curx, cury, curz, _ca_torealease);
        }
        public void doMotorBehaviors(AbstractEnvironmentType env, Libutility util)
        {
            _distance_traveled++;
            prev_loc = Location;
            //_cur_speed = _max_speed * (1 - _distance_traveled / _deathDistance);
            _cur_speed = PhysaSetting._speed;
            if (env.getGriddataByIndex(curx, cury, curz) == 1)
                _distance_traveled = 0;
            _moved_successfully = false;
            if (util.getRandDouble() < PhysaSetting._pcd)
            {
              selectRandomDirection(util);
              resetFloatingPointPosition(env);
              return;
            }
            Point3d curLoc = Location;
            curLoc.Transform(Transform.Translation(orientation));
            //Location = curLoc;
            tempfloatx = (float)curLoc.X;
            tempfloaty = (float)curLoc.Y;
            tempfloatz = (float)curLoc.Z;
            switch (PhysaSetting.border_type) 
            {
                case 0:
                    if(env.constrainPos(ref tempfloatx, ref tempfloaty, ref tempfloatz,0))
                        selectRandomDirection(util);
                    break;
                case 1: 
                    env.constrainPos(ref tempfloatx, ref tempfloaty, ref tempfloatz, 1);
                    break;
                case 2:
                    env.constrainPos(ref tempfloatx, ref tempfloaty, ref tempfloatz, 0);
                    orientation = env.bounceOrientation(curLoc, orientation);
                    break;
                default:
                    break;
            }
            //if(env.constrainPos(ref tempfloatx, ref tempfloaty, ref tempfloatz))
            //    selectRandomDirection(util);
            Point3d temppos = env.getIndexByPosition(tempfloatx, tempfloaty, tempfloatz);
            tempx = (int)temppos.X;
            tempy = (int)temppos.Y;
            tempz = (int)temppos.Z;
            if (env.isOccupidByParticleByIndex(tempx, tempy, tempz))
            {
                selectRandomDirection(util);
                return;
            }
            else if (env.isWithinObstacleByIndex(tempx, tempy, tempz) && util.getRandDouble() > PhysaSetting.escape_p) 
            {
                selectRandomDirection(util);
                return;
            }
            else
            {
                _moved_successfully = true;
                Location = new Point3d(tempfloatx, tempfloaty, tempfloatz);
                env.clearGridCellByIndex(curx, cury, curz);
                //env.agedata[curx, cury, curz]++;
                env.occupyGridCellByIndex(tempx, tempy, tempz, ID);
                curx = tempx;
                cury = tempy;
                curz = tempz;
                //float trailIncrement = calculateTrailIncrement(util);
                //env.increaseTrailByIndex(curx, cury, curz, trailIncrement);
                env.increaseTrailByIndex(curx, cury, curz, _ca_torealease);
                //if (_moved_successfully && !_die && _distance_traveled % division_frequency_test == 0)
                if (_moved_successfully && !_die)
                    doDivisionTest(env);
            }
        }
        public float calculateTrailIncrement(Libutility util)
        {
            return util.getIncrement(_distance_traveled, PhysaSetting._death_distance);
        }
        public void selectRandomDirection(Libutility util)
        {
            double randx = (util.getRandDouble() - 0.5) * 2;
            double randy = (util.getRandDouble() - 0.5) * 2;
            double randz = (util.getRandDouble() - 0.5) * 2;
            Vector3d randDir = new Vector3d(randx, randy, randz);
            Double leng = randDir.Length;
            Double factor = _cur_speed / leng;
            orientation = Vector3d.Multiply(factor, randDir);
            return;
        }
        public void selectRandomDirection(Libutility util, Vector3d preDir)
        {
            double randx = (util.getRandDouble() - 0.5) * 2;
            double randy = (util.getRandDouble() - 0.5) * 2;
            double randz = (util.getRandDouble() - 0.5) * 2;
            Vector3d randDir = new Vector3d(randx, randy, randz);
            randDir = Vector3d.Add(randDir, preDir);
            Double leng = randDir.Length;
            Double factor = _cur_speed / leng;
            orientation = Vector3d.Multiply(factor, randDir);
            return;
        }
        public void resetFloatingPointPosition(AbstractEnvironmentType env)
        {
            Location = env.getPositionByIndex(curx, cury, curz);
            //tempfloatx = curx;
            //tempfloaty = cury;
            //tempfloatz = curz;
            //Location = new Point3d(curx, cury, curz);
            return;
        }
        public void doSensorBehaviors(AbstractEnvironmentType env, Libutility util)
        {
            this.doDeathTest(env);
            orientation = env.projectOrientationToEnv(Location, orientation);
            int det_count = PhysaSetting.DetectDirRSubd * PhysaSetting.DetectDirPhySubd + 1;
            int max_item = 0;
            float max_item_phy = 0;
            float max_item_theta = 0;
            sensor_data = new float[det_count];
            //List<trailInfo> infos = new List<trailInfo>();
            //infos.Add(env.getOffsetTrailValue(curx, cury, curz, orientation, 0, 0, _sensor_offset, util));
            //float maxtrail = 0;
            //Point3d tgtPos = new Point3d();
            sensor_data[0] = env.getOffsetTrailValue(curx, cury, curz, orientation, 0, 0, PhysaSetting._sense_offset, util);
            int count_cur = 1;
            for (int i = 0; i < PhysaSetting.DetectDirRSubd; i++)
            {
                for (int j = 1; j <= PhysaSetting.DetectDirPhySubd; j++)
                {
                    sensor_data[count_cur] = env.getOffsetTrailValue(curx, cury, curz, orientation, j * PhysaSetting._sensor_phy_step_angle, i * PhysaSetting._sensor_theta_step_angle, PhysaSetting._sense_offset, util);
                    //infos.Add(env.getOffsetTrailValue(curx, cury, curz, orientation, j * _sensor_phy_step_angle, i * _sensor_theta_step_angle, _sensor_offset, util));
                    if (sensor_data[count_cur] > sensor_data[max_item])
                    {
                        max_item = count_cur;
                        max_item_phy = j * PhysaSetting._sensor_phy_step_angle;
                        max_item_theta = i * PhysaSetting._sensor_theta_step_angle;
                    }
                    count_cur++;
                }
            }
            /*foreach(trailInfo inf in infos)
            {
              if(inf.trailValue > maxtrail)
              {
                maxtrail = inf.trailValue;
                tgtPos = inf.targetPos;
              }
            }
            Vector3d newOri = Point3d.Subtract(tgtPos, new Point3d(curx, cury, curz));
            double curLength = newOri.Length;
            double scaleFactor = _cur_speed / curLength;
            orientation = Vector3d.Multiply(scaleFactor, newOri);
            //orientation = newOri;
            */

            rotate(max_item_phy * PhysaSetting._rotate_angle / PhysaSetting._sense_angle, max_item_theta);
            guideOrientation();
        }
        public void rotate(float rotate_phy, float rotate_theta)
        {
            Vector3d orienOri = orientation;
            Vector3d toOri = orientation;
            float phyrad = rotate_phy * 3.1416F / 180;
            float thetarad = rotate_theta * 3.1416F / 180;
            Point3d intLoc = new Point3d(curx, cury, curz);
            Plane oriplane = new Plane(intLoc, orienOri);
            toOri.Transform(Transform.Rotation(phyrad, oriplane.YAxis, intLoc));
            toOri.Transform(Transform.Rotation(thetarad, oriplane.ZAxis, intLoc));
            //orientation.Rotate(phyrad, oriplane.YAxis);
            //orientation.Transform(thetarad, oriplane.ZAxis);

            double curLength = toOri.Length;
            double scaleFactor = _cur_speed / curLength;
            toOri = Vector3d.Multiply(scaleFactor, toOri);
            //moveangle = rotate_theta;
            //moved = toOri - orienOri;
            orientation = toOri;
        }
        public void doDivisionTest(AbstractEnvironmentType env)
        {
            _divide = false;
            if (env.isOutsideBorderRangeByIndex(curx, cury, curz))
                return;
            if (isWithinThresholdRange(curx, cury, curz, env))
            {
                _divide = true;
            }
        }
        public void doDeathTest(AbstractEnvironmentType env)
        {
            _die = false;
            if (env.isOutsideBorderRangeByIndex(curx, cury, curz) && PhysaSetting.border_type != 2)
            {
                _die = true;
            }
            //if (env.envdata[curx, cury, curz] == 2)
            //  _die = true;
            //return;
            if (isOutsideSurvivalRange(curx, cury, curz, env))
                _die = true;
        }
        public bool isWithinThresholdRange(int x, int y, int z, AbstractEnvironmentType env)
        {
            int d = env.countNumberOfParticlesPresentByIndex(x, y, z, PhysaSetting.gw);
            //_around = d;
            if (d >= PhysaSetting.gmin && d <= PhysaSetting.gmax)
                return true;
            else return false;
        }
        public bool isOutsideSurvivalRange(int x, int y, int z, AbstractEnvironmentType env)
        {
            if (_distance_traveled > PhysaSetting._death_distance)
            {
                return true;
            }
            double d = env.countNumberOfParticlesPresentByIndex(x, y, z, PhysaSetting.sw);
            if (d < PhysaSetting.smin || d > PhysaSetting.smax)
                return true;
            else return false;
        }

        private void guideOrientation()
        {
            Vector3d curOri = orientation;
            curOri.Unitize();
            if (PhysaSetting.both_dir_flag)
            {
                if (curOri.Z > 0)
                    curOri.Z += PhysaSetting.guide_factor;
                else
                    curOri.Z -= PhysaSetting.guide_factor;
            }
            else 
            {
                curOri.Z = curOri.Z > 0 ? curOri.Z + PhysaSetting.guide_factor : -curOri.Z + PhysaSetting.guide_factor;
            }
            curOri = Vector3d.Multiply(_cur_speed, curOri);
            orientation = curOri;

        }

        public void Dispose()
        {
            sensor_data.Initialize();
        }
    }//end of Amoeba class
}

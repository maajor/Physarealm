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
        private float tempfloatx;
        private float tempfloaty;
        private float tempfloatz;
        private int _id;
        public int curx { get; private set; }
        public int cury { get; private set; }
        public int curz { get; private set; }
        private int tempx;
        private int tempy;
        private int tempz;
        public float _sensor_angle { get; set; }
        public float _rotate_angle { get; set; }
        public float _sensor_offset { get; set; }
        public int _detectDir { get; set; }
        public int _deathDistance { get; set; }
        public float _max_speed { get; set; }
        private float _sensor_theta_step_angle { get; set; }
        private float _sensor_phy_step_angle { get; set; }
        public int _distance_traveled;
        private float _cur_speed;
        public bool _moved_successfully;
        public bool _divide { set; get; }
        public bool _die { set; get; }
        public float _depT { get; set; }
        private float _ca_torealease;
        public float _pcd { get; set; }
        //private int division_frequency_test = 5;
        //private int death_frequency_test = 5;
        public int _div_radius { get; set; }
        public int _die_radius { get; set; }
        public int _div_max { get; set; }
        public int _div_min { get; set; }
        public int _die_min { get; set; }
        public int _die_max { get; set; }
        //public int _unitized;
        //public float moveangle;
        public float[] sensor_data;
        //public Vector3d moved;
        public double _guide_factor { get; set; }

        public Amoeba() : base() { }
        public Amoeba(int id)
            : base()
        {
            _id = id;
            _ca_torealease = 3;
        }
        public Amoeba(int id, float sensor_angle = (float) 45, float rotate_angle = 45, float sensor_offset = 7, int detectDir = 3, int deathDistance = 100, float max_speed = 3, float pcd = (float) 0.1, float dept = 3)
            : base()
        {
            _id = id;
            _sensor_angle = sensor_angle;
            _rotate_angle = rotate_angle;
            _sensor_offset = sensor_offset;
            _detectDir = detectDir;
            _deathDistance = deathDistance;
            _max_speed = max_speed;
            _cur_speed = _max_speed;
            _pcd = pcd;
            _depT = dept;
            if (detectDir < 3)
                detectDir = 3;
            _sensor_theta_step_angle = 360 / detectDir;
            _sensor_phy_step_angle = sensor_angle / (detectDir - 3);
            _ca_torealease = dept;
            _div_radius = 3;
            _die_radius = 2;
            _div_max = 10;
            _div_min = 0;
            _die_min = 0;
            _die_max = 123;
            _moved_successfully = true;
            _guide_factor = 0.2;
        }
        public void initializeAmoeba(AbstractEnvironmentType env, Libutility util)
        {
            do
            {
                tempx = util.getRand(env.u);
                tempy = util.getRand(env.v);
                tempz = util.getRand(env.w);
            }
            while (!iniSuccess(tempx, tempy, tempz, env));
            curx = tempx;
            cury = tempy;
            curz = tempz;
            Location = new Point3d(curx, cury, curz);
            occupyCell(curx, cury, curz, env);
            selectRandomDirection(util);
        }
        public void initializeAmoeba(int x, int y, int z, AbstractEnvironmentType env, Libutility util)
        {
            tempx = x;
            tempy = y;
            tempz = z;
            curx = tempx;
            cury = tempy;
            curz = tempz;
            Location = new Point3d(curx, cury, curz);
            occupyCell(curx, cury, curz, env);
            selectRandomDirection(util);
        }
        public void initializeAmoeba(int x, int y, int z, int maxx, int maxy, int maxz, int radius, AbstractEnvironmentType env, Libutility util)
        {
            int start_x = x - radius > 0 ? x - radius : 0;
            int start_y = y - radius > 0 ? y - radius : 0;
            int start_z = z - radius > 0 ? z - radius : 0;
            int end_x = x + radius < maxx ? x + radius : maxx - 1;
            int end_y = y + radius < maxy ? y + radius : maxy - 1;
            int end_z = z + radius < maxz ? z + radius : maxz - 1;
            do
            {
                tempx = util.getRand(start_x, end_x + 1);
                tempy = util.getRand(start_y, end_y + 1);
                tempz = util.getRand(start_z, end_z + 1);
            }
            while (!iniSuccess(tempx, tempy, tempz, env));
            curx = tempx;
            cury = tempy;
            curz = tempz;
            Location = new Point3d(curx, cury, curz);
            occupyCell(curx, cury, curz, env);
            selectRandomDirection(util);
        }
        public bool iniSuccess(int x, int y, int z, AbstractEnvironmentType env)
        {
            if (env.isOccupidByParticle(x, y, z) == true || env.getGriddata(x, y, z) == 2)
                return false;
            return true;
        }
        public void occupyCell(int x, int y, int z, AbstractEnvironmentType env)
        {
            env.clearGridCell(curx, cury, curz);
            env.occupyGridCell(tempx, tempy, tempz, _id);
            curx = tempx;
            cury = tempy;
            curz = tempz;
            resetFloatingPointPosition();
            if (_moved_successfully)
                env.increaseTrail(curx, cury, curz, _ca_torealease);
        }
        public void doMotorBehaviors(AbstractEnvironmentType env, Libutility util)
        {
            _distance_traveled++;
            //_cur_speed = _max_speed * (1 - _distance_traveled / _deathDistance);
            _cur_speed = _max_speed;
            if (env.getGriddata(curx, cury, curz) == 1)
                _distance_traveled = 0;
            _moved_successfully = false;
            //if (util.getRandDouble() < _pcd)
            //{
            //  selectRandomDirection(util);
            //  resetFloatingPointPosition();
            //  return;
            //}
            Point3d curLoc = Location;
            curLoc.Transform(Transform.Translation(orientation));
            //Location = curLoc;
            tempfloatx = (float)curLoc.X;
            tempfloaty = (float)curLoc.Y;
            tempfloatz = (float)curLoc.Z;
            if (tempfloatx < 0)
            {
                tempfloatx = -tempfloatx;
                //orientation.Transform(Transform.Mirror(Plane.WorldYZ));
                selectRandomDirection(util);
            }
            if (tempfloatx > env.u - 1)
            {
                //tempfloatx = (env.u - 1) * 2 - tempfloatx - 1;
                tempfloatx = env.u - 1;
                //orientation.Transform(Transform.Mirror(Plane.WorldYZ));
                selectRandomDirection(util);
            }
            if (tempfloaty < 0)
            {
                tempfloaty = -tempfloaty;
                //orientation.Transform(Transform.Mirror(Plane.WorldZX));
                selectRandomDirection(util);
            }
            if (tempfloaty > env.v - 1)
            {
                //tempfloaty = (env.v - 1) * 2 - tempfloaty - 1;
                tempfloaty = env.v - 1;
                //orientation.Transform(Transform.Mirror(Plane.WorldZX));
                selectRandomDirection(util);
            }
            if (tempfloatz < 0)
            {
                tempfloatz = -tempfloatz;
                //orientation.Transform(Transform.Mirror(Plane.WorldXY));
                selectRandomDirection(util);
            }
            if (tempfloatz > env.w - 1)
            {
                //tempfloatz = (env.w - 1) * 2 - tempfloatz - 1;
                tempfloatz = env.w - 1;
                //orientation.Transform(Transform.Mirror(Plane.WorldXY));
                selectRandomDirection(util);
            }

            tempx = (int)Math.Round(tempfloatx);
            tempy = (int)Math.Round(tempfloaty);
            tempz = (int)Math.Round(tempfloatz);
            if (env.isOccupidByParticle(tempx, tempy, tempz))
            {
                selectRandomDirection(util);
                return;
            }
            else
            {
                _moved_successfully = true;
                Location = new Point3d(tempfloatx, tempfloaty, tempfloatz);
                env.clearGridCell(curx, cury, curz);
                //env.agedata[curx, cury, curz]++;
                env.occupyGridCell(tempx, tempy, tempz, _id);
                curx = tempx;
                cury = tempy;
                curz = tempz;
                //float trailIncrement = calculateTrailIncrement(util);
                //env.increaseTrail(curx, cury, curz, trailIncrement);
                env.increaseTrail(curx, cury, curz, _depT);
                //if (_moved_successfully && !_die && _distance_traveled % division_frequency_test == 0)
                if (_moved_successfully && !_die)
                    doDivisionTest(env);
            }
        }
        public float calculateTrailIncrement(Libutility util)
        {
            return util.getIncrement(_distance_traveled, _deathDistance);
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
        public void resetFloatingPointPosition()
        {
            tempfloatx = curx;
            tempfloaty = cury;
            tempfloatz = curz;
            Location = new Point3d(curx, cury, curz);
            return;
        }
        public void doSensortBehaviors(AbstractEnvironmentType env, Libutility util)
        {
            doDeathTest(env);
            int det_count = _detectDir * (_detectDir - 3) + 1;
            int max_item = 0;
            float max_item_phy = 0;
            float max_item_theta = 0;
            sensor_data = new float[det_count];
            //List<trailInfo> infos = new List<trailInfo>();
            //infos.Add(env.getOffsetTrailValue(curx, cury, curz, orientation, 0, 0, _sensor_offset, util));
            //float maxtrail = 0;
            //Point3d tgtPos = new Point3d();
            sensor_data[0] = env.getOffsetTrailValue(curx, cury, curz, orientation, 0, 0, _sensor_offset, util);
            int count_cur = 1;
            for (int i = 0; i < _detectDir; i++)
            {
                for (int j = 1; j <= _detectDir - 3; j++)
                {
                    sensor_data[count_cur] = env.getOffsetTrailValue(curx, cury, curz, orientation, j * _sensor_phy_step_angle, i * _sensor_theta_step_angle, _sensor_offset, util);
                    //infos.Add(env.getOffsetTrailValue(curx, cury, curz, orientation, j * _sensor_phy_step_angle, i * _sensor_theta_step_angle, _sensor_offset, util));
                    if (sensor_data[count_cur] > sensor_data[max_item])
                    {
                        max_item = count_cur;
                        max_item_phy = j * _sensor_phy_step_angle;
                        max_item_theta = i * _sensor_theta_step_angle;
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

            rotate(max_item_phy * _rotate_angle / _sensor_angle, max_item_theta);
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
            if (isOutsideLatticeBorderRange(curx, cury, curz, env))
                return;
            if (isWithinThresholdRange(curx, cury, curz, env))
            {
                _divide = true;
            }
        }
        public void doDeathTest(AbstractEnvironmentType env)
        {
            _die = false;
            if (isOutsideLatticeBorderRange(curx, cury, curz, env))
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
            int d = env.countNumberOfParticlesPresent(x, y, z, _div_radius);
            //_around = d;
            if (d >= _div_min && d <= _div_max)
                return true;
            else return false;
        }
        public bool isOutsideSurvivalRange(int x, int y, int z, AbstractEnvironmentType env)
        {
            if (_distance_traveled > _deathDistance)
            {
                return true;
            }
            double d = env.countNumberOfParticlesPresent(x, y, z, _die_radius);
            if (d < _die_min || d > _die_max)
                return true;
            else return false;
        }
        public bool isOutsideLatticeBorderRange(int x, int y, int z, AbstractEnvironmentType env)
        {
            if (x < 2|| x > (env.u - 2))
                return true;
            else if (y < 2 || y > (env.v - 2))
                return true;
            else if (z < 2 || z > (env.w - 2))
                return true;
            else return false;
        }
        public void setBirthDeathCondition(int gw, int gmin, int gmax, int sw, int smin, int smax)
        {
            _div_radius = gw;
            _die_radius = sw;
            _div_max = gmax;
            _div_min = gmin;
            _die_min = smin;
            _die_max = smax;
        }
        private void guideOrientation()
        {
            Vector3d curOri = orientation;
            curOri.Unitize();
            if (curOri.Z > 0)
                curOri.Z += _guide_factor;
            else
                curOri.Z -= _guide_factor;
            curOri = Vector3d.Multiply(_cur_speed, curOri);
            orientation = curOri;

        }

        public void Dispose()
        {
            sensor_data.Initialize();
        }
    }//end of Amoeba class
}

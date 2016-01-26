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
        public Vector3d indexOrientation;
        private Index3f tempAccuPos;
        public Index3f indexPos;
        private Index3f tempIndexPos;
        /*
        private float tempfloatx;//a temporary accurate position x
        private float tempfloaty;//a temporary accurate position z
        private float tempfloatz;//a temporary accurate position z
        
        public int curx { get; private set; }// u index
        public int cury { get; private set; }// v index
        public int curz { get; private set; }// w index
        private int tempx; // temporary u index
        private int tempy; // temporary v index
        private int tempz; // temporary v index*/
        public int ID { get; private set; }
        public int _distance_traveled;
        private float _cur_speed;
        public bool _moved_successfully;
        public bool _divide {set; get; }
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
        public bool initializeAmoeba(AbstractEnvironmentType env)
        {
            do
            {
                tempAccuPos = new Index3f(
                    (float)Libutility.getDoubleRand(env.getUMin(), env.getUMax()),
                    (float)Libutility.getDoubleRand(env.getVMin(), env.getVMax()),
                    (float)Libutility.getDoubleRand(env.getWMin(), env.getWMax()));
                tempIndexPos = env.getIndexByPosition(tempAccuPos.x, tempAccuPos.y, tempAccuPos.z);
            }
            while (!iniSuccess(tempIndexPos, env));
            indexPos = tempIndexPos;
            Location = env.getPositionByIndex(tempIndexPos.convertToIndex(env.u, env.v, env.w));
            occupyCell(env);
            selectRandomDirection();
            prev_loc = Location;
            return true;
        }
        public bool initializeAmoeba(Point3d pos, AbstractEnvironmentType env)
        {
            indexPos = env.getIndexByPosition(pos);
            tempIndexPos = indexPos;
            Location = env.getPositionByIndex(indexPos.convertToIndex(env.u, env.v, env.w));
            occupyCell(env);
            selectRandomDirection();
            prev_loc = Location;
            return true;
        }
        public bool initializeAmoeba(Point3d pos, int radius, AbstractEnvironmentType env)
        {
            int try_count = 0;
            Index3f tempIndexLoc = env.getIndexByPosition(pos);
            do
            {
                tempIndexPos = tempIndexLoc.getRandNearbyPos(radius, env.u, env.v, env.w);
                try_count++;
                if (try_count > 10)
                    return false;
            }
            while (!iniSuccess(tempIndexPos, env));
            indexPos = tempIndexPos;
            Location = env.getPositionByIndex(tempIndexPos.convertToIndex(env.u, env.v, env.w));
            occupyCell(env);
            selectRandomDirection();
            prev_loc = pos;
            return true;
        }
        public bool iniSuccess(Index3f thisindexPos, AbstractEnvironmentType env)
        {
            int index = thisindexPos.convertToIndex(env.u, env.v, env.w);
            if (env.isOccupidByParticleByIndex(index) == true)
                return false;
            if (env.isWithinObstacleByIndex(index) && Libutility.getRandDouble() > PhysaSetting.escape_p)
                return false;
            return true;
        }
        public void occupyCell(AbstractEnvironmentType env)
        {
            int prvindex = indexPos.convertToIndex(env.u, env.v, env.w);
            int index = tempIndexPos.convertToIndex(env.u, env.v, env.w);
            env.clearGridCellByIndex(prvindex);
            env.occupyGridCellByIndex(index, ID);
            indexPos = tempIndexPos;
            resetFloatingPointPosition(env);
            if (_moved_successfully)
                env.increaseTrailByIndex(index, _ca_torealease);
        }
        public void doMotorBehaviors(AbstractEnvironmentType env)
        {
            _distance_traveled++;
            prev_loc = Location;
            //_cur_speed = _max_speed * (1 - _distance_traveled / _deathDistance);
            _cur_speed = PhysaSetting._speed;
            if (env.getGriddataByIndex(indexPos.convertToIndex(env.u, env.v, env.w)) == 1)
                _distance_traveled = 0;
            _moved_successfully = false;
            if (Libutility.getRandDouble() < PhysaSetting._pcd)
            {
              selectRandomDirection();
              resetFloatingPointPosition(env);
              return;
            }
            Point3d curLoc = Location;
            curLoc.Transform(Transform.Translation(orientation));//here we transform location using acualy orientation than index orientation
            //Location = curLoc;
            tempAccuPos = new Index3f((float)curLoc.X, (float)curLoc.Y, (float)curLoc.Z);
            switch (PhysaSetting.border_type) 
            {
                case 0:
                    if (env.constrainPos(tempAccuPos, 0))
                    {
                        selectRandomDirection();
                        orientationFromIndexOrientation(env);
                    }
                    break;
                case 1: 
                    env.constrainPos(tempAccuPos, 1);
                    break;
                case 2:
                    env.constrainPos(tempAccuPos, 0);
                    indexOrientation = env.bounceOrientation(curLoc, indexOrientation);
                    orientationFromIndexOrientation(env);
                    break;
                default:
                    break;
            }
            //if(env.constrainPos(ref tempfloatx, ref tempfloaty, ref tempfloatz))
            //    selectRandomDirection(util);
            tempIndexPos = env.getIndexByPosition(tempAccuPos.x, tempAccuPos.y, tempAccuPos.z);
            if (env.isOccupidByParticleByIndex(tempIndexPos.convertToIndex(env.u, env.v,env.w)))
            {
                selectRandomDirection();
                orientationFromIndexOrientation(env);
                return;
            }//move to a place where exist an agent
            else if (env.isWithinObstacleByIndex(tempIndexPos.convertToIndex(env.u, env.v, env.w)) && Libutility.getRandDouble() > PhysaSetting.escape_p) 
            {
                selectRandomDirection();
                orientationFromIndexOrientation(env);
                return;
            } //move out of valid aera
            else
            {
                _moved_successfully = true;
                Location = new Point3d(tempAccuPos.x, tempAccuPos.y, tempAccuPos.z);
                occupyCell(env);
                //env.agedata[curx, cury, curz]++;
                //float trailIncrement = calculateTrailIncrement(util);
                //env.increaseTrailByIndex(curx, cury, curz, trailIncrement);
                //if (_moved_successfully && !_die && _distance_traveled % division_frequency_test == 0)
                if (_moved_successfully && !_die)
                    doDivisionTest(env);
            }
        }
        public float calculateTrailIncrement()
        {
            return Libutility.getIncrement(_distance_traveled, PhysaSetting._death_distance);
        }
        public void selectRandomDirection()
        {
            double randx = (Libutility.getRandDouble() - 0.5) * 2;
            double randy = (Libutility.getRandDouble() - 0.5) * 2;
            double randz = (Libutility.getRandDouble() - 0.5) * 2;
            Vector3d randDir = new Vector3d(randx, randy, randz);
            Double leng = randDir.Length;
            Double factor = _cur_speed / leng;
            indexOrientation = Vector3d.Multiply(factor, randDir);

            return;
        }
        public void selectRandomDirection( Vector3d preDir)
        {
            double randx = (Libutility.getRandDouble() - 0.5) * 2;
            double randy = (Libutility.getRandDouble() - 0.5) * 2;
            double randz = (Libutility.getRandDouble() - 0.5) * 2;
            Vector3d randDir = new Vector3d(randx, randy, randz);
            randDir = Vector3d.Add(randDir, preDir);
            Double leng = randDir.Length;
            Double factor = _cur_speed / leng;
            indexOrientation = Vector3d.Multiply(factor, randDir);
            return;
        }
        public void resetFloatingPointPosition(AbstractEnvironmentType env)
        {
            Location = env.getPositionByIndex(indexPos.convertToIndex(env.u, env.v, env.w));
            return;
        }
        public void doSensorBehaviors(AbstractEnvironmentType env)//sensor behavior find best index orientation and transform to accualy orientation later
        {
            this.doDeathTest(env);
            //orientation = env.projectOrientationToEnv(Location, orientation);
            int det_count =PhysaSetting.DetectDirRSubd * PhysaSetting.DetectDirPhySubd + 1;
            int max_item = 0;
            float max_item_phy = 0;
            float max_item_theta = 0;
            sensor_data = new float[det_count];
            sensor_data[0] = env.getOffsetTrailValueByIndex(indexPos.convertToIndex(env.u, env.v, env.w), 
                indexOrientation, 0, 0 ,PhysaSetting._sense_offset);
            int count_cur = 1;
            for (int i = 0; i < PhysaSetting.DetectDirRSubd; i++)
            {
                for (int j = 1; j <= PhysaSetting.DetectDirPhySubd; j++)
                {
                    sensor_data[count_cur] = env.getOffsetTrailValueByIndex(indexPos.convertToIndex(env.u, env.v, env.w), 
                        indexOrientation, 
                        j * PhysaSetting._sensor_phy_step_angle, 
                        i * PhysaSetting._sensor_theta_step_angle, 
                        PhysaSetting._sense_offset);
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
            orientationFromIndexOrientation(env);//project index orientation to accualy orientation
        }
        public void rotate(float rotate_phy, float rotate_theta)
        {
            //Vector3d orienOri = orientation;
            //Vector3d toOri = orientation;
            Vector3d orienOri = indexOrientation;
            Vector3d toOri = indexOrientation;
            float phyrad = rotate_phy * 3.1416F / 180;
            float thetarad = rotate_theta * 3.1416F / 180;
            //Point3d intLoc = new Point3d(curx, cury, curz);
            Point3d intLoc = new Point3d(indexPos.x, indexPos.y, indexPos.z);
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
            //orientation = toOri;
            indexOrientation = toOri;
        }
        public void doDivisionTest(AbstractEnvironmentType env)
        {
            _divide = false;
            if (env.isOutsideBorderRangeByIndex(indexPos.convertToIndex(env.u, env.v, env.w)))
                return;
            if (isWithinDivisionRange(indexPos, env))
            {
                _divide = true;
            }
        }
        public void doDeathTest(AbstractEnvironmentType env)
        {
            _die = false;
            if (env.isOutsideBorderRangeByIndex(indexPos.convertToIndex(env.u, env.v, env.w)) && PhysaSetting.border_type != 2)
            {
                _die = true;
            }
            //if (env.envdata[curx, cury, curz] == 2)
            //  _die = true;
            //return;
            if (isOutsideSurvivalRange(indexPos, env))
                _die = true;
        }
        public bool isWithinDivisionRange(Index3f thispos, AbstractEnvironmentType env)
        {
            int d = env.countNumberOfParticlesPresentByIndex(thispos.convertToIndex(env.u, env.v, env.w), PhysaSetting.gw);
            //_around = d;
            if (d >= PhysaSetting.gmin && d <= PhysaSetting.gmax)
                return true;
            else return false;
        }
        public bool isOutsideSurvivalRange(Index3f thispos, AbstractEnvironmentType env)
        {
            if (_distance_traveled >PhysaSetting._death_distance)
            {
                return true;
            }
            double d = env.countNumberOfParticlesPresentByIndex(thispos.convertToIndex(env.u, env.v, env.w), PhysaSetting.sw);
            if (d < PhysaSetting.smin || d > PhysaSetting.smax)
                return true;
            else return false;
        }
        private void guideOrientation()
        {
            Vector3d curOri = indexOrientation;
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
            indexOrientation = curOri;

        }
        public void orientationFromIndexOrientation(AbstractEnvironmentType env) 
        {
            orientation = env.getAcuOrientation(indexPos, indexOrientation);//project index orientation to accualy orientation
        }

        public void Dispose()
        {
            sensor_data.Initialize();
        }
    }//end of Amoeba class
}

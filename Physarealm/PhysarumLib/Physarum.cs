using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;
using Physarealm.Environment;

namespace Physarealm
{
    public class Physarum:GH_Goo<object>, IDisposable
    {
        public List<Amoeba> population = new List<Amoeba>();
        public List<Amoeba> _toborn_population = new List<Amoeba>();
        public List<int> _todie_id = new List<int>();
        public Vector3d initOrient { get; set; }
        public int _current_id;
        private int _step;

        public Physarum()
            : base()
        {
            /*
            _current_id = 0;
            _step = 0;
            _division_frequency_test = 4;
            _death_frequency_test = 4;
            guide_factor = 0;
            _popsize = 200;
            escape_p = 0;
            initOrient = new Vector3d(0, 0, 0);
            both_dir_flag = true;
            border_type = 0;*/
            initParameters();
        }
        public Physarum(Physarum p) :base()
        {
            population = p.population;
            initOrient = p.initOrient;
        
        }
        public void initParameters()
        {
            _current_id = 0;
            _step = 0;
            initOrient = new Vector3d(0, 0, 0);
        }
        public void initPopulation(AbstractEnvironmentType env)
        {
            //_popsize = popsize;
            int init_error_threshold = 5;
            int init_error_count = 0;
            for (int i = 0; i <PhysaSetting._popsize; i++)
            {
                Amoeba newAmo = new Amoeba(_current_id);
                Point3d birthPlace = env.getRandomBirthPlace();
                if (!newAmo.initializeAmoeba(birthPlace, 4, env))
                {
                    init_error_count++;
                    if (init_error_count > init_error_threshold)
                        break;
                    continue;
                }
               // newAmo.initializeAmoeba(birthPlace, 4, env);
                if (initOrient.Length > 0)
                    newAmo.orientation = initOrient;//unsafe further amendment
                population.Add(newAmo);
                _current_id++;
            }
            env._escape_p =PhysaSetting.escape_p;
        }
        public void updatePopulation(AbstractEnvironmentType env)
        {
            
            foreach (Amoeba amo in population)
            {
                amo.doMotorBehaviors(env);
            }
            
            shuffleOrder();
            foreach (Amoeba amo in population)
            {
                amo.doSensorBehaviors(env);
            }
             
            /*
            System.Threading.Tasks.Parallel.ForEach(population, amo =>
            {
                amo.doMotorBehaviors(env, util);
            });
            System.Threading.Tasks.Parallel.ForEach(population, amo =>
            {
                amo.doSensorBehaviors(env, util);
            });
            *///Parallel will be a little bit faster, but may encounter unexpected error that every agent freeze....so i disable
        }
        public void doDivisionTest(AbstractEnvironmentType env)
        {
            //shuffleOrder();
            /*
            System.Threading.Tasks.Parallel.ForEach(population, amo =>
            {
            if (amo._divide)
            {
            birthNew(amo);
            //_current_population++;
            }
            amo._divide = false;
            });*/

            foreach (Amoeba amo in population)
            {
                if (amo._divide)
                {
                    birthNew(amo, env);
                    _current_id++;
                    //_current_population++;
                }
                amo._divide = false;
            }
        }
        public void updateTrails(AbstractEnvironmentType env)
        {
            env.projectToTrail();
            env.diffuseTrails();
        }
        public void doDeathTest(AbstractEnvironmentType env)
        {
            //shuffleOrder();
            /*
            System.Threading.Tasks.Parallel.ForEach(population, amo =>
            {
            if (amo._die )
            {
            _grid.clearGridCellByIndex(amo.curx, amo.cury, amo.curz);
            _todie_population.Add(amo);
            //population.Remove(amo);
            _current_population--;
            }
            });
            */

            foreach (Amoeba amo in population)
            {
                if (amo._die)
                {
                    env.clearGridCellByIndex(amo.indexPos.convertToIndex(env.u, env.v, env.w));
                    _todie_id.Add(amo.ID);
                    //population.Remove(amo);
                    //_current_population--;
                }

            }
        }
        public void birthNew(Amoeba agent, AbstractEnvironmentType env)
        {
            //if (agent.curx <= 4 || agent.curx >= env.u - 4 || agent.cury <= 4 || agent.cury >= env.v - 4 || agent.curz <= 4 || agent.curz >= env.w - 4)
            //    return;
            if (env.isOutsideBorderRangeByIndex(agent.indexPos.convertToIndex(env.u, env.v, env.w)))
                return;
            //if (agent.curx == agent.cury || agent.curx == agent.curz)
            //  return;
            Point3d newPos = env.getNeighbourhoodFreePosByIndex(agent.indexPos.convertToIndex(env.u, env.v, env.w), 1);
            if (newPos.X == -1 || newPos.Y == -1 || newPos.Y == -1)
                return;
            _current_id++;
            int thisindex = _current_id - 1;
            Amoeba newAmo = new Amoeba(thisindex);
            newAmo.initializeAmoeba(newPos, env);
            newAmo.prev_loc = agent.Location;
            //newAmo.initializeAmoeba(agent.curx, agent.cury, agent.curz, 2, _grid, util);
            newAmo.selectRandomDirection( agent.orientation);
            //Amoeba newAmo = new Amoeba(_current_population - 1, _sense_angle, _rotate_angle, _sense_offset, _detectDir, _death_distance, _speed, _pcd, _depT);
            //Point3d birthPlace = _grid.getRandomBirthPlace(util);
            //newAmo.initializeAmoeba((int) birthPlace.X, (int) birthPlace.Y, (int) birthPlace.Z, 3, _grid, util);
            //newAmo._guide_factor = guide_factor;
            newAmo.orientationFromIndexOrientation(env);
            _toborn_population.Add(newAmo);

        }
        public void shuffleOrder()
        {
            Random rd = new Random((int)DateTime.Now.Ticks);
            population.OrderBy(x => rd.Next());
        }
        public void Update(AbstractEnvironmentType env)
        {
            updatePopulation(env);
            
            updateTrails(env);
            
            if (_step % PhysaSetting._death_frequency_test == 0)
                doDeathTest(env);
            if (_step % PhysaSetting._division_frequency_test == 0)
                doDivisionTest(env);
            foreach (Amoeba tb in _toborn_population)
                population.Add(tb);
            foreach (int tdid in _todie_id)
                population.Remove(population.Where(a => a.ID == tdid).First());
            _toborn_population = new List<Amoeba>();
            _todie_id = new List<int>();
            _step++;
        }
        public void Clear() 
        {
            population.Clear();
            _toborn_population.Clear();
            _todie_id.Clear();
        
        }
        public void Dispose()
        {
            Clear();
        }

        public override IGH_Goo Duplicate()
        {
            return new Physarum(this);
        }

        public override bool IsValid
        {
            get { return true; }
        }

        public override string ToString()
        {

            return TypeName + "\n current population: " + population.Count;
        }

        public override string TypeDescription
        {
            get { return "Physarealm.Physarum"; }
        }

        public override string TypeName
        {
            get { return "Physarealm.Physarum"; }
        }
    }//end of Phy
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physarealm
{
    public class Libutility
    {
        public float[] sinlut { private set; get; }
        public float[] coslut { private set; get; }
        public Random rand;
        public int trailIncrementMethodSelector { get; set; }
        public float negaprop_factor { set; get; }
        public float invprop_factor { set; get; }
        public float log_factor { get; set; }
        public float incrementMinThreshold { get; set; }
        delegate float trailIncrementHandler(int distance_traveled, int death_distance);
        trailIncrementHandler[] trailMethods = new trailIncrementHandler[3];

        public Libutility()
        {
            sinlut = new float[360];
            coslut = new float[360];
            rand = new Random(DateTime.Now.Second);
            for (int i = 0; i < 360; i++)
            {
                sinlut[i] = (float)Math.Sin(i * 3.1416 / 180);
                coslut[i] = (float)Math.Cos(i * 3.1416 / 180);
            }
            negaprop_factor = (float)1.3;
            invprop_factor = 3;
            log_factor = 3;
            trailMethods[0] = negaproportion;
            trailMethods[1] = inverseproportion;
            trailMethods[2] = logrithm;
            trailIncrementMethodSelector = 0;
            incrementMinThreshold = (float)0.2;
        }
        public int getRand(int max)
        {
            return rand.Next(max);
        }
        public int getRand(int min, int max)
        {
            return rand.Next(min, max);
        }
        public double getDoubleRand(double min, double max) 
        {
            return rand.NextDouble() * (max - min) + min;
        }
        public double getRandDouble()
        {
            return rand.NextDouble();
        }
        private float negaproportion(int distance_traveled, int death_distance)
        {
            return death_distance - distance_traveled * negaprop_factor;
        }
        private float inverseproportion(int distance_traveled, int death_distance)
        {
            return invprop_factor / (distance_traveled + 1);
        }
        private float logrithm(int distance_traveled, int death_distance)
        {
            return (float)(log_factor / Math.Log(distance_traveled + 2));
        }
        public float getIncrement(int distance_traveled, int death_distance)
        {
            float ret = trailMethods[trailIncrementMethodSelector](distance_traveled, death_distance);
            if (ret < incrementMinThreshold)
                return incrementMinThreshold;
            return ret;
        }

    }//end of Libutility
}

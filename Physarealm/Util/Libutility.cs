using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physarealm
{
    public class Libutility
    {
        public static float[] sinlut = new float[360];
        public static float[] coslut = new float[360];
        public static Random rand = new Random(DateTime.Now.Second);
        public static int trailIncrementMethodSelector { get; set; }
        public static float negaprop_factor { set; get; }
        public static float invprop_factor { set; get; }
        public static float log_factor { get; set; }
        public static float incrementMinThreshold { get; set; }
        delegate float trailIncrementHandler(int distance_traveled, int death_distance);
        static trailIncrementHandler[] trailMethods = new trailIncrementHandler[3];

        public Libutility()
        {
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
        static public int getRand(int max)
        {
            return rand.Next(max);
        }
        static public int getRand(int min, int max)
        {
            return rand.Next(min, max);
        }
        static public double getDoubleRand(double min, double max) 
        {
            return rand.NextDouble() * (max - min) + min;
        }
        static public double getRandDouble()
        {
            return rand.NextDouble();
        }
        static private float negaproportion(int distance_traveled, int death_distance)
        {
            return death_distance - distance_traveled * negaprop_factor;
        }
        static private float inverseproportion(int distance_traveled, int death_distance)
        {
            return invprop_factor / (distance_traveled + 1);
        }
        static private float logrithm(int distance_traveled, int death_distance)
        {
            return (float)(log_factor / Math.Log(distance_traveled + 2));
        }
        static public float getIncrement(int distance_traveled, int death_distance)
        {
            float ret = trailMethods[trailIncrementMethodSelector](distance_traveled, death_distance);
            if (ret < incrementMinThreshold)
                return incrementMinThreshold;
            return ret;
        }

    }//end of Libutility
}

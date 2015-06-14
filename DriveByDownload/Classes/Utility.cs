using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DriveByDownload.Classes
{
    public class Utility
    {
        static Utility _Instance;


        private Utility() 
        { 
            

        }

        public static Utility Instance
        {
            get
            {
                if (_Instance == null) _Instance = new Utility();
                return _Instance;
            }
        }

        public static Boolean SetBoolByProbability(Double probability)
        {
            Double randy = Globals.Randy.NextDouble();
            return randy <= probability;
        }

        public static double GenerateObjectIndex(double randy, List<double> probabilities)
        {
            //Returns the key to choose a certain type of object in a dictionary <double,Object>  
            //Assumes that probabilities[0] is the upper bound of the first range
            //If rnd is between the two bounds then return the upper one
            int i = 0;
            double lower, upper = 0;
            probabilities.Sort();
            foreach (double k in probabilities)
            {
                lower = i == 0 ? 0 : upper;
                upper = k;

                if (randy >= lower && randy <= upper)
                {
                    return k;
                }
                i++;

            }
            //For distributions with "other" then this will be the final key  
            return 1.0;
        }
    }

    //Singleton class, to make sure we use the same random instance across the application.
    //Continually using the new one causes predictable number generation, 
    //see remarks at http://msdn.microsoft.com/en-us/library/system.random.aspx
    public class SingleRandom : Random
    {
        [ThreadStatic]
        //static SingleRandom _instance;
        public int Seed;
                 
        //public static SingleRandom Instance
        //{
        //    get
        //    {
                
        //        if (_instance == null)
        //        {
        //            int seed = Globals.SEED;
        //            _instance = new SingleRandom(seed);
        //            Console.WriteLine("Seed: " + Globals.Randy.Seed.ToString());
        //        }
        //        return _instance;
        //    }
        //    set { }
        //}

        //public static void RefreshSeed(int seed)
        //{
        //    Globals.Randy = null;
        //    Globals.Randy = new SingleRandom(seed);
        //}

        public SingleRandom(int seed) : base(seed) {
            this.Seed = seed;
            System.Diagnostics.Debug.WriteLine("Seed for this simulation is " + seed);
        }
    }
}

using StereoKit;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;

namespace fitness_game
{
    
    class Program
    {
        
        static void Main(string[] args)
        {
            App app = new App();

            if (!SK.Initialize(app.Settings))
                Environment.Exit(1);

            app.Init();

            SK.Run(app.Step, () => Log.Info("Bye!"));
        }
    }
}

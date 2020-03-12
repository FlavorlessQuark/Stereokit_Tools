using System;
using StereoKit;
using System.Collections.Generic;

namespace StereoKitProjectTest
{

    class Program
    {
        public static bool Grow(Model mod)
        {
            Console.WriteLine("\nGrowing " + mod.GetType());
            return true;
        }
        static void Main(string[] args)
        {
            if (!StereoKitApp.Initialize("MessingWithBounds", Runtime.MixedReality))
                Environment.Exit(1);
            // Model mod = Model.FromFile("‪C:\\Users\\evil_\\Desktop\\Balloon.glb", Default.Shader);
            Model mod = Model.FromMesh(Mesh.GenerateSphere(0.1f, 3), Default.Material);
            Touchable cube;
            Action act = () => Console.WriteLine("touched");
            Action act2 = () => Console.WriteLine("released");
           // Action a = (mod) => { Console.WriteLine("\nGrowing " + mod.GetType()); };
            var method = (Func<Model, bool>)Grow;
            object[] arg = new object[1];
            arg[0] = mod;
            cube = new Touchable(ref mod);
            cube.OnClick(method, arg);
           // Model cube = Model.FromMesh(
              // Mesh.GenerateRoundedCube(Vec3.One, 0.2f),
              //  Default.Material);
           // Bounds cubecol = cube.Bounds;
            while (StereoKitApp.Step(() =>
            {
                cube.Update();
               // mod.Draw(Matrix.TS(Vec3.Zero, 0.1f));
            })) ;
            StereoKitApp.Shutdown();
        }
    }
}

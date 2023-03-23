using StereoKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fitness_game
{

    enum GameState
    {
        Idle,
        Setup,
        Playing
    }

    class App
    {
        SKSettings settings = new SKSettings
        {
            appName = "WallTap",
            assetsFolder = "Assets",
            logFilter = LogLevel.Diagnostic
        };
        public SKSettings Settings => settings;

        // Floor.
        Model floorMesh;
        Matrix floorTransform;
        Material floorMaterial;

        // Log Window.
        Pose logPose;
        List<string> logList;
        string logText;

        GameState state;
        List<Cube> cubes;
        bool debugOn;

        public App()
        {
            
        }

        public void Init()
        {
            floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
            floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;

            logPose = new Pose();
            // Position the log window right in front of the user's center of the eyes.
            // Set the window to face the user center of the eyes.
            logPose.position = Input.Head.position + Input.Head.Forward * 0.5f;
            logPose.orientation = Quat.LookAt(logPose.position, Input.Head.position);
            logList = new List<string>();
            logText = "";

            state = GameState.Setup;
            cubes = new List<Cube>();
            debugOn = true;

            Log.Subscribe(OnLog);
        }

        public void Step()
        {
            if (SK.System.displayType == Display.Opaque)
                Default.MeshCube.Draw(floorMaterial, floorTransform);

            LogWindow();
            DrawWorldOriginIndicator();
            UI.ShowVolumes = debugOn;

            DrawHandMenu(Handed.Left);

            // Draw cubes.
            foreach (var cube in cubes)
            {
                cube.Step();
            }
        }

        void DrawWorldOriginIndicator()
        {
            Lines.AddAxis(new Pose(0, 0, 0.0f, Quat.Identity));
        }

        void OnLog(LogLevel level, string text)
        {
            if (logList.Count > 15)
                logList.RemoveAt(logList.Count - 1);
            logList.Insert(0, text.Length < 100 ? text : text.Substring(0, 100) + "...\n");

            logText = "";
            for (int i = 0; i < logList.Count; i++)
                logText += logList[i];
        }

        bool HandFacingHead(Handed handed)
        {
            Hand hand = Input.Hand(handed);
            if (!hand.IsTracked)
            {
                return false;
            }

            Vec3 palmDirection = (hand.palm.Forward).Normalized;
            Vec3 directionToHead = (Input.Head.position - hand.palm.position).Normalized;

            return Vec3.Dot(palmDirection, directionToHead) > 0.5f;
        }

        void DrawHandMenu(Handed handed)
        {
            if (!HandFacingHead(handed))
            {
                return;
            }

            Vec2 size = new Vec2(4, 16);
            float offset = handed == Handed.Left ? -2 - size.x : 2 + size.x;

            Hand hand = Input.Hand(handed);
            Vec3 at = hand[FingerId.Little, JointId.KnuckleMajor].position;
            Vec3 down = hand[FingerId.Little, JointId.Root].position;
            Vec3 across = hand[FingerId.Index, JointId.KnuckleMajor].position;

            Pose menuPose = new Pose(at, Quat.LookAt(at, across, at - down) * Quat.FromAngles(0, handed == Handed.Left ? 90 : -90, 0));
            menuPose.position += menuPose.Right * offset * U.cm;
            menuPose.position += menuPose.Up * (size.y / 2) * U.cm;

            UI.WindowBegin("HandMenu", ref menuPose, size * U.cm, UIWin.Empty);
            
            if (UI.Button("New Cube"))
            {
                Cube cube = new Cube();
                cubes.Add(cube);
            } else if (UI.Button("Toggle Debug"))
            {
                debugOn = !debugOn;
            }
            UI.WindowEnd();
        }

        void LogWindow()
        {
            UI.WindowBegin("Log", ref logPose, new Vec2(40, 0) * U.cm);
            UI.Text(logText);
            UI.WindowEnd();
        }
    }
}

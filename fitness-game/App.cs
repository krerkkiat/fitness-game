using RestSharp.Authenticators;
using RestSharp;
using StereoKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using RestSharp;
using RestSharp.Authenticators;

namespace fitness_game
{

    enum GameState
    {
        Idle,
        Setup,
        Playing
    }

    class Tap
    {
        public int cubeId;
        public Cube cube;
        public float timestamp;

        public Tap(int cubeId, Cube cube, float timestamp) {
            this.cubeId = cubeId;
            this.cube = cube;
            this.timestamp = timestamp;
        }
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

        // MainMenu
        bool showMainMenu;
        MainMenu mainMenu;

        GameState state;
        int cubeRunningId;
        List<Cube> cubes;
        bool debugOn;

        List<Tap> taps;

        public App()
        {
            
        }

        public async void Init()
        {
            floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
            floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
            floorMaterial.Transparency = Transparency.Blend;

            logPose = new Pose();
            // Position the log window right in front of the user's center of the eyes.
            // Set the window to face the user center of the eyes.
            logPose.position = Input.Head.position + Input.Head.Forward * 0.5f + Input.Head.Up * 0.7f;
            logPose.orientation = Quat.LookAt(logPose.position, Input.Head.position);
            logList = new List<string>();
            logText = "";

            // MainMenu Init
            mainMenu = new MainMenu(this);

            state = GameState.Idle;
            cubeRunningId = 0;
            cubes = new List<Cube>();
            debugOn = false;
            showMainMenu = true;

            Log.Subscribe(OnLog);

            // Testing http client.
            try
            {
                var client = new Api.ApiV1Client();
                var seqs = await client.GetSequences();
                foreach (var seq in seqs)
                {
                    Log.Info(seq.Username + ":" + seq.Cubes.Length.ToString());
                }
            } catch (Exception ex)
            {
                Log.Err(ex.ToString());
            }
        }

        public void Step()
        {
            if (SK.System.displayType == Display.Opaque)
                Default.MeshCube.Draw(floorMaterial, floorTransform);

            LogWindow();
            DrawWorldOriginIndicator();
            UI.ShowVolumes = debugOn;

            if (showMainMenu)
            {
                mainMenu.Step();
            }

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
            
            if (UI.Button("Toggle Main Menu"))
            {
                showMainMenu = !showMainMenu;
            }
            if (UI.Button("New Cube"))
            {
                Cube cube = new Cube(this, cubeRunningId);
                cubes.Add(cube);
                cubeRunningId += 1;
            }
            if (UI.Button("Toggle Debug"))
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

        public void SetGameState(GameState state)
        {
            this.state = state;
        }

        public GameState GetGameState()
        {
            return state;
        }

        public void AddCube(Cube cube)
        {
            cubes.Add(cube);
            cubeRunningId += 1;
        }

        public int GetCubeRunningId()
        {
            return cubeRunningId;
        }

        public void StartPlay() {
            state = GameState.Playing;
            taps = new List<Tap>();
        }

        public void StopPlay() {
            state = GameState.Idle;
            // TODO: Record the sequences.
        }

        public void TapCube(int cubeId, Cube cube, float timeSinceLastTap) {
            taps.Add(new Tap(cubeId, cube, timeSinceLastTap));
        }
    }
}

using fitness_game.Api;
using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace fitness_game
{
    enum MainMenuPage
    {
        Main,
        ListSequence,
        SpawnControl,
    }

    class MainMenu
    {
        App parent;

        Model clipboard;
        Pose pose;
        float size;

        MainMenuPage page;

        public MainMenu(App parent)
        {
            this.parent = parent;

            // Load the model.
            clipboard = Model.FromFile("Clipboard.glb", Default.ShaderUI);

            // Default pose is in-front of the user.
            pose = new Pose();
            pose.position = Input.Head.position + Input.Head.Forward * 0.5f;
            pose.orientation = Quat.LookAt(pose.position, Input.Head.position);
            size = 0.2f;

            page = MainMenuPage.Main;
        }

        public void Init() { }

        public void Step()
        {
            UI.HandleBegin("MainMenu", ref pose, clipboard.Bounds);
            clipboard.Draw(Matrix.Identity);

            UI.LayoutArea(new Vec3(12, 15, 0) * U.cm, new Vec2(24, 30) * U.cm);

            UI.Text("Fitness Game", TextAlign.TopCenter);
            UI.HSeparator();

            if (page == MainMenuPage.Main)
            {
                StepMainPage();
            } else if (page == MainMenuPage.SpawnControl)
            {
                StepSpawnControl();
            } else if (page == MainMenuPage.ListSequence)
            {
                StepListSequencePage();
            }

            UI.HandleEnd();
        }

        public void StepMainPage()
        {
            if (UI.Button("New sequence"))
            {
                page = MainMenuPage.SpawnControl;
            }

            if (UI.Button("Browse sequences"))
            {
                page = MainMenuPage.ListSequence;
            }

            if (UI.Button("Quit"))
            {
                SK.Quit();
            }
        }

        public void StepSpawnControl()
        {
            if (UI.Button("Add cube"))
            {
                Cube cube = new Cube(parent, parent.GetCubeRunningId());
                parent.AddCube(cube);
            }

            if (parent.GetGameState() == GameState.Idle)
            {
                if (UI.Button("Play"))
                {
                    parent.StartPlay();
                }
            } else
            {
                if (UI.Button("Stop"))
                {
                    parent.StopPlay();
                }
            }

            if (UI.Button("Upload"))
            {
                var task = Task.Run(parent.UploadSequence);
            }

            if (UI.Button("Back"))
            {
                page = MainMenuPage.Main;
            }
        }

        public void StepListSequencePage()
        {
            if (UI.Button("Seq #1"))
            {
                
            }

            if (UI.Button("Seq #2"))
            {

            }

            if (UI.Button("Back"))
            {
                page = MainMenuPage.Main;
            }
        }
    }
}

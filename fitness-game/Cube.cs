using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Notifications;

namespace fitness_game
{

    class Cube
    {
        App parent;

        Pose pose;
        float size;

        BtnState lastFrameBtnState;
        float timeSinceLastTap;

        int id;
        int tapCount;

        Mesh cubeMesh;
        Model cubeModel;

        public Cube(App parent, int id)
        {
            this.parent = parent;
            this.id = id;

            // Default pose is in-front of the user.
            pose = new Pose();
            pose.position = Input.Head.position + Input.Head.Forward * 0.5f + Input.Head.Right * 0.5f;
            pose.orientation = Quat.LookAt(pose.position, Input.Head.position);

            size = 0.2f;
            timeSinceLastTap = 0.0f;
            tapCount = 0;

            // Use the pre-generated mesh instead of Mesh.Generate since they say
            // that function will generate new mesh on the GPU on the fly.
            // And this cube is dynamically generated.
            cubeModel = Model.FromMesh(Default.MeshCube, Default.MaterialUIBox);
        }

        public Cube(App parent, int id, Pose pose)
        {
            this.parent = parent;
            this.id = id;
            this.pose = pose;
        }

        public void Init() {}

        public void Step()
        {
            UI.HandleBegin($"Cube {id}", ref pose, cubeModel.Bounds * size);
            cubeModel.Draw(Matrix.TS(cubeModel.Bounds.center * size, cubeModel.Bounds.dimensions * size));
            if (parent.GetGameState() == GameState.Idle)
            {
                StepMove();
            } else if (parent.GetGameState() == GameState.Playing)
            {
                StepTap();
            }

            UI.HandleEnd();
        }

        public void StepMove()
        {
            
            
        }

        public void StepTap()
        {
            Text.Add(tapCount.ToString(), Matrix.T(cubeModel.Bounds.center * size));

            BtnState volumeState = UI.VolumeAt($"Cube-{id}-volume", cubeModel.Bounds * size, UIConfirm.Push, out Handed hand, out BtnState focusState);
            if (volumeState != BtnState.Inactive)
            {
                // If it just changed interaction state, make it jump in size
                float scale = volumeState.IsChanged()
                    ? 0.1f
                    : 0.05f;
                Lines.AddAxis(Input.Hand(hand)[FingerId.Index, JointId.Tip].Pose, scale);
            }

            if (lastFrameBtnState != null)
            {
                if (lastFrameBtnState.IsActive() && !volumeState.IsActive())
                {
                    Log.Info(hand.ToString() + " hand lefts a cube");

                    parent.TapCube(id, this, timeSinceLastTap);
                    tapCount += 1;
                    timeSinceLastTap = 0.0f;
                }
            }

            lastFrameBtnState = volumeState;
            timeSinceLastTap += Time.Elapsedf;
        }
    }
}

using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fitness_game
{
    class Cube
    {
        Pose pose;
        float size;
        BtnState lastFrameBtnState;

        public Cube()
        {
            // Default pose is in-front of the user.
            pose = new Pose();
            pose.position = Input.Head.position + Input.Head.Forward * 0.5f;
            pose.orientation = Quat.LookAt(pose.position, Input.Head.position);

            size = 0.2f;
        }

        public Cube(Pose pose)
        {
            this.pose = pose;
        }

        public void Init() {}

        public void Step()
        {
            Default.MeshCube.Draw(Default.MaterialUIBox, Matrix.TS(pose.position, size));

            BtnState volumeState = UI.VolumeAt("Volume", new Bounds(pose.position, Vec3.One * size), UIConfirm.Push, out Handed hand, out BtnState focusState);
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
                }
            }
            lastFrameBtnState = volumeState;
        }
    }
}

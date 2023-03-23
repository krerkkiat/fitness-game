using StereoKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace fitness_game
{

    class Cube
    {
        Pose pose;
        float size;

        BtnState lastFrameBtnState;
        float timeSinceLastTap;

        int id;
        List<float> taps;

        public Cube(int id)
        {
            // Default pose is in-front of the user.
            pose = new Pose();
            pose.position = Input.Head.position + Input.Head.Forward * 0.5f;
            pose.orientation = Quat.LookAt(pose.position, Input.Head.position);

            size = 0.2f;
            timeSinceLastTap = 0.0f;

            this.id = id;
            taps = new List<float>();
        }

        public Cube(Pose pose)
        {
            this.pose = pose;
        }

        public void Init() {}

        public void Step()
        {
            Default.MeshCube.Draw(Default.MaterialUIBox, Matrix.TS(pose.position, size));
            Text.Add(taps.Count.ToString(), Matrix.T(pose.position));

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

                    taps.Add(timeSinceLastTap);
                    timeSinceLastTap = 0.0f;
                }
            }

            lastFrameBtnState = volumeState;
            timeSinceLastTap += Time.Elapsedf;
        }
    }
}

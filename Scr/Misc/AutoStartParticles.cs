using Godot;

namespace JumpRun.Scr.Misc
{
    public class AutoStartParticles : Particles2D
    {
        public override void _EnterTree()
        {
            Emitting = true;
        }

        private void DeleteThis()
        {
            QueueFree();
        }
    }
}
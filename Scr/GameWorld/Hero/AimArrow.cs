using Godot;

namespace JumpRun.Scr.GameWorld.Hero
{
    public class AimArrow : Sprite
    {
        private Hero parentHero;

        public override void _Ready()
        {
            parentHero = GetParent<Hero>();
        }

        public override void _Process(float delta)
        {
            if (parentHero.AimAngle == null)
            {
                Visible = false;
                return;
            }
            Rotation = (float)parentHero.AimAngle;
            Visible = true;
        }
    }
}
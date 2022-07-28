using Godot;
using JumpRun.Scr.GameWorld.Hero;

namespace JumpRun.Scr.GameWorld.Objects
{
    public class FairyDustBucket : Area2D
    {
        [Export]
        private NodePath npTimer, npParticles;
        [Export]
        private PackedScene psHeroSpaceShip;
        private bool active = true;

        private void OnAreaEntered(Node body)
        {
            if (!active) { return; }
            if (body is IHero)
            {
                if (body is KinematicPlatformer hero)
                {
                    hero.ChangeInto(psHeroSpaceShip.Instance<HeroSpaceShip>());
                    GetNode<Particles2D>(npParticles).Emitting = false;
                    active = false;
                    GetNode<Timer>(npTimer).Start();
                }
            }
        }

        private void OnRespawnTimer()
        {
            GetNode<Particles2D>(npParticles).Emitting = true;
            active = true;
        }
    }
}
using System;
using Godot;

namespace JumpRun.Scr.GameWorld.Hero
{
    public class HeroReference
    {
        private IHero current;
        public IHero Current => current;
        public event EventHandler<RegisterCurrentHeroArgs> NewCurrentHeroSet;

        public HeroReference(IHero initialHero)
        {
            current = initialHero;
        }

        public void ChangeHero(IHero newForm, bool keepMomentum = true)
        {
            Node2D currentNode = (Node2D)current;
            ((Node2D)newForm).GlobalPosition = currentNode.GlobalPosition;
            if (keepMomentum)
            {
                Vector2 momentum = new Vector2();
                if (current is KinematicPlatformer kinematicPlatformer)
                {
                    momentum = kinematicPlatformer.Momentum;
                }
                if (current is RigidBody2D rigidBody)
                {
                    momentum = rigidBody.LinearVelocity;
                }

                if (momentum != Vector2.Zero)
                {
                    if (newForm is KinematicPlatformer newKinematicPlatformer)
                    {
                        newKinematicPlatformer.Momentum = momentum;
                    }
                    if (newForm is RigidBody2D newRigidBody)
                    {
                        newRigidBody.LinearVelocity = momentum;
                    }
                }

            }
            GameController.Current.AddChildDeferred((Node2D)newForm);
            currentNode.QueueFree();
            current = newForm;
            current.HRef = this;
            NewCurrentHeroSet.Invoke(this, new RegisterCurrentHeroArgs((IHero)newForm));
        }
    }
}
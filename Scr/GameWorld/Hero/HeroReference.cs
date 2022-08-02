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
            if (keepMomentum && newForm is KinematicPlatformer kinematicPlatformer && current is KinematicPlatformer kinCurrent)
            {
                kinematicPlatformer.Momentum = kinCurrent.Momentum;
            }
            currentNode.GetParent().AddChild((Node2D)newForm);
            currentNode.QueueFree();
            current = newForm;
            current.HRef = this;
            NewCurrentHeroSet.Invoke(this, new RegisterCurrentHeroArgs((IHero)newForm));
        }
    }
}
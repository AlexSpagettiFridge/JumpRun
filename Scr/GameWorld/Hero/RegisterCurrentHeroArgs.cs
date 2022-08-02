using System;

namespace JumpRun.Scr.GameWorld.Hero
{
    public class RegisterCurrentHeroArgs : EventArgs
    {
        public IHero NewCurrentHero;

        public RegisterCurrentHeroArgs(IHero newCurrentHero)
        {
            NewCurrentHero = newCurrentHero;
        }
    }
}
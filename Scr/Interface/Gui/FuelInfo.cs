using Godot;
using JumpRun.Scr.GameWorld.Hero;

namespace JumpRun.Scr.Interface.Gui
{
    public class FuelInfo : Control
    {
        [Export]
        private NodePath npProgressBar;
        private ProgressBar progressBar;
        public HeroSpaceShip TrackedHero;

        public override void _Ready()
        {
            progressBar = GetNode<ProgressBar>(npProgressBar);
        }

        public override void _Process(float delta)
        {
            Transform2D heroTransform = TrackedHero.GetViewportTransform() * TrackedHero.GetGlobalTransform();
            RectPosition = heroTransform.origin;
            progressBar.Value = TrackedHero.Fuel*10;
        }

        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            if (@event.IsActionPressed("ui_focus_next"))
            {
                GD.Print($"VpO:{RectPosition} Po:{TrackedHero.Position}");
            }
        }
    }
}
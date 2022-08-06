using System.Collections.Generic;
using Godot;
using JumpRun.Scr.GameWorld;
using JumpRun.Scr.GameWorld.Hero;

namespace JumpRunPlugin
{
    [Tool]
    public class CameraRail : Node2D
    {
        [Export]
        private NodePath npCamera, npFollowNode;
        private Camera2D camera;
        private Node2D followNode;
        private List<Rect2> areas = new List<Rect2>();

        public override void _Ready()
        {
            if (!Engine.EditorHint)
            {
                camera = GetNode<Camera2D>(npCamera);
                followNode = GetNode<Node2D>(npFollowNode);
                if (followNode is IHero hero)
                {
                    hero.HRef.NewCurrentHeroSet += OnHeroChanged;
                }
            }
        }

        public override void _Process(float delta)
        {
            if (!Engine.EditorHint)
            {
                camera.Position = followNode.Position;
            }
        }

#if TOOLS
        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseButton inputEventMouseButton)
            {
                if (inputEventMouseButton.ButtonIndex == 1 && inputEventMouseButton.Pressed)
                {
                    GD.Print(GetLocalMousePosition());
                    GetTree().SetInputAsHandled();
                }
            }
        }
#endif

        public override void _Draw()
        {
            if (Engine.EditorHint)
            {
                Color color = new Color(0, 0, 1, 0.75f);
                foreach (Rect2 rect in areas)
                {
                    DrawRect(rect, color, false, 2);
                }
            }
        }

        public void OnHeroChanged(object sender, RegisterCurrentHeroArgs args)
        {
            followNode = (Node2D)args.NewCurrentHero;
        }
    }
}
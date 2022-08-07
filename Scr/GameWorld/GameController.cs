using Godot;

namespace JumpRun.Scr.GameWorld
{
    public class GameController : Node2D
    {
        public static GameController Current => current;
        private static GameController current;

        public GameController()
        {
            current = this;
        }

        [Export]
        private NodePath npGui;
        private Control gui;

        public Control Gui => gui;



        public override void _Ready()
        {
            gui = GetNode<Control>(npGui);
        }

        public void AddChildDeferred(Node child) => CallDeferred(nameof(DeferredAddChildCall), new object[] { child });
        private void DeferredAddChildCall(Node child) => AddChild(child);
    }
}
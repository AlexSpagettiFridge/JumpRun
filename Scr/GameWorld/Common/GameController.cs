using Godot;
using JumpRun.Scr.Interface.Gui;

namespace JumpRun.Scr.GameWorld.Common
{
    public class GameController : Node2D
    {
        [Signal]
        public delegate void CharacterSpeaks(string text);
        [Export]
        private NodePath npGui,npCamera;
        private GuiController gui;
        private GameCamera camera;

        public GuiController Gui => gui;
        public GameCamera Camera => camera;

        public static GameController Current => current;
        private static GameController current;

        private int totalCrunchyAmount = 0, crunchyAmount = 0;

        public GameController()
        {
            current = this;
        }

        public override void _Ready()
        {
            gui = GetNode<GuiController>(npGui);
            camera = GetNode<GameCamera>(npCamera);
        }

        public void SetLevelCrunchyAmount(int amount) 
        {
            totalCrunchyAmount = amount;
            crunchyAmount = 0;
            UpdateCrunchyCounter();
        }

        public void OnCrunchyCollected()
        {
            crunchyAmount++;
            UpdateCrunchyCounter();
        }

        private void UpdateCrunchyCounter() => Gui.UpdateCrunchyCounter(crunchyAmount ,totalCrunchyAmount);
        public void AddChildDeferred(Node child) => CallDeferred(nameof(DeferredAddChildCall), new object[] { child });
        private void DeferredAddChildCall(Node child) => AddChild(child);
    }
}
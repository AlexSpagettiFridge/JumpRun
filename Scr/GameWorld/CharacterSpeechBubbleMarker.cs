using Godot;
using JumpRunPlugin.Dialog;

namespace JumpRun.Scr.GameWorld
{
    public class CharacterSpeechBubbleMarker : Node2D
    {
        [Export]
        public DialogCharacter Character = null;

        public override void _EnterTree()
        {
            GameController.Current.Connect(nameof(GameController.CharacterSpeaks), this, nameof(OnCharacterSpeaks));
        }

        private void OnCharacterSpeaks(string text)
        {

        }

    }
}
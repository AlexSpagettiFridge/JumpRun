using Godot;
using JumpRun.Scr.GameWorld.Common;
using JumpRunPlugin.Dialog;

namespace JumpRun.Scr.GameWorld
{
    public class CharacterSpeechBubbleMarker : Node2D
    {
        [Export]
        public DialogCharacter Character = null;

        public override void _EnterTree()
        {
            AddToGroup("speechBubbleMarker");
        }

    }
}
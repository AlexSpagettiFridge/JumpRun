using Godot;

namespace JumpRunPlugin.Dialog
{
    [Tool]
    public class DialogElement : Resource
    {
        [Export]
        public DialogCharacter Character = null;
        [Export]
        public string Text = null;
        [Export]
        public int SpriteFrame = 0;
    }
}
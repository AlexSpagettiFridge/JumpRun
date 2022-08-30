using Godot;

namespace JumpRunPlugin.Dialog
{
    [Tool]
    public class DialogCharacter : Resource
    {
        [Export]
        public string Name = null;
        [Export]
        public SpriteFrames Portrait;
    }
}
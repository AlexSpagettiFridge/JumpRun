using Godot;

namespace JumpRunPlugin.Dialog
{
    [Tool]
    public class DialogScript : Resource
    {
        [Export]
        public DialogElement[] Elements;

        public DialogScriptPlayer CreatePlayer() => new DialogScriptPlayer(this);
    }
}
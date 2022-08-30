using Godot;
using JumpRun.Scr.GameWorld.Hero;
using JumpRunPlugin.Dialog;

namespace JumpRun.Scr.GameWorld.Characters
{
    public class ScriptActivatorArea : Area2D
    {
        [Export]
        public DialogScript ReferencedScript = null;
        private bool alreadyActivated = false;

        public override void _Ready()
        {
            Connect("body_entered", this, nameof(OnBodyEntered));
        }

        public void OnBodyEntered(Node body)
        {
            if (alreadyActivated) { return; }
            if (body is IHero)
            {
                DialogScriptPlayer dialogScriptPlayer = ReferencedScript.CreatePlayer();
                AddChild(dialogScriptPlayer);
                dialogScriptPlayer.Start();
                alreadyActivated = true;
            }
        }
    }
}
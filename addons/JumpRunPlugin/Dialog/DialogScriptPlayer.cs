using Godot;
using JumpRun.Scr.GameWorld.Common;
using JumpRun.Scr.Interface.Gui;

namespace JumpRunPlugin.Dialog
{
    public class DialogScriptPlayer : Node
    {
        private DialogScript followedScript;
        private int elementIndex;

        public DialogScriptPlayer()
        {

        }

        public DialogScriptPlayer(DialogScript followedScript)
        {
            this.followedScript = followedScript;
        }

        public void Start()
        {
            GameController.Current.Gui.GetDialogBox().Connect(nameof(DialogBox.DialogBoxClosed), this, nameof(OnDialogBoxClosed));
            elementIndex = 0;
            NextElement();
        }

        public void NextElement()
        {
            GameController.Current.AddChild(new SpeechBubble(followedScript.Elements[elementIndex]));
            elementIndex++;
            if (elementIndex > followedScript.Elements.Length)
            {
                QueueFree();
            }
        }

        private void OnDialogBoxClosed()
        {
            NextElement();
        }
    }
}
using Godot;

namespace JumpRun.Scr.Interface.Gui
{
    public class GuiController : Control
    {
        [Export]
        private NodePath npCrunchyCounter,npDialogBox;

        public void UpdateCrunchyCounter(int amount,int total)
        {
            GetNode<Label>(npCrunchyCounter).Text=$"{amount} / {total}";
        }

        public DialogBox GetDialogBox() => GetNode<DialogBox>(npDialogBox);
    }
}
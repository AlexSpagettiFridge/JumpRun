using Godot;
using JumpRunPlugin.Dialog;

namespace JumpRun.Scr.Interface.Gui
{
    public class DialogBox : Control
    {
        [Signal]
        public delegate void DialogBoxClosed();
        [Export]
        private NodePath npPortrait, npText, npTimer;
        private Tween tween;

        public override void _Ready()
        {
            KeepPortraitAspect();
            GetNode<TextureRect>(npPortrait).Connect("resized", this, nameof(KeepPortraitAspect));
            GetNode<Timer>(npTimer).Connect("timeout", this, nameof(CloseDialog));
            tween = new Tween();
            AddChild(tween);
        }

        public void ApplyDialogElement(DialogElement element)
        {
            float spellDuration = element.Text.Length * 0.03f;
            RichTextLabel label = GetNode<RichTextLabel>(npText);
            label.BbcodeText = Tr(element.Text);
            tween.InterpolateProperty(label, "percent_visible", 0, 1, spellDuration, Tween.TransitionType.Linear);
            tween.Start();
            GetNode<Timer>(npTimer).Start(3 + spellDuration);
            Visible = true;
        }

        public void CloseDialog()
        {
            Visible = false;
            EmitSignal(nameof(DialogBoxClosed));
        }

        private void KeepPortraitAspect()
        {
            TextureRect portrait = GetNode<TextureRect>(npPortrait);
            portrait.RectMinSize = new Vector2(portrait.RectSize.y, 0);
        }
    }
}
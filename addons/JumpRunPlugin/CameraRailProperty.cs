using Godot;

namespace JumpRunPlugin
{
    public class CameraRailProperty : EditorProperty
    {
        public override void _Ready()
        {
            Label = "Rail Path";
            VBoxContainer vBoxContainer = new VBoxContainer();
            Label label = new Label();
            label.Text = "Camera Rail:";
            vBoxContainer.AddChild(label);
            //HBoxContainer
            HBoxContainer hBoxContainer = new HBoxContainer();
            Button newRailButton = new Button();
            newRailButton.Text = "+";
            hBoxContainer.AddChild(newRailButton);

            vBoxContainer.AddChild(hBoxContainer);
            AddChild(vBoxContainer);
        }

        public override void UpdateProperty()
        {

        }
    }
}
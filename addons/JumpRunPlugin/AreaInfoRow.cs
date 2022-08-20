using Godot;

namespace JumpRunPlugin
{
    public class AreaInfoRow : HBoxContainer
    {
        private int areaId = 0;
        private CameraRail referencedCameraRail = null;

        public AreaInfoRow() { }
        public AreaInfoRow(int areaId, CameraRail referencedCameraRail)
        {
            this.areaId = areaId;
            this.referencedCameraRail = referencedCameraRail;
        }

        public override void _EnterTree()
        {
            Rect2 area = referencedCameraRail.GetAreaById(areaId);
            Label startLabel = new Label();
            startLabel.Text = area.Position.ToString();
            startLabel.SizeFlagsHorizontal = (int)SizeFlags.Expand;
            AddChild(startLabel);
            Label endLabel = new Label();
            endLabel.Text = area.End.ToString();
            endLabel.SizeFlagsHorizontal = (int)SizeFlags.Expand;
            AddChild(endLabel);

            Button deleteButton = new Button();
            deleteButton.Text = "x";
            deleteButton.Connect("pressed", this, nameof(OnDeleteButtonPressed));
            AddChild(deleteButton);

            Connect("mouse_entered", this, nameof(OnMouseEntered));
            Connect("mouse_exited", this, nameof(OnMouseExited));
        }

        private void OnMouseEntered()
        {
            referencedCameraRail.CurrentlySelectedAreaId = areaId;
        }

        private void OnMouseExited()
        {
            if (referencedCameraRail.CurrentlySelectedAreaId == areaId)
            {
                referencedCameraRail.CurrentlySelectedAreaId = -1;
            }
        }

        private void OnDeleteButtonPressed()
        {
            referencedCameraRail.RemoveAreaById(areaId);
        }
    }
}
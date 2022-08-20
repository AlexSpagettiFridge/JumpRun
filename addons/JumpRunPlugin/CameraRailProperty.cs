using System.Collections.Generic;
using Godot;

namespace JumpRunPlugin
{
    public class CameraRailProperty : EditorProperty
    {
        private JumpRunPlugin plugin;
        private CameraRail referencedCameraRail;
        private VBoxContainer areaInfoContainer;
        private VBoxContainer mainContainer;

        public CameraRailProperty(JumpRunPlugin plugin, CameraRail referencedCameraRail)
        {
            this.plugin = plugin;
            this.referencedCameraRail = referencedCameraRail;
            referencedCameraRail.Connect(nameof(CameraRail.AreasChanged), this, nameof(OnCameraRailAreasChanged));
        }

        private CameraRailProperty() { }

        public override void _Ready()
        {
            Label = "Rail Area";
            mainContainer = new VBoxContainer();
            //ButtonRow
            HBoxContainer hBoxContainer = new HBoxContainer();
            Button newRailButton = new Button();
            newRailButton.Text = "Add Area";
            newRailButton.Connect("pressed", this, nameof(OnNewRailButtonPressed));
            hBoxContainer.AddChild(newRailButton);

            hBoxContainer.SizeFlagsVertical = (int)SizeFlags.Expand;
            mainContainer.AddChild(hBoxContainer);
            AddChild(mainContainer);

            UpdateAreaInfoContainer();
        }

        public void UpdateAreaInfoContainer()
        {
            if (areaInfoContainer != null) { areaInfoContainer.QueueFree(); }
            areaInfoContainer = new VBoxContainer();
            areaInfoContainer.SizeFlagsVertical = (int)SizeFlags.Expand;
            for (int i = 0; i < referencedCameraRail.GetAreaCount(); i++)
            {
                areaInfoContainer.AddChild(new AreaInfoRow(i, referencedCameraRail));
            }
            mainContainer.AddChild(areaInfoContainer);
        }

        public override void UpdateProperty()
        {

        }

        public void OnNewRailButtonPressed()
        {
            plugin.StartCreatingRect(referencedCameraRail);
        }

        private void OnCameraRailAreasChanged(List<Rect2> areas)
        {
            UpdateAreaInfoContainer();
        }
    }
}
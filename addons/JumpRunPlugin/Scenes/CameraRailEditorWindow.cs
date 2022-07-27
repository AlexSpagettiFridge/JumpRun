#if TOOLS
using Godot;

namespace JumpRunPlugin.Scenes
{
    public class CameraRailEditorWindow : Control
    {
        public CameraRailEditorWindow(Node sceneRoot = null)
        {
            if (sceneRoot == null)
            {
                ShowInvalidDialog();
                return;
            }

            Color[] colors = new Color[] { new Color(0, 0, 0, 1), new Color(0, 0, 0.5f, 1), new Color(0, 0.45f, 0, 1) };
            int numberTileMaps = 0;
            bool hasCameraRail = false;
            foreach (Node node in sceneRoot.GetChildren())
            {
                if (node is CameraRail)
                {
                    //Do things
                    hasCameraRail = true;
                }
                if (node is TileMap tileMap)
                {

                    AddChild(new MiniTilemap(tileMap, colors[numberTileMaps]));
                    numberTileMaps++;
                }
            }
            if (numberTileMaps == 0 || !hasCameraRail) { ShowInvalidDialog(); }

        }

        public override void _Ready()
        {
            AnchorRight = 1;
            AnchorBottom = 1;
            RectClipContent = true;
        }

        private void ShowInvalidDialog()
        {
            AcceptDialog popupDialog = new AcceptDialog();
            popupDialog.DialogText = "Scene is not valid.";
            popupDialog.PopupExclusive = true;
            popupDialog.Connect("confirmed", this, nameof(OnInvalidDialogAccepted));
        }

        public void OnInvalidDialogAccepted()
        {
            QueueFree();
        }
    }
}
#endif
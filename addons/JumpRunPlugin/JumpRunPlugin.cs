#if TOOLS
using Godot;
using JumpRunPlugin.Scenes;

namespace JumpRunPlugin
{
    [Tool]
    public class JumpRunPlugin : EditorPlugin
    {
        public override string GetPluginName() => "JumpRunPlugin";

        private CamerarailInspectorPlugin camerarailInspectorPlugin;
        private Script cameraRailScript = ResourceLoader.Load<Script>("res://addons/JumpRunPlugin/CameraRail.cs");
        private EditState currentEditState = EditState.None;
        private Vector2 railRectStart;
        private CameraRail editedRail = null;

        public override void _EnterTree()
        {
            Script cameraRailScript = ResourceLoader.Load<Script>("res://addons/JumpRunPlugin/CameraRail.cs");
            Script dialogSciptScript = ResourceLoader.Load<Script>("res://addons/JumpRunPlugin/Dialog/DialogScript.cs");
            Script dialogElementScript = ResourceLoader.Load<Script>("res://addons/JumpRunPlugin/Dialog/DialogElement.cs");
            Script dialogCharacterScript = ResourceLoader.Load<Script>("res://addons/JumpRunPlugin/Dialog/DialogCharacter.cs");
            Texture cameraRailIcon = ResourceLoader.Load<Texture>("res://Gfx/Icons/CameraRailIcon.svg");
            camerarailInspectorPlugin = new CamerarailInspectorPlugin(this);
            AddInspectorPlugin(camerarailInspectorPlugin);
            AddCustomType("CameraRail", "Node2D", cameraRailScript, cameraRailIcon);
            AddCustomType("DialogScript", "Resource", dialogSciptScript, cameraRailIcon);
            AddCustomType("DialogElement", "Resource", dialogElementScript, cameraRailIcon);
            AddCustomType("DialogCharacter", "Resource", dialogCharacterScript, cameraRailIcon);

            GD.Print("--- JumpRunPlugin Initialized ---");
        }

        public override void _ExitTree()
        {
            RemoveInspectorPlugin(camerarailInspectorPlugin);
            RemoveCustomType("CameraRail");
            RemoveCustomType("DialogScript");
            RemoveCustomType("DialogElement");
            RemoveCustomType("DialogCharacter");
        }

        public override void _Process(float delta)
        {
            UpdateOverlays();
        }

        public override Texture GetPluginIcon() => ResourceLoader.Load<Texture>("res://Gfx/Icons/PluginIcon.svg");

        public override bool Handles(Object @object) => @object.GetScript() == cameraRailScript;

        public override bool ForwardCanvasGuiInput(InputEvent @event)
        {
            if (currentEditState == EditState.None) { return false; }
            if (@event is InputEventMouseButton eventMouseButton)
            {
                if (eventMouseButton.ButtonIndex == 1 && eventMouseButton.Pressed)
                {
                    Vector2 gridMousePosition = (editedRail.GetLocalMousePosition() / 16).Floor() * 16;
                    switch (currentEditState)
                    {
                        case EditState.MarkStart:
                            railRectStart = gridMousePosition;
                            currentEditState = EditState.DragEnd;
                            break;
                        case EditState.DragEnd:
                            currentEditState = EditState.None;
                            Edit(editedRail);
                            editedRail.AddArea(new Rect2(railRectStart, gridMousePosition-railRectStart));
                            break;
                    }
                    return true;
                }
            }
            return false;
        }

        public void StartCreatingRect(CameraRail cameraRail)
        {
            currentEditState = EditState.MarkStart;
            editedRail = cameraRail;
        }

        private enum EditState
        {
            None, MarkStart, DragEnd
        }

        public override void ForwardCanvasDrawOverViewport(Control overlay)
        {
            if (currentEditState == EditState.DragEnd)
            {
                Color color = new Color(0, 1, 0, 1);
                Rect2 dragRect = new Rect2(railRectStart, editedRail.GetLocalMousePosition()-railRectStart);
                overlay.DrawRect(dragRect, color, false);
            }
        }
    }
}
#endif
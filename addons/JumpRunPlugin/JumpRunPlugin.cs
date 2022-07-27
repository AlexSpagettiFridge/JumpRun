#if TOOLS
using Godot;
using JumpRunPlugin.Scenes;

namespace JumpRunPlugin
{
    [Tool]
    public class JumpRunPlugin : EditorPlugin
    {
        public override string GetPluginName() => "JumpRunPlugin";

        private PluginMainWindow mainWindow;
        private CamerarailInspectorPlugin camerarailInspectorPlugin;

        public override void _EnterTree()
        {
            Script cameraRailScript = ResourceLoader.Load<Script>("res://addons/JumpRunPlugin/CameraRail.cs");
            Texture cameraRailIcon = ResourceLoader.Load<Texture>("res://Gfx/Icons/CameraRailIcon.svg");
            camerarailInspectorPlugin = new CamerarailInspectorPlugin();
            AddInspectorPlugin(camerarailInspectorPlugin);
            AddCustomType("CameraRail", "Node2D", cameraRailScript, cameraRailIcon);

            mainWindow = new PluginMainWindow(GetEditorInterface().GetEditedSceneRoot());
            GetEditorInterface().GetEditorViewport().AddChild(mainWindow);
            GetEditorInterface().GetEditorViewport().RectClipContent = true;

            Connect("scene_changed", this, nameof(OnSceneChanged));
            GD.Print("--- JumpRunPlugin Initialized ---");
        }

        public override void _ExitTree()
        {
            mainWindow.QueueFree();
            RemoveInspectorPlugin(camerarailInspectorPlugin);
            RemoveCustomType("CameraRail");
        }

        public override bool HasMainScreen() => true;

        public override Texture GetPluginIcon() => ResourceLoader.Load<Texture>("res://Gfx/Icons/PluginIcon.svg");

        private void OnSceneChanged(Node sceneRoot)
        {
            mainWindow.SceneChangeReset(sceneRoot);
        }
    }
}
#endif
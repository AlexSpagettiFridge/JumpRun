#if TOOLS
using Godot;

namespace JumpRunPlugin.Scenes
{
    public class PluginMainWindow : Control
    {
        private Node currentSceneRoot;
        private PackedScene psMainMenu = ResourceLoader.Load<PackedScene>("res://addons/JumpRunPlugin/Scenes/PluginMainMenu.tscn");
        private Control mainMenu, subWindow = null;

        public PluginMainWindow(Node sceneRoot = null)
        {
            currentSceneRoot = sceneRoot;
        }

        public override void _Ready()
        {
            AnchorRight = 1;
            AnchorBottom = 1;
            RectClipContent = true;

            mainMenu = psMainMenu.Instance<Control>();
            AddChild(mainMenu);
            GD.Print(mainMenu.RectSize);
            
            GetNode<Button>("MainMenu/VBox/Button").Connect("pressed", this, nameof(OnCameraRailButtonPressed));
        }

        public void HideMenu()
        {
            mainMenu.Visible = false;
        }

        public void BackToMenu()
        {
            mainMenu.Visible = true;
            subWindow = null;
        }

        public void OnCameraRailButtonPressed()
        {
            subWindow = new CameraRailEditorWindow(currentSceneRoot);
            subWindow.Connect("tree_exited", this, nameof(BackToMenu));
            AddChild(subWindow);
            HideMenu();
        }

        public void SceneChangeReset(Node sceneRoot)
        {
            currentSceneRoot = sceneRoot;
            mainMenu.Visible = true;
            if (subWindow is CameraRailEditorWindow cameraRailEditorWindow)
            {
                cameraRailEditorWindow.QueueFree();
            }
        }
    }
}
#endif
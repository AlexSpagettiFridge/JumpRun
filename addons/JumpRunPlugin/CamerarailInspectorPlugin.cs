#if TOOLS
using Godot;

namespace JumpRunPlugin
{
    [Tool]
    public class CamerarailInspectorPlugin : EditorInspectorPlugin
    {
        private JumpRunPlugin plugin;

        public CamerarailInspectorPlugin(JumpRunPlugin plugin)
        {
            this.plugin = plugin;
        }

        private CamerarailInspectorPlugin() { }

        public override bool CanHandle(Object @object) => @object is CameraRail;

        public override void ParseBegin(Object @object)
        {
            AddPropertyEditor("Rail", new CameraRailProperty(plugin, (CameraRail)@object));
        }
    }
}
#endif
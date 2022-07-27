#if TOOLS
using Godot;

namespace JumpRunPlugin
{
    [Tool]
    public class CamerarailInspectorPlugin : EditorInspectorPlugin
    {

        public override bool CanHandle(Object @object) => @object is CameraRail;

        public override void ParseBegin(Object @object)
        {
            AddPropertyEditor("Rail", new CameraRailProperty());
        }
    }
}
#endif
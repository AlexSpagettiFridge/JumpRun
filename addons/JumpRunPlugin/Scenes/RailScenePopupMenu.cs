using Godot;
using System.Collections.Generic;
using System.IO;
using Dir = System.IO.Directory;

namespace JumpRunPlugin.Scenes
{
    public class RailScenePopupMenu : PopupMenu
    {
        public List<string> options = new List<string>();

        public override void _Ready()
        {
            AnchorRight = 1;
            AnchorBottom = 1;
            int currentId = 0;
            SearchDirectory("Scenes/", ref currentId);
            PopupCenteredMinsize(new Vector2(200, 200));
        }

        public void SearchDirectory(string directoryName, ref int currentId)
        {
            foreach (string subDirName in Dir.GetDirectories(directoryName))
            {
                SearchDirectory(subDirName, ref currentId);
            }
            foreach (string fileName in Dir.GetFiles(directoryName))
            {
                AddItem(fileName, currentId);
                options.Add(fileName);
                currentId++;
            }
        }
    }
}
#if TOOLS
using Godot;
using Garray = Godot.Collections.Array;

namespace JumpRunPlugin.Scenes
{
    public class MiniTilemap : Control
    {
        private Vector2[] tiles;
        private Color drawColor;
        public MiniTilemap(TileMap original = null, Color? color = null)
        {
            if (color == null)
            {
                drawColor = new Color(1, 1, 1, 1);
            }
            else
            {
                drawColor = (Color)color;
            }
            if (original == null)
            {
                tiles = new Vector2[0];
                return;
            }
            Garray usedCells = original.GetUsedCells();
            tiles = new Vector2[usedCells.Count];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = original.MapToWorld((Vector2)usedCells[i])/original.CellSize;
            }
        }

        public override void _Ready()
        {
            AnchorRight = 1;
            AnchorBottom = 1;
            Connect("draw", this, nameof(_Draw));
        }

        public override void _Draw()
        {
            Vector2 tileSize = new Vector2(4, 4);
            foreach (Vector2 tile in tiles)
            {
                DrawRect(new Rect2(tile * tileSize, tileSize), drawColor);
            }
        }
    }
}
#endif
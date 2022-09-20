using Godot;

namespace JumpRun.Scr.GameWorld.Common
{
    public class ForegroundTilemap : TileMap
    {
        private PackedScene psFairyBucket = ResourceLoader.Load<PackedScene>("res://Scenes/Entities/FairyDustBucket.tscn");
        private PackedScene psCrunchy = ResourceLoader.Load<PackedScene>("res://Scenes/Entities/Crunchy.tscn");
        private const int crunchyId = 2, bucketId = 3;

        public override void _Ready()
        {
            int crunchies = 0;
            foreach (Vector2 tileCoords in GetUsedCells())
            {
                int cellId = GetCell((int)tileCoords.x, (int)tileCoords.y);
                bool removeTile = true;
                switch (cellId)
                {
                    case crunchyId:
                        Node2D crunchy = psCrunchy.Instance<Node2D>();
                        crunchy.Position = tileCoords * 16 + new Vector2(8, 8);
                        AddChild(crunchy);
                        crunchies++;
                        break;
                    case bucketId:
                        Node2D bucket = psFairyBucket.Instance<Node2D>();
                        bucket.Position = tileCoords * 16 + new Vector2(8, 8);
                        AddChild(bucket);
                        break;
                    default:
                        removeTile = false;
                        break;
                }
                if (removeTile)
                {
                    SetCell((int)tileCoords.x, (int)tileCoords.y, -1);
                }
            }
            CallDeferred(nameof(AfterReady),new object[]{crunchies});
        }

        public void AfterReady(int crunchies)
        {
            GameController.Current.SetLevelCrunchyAmount(crunchies);
        }
    }
}
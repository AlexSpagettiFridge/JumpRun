using System.Collections.Generic;
using Godot;
using JumpRun.Scr.GameWorld.Hero;

namespace JumpRunPlugin
{
    [Tool]
    public class CameraRail : Node2D
    {
        [Signal]
        internal delegate void AreasChanged(List<Rect2> areas);
        [Export]
        private NodePath npCamera, npFollowNode;
        [Export]
        private List<Rect2> areas = new List<Rect2>();
        private Camera2D camera;
        private Node2D followNode;
        private int currentlySelectedAreaId = -1;
        private int _currentAreaId = -1;

        public int CurrentlySelectedAreaId
        {
            get => currentlySelectedAreaId;
            set
            {
                currentlySelectedAreaId = value;
                Update();
            }
        }

        //TODO: Rewrite for multiple areas
        private int currentAreaId
        {
            get => _currentAreaId;
            set
            {
                _currentAreaId = value;
                if (_currentAreaId <= -1) { return; }
                Rect2 currentArea = areas[value];
                camera.LimitLeft = (int)currentArea.Position.x;
                camera.LimitTop = (int)currentArea.Position.y;
                camera.LimitRight = (int)currentArea.End.x;
                camera.LimitBottom = (int)currentArea.End.y;
            }
        }

        internal void AddArea(Rect2 newArea)
        {
            areas.Add(newArea);
            EmitSignal(nameof(AreasChanged), new object[] { areas });
            Update();
        }
        internal void ResetAreas()
        {
            areas = new List<Rect2>();
            EmitSignal(nameof(AreasChanged), new object[] { areas });
            Update();
        }
        internal Rect2 GetAreaById(int id) => areas[id];
        internal void RemoveAreaById(int id)
        {
            areas.RemoveAt(id);
            EmitSignal(nameof(AreasChanged), new object[] { areas });
            Update();
        }
        internal int GetAreaCount() => areas.Count;

        public override void _Ready()
        {
            if (!Engine.EditorHint)
            {
                camera = GetNode<Camera2D>(npCamera);
                followNode = GetNode<Node2D>(npFollowNode);
                if (followNode is IHero hero)
                {
                    hero.HRef.NewCurrentHeroSet += OnHeroChanged;
                }
                List<int> areaIds = GetFollowNodeAreas();
                if (areaIds.Count != 0)
                {
                    currentAreaId = areaIds[0];
                }
            }

        }

        public override void _Process(float delta)
        {
            if (!Engine.EditorHint)
            {
                camera.Position = followNode.Position;
                if (currentAreaId < 0) { return; }
                if (followNode.Position < areas[currentAreaId].Position || followNode.Position > areas[currentAreaId].End)
                {
                    List<int> areaIds = GetFollowNodeAreas();
                    if (areaIds.Count != 0)
                    {
                        currentAreaId = areaIds[0];
                    }
                }
            }
        }

        public void OnHeroChanged(object sender, RegisterCurrentHeroArgs args)
        {
            followNode = (Node2D)args.NewCurrentHero;
        }

        private List<int> GetFollowNodeAreas()
        {
            List<int> rectIds = new List<int>();
            Vector2 point = followNode.Position;

            for (int i = 0; i < areas.Count; i++)
            {
                if (point >= areas[i].Position && point <= areas[i].End)
                {
                    rectIds.Add(i);
                }
            }

            return rectIds;
        }

        public override string ToString()
        {
            return "[CameraRail:" + GetIndex() + "]";
        }

        public override void _Draw()
        {
            if (Engine.EditorHint)
            {
                Color color = new Color(0, 0, 1, 0.75f);
                Color selectedColor = new Color(1, 0, 1, 0.9f);
                for (int i = 0; i < areas.Count; i++)
                {
                    if (currentlySelectedAreaId == i)
                    {
                        DrawRect(areas[i],new Color(1,0,1,0.5f),true);
                    }
                    DrawRect(areas[i], i == currentlySelectedAreaId ? selectedColor : color, false, 2);
                }
            }
        }
    }
}
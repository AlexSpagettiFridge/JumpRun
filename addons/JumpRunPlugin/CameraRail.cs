using System.Collections.Generic;
using Godot;
using JumpRun.Scr.GameWorld.Hero;
using Garray = Godot.Collections.Array;

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
        private Garray areas = new Garray();
        private Camera2D camera;
        private Node2D followNode;
        private int currentlySelectedAreaId = -1;
        private int[] _currentAreaIds = { };

        public int CurrentlySelectedAreaId
        {
            get => currentlySelectedAreaId;
            set
            {
                currentlySelectedAreaId = value;
                Update();
            }
        }

        private int[] currentAreaIds
        {
            get => _currentAreaIds;
            set
            {
                _currentAreaIds = value;
                if (_currentAreaIds.Length == 0) { return; }
                Rect2 firstArea = GetAreaById(value[0]);
                camera.LimitLeft = (int)firstArea.Position.x;
                camera.LimitTop = (int)firstArea.Position.y;
                camera.LimitRight = (int)firstArea.End.x;
                camera.LimitBottom = (int)firstArea.End.y;
                for (int i = 1; i < value.Length; i++)
                {
                    Rect2 area = GetAreaById(value[i]);
                    camera.LimitLeft = Mathf.Min((int)area.Position.x, camera.LimitLeft);
                    camera.LimitTop = Mathf.Min((int)area.Position.y, camera.LimitTop);
                    camera.LimitRight = Mathf.Max((int)area.End.x, camera.LimitRight);
                    camera.LimitBottom = Mathf.Max((int)area.End.y, camera.LimitBottom);
                }
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
            areas = new Garray();
            EmitSignal(nameof(AreasChanged), new object[] { areas });
            Update();
        }
        internal Rect2 GetAreaById(int id) => (Rect2)areas[id];
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
                areaIds.CopyTo(currentAreaIds);
            }

        }

        public override void _Process(float delta)
        {
            if (!Engine.EditorHint)
            {
                camera.Position = followNode.Position;
                foreach (int id in currentAreaIds)
                {
                    if (followNode.Position < GetAreaById(id).Position || followNode.Position > GetAreaById(id).End)
                    {
                        List<int> areaIds = GetFollowNodeAreas();
                        areaIds.CopyTo(currentAreaIds);
                        return;
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
                if (point >= GetAreaById(i).Position && point <= GetAreaById(i).End)
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
                        DrawRect(GetAreaById(i), new Color(1, 0, 1, 0.5f), true);
                    }
                    DrawRect(GetAreaById(i), i == currentlySelectedAreaId ? selectedColor : color, false, 2);
                }
            }
        }
    }
}
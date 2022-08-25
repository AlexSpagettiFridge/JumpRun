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
        private List<int> currentAreaIds = new List<int>();

        public int CurrentlySelectedAreaId
        {
            get => currentlySelectedAreaId;
            set
            {
                currentlySelectedAreaId = value;
                Update();
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
            }

        }

        public override void _Process(float delta)
        {
            if (Engine.EditorHint) { return; }
            camera.Position = followNode.Position;
            for (int i = 0; i < areas.Count; i++)
            {
                Rect2 area = GetAreaById(i);
                bool isInside = followNode.Position >= area.Position && followNode.Position <= area.End;
                SetCurrentArea(i, isInside);
            }
        }

        public void OnHeroChanged(object sender, RegisterCurrentHeroArgs args)
        {
            followNode = (Node2D)args.NewCurrentHero;
        }

        private void SetCurrentArea(int id, bool isCurrent = true)
        {
            if (isCurrent && !areas.Contains(id))
            {
                currentAreaIds.Add(id);
                Rect2 area = GetAreaById(id);
                if (areas.Count == 1)
                {
                    camera.LimitLeft = (int)area.Position.x;
                    camera.LimitTop = (int)area.Position.y;
                    camera.LimitRight = (int)area.End.x;
                    camera.LimitBottom = (int)area.End.y;
                }
                else
                {
                    camera.LimitLeft = Mathf.Min(camera.LimitLeft, (int)area.Position.x);
                    camera.LimitTop = Mathf.Min(camera.LimitTop, (int)area.Position.y);
                    camera.LimitRight = Mathf.Max(camera.LimitRight, (int)area.End.x);
                    camera.LimitBottom = Mathf.Max(camera.LimitBottom, (int)area.End.y);
                }
                return;
            }
            if (!isCurrent && areas.Contains(id))
            {
                currentAreaIds.Remove(id);
                if (areas.Count == 0) { return; }
                Rect2 firstArea = GetAreaById(currentAreaIds[0]);
                camera.LimitLeft = (int)firstArea.Position.x;
                camera.LimitTop = (int)firstArea.Position.y;
                camera.LimitRight = (int)firstArea.End.x;
                camera.LimitBottom = (int)firstArea.End.y;
                for (int i = 1; i < currentAreaIds.Count; i++)
                {
                    Rect2 area = GetAreaById(i);
                    camera.LimitLeft = Mathf.Min(camera.LimitLeft, (int)area.Position.x);
                    camera.LimitTop = Mathf.Min(camera.LimitTop, (int)area.Position.y);
                    camera.LimitRight = Mathf.Max(camera.LimitRight, (int)area.End.x);
                    camera.LimitBottom = Mathf.Max(camera.LimitBottom, (int)area.End.y);
                }
            }
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
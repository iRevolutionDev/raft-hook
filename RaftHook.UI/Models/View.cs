using System.Collections.Generic;
using UnityEngine;

namespace RaftHook.UI.Models
{
    public class View
    {
        private readonly List<View> _children = new List<View>();
        private readonly int _windowId;
        private bool _isVisible;
        private Rect _rect = new Rect(20, 20, 400, 400);
        private string _title;

        protected View(string title)
        {
            _title = title;
            _windowId = Random.Range(0, 100000);
        }

        protected View(string title, bool isVisible)
        {
            _title = title;
            _isVisible = isVisible;
            _windowId = Random.Range(0, 100000);
        }

        protected View(bool isVisible)
        {
            _isVisible = isVisible;
            _windowId = Random.Range(0, 100000);
        }

        protected View(Rect rect, string title)
        {
            _rect = rect;
            _title = title;
            _isVisible = true;

            _windowId = Random.Range(0, 100000);
        }

        protected View(Rect rect, string title, bool isVisible)
        {
            _rect = rect;
            _title = title;
            _isVisible = isVisible;

            _windowId = Random.Range(0, 100000);
        }

        protected void AddChild(View view)
        {
            _children.Add(view);
            view.Start();
        }

        public virtual void Start()
        {
        }

        protected virtual void Render(int id)
        {
            GUI.DragWindow();
        }

        public virtual void Update()
        {
            if (_children.Count <= 0) return;

            foreach (var child in _children) child.Update();
        }

        public virtual void OnGUI()
        {
            if (!_isVisible) return;

            //Add auto resize to the window
            GUI.skin.window.stretchHeight = true;
            GUI.skin.window.stretchWidth = true;

            _rect = GUILayout.Window(_windowId, _rect, Render, _title);

            if (_children.Count <= 0) return;

            foreach (var child in _children) child.OnGUI();
        }

        public void SetRect(Rect rect)
        {
            _rect.x = rect.x;
            _rect.y = rect.y;
            _rect.width = rect.width;
            _rect.height = rect.height;
        }

        public void SetRect(float x, float y, float width, float height)
        {
            _rect.x = x;
            _rect.y = y;
            _rect.width = width;
            _rect.height = height;
        }

        public void SetTitle(string title)
        {
            _title = title;
        }

        public void SetVisible(bool visible)
        {
            _isVisible = visible;
        }

        public bool IsVisible()
        {
            return _isVisible;
        }

        public void ToggleVisible()
        {
            _isVisible = !_isVisible;
        }

        public void Show()
        {
            _isVisible = true;
        }

        public void Hide()
        {
            _isVisible = false;
        }

        public bool IsMouseOver()
        {
            return _rect.Contains(Event.current.mousePosition);
        }
    }
}
using UnityEngine;

namespace RaftHook.Features.Features.Boat
{
    public abstract class ForceAnchor
    {
        private static bool _forceAnchor;
        
        public static void ToggleForceAnchor()
        {
            _forceAnchor = !_forceAnchor;
            
            if (_forceAnchor) AddAnchor();
            else RemoveAnchor();
        }
        
        public static void AddAnchor()
        {
            ComponentManager<Raft>.Value.AddAnchor(false);
        }
        
        public static void RemoveAnchor()
        {
            ComponentManager<Raft>.Value.RemoveAnchor(10);
        }
    }
}
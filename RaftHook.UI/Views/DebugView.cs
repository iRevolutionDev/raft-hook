using System.Linq;
using RaftHook.UI.Models;
using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.UI.Views
{
    public class DebugView : View
    {
        public DebugView() : base("Debug Mode")
        {
        }

        protected override void Render(int id)
        {
            var sharks = RaftClient.GetSharks();
            var sharkCount = sharks.Count();

            GUILayout.Label($"Shark Count: {sharkCount}");

            if (sharkCount > 0)
            {
                var shark = sharks.First();
                GUILayout.Label($"Shark Position: {shark.transform.position}");
                GUILayout.Label($"Shark Biting Raft: {shark.bitingRaft}");
            }

            base.Render(id);
        }
    }
}
using RaftHook.UI.Models;
using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.UI.Views
{
    public class WorldView : View
    {
        public WorldView() : base("World")
        {
        }

        protected override void Render(int id)
        {
            RaftSettings.NoShark = GUILayout.Toggle(RaftSettings.NoShark, "No Shark");
            RaftSettings.NoSharkAttack = GUILayout.Toggle(RaftSettings.NoSharkAttack, "No Shark Attack");
            RaftSettings.NoSharkAttackPlayer =
                GUILayout.Toggle(RaftSettings.NoSharkAttackPlayer, "No Shark Attack Player");
            RaftSettings.NoSharkAttackRaft = GUILayout.Toggle(RaftSettings.NoSharkAttackRaft, "No Shark Attack Raft");

            base.Render(id);
        }
    }
}
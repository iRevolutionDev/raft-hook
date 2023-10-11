using RaftHook.Features.Features.Boat;
using RaftHook.Features.Features.Miscellaneous;
using RaftHook.Features.Features.Self;
using RaftHook.UI.Models;
using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.UI.Views
{
    public class MiscellaneousView : View
    {
        public MiscellaneousView() : base("Miscellaneous")
        {
        }

        protected override void Render(int id)
        {
            RaftSettings.Fly = GUILayout.Toggle(RaftSettings.Fly, "Fly [F1]");
            if (GUILayout.Button("Force Anchor")) ForceAnchor.AddAnchor();
            if (GUILayout.Button("Unlock All")) Unlockables.UnlockAll();
            if (GUILayout.Button("Unlock Achievements")) Unlockables.UnlockAchievements();
            if (GUILayout.Button("Unlock All Notes")) Unlockables.UnlockAllNotes();
            if (GUILayout.Button("Unlock All Recipes")) Unlockables.UnlockAllRecipes();
            if (GUILayout.Button("Kill All Entities")) KillEntities.KillAllEntities();
            if (GUILayout.Button("Kill All Enemies")) KillEntities.KillAllEnemies();

            base.Render(id);
        }
    }
}
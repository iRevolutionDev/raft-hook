using RaftHook.UI.Models;
using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.UI.Views
{
    public class VisualsView : View
    {
        public VisualsView() : base("Visuals")
        {
        }

        protected override void Render(int id)
        {
            RaftSettings.EnableEsp = GUILayout.Toggle(RaftSettings.EnableEsp, "Enable ESP");
            RaftSettings.Landmark = GUILayout.Toggle(RaftSettings.Landmark,
                "Landmark [" + Mathf.Round(RaftSettings.FLandmark) + "m]");
            RaftSettings.FLandmark =
                Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FLandmark, 1f, 1000f) * 1000f) / 1000f;
            RaftSettings.TradingPost = GUILayout.Toggle(RaftSettings.TradingPost,
                "Trading Post [" + Mathf.Round(RaftSettings.FTradingPost) + "m]");
            RaftSettings.FTradingPost =
                Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FTradingPost, 1f, 500f) * 500f) / 500f;
            RaftSettings.Treasures = GUILayout.Toggle(RaftSettings.Treasures,
                "Treasures [" + Mathf.Round(RaftSettings.FTreasures) + "m]");
            RaftSettings.FTreasures =
                Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FTreasures, 1f, 500f) * 500f) / 500f;
            RaftSettings.Item = GUILayout.Toggle(RaftSettings.Item, "Item ESP");
            RaftSettings.ItemDefault = GUILayout.Toggle(RaftSettings.ItemDefault,
                "Default Items [" + Mathf.Round(RaftSettings.FItemDefault) + "m]");
            RaftSettings.FItemDefault =
                Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FItemDefault, 1f, 150f) * 150f) / 150f;
            RaftSettings.ItemDomesticAnimal = GUILayout.Toggle(RaftSettings.ItemDomesticAnimal,
                "Domestic Animal Items [" + Mathf.Round(RaftSettings.FItemDomesticAnimal) + "m]");
            RaftSettings.FItemDomesticAnimal =
                Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FItemDomesticAnimal, 1f, 150f) * 150f) /
                150f;
            RaftSettings.ItemNoteBookNote = GUILayout.Toggle(RaftSettings.ItemNoteBookNote,
                "Notebook Notes [" + Mathf.Round(RaftSettings.FItemNoteBookNote) + "m]");
            RaftSettings.FItemNoteBookNote =
                Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FItemNoteBookNote, 1f, 250f) * 250f) / 250f;
            RaftSettings.ItemQuestItem = GUILayout.Toggle(RaftSettings.ItemQuestItem,
                "Quest Items [" + Mathf.Round(RaftSettings.FItemQuestItem) + "m]");
            RaftSettings.FItemQuestItem =
                Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FItemQuestItem, 1f, 250f) * 250f) / 250f;
            RaftSettings.HostileAnimal = GUILayout.Toggle(RaftSettings.HostileAnimal,
                "Hostile Animals [" + Mathf.Round(RaftSettings.FHostileAnimal) + "m]");
            RaftSettings.FHostileAnimal =
                Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FHostileAnimal, 1f, 250f) * 250f) / 250f;
            RaftSettings.FriendlyAnimal = GUILayout.Toggle(RaftSettings.FriendlyAnimal,
                "Animals [" + Mathf.Round(RaftSettings.FFriendlyAnimal) + "m]");
            RaftSettings.FFriendlyAnimal =
                Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FFriendlyAnimal, 1f, 250f) * 250f) / 250f;
            RaftSettings.Npc =
                GUILayout.Toggle(RaftSettings.Npc, "NPC [" + Mathf.Round(RaftSettings.FNpc) + "m]");
            RaftSettings.FNpc = Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FNpc, 1f, 200f) * 200f) /
                                200f;

            base.Render(id);
        }
    }
}
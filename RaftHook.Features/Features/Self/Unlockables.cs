using System;
using UltimateWater.Internal;

namespace RaftHook.Features.Features.Self
{
    public class Unlockables
    {
        public static void UnlockAll()
        {
            ComponentManager<NoteBook>.Value.UnlockAllNotes();
            Singleton<Inventory_ResearchTable>.Instance.LearnAllRecipesInstantly();
            foreach (var itemBase in ItemManager.GetAllItems())
            {
                Singleton<Inventory_ResearchTable>.Instance.ResearchBlueprint(itemBase);
                Singleton<Inventory_ResearchTable>.Instance.Research(itemBase, true);
            }

            foreach (var questItemType in (QuestItemType[])Enum.GetValues(typeof(QuestItemType)))
                ComponentManager<QuestItemManager>.Value.AddQuestItemsNetworked(true,
                    new QuestItem(QuestItemManager.GetSOQuestItemFromType(questItemType), 1));
        }

        public static void UnlockAchievements()
        {
            foreach (var obj in Enum.GetValues(typeof(AchievementType)))
                AchievementHandler.UnlockAchievement((AchievementType)obj);
        }

        public static void UnlockAllNotes()
        {
            ComponentManager<NoteBook>.Value.UnlockAllNotes();
        }

        public static void UnlockAllRecipes()
        {
            Singleton<Inventory_ResearchTable>.Instance.LearnAllRecipesInstantly();
        }
    }
}
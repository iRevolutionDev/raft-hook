namespace RaftHook.Utilities
{
    public abstract class RaftSettings
    {
        public static bool ShowMenu = false;

        // Self
        public static bool Fly;
        public static bool UnlimitedHealth;
        public static bool NoThirst;
        public static bool NoHunger;
        public static bool NoFallDamage;
        public static bool NoBuildRestrictions;
        public static bool NoBuildResourceRestrictions;
        public static bool UnlimitedOxygen;
        public static bool NoCraftingRestrictions;

        //World
        public static bool NoShark;
        public static bool NoSharkAttack;
        public static bool NoSharkAttackPlayer;
        public static bool NoSharkAttackRaft;

        // ESP
        public static bool EnableEsp;
        public static bool Players;
        public static bool PlayersBox;
        public static bool Npc;
        public static bool Landmark;
        public static bool TradingPost;
        public static bool Treasures;
        public static bool Item;
        public static bool ItemDefault;
        public static bool ItemDomesticAnimal;
        public static bool ItemNoteBookNote;
        public static bool ItemQuestItem;
        public static bool HostileAnimal;
        public static bool FriendlyAnimal;

        public static float FLandmark = 500f;
        public static float FTradingPost = 200f;
        public static float FFriendlyAnimal = 100f;
        public static float FTreasures = 100f;
        public static float FItemDefault = 50f;
        public static float FItemDomesticAnimal = 20f;
        public static float FItemNoteBookNote = 75f;
        public static float FItemQuestItem = 100f;
        public static float FHostileAnimal = 150f;
        public static float FNpc = 100f;
        public static float FPlayers = 100f;
    }
}
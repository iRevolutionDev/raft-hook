namespace RaftHook.Utilities.Models
{
    public class RNotificationInfo : Notification_QuestItem_Info
    {
        public RNotificationInfo(string description) : base(null, 0, null)
        {
            itemSprite = RaftClient.CheatLogo;
            itemName = description;
        }
    }
}
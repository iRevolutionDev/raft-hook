using RaftHook.Utilities;
using Sirenix.Utilities;
using UnityEngine;

namespace RaftHook.Features.Features.Miscellaneous
{
    public abstract class KillEntities
    {
        public static void KillAllEntities()
        {
            if (!RaftClient.IsInGame) return;

            var entities = Object.FindObjectsOfType<Network_Entity>();

            entities.ForEach(entity =>
            {
                if (entity.entityType == EntityType.Player) return;

                entity.Damage(entity.stat_health.Value, entity.transform.position, Vector3.up, EntityType.Player);
            });
        }
        
        public static void KillAllEnemies()
        {
            if (!RaftClient.IsInGame) return;

            var entities = Object.FindObjectsOfType<Network_Entity>();

            entities.ForEach(entity =>
            {
                if (entity.entityType != EntityType.Enemy) return;

                entity.Damage(entity.stat_health.Value, entity.transform.position, Vector3.up, EntityType.Player);
            });
        }
    }
}
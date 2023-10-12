using System.Linq;
using RaftHook.Utilities;

namespace RaftHook.Features.Features.World
{
    public class Plants
    {
        public static void WaterPlants()
        {
            foreach (var cropplot in PlantManager.allCropplots.Where(cropplot => cropplot.SlotsNeedWater()))
                cropplot.AddWater();
        }

        public static void GrowPlants()
        {
            WaterPlants();
            RaftClient.LocalPlayer.PlantManager.ForwardTime(9999f);
        }
    }
}
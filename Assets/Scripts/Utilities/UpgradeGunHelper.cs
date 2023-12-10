using System.Collections.Generic;
using App;

public class UpgradeGunHelper
{
    public static Dictionary<GunSkin, DefaultUpgradeGunManager.SkillConfig> SkinConfigs()
    {
        return new Dictionary<GunSkin, DefaultUpgradeGunManager.SkillConfig>()
        {
            {
                GunSkin.Bazooka, new DefaultUpgradeGunManager.SkillConfig()
                {
                    costs = new List<int>() { 50, 100, 200, 300, 400, 500, 600 }
                }
            },
            {
                GunSkin.FireBlaster, new DefaultUpgradeGunManager.SkillConfig()
                {
                    costs = new List<int>() { 100, 200, 300, 400, 500, 600, 700 }
                }
            },
            {
                GunSkin.Laser, new DefaultUpgradeGunManager.SkillConfig()
                {
                    costs = new List<int>() { 200, 300, 400, 500, 600, 700, 800 }
                }
            },
        };
    }

    public static List<GunSkin> AllGuns()
    {
        return new List<GunSkin>()
        {
            GunSkin.Bazooka,
            GunSkin.FireBlaster,
            GunSkin.Laser,
        };
    }
}
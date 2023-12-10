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
                    damages = new List<float>() { 10, 20, 30, 40, 50, 60, 70 },
                    fireRates = new List<float>() { 2, 2.2f, 2.4f, 2.6f, 2.8f, 3, 3.2f },
                    costs = new List<int>() { 50, 100, 200, 300, 400, 500, 600 }
                }
            },
            {
                GunSkin.FireBlaster, new DefaultUpgradeGunManager.SkillConfig()
                {
                    damages = new List<float>() { 15, 20, 25, 30, 35, 45, 50 },
                    fireRates = new List<float>() { 2.2f, 2.4f, 2.6f, 2.8f, 3, 3.2f, 3.4f },
                    costs = new List<int>() { 100, 200, 300, 400, 500, 600, 700 }
                }
            },
            {
                GunSkin.Laser, new DefaultUpgradeGunManager.SkillConfig()
                {
                    damages = new List<float>() { 20, 25, 30, 35, 40, 45, 50 },
                    fireRates = new List<float>() { 2.4f, 2.6f, 2.8f, 3, 3.2f, 3.4f, 3.6f },
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
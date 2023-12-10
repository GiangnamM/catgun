using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App
{
    public class SkinGunHelper
    {
        public static Dictionary<GunSkin, DefaultSkinGunManager.SkinConfig> SkinConfigs()
        {
            return new Dictionary<GunSkin, DefaultSkinGunManager.SkinConfig>()
            {
                {
                    GunSkin.M4A1, new DefaultSkinGunManager.SkinConfig()
                    {
                        cost = 0,
                    }
                },    {
                    GunSkin.Bazooka, new DefaultSkinGunManager.SkinConfig()
                    {
                        cost = 0,
                    }
                },    {
                    GunSkin.FireBlaster, new DefaultSkinGunManager.SkinConfig()
                    {
                        cost = 400,
                    }
                },
                {
                    GunSkin.Laser, new DefaultSkinGunManager.SkinConfig()
                    {
                        cost = 800,
                    }
                },
            };
        }

        public static List<GunSkin> AllSkins()
        {
            return new List<GunSkin>()
            {
                GunSkin.M4A1,
                GunSkin.Bazooka,
                GunSkin.FireBlaster,
                GunSkin.Laser,
            };
        }
    }
}
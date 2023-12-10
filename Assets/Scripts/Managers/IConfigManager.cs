using System;
using System.Collections.Generic;
using Extension;

namespace App
{
    [Serializable]
    public class UpgradeGun
    {
        public GunSkin gunSkin;
        public List<float> damage;
        public List<float> fireRate;
    }

    [Serializable]
    public class BaseGun
    {
        public GunSkin gunSkin;
        public float damage;
        public float fireRate;
    }

    [Service(typeof(IConfigManager))]
    public interface IConfigManager
    {
        IDictionary<GunSkin, (float, float)> GunBaseInfo { get; }
        IDictionary<GunSkin, (List<float>, List<float>)> AllUpgradeGunTuples { get; }
    }
}
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
        public List<int> bullet;
    }

    [Service(typeof(IConfigManager))]
    public interface IConfigManager
    {
        List<UpgradeGun> UpgradeGuns { get; }

        IDictionary<GunSkin, (List<float>, List<int>)> AllUpgradeGunTuples { get; }
    }
}
using System;
using System.Collections.Generic;
using Extension;

namespace App
{
    public class UpgradeGunObserver
    {
        public Action<GunSkin> OnLevelBoosterChanged { get; set; }
    }

    public interface IGunInfo
    {
        GunSkin GunSkin { get; }
        int Cost { get; }
        List<float> Damages { get; }
        List<float> FireRates { get; }
        int MaxLevel { get; }
    }

    [Service(typeof(IUpgradeGunManager))]
    public interface IUpgradeGunManager : IObserverManager<UpgradeGunObserver>
    {
        Dictionary<GunSkin, int> AllGunsAndCurLevel { get; }
        int GetLevelGun(GunSkin type);
        void UpgradeGun(GunSkin type, int level);
        IGunInfo GetInfo(GunSkin type);
        List<GunSkin> AllGuns { get; }
    }
}
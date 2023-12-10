using System;
using System.Collections.Generic;

using Extension;

namespace App {
    public class UpgradeGunObserver {
        public Action<GunSkin> OnLevelBoosterChanged { get; set; }
    }

    public interface IGunInfo {
        GunSkin GunSkin { get; }
        int Cost { get; }
        int Damage { get; }
        float FireRate { get; }
        int MaxLevel { get; }
    }

    [Service(typeof(IUpgradeGunManager))]
    public interface IUpgradeGunManager : IObserverManager<UpgradeGunObserver> {
        Dictionary<GunSkin, int> AllBoostersAndCurLevel { get; }
        int GetLevelBooster(GunSkin type);
        void UpgradeBooster(GunSkin type, int level);
        IGunInfo GetInfo(GunSkin type);
        List<GunSkin> AllBoosters { get; }
    }
}
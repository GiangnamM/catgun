using System.Collections.Generic;

using Extension;

namespace App {
    public enum GunSkin {
        M4A1,
        Bazooka,
        FireBlaster,
        Laser
    }

    public interface ISkinInfo {
        GunSkin Skin { get; }
        int Cost { get; }
        bool IsOwned { get; set; }
        bool IsSelected { get; }

    }

    [Service(nameof(ISkinGunManager))]
    public interface ISkinGunManager {
        GunSkin CurrentSkin { get; set; }

        void SetOwned(GunSkin gunSkin, bool owned);

        List<GunSkin> AllSkins { get; }

        ISkinInfo GetInfo(GunSkin gunSkin);

    }
}
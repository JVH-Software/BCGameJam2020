using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeList : List<Upgrades>
{
    private Pack pack;

    public UpgradeList(Pack pack)
    {
        this.pack = pack;
    }

    public new void Add(Upgrades upgrade)
    {
        base.Add(upgrade);

        switch (upgrade)
        {
            case Upgrades.SpeedBoost:
                pack.speed += 3;
                break;
            case Upgrades.FasterShot:
                pack.shootSpeed += 50;
                break;
        }
    }

    public new void Remove(Upgrades upgrade)
    {
        base.Remove(upgrade);

        switch (upgrade)
        {
            case Upgrades.SpeedBoost:
                pack.speed -= 3;
                break;
            case Upgrades.FasterShot:
                pack.shootSpeed -= 50;
                break;
        }
    }
}

public enum Upgrades {SpeedBoost, FasterShot};

﻿using System.Collections;
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
                pack.speedMultiplier *= 2;
                break;
            case Upgrades.FasterShot:
                pack.projectileSpeedMultiplier *= 2;
                break;
            case Upgrades.DamageBoost:
                pack.damageMultiplier *= 2;
                break;
            case Upgrades.DefenceBoost:
                pack.defenceMultiplier /= 2;
                break;
            case Upgrades.ExtraKnockback:
                pack.knockbackMultiplier *= 2;
                break;
            case Upgrades.MachineGun:
                pack.fireRateMultiplier *= 2;
                break;
        }
    }

    public new void Remove(Upgrades upgrade)
    {

        base.Remove(upgrade);

        switch (upgrade)
        {
            case Upgrades.SpeedBoost:
                pack.speedMultiplier /= 2;
                break;
            case Upgrades.FasterShot:
                pack.projectileSpeedMultiplier /= 2;
                break;
            case Upgrades.DamageBoost:
                pack.damageMultiplier /= 2;
                break;
            case Upgrades.DefenceBoost:
                pack.defenceMultiplier *= 2;
                break;
            case Upgrades.ExtraKnockback:
                pack.knockbackMultiplier /= 2;
                break;
            case Upgrades.MachineGun:
                pack.fireRateMultiplier /= 2;
                break;
        }
    }
}

public enum Upgrades {SpeedBoost, FasterShot, DamageBoost, DefenceBoost, PerfectAim, NoRecoil, ExtraKnockback, MachineGun};

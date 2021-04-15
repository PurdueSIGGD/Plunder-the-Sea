using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeahorseLance : StateCombat
{
    public override void MakeElite(int numEffects)
    {
        base.MakeElite(numEffects);

        // Elite seahorse lance can summon any other non-boss enemy in the game
        RetreatMovement rm = ((RetreatMovement)myStateMovement);
        rm.actCooldownTime = 1.5f;
        rm.maxEnemiesToSpawn = 8;
        rm.numberOfEnemiesToSpawn = 1;
        myBase.myStats.currentHP *= 3;
        myBase.myStats.maxHP *= 3;
        transform.localScale *= 0.75f;
    }
}

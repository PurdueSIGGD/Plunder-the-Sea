using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class EntityStats : MonoBehaviour
{
    public string displayName;
    public float movementSpeed = 10.0f;
    public float maxHP = 1;
    public float currentHP = 1;
    //Ratio of damage which armor absorbs
    public float armorMult = 0.0f;
    //Constant amount of damage armor absorbs
    public float armorStatic = 0.0f;
    // Float dictating how much kill regen this enemy contributes
    public float killRegenMult = 1.0f;
    public Slider healthbar;
    public int lastHitAmmoAddition = 0;

    /*
     * Storing endtime in attribute class does not allow for the same attribute
     * to be applied to multiple entities. Store in this struct instead.
     */
    private struct AppliedAttribute
    {
        public EntityAttribute attr;
        public EntityStats source;
        public float endTime;
    };

    private List<AppliedAttribute> attribList = new List<AppliedAttribute>();

    /* Extending classes must call this in update function */
    protected void StatUpdate()
    {
        for (int i = 0; i < attribList.Count; i++)
        {
            AppliedAttribute app = attribList[i];
            app.attr.Update(this, app.source);
            if (Time.time >= app.endTime)
            {
                RemoveAttribute(app.attr);
                i--;//Decrement to account for removed attribute
            }
        }
    }

    public void damageReturnCall()
    {
        if (GetComponent<EnemyStats>())
        {
            GetComponent<EnemyStats>().enemyDamageReturnCall();
        }
    }
    public void ReplenishHealth(float amount) {
        this.currentHP = Mathf.Min(this.currentHP + amount, this.maxHP);
        updateHealthBar();
    }

    public void setLastHit(int ammoRefillAmmount)
    {
        lastHitAmmoAddition = ammoRefillAmmount;
    }

    //Return true if results in death
    public virtual bool TakeDamage(float amount, EntityStats source, bool tickDamage = false)
    {
        //player damage call
        damageReturnCall();
        bool isPlayer = false;

        //damage scaling is not stored as it can update
        float multiplier = 1;
        float realDmg = amount;
        if (!tickDamage)
        {
            realDmg = Mathf.Max((amount - armorStatic), Mathf.Min(1, amount)) * Mathf.Max(1 - armorMult, 0);
        }
        if (transform.tag == "Player")
        {
            multiplier = Mathf.Min(1 + transform.GetComponent<PlayerStats>().dungeonLevel * 0.1f, 2);
            realDmg *= multiplier;
            PlayerPrefs.SetInt("Hurt", PlayerPrefs.GetInt("Hurt") + (int)realDmg);
            isPlayer = true;
            //print("player hit");
        } else
        {
            if (source is PlayerStats)
            {
                PlayerStats pStats = (PlayerStats)source;
                multiplier = 1 / (1 + .3f * pStats.dungeonLevel);
                realDmg *= multiplier;
            }
            PlayerPrefs.SetInt("Damage", PlayerPrefs.GetInt("Damage") + (int)realDmg);
        }
        //Debug.Log("Vlaue: " + realDmg);

        bool died = false;
        currentHP -= realDmg;
        if (currentHP <= 0)
        {
            currentHP = 0;
            RemoveAllAttributes();
            Die();
            
            died = true;
            if (source)
            {
                source.OnKill(this);

                //if is not the player
                if (isPlayer)
                {

                    string killerName = source.displayName.Length > 0 ? source.displayName : source.name;
                    if (killerName.Contains("(Clone)"))
                    {
                        PlayerPrefs.SetString("Killer", killerName.Substring(0, killerName.Length - 7));
                    }
                    else
                    {
                        PlayerPrefs.SetString("Killer", killerName);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt("Kills", PlayerPrefs.GetInt("Kills") + 1);
                }

                //is ranged weapon on player
                if (lastHitAmmoAddition != 0)
                {
                    source.GetComponent<PlayerStats>().replenishAmmo(lastHitAmmoAddition);
                }
            }
        }
        updateHealthBar();

        return died;
    }

    public void updateHealthBar() {
        if (healthbar != null)
        {
            healthbar.value = currentHP / maxHP;
        }
    }

    public virtual void AddAttribute(EntityAttribute attr, EntityStats source)
    {
        AppliedAttribute app;
        app.attr = attr;
        app.source = source;
        if (attr.duration == float.PositiveInfinity)
        {
            app.endTime = float.PositiveInfinity;
        }
        else
        {
            app.endTime = Time.time + attr.duration;
        }

        if (!attr.stackable)
        {
            for (int i = 0; i < attribList.Count; i++)
            {
                string temp = attribList[i].attr.name;
                if (temp != "" && temp == attr.name)
                {
                    return;
                }
            }
        }
        
        attribList.Add(app);
        attr.OnAdd(this);
    }

    public virtual void RemoveAttribute(EntityAttribute attr)
    {
        for (int i = 0; i < attribList.Count; i++)
        {
            if (attribList[i].attr == attr)
            {
                attribList.RemoveAt(i);
                attr.OnRemove(this);
            }
        }
    }

    public virtual void RemoveAllAttributes()
    {
        for (int i = 0; i < attribList.Count; i++)
        {
            attribList[i].attr.OnRemove(this);
        }
        attribList.Clear();
    }
    
    public virtual void Die()
    {
        //Do nothing by default
    }

    //When entity is source of a kill
    public virtual void OnKill(EntityStats victim)
    {
        //Do nothing by default
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CombatEntity : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI nameField;
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private Button selectionButton;
    [SerializeField]
    private Image indicator;
    [SerializeField]
    private StatusEffectDurationDictionary currentStatusEffects;
    [SerializeField]
    private StatData baseStats;
    [SerializeField]
    private StatData currentStats;


    

    private float health;
    public float Health => health;

    public virtual bool HasBeenSaved => Health >= currentStats.Health;
    public virtual bool HasBeenFailed => Health <= 0;
    public virtual bool IsAlive => Health > 0 && !HasBeenSaved;

    private List<Skill> skills;
    public List<Skill> Skills => skills;

    protected CombatEntityData entityData;
    public int ExpForDefeat => entityData.ExpOnDefeat;

    private System.Action<CombatEntity> OnEntitySelected;

    public void Initialise(CombatEntityData data, StatData baseStatData, List<Skill> baseSkills, System.Action<CombatEntity> OnSelected = null)
    {
        entityData = data;
        baseStats = baseStatData;
        currentStats.AddStats(baseStats);
        skills = new List<Skill>();
        foreach(Skill skill in baseSkills)
        {
            skills.Add(skill);
        }
        health = baseStats.StartingHealth;
        healthBar.UpdateBar((int)Health, baseStats.Health);

        OnEntitySelected = OnSelected;
        selectionButton?.onClick.AddListener(OnSelfSelected);
    }

    private void OnSelfSelected()
    {
        OnEntitySelected?.Invoke(this);
    }

    public void ToggleIndicator(bool on)
    {
        indicator.gameObject.SetActive(on);
    }

    public void SetName(string name)
    {
        nameField.text = name;
    }

    public void CastSkill(Skill skill, CombatEntity target)
    {
        if (skill.IsSelfTargeted)
        {
            Heal(skill.TotalDamageAfterScaling(PostStatusEffectStats().Attack, 0));
            if (skill.HasStatusEffect)
            {
                TryApplyStatus(skill.Status, skill.ApplicationData);
            }
        }
        else
        {
            AttackTarget(target, skill);
            if(skill.HasStatusEffect)
            {
                target.TryApplyStatus(skill.Status, skill.ApplicationData);
            }
        }
        if(skill.HasDelayedResolution)
        {
            skill.AlternativeCastEffect(this);
        }
        else
        {
            skill.ResolveSkill(true);
        }
    }

    private StatData PostStatusEffectStats()
    {
        StatData data = new StatData();
        data.AddStats(currentStats);
        ApplyStatusEffectsOverTop(ref data);
        return data;
    }

    public void ApplyStatusEffectsOverTop(ref StatData affectedData)
    {
        float totalPercentageShift = 0.0f;
        float totalFlatShift = 0.0f;

        Dictionary<StatType, float[]> shifts = new Dictionary<StatType, float[]>()
        {
            {StatType.Attack, new float[2]{0, 0 } },
            {StatType.Defence, new float[2]{0, 0 } },
            {StatType.Speed, new float[2]{0, 0 } },
        };

        foreach(var v in currentStatusEffects)
        {
            foreach(var l in v.Key.StatsImpacted)
            {
                switch(l.changeType)
                {
                    case StatChangeType.Flat:
                        if(l.isBuff)
                        {
                            shifts[l.affectedStat][0] += l.changeValue;
                        }
                        else
                        {
                            shifts[l.affectedStat][0] -= l.changeValue;
                        }
                        break;
                    case StatChangeType.Percent:
                        if (l.isBuff)
                        {
                            shifts[l.affectedStat][1] += l.changeValue;
                        }
                        else
                        {
                            shifts[l.affectedStat][1] -= l.changeValue;
                        }
                        break;
                }
            }
        }

        foreach(var v in shifts)
        {
            affectedData.ModifyStat(v.Key, StatChangeType.Flat, v.Value[0]);
            affectedData.ModifyStat(v.Key, StatChangeType.Percent, v.Value[1]);
        }


    }

    public void AttackTarget(CombatEntity target, Skill skill)
    {
        target.TakeDamage(this, skill);
    }

    public void TakeDamage(CombatEntity attacker, Skill skill)
    {
        int damage = skill.TotalDamageAfterScaling(attacker.PostStatusEffectStats().Attack, PostStatusEffectStats().Defence);
        ModifyHealth(-damage);
    }
    public void Heal(int amount)
    {
        ModifyHealth(amount);
    }

    protected void ModifyHealth(int amount)
    {
        health += amount;
        health = Mathf.RoundToInt(Mathf.Clamp(health, 0, baseStats.Health));
        healthBar.UpdateBar((int)health, baseStats.Health);
    }

    public void TryApplyStatus(StatusEffect effect, StatusEffectApplicationData applicationData)
    {
        float rand = UnityEngine.Random.Range(0, 100.0f);
        if(rand > applicationData.chance)
        {
            Debug.Log("application of " + effect.EffectName + " failed.");
            return;
        }
        currentStatusEffects.TryGetValue(effect, out int turns);
        currentStatusEffects[effect] = turns > applicationData.turnDuration ? turns : applicationData.turnDuration;
    }

    public void ReduceStatusTimers(int toReduceBy)
    {
        for(int i = currentStatusEffects.Count - 1; i >= 0; i--)
        {
            StatusEffect key = currentStatusEffects.ElementAt(i).Key;
            currentStatusEffects[key] -= toReduceBy;
            if(currentStatusEffects[key] <= 0)
            {
                currentStatusEffects.Remove(key);
            }
        }
    }

    private void ModifyStats(StatData data)
    {
        currentStats.AddStats(data);
        Heal(data.Health);
    }

    private void ConsumeFood(FoodItem food)
    {
        if(food.Consume())
        {
            ModifyStats(food.GetStatData());
        }
    }

    public Skill GetRandomSkill()
    {
        if (Skills == null)
        {
            return null;
        }
        return Skills[UnityEngine.Random.Range(0, skills.Count)];
    }

    private void OnDestroy()
    {
        OnEntitySelected = null;
    }
}

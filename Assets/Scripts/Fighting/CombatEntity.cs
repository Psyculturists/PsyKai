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
    private Animator animator;
    [SerializeField]
    private StatusEffectDurationDictionary currentStatusEffects;
    [SerializeField]
    private StatData baseStats;
    [SerializeField]
    private StatData currentStats;
    [SerializeField]
    private AnimationClip castAnimation;
    [SerializeField]
    private BattleEntityDialogue entityDialogue;


    

    private float health;
    public float Health => health;
    public float Speed => currentStats.Speed;

    public virtual bool HasBeenSaved => Health >= currentStats.Health;
    public virtual bool HasBeenFailed => Health <= 0;
    public virtual bool IsAlive => Health > 0 && !HasBeenSaved;

    private List<Skill> skills;
    public List<Skill> Skills => skills;

    protected CombatEntityData entityData;
    public int ExpForDefeat => entityData.ExpOnDefeat;

    private string entityName;
    public string EntityName => entityName;

    private bool havingTurn = false;
    public bool HavingTurn => havingTurn;
    private bool hasCastThisTurn = false;
    private bool hasAnimatedThisTurn = false;

    private System.Action<CombatEntity> OnEntitySelected;
    private System.Action OnTurnFinished;

    private Skill skillToUse;
    private CombatEntity turnTarget;

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

    public void SetEndTurnAction(System.Action endTurnAction)
    {
        OnTurnFinished = endTurnAction;
    }

    public void StartEntityTurn(Skill skill, CombatEntity target)
    {
        havingTurn = true;
        hasAnimatedThisTurn = false;
        hasCastThisTurn = false;
        skillToUse = skill;
        turnTarget = target;
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
        entityName = name;
        nameField.text = name;
    }

    public async void CastSkill(Skill skill, CombatEntity target)
    {
        Debug.Log(this.nameField.text + " used " + skill.skillName);
        int damageResult = 0;
        if (skill.isSelfTargeted)
        {
            //animator?.Play(skill.AnimationToUse);
            if (skill.heals)
            {
                damageResult = Heal(skill.TotalDamageAfterScaling(PostStatusEffectStats().Attack, 0));
            }
            else
            {
                damageResult = SelfDamage(skill);
            }
            if (skill.HasStatusEffect)
            {
                TryApplyStatus(skill.Status, skill.ApplicationData);
            }
        }
        else
        {
            damageResult = AttackTarget(target, skill);
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
        FightingManager.Instance.LogBattleMessage(this, target, skill, damageResult);
        entityDialogue.SpawnDialogue(skill.DialogueOnCast);
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

    public int AttackTarget(CombatEntity target, Skill skill)
    {
        if (skill.heals)
        {
            return target.Heal(this, skill);
        }
        else
        {
            return target.TakeDamage(this, skill);
        }
    }

    public int TakeDamage(CombatEntity attacker, Skill skill)
    {
        int damage = skill.TotalDamageAfterScaling(attacker.PostStatusEffectStats().Attack, PostStatusEffectStats().Defence);
        ModifyHealth(-damage);
        return damage;
    }

    public int Heal(CombatEntity attacker, Skill skill)
    {
        int damage = skill.TotalDamageAfterScaling(attacker.PostStatusEffectStats().Attack, PostStatusEffectStats().Defence);
        ModifyHealth(damage);
        return damage;
    }
    public int Heal(int amount)
    {
        ModifyHealth(amount);
        return amount;
    }
    public int SelfDamage(Skill skill)
    {
        int damage = skill.TotalDamageAfterScaling(PostStatusEffectStats().Attack, 0);
        ModifyHealth(-damage);
        return damage;
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

    // Set up actual animator bools later
    private float PlayCastAnimation()
    {
        float animTime = 0.0f;
        if (animator)
        {
            animator.SetBool("Jump", true);
        }
        if (entityData is PlayerEntityData)
        {
            animator.SetBool("Jump", true);
        }
        // grab next state length
        AnimatorStateInfo? info = animator?.GetNextAnimatorStateInfo(0);
        animTime = info?.length > 0.2f ? info.Value.length : 0.5f;
        return animTime;
    }

    private void OnDestroy()
    {
        OnEntitySelected = null;
        OnTurnFinished = null;
    }

    float updateCycleWaitTime = 0.0f;
    private void Update()
    {
        if(havingTurn)
        {
            if(!IsAlive)
            {
                //shouldn't be in combat
                havingTurn = false;
                OnTurnFinished?.Invoke();
                return;
            }

            if (updateCycleWaitTime > 0.0f)
            {
                updateCycleWaitTime -= Time.deltaTime;
                return;
            }

            if(!hasAnimatedThisTurn)
            {
                hasAnimatedThisTurn = true;
                updateCycleWaitTime = PlayCastAnimation();
                return;
            }
            else if(!hasCastThisTurn)
            {
                hasCastThisTurn = true;
                CastSkill(skillToUse, turnTarget);
                updateCycleWaitTime = 1.5f; // small buffer time for end of round
                return;
            }

            havingTurn = false;
            OnTurnFinished?.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Consume Food Skill", menuName = "Data/Food Skill")]
public class ConsumeFoodSkill : Skill
{
    public System.Action<FoodItem> OnFoodResolution;

    private CombatEntity targetEntity = null;

    public override void AlternativeCastEffect(CombatEntity target)
    {
        targetEntity = target;
        base.AlternativeCastEffect(target);
        OpenFoodSelect();
    }

    [Button]
    public void OpenFoodSelect()
    {
        CookingUIParent.Instance.OnExitedWithoutUse += ResolveNoFood;
        CookingUIParent.Instance.OnUsedFood += ResolveWithFood;
        NavigationBar.Instance.OpenFoodFromBattle();
        //something
    }

    private void ClearSubscriptions()
    {
        CookingUIParent.Instance.OnExitedWithoutUse -= ResolveNoFood;
        CookingUIParent.Instance.OnUsedFood -= ResolveWithFood;
    }

    private void ResolveNoFood()
    {
        ClearSubscriptions();
        ResolveSkill(false);
    }
    public void ResolveWithFood(FoodItem item)
    {
        ClearSubscriptions();
        ResolveSkill(true);
        OnFoodResolution?.Invoke(item);
        OnFoodResolution = null;
        foreach (var effect in item.GetRecipe().StatusEffectsOnUse)
        {
            targetEntity.TryApplyStatus(effect.Key, new StatusEffectApplicationData() { chance = 100, turnDuration = effect.Value });
        }
    }
}

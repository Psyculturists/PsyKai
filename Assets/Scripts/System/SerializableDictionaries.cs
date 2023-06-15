using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class FloatDictionary : SerializableDictionary<float, float> { }

[Serializable]
public class IntDictionary : SerializableDictionary<int, int> { }

[Serializable]
public class GameObjectDictionary : SerializableDictionary<string, GameObject> { }

[Serializable]
public class SpriteDictionary : SerializableDictionary<string, Sprite> { }

[Serializable]
public class Vector3Dictionary : SerializableDictionary<string, Vector3> { }

[Serializable]
public class StatusEffectDurationDictionary : SerializableDictionary<StatusEffect, int> { }

[Serializable]
public class InputDictionary : SerializableDictionary<Inputs, KeyCode> { }

[Serializable]
public class IngredientAmountDictionary : SerializableDictionary<IngredientData, int> { }

[Serializable]
public class ItemOddsDictionary : SerializableDictionary<ItemData, float> { }

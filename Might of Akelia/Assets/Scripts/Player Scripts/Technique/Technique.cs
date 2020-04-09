using UnityEngine;

public abstract class technique : ScriptableObject
{
    public string Name;
    public float StaminaCost;
    public int RequiredLevel;
    public string Description;
    public abstract void Cast(TechniqueCaster caster);
}
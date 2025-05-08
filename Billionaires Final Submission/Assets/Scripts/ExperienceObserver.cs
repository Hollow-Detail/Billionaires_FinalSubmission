using UnityEngine;
using System.Collections.Generic;

public class ExperienceObserver : MonoBehaviour
{
    public static ExperienceObserver Instance;

    Dictionary<teamColor.ColorChoice, BaseExperience> baseMap = new();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterBase(BaseExperience baseExp)
    {
        baseMap[baseExp.baseTeam] = baseExp;
    }

    public void ReportDestruction(teamColor.ColorChoice aggressor)
    {
        if (baseMap.TryGetValue(aggressor, out var baseExp))
            baseExp.AddXP(10); 
    }
}
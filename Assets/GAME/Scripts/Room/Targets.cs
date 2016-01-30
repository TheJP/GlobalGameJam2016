using UnityEngine;
using System.Collections;

public enum Targets
{
    Blue,
    Green
}

public static class TargetExtensions
{
    public static Color ToColor(this Targets target)
    {
        switch (target)
        {
            case Targets.Blue:
                return Color.blue;
            case Targets.Green:
                return Color.green;
            default:
                Debug.LogWarning("No color was specified for target " + target);
                return Color.white;
        }
    }

    public static Targets[] GetTargets()
    {
        return (Targets[])System.Enum.GetValues(typeof(Targets));
    }
}

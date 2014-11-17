using System;

public class WeaponProperties
{
    protected float[,] damageOverRange;
    protected float rangeMax;
    protected int rof; //in RPM

    /// <summary>
    /// 1. head
    /// 2. body as toso + limbs
    /// 3. armored torso
    /// </summary>
    protected float[] damageMultipliers;

    public enum BodyPoints
    {
        Head,
        Body,
        ArmoredBody
    }

    public WeaponProperties()  { }

    #region Damage

    public float getDamage(float distance, BodyPoints point)
    {
        return getDamageOverDistance(distance) * damageMultipliers[(int)point];
    }

    public float getDamageOverDistance(float distance)
    {
        // It could be 2 pointers, but I'll take IDs only.
        // Get what range is applicable to distance value.
        int startVal = 0
          , endVal = 0;

        for (int i = 0; i < damageOverRange.GetLength(0); i++)
        {
            if (distance >= damageOverRange[i, 0])
            {
                startVal = i;
            }
            if (distance <= damageOverRange[i, 0])
            {
                endVal = i;
                break;
            }
        }

        if (endVal == 0)
        {
            endVal = startVal;
        }

        return getDamageOverRangePoints(distance, startVal, endVal);
    }

    protected float getDamageOverRangePoints(float distance, int startPointer, int endPointer)
    {
        if (damageOverRange[endPointer, 1] == damageOverRange[startPointer, 1])
        {
            return damageOverRange[startPointer, 1];
        }

        float diffRangeBase = damageOverRange[endPointer, 0] - damageOverRange[startPointer, 0];

        // Thales' theorem to count a damage over passed distance.
        // 1st case:
        // .startVal[DMG]
        // |\ 
        // | \
        // |__\.endVal[DMG]
        if (damageOverRange[startPointer, 1] > damageOverRange[endPointer, 1])
        {
            float diffDmg = damageOverRange[startPointer, 1] - damageOverRange[endPointer, 1];
            return ((diffDmg * (diffRangeBase - distance)) / diffRangeBase) + damageOverRange[endPointer, 1];
        }
        // 2st case:
        //    . endVal[DMG]
        //   /|
        //  / |
        //./__| startVal[DMG]
        else
        {
            float diffDmg = damageOverRange[endPointer, 1] - damageOverRange[startPointer, 1];
            return ((diffDmg * distance) / diffRangeBase) + damageOverRange[startPointer, 1];
        }
    }

    #endregion

    #region Damage Multipliers

    public float[] getDamageMultipliers()
    {
        return damageMultipliers;
    }

    #endregion

    #region Rate Of Fire

    public int getRof()
    {
        return rof;
    }

    #endregion

    #region Range

    public float getRange()
    {
        return rangeMax;
    }

    #endregion

}
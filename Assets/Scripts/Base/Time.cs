using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Represents a time value, either an absolute time or a duration. One turn is one hour.
/// </summary>
public class Time
{
    /// <summary>
    /// The absolute value of the time, in hours/turns.
    /// </summary>
    public int Value;

    public const int GAME_START_DAY = 1;
    public const int GAME_START_HOUR = 8;

    public Time(int hours = 0)
    {
        Value = hours;
    }

    public void IncreaseTime(int hours)
    {
        Value += hours;
    }
    public void SetTime(int hoursAbsolute)
    {
        Value = hoursAbsolute;
    }

    public string GetAsAbsoluteTimeString()
    {
        int hour = (GAME_START_HOUR + Value) % 24;
        int day = GAME_START_DAY + ((GAME_START_HOUR + Value) / 24);

        return $"Day {day}, {hour.ToString("00")}:00";
    }

    public string GetAsDurationString()
    {
        return $"{Value.ToString("00")}:00";
    }

    // Operator Overloads
    public static Time operator +(Time a, int val)
    {
        return new Time(a.Value + val);
    }

    public static Time operator -(Time a, int val)
    {
        return new Time(a.Value - val);
    }

    public static Time operator +(Time a, Time b)
    {
        return new Time(a.Value + b.Value);
    }

    public static Time operator -(Time a, Time b)
    {
        return new Time(a.Value - b.Value);
    }
}


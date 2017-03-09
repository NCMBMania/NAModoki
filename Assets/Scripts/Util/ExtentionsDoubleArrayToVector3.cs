using System;
using System.Collections;
using UnityEngine;

public static class ExtentionsDoubleArrayToVector3
{
    public static double[] ToDoubleArray(this Vector3 value)
    {
        return new double[3] { (double)value.x, (double)value.y, (double)value.z };
    }

    public static Vector3 ToVector3(this ArrayList value)
    {
        if (value.Count != 3)
        {
            Debug.LogError("Invalid Data Count");
            return Vector3.zero;
        }
        else
        {
            float vf0 = (float)Convert.ToDouble(value[0]);
            float vf1 = (float)Convert.ToDouble(value[1]);
            float vf2 = (float)Convert.ToDouble(value[2]);

            return new Vector3(vf0, vf1, vf2);
        }
    }
}
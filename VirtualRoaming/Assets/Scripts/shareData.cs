using UnityEngine;
using System.Collections.Generic;

public class shareData : MonoBehaviour {
    public static Vector3 basePosition;
    public static Vector3 baseScale;

    public static List<Vector3> positions;
    public static List<Vector3> scales;

    public static List<float> topPos;
    public static List<float> bottomPos;
    public static List<float> leftPos;
    public static List<float> rightPos;

    /// <summary>
    /// base top
    /// </summary>
    public static float btPos;
    /// <summary>
    /// base bottom
    /// </summary>
    public static float bbPos;
    /// <summary>
    /// base left
    /// </summary>
    public static float blPos;
    /// <summary>
    /// base right
    /// </summary>
    public static float brPos;
}

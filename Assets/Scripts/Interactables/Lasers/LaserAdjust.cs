using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdjustType {
    Reflect,
    Refract
}

public class LaserAdjust : MonoBehaviour {

    [SerializeField] private AdjustType _laserAdjustType = AdjustType.Reflect;
    [HideInInspector] public AdjustType LaserAdjustType => _laserAdjustType;

}
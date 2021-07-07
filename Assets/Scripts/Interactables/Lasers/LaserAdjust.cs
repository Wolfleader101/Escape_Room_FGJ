using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum for the types of laser adjustments.
public enum AdjustType {
    Reflect,
    Refract
}

// Simple script which holds information on what type of laser adjustment a LaserAdjust object is.
public class LaserAdjust : MonoBehaviour {

    [SerializeField] private AdjustType _laserAdjustType = AdjustType.Reflect;
    [HideInInspector] public AdjustType LaserAdjustType => _laserAdjustType;

}
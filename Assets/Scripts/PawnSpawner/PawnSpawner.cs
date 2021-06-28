using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSpawner : MonoBehaviour
{
    private void Awake()
    {
        throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnDrawGizmos()
    {
        var pos = transform.position;

        var radius = 0.75f;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pos, radius);
        
        Gizmos.color = Color.red;
        pos.z += radius;
        Gizmos.DrawLine(pos, pos + transform.forward * 2);
    }

}

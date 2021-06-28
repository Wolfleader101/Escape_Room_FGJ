using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<string> components;


    // Start is called before the first frame update
    void Start()
    {
        var player = Instantiate(playerPrefab, transform.position, transform.rotation);

        foreach (var component in components)
        {
            Type type = System.Type.GetType(component);

            player.AddComponent(type);
        }
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
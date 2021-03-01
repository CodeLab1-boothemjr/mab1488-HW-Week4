using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float speed = 1.0f;
    
    private GameObject _target;
    
    void Awake()
    {
        _target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float step =  speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, step);
    }
}

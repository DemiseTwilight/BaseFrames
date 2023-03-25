using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 velocity;
    public bool fixedRotate;
    void Start()
    {
        
    }
    void Update()
    {
        if (fixedRotate) { return; }
        this.transform.Rotate(velocity);
    }

    private void FixedUpdate() {
        if (!fixedRotate) { return; }
        this.transform.Rotate(velocity);
    }
}

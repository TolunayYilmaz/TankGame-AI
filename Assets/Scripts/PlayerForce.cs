using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForce : MonoBehaviour
{
    float Vertical;
    float Horizontal;
    Rigidbody rb;
    [SerializeField]
    private float speed;
    
    // Start is called before the first frame update
    void Start()
    {   rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vertical = Input.GetAxis("Vertical")*speed*Time.deltaTime;
        Horizontal = Input.GetAxis("Horizontal")*speed*Time.deltaTime;
        rb.AddForce(-Vertical, 0, Horizontal);

    }
}

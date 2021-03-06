using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Vector3 velocity;
    Rigidbody player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity) 
    {
        velocity = _velocity;
    }

    public void LookAt(Vector3 lookPoint) {
        Vector3 heightCorrectPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);

        transform.LookAt(heightCorrectPoint);
    }

    // Update is called once per frame
    void Update()
    {
        player.MovePosition(player.position + velocity * Time.deltaTime);
    }
}

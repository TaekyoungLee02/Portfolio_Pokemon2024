using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    private float gravity = -9.81f;
    private CharacterController playerCC;

    // Start is called before the first frame update
    void Awake()
    {
        playerCC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCC.isGrounded)
            playerCC.Move(new Vector3(0, gravity * Time.deltaTime, 0));
        else
            playerCC.Move(new Vector3(0, 0, 0));
    }
}

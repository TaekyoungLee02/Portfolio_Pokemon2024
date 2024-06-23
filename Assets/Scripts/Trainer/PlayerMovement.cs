using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float runSpeed = 1.5f;
    private GameObject playerPosition;
    private CharacterController playerCC;
    private Animator animator;

    private GameObject playerCamera;

    private float x;
    private float z;

    private int axisXID;
    private int axisZID;
    private int isMoveID;
    private int isRunID;

    private float run;

    void Awake()
    {
        playerCamera = GameObject.FindWithTag("PlayerThirdCamera");
        playerCC = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        axisXID = Animator.StringToHash("AxisX");
        axisZID = Animator.StringToHash("AxisZ");
        isMoveID = Animator.StringToHash("isMove");
        isRunID = Animator.StringToHash("isRun");

        playerCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform.GetChild(0);
        playerCamera.GetComponent<CinemachineVirtualCamera>().LookAt = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.LeftAlt)) OnAltButtonDown();
        else if (Input.GetKeyUp(KeyCode.LeftAlt)) OnAltButtonUp();
    }

    private void Move()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0)
        {
            animator.SetFloat(axisXID, x);
            animator.SetFloat(axisZID, z);
            animator.SetBool(isMoveID, true);

            Vector3 moveDirection = playerCamera.transform.TransformDirection(new Vector3(x, 0, z));
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z).normalized;

            transform.forward = moveDirection;

            playerCC.Move(moveDirection * speed * run * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool(isRunID, true);
                run = runSpeed;
            }
            else
            {
                animator.SetBool(isRunID, false);
                run = 1;
            }
        }
        else
        {
            animator.SetBool(isMoveID, false);
        }

        playerPosition.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    private void OnAltButtonDown()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnAltButtonUp()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        animator.SetBool(isMoveID, false);
        animator.SetBool(isRunID, false);

        playerPosition.transform.position = new Vector3(playerPosition.transform.position.x, playerPosition.transform.position.y + 1, playerPosition.transform.position.z);
    }

    private void OnEnable()
    {
        playerPosition = GetComponent<Player>().playerPos;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCamera = GameObject.FindWithTag("PlayerThirdCamera");
        playerCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform.GetChild(0);
        playerCamera.GetComponent<CinemachineVirtualCamera>().LookAt = transform.GetChild(0);

        transform.SetPositionAndRotation(playerPosition.transform.position, playerPosition.transform.rotation);
    }

    private void OnDestroy()
    {
        Destroy(playerPosition);
    }
}

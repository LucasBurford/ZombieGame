using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public Shooting shooting;
    public Rigidbody rb;
    private Camera mainCamera;
    public Animator animator;
    public AudioSource walkAudio;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Vector3 lookingdirection;

    public float defaultMoveSpeed;
    public float currentMoveSpeed;
    public float sprintingSpeed;

    public bool isShooting;
    public bool isAxing;
    public bool isPlayerMoving;
    public bool isSprinting;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        currentMoveSpeed = defaultMoveSpeed;
        rb.velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {        
        Movement();
        SetAnimations();
        Shooting();
        WalkingAudio();
    }

    private void FixedUpdate()
    {
        if (isSprinting)
        {
            currentMoveSpeed = sprintingSpeed;
        }
        else
        {
            currentMoveSpeed = defaultMoveSpeed;
        }

        rb.velocity = moveVelocity;
    }

    private void WalkingAudio()
    {
        if (moveInput.magnitude != 0)
        {
            isPlayerMoving = true;
        }
        else
        {
            isPlayerMoving = false;
        }

        if (isPlayerMoving)
        {
            if (!walkAudio.isPlaying)
            {
                walkAudio.Play();
            }
        }
        else
        {
            walkAudio.Stop();
        }
    }

    private void SetAnimations()
    {
        if (moveInput.z != 0)
        {
            animator.SetFloat("ForwardBackSpeed", currentMoveSpeed);
        }
        else
        {
            animator.SetFloat("ForwardBackSpeed", 0);
        }

        if (moveInput.x < 0)
        {
            animator.SetFloat("LeftSpeed", currentMoveSpeed);
        }
        else
        {
            animator.SetFloat("LeftSpeed", 0);
        }

        if (moveInput.x > 0)
        {
            animator.SetFloat("RightSpeed", currentMoveSpeed);
        }
        else
        {
            animator.SetFloat("RightSpeed", 0);
        }

        animator.SetBool("IsShooting", isShooting);
        animator.SetBool("IsAxing", isAxing);
    }

    private void Movement()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput * currentMoveSpeed;

        if (moveInput.magnitude == 0)
        {
            rb.angularVelocity = new Vector3(0, 0, 0);
        }

        isSprinting = (Input.GetKey(KeyCode.LeftShift));

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            lookingdirection = pointToLook;

            //transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shooting.isFiring = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            shooting.isFiring = false;
        }
    }
}

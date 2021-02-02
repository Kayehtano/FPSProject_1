using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Animator animator;
    public AudioSource audio;
    public MouseLook cam;

    [Header("Attributes")]
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float sprintModifier;
    public float health = 100f;

    [Header("Ground")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isSprinting;
    public bool isADS;

    public Camera normalCam;
    public Transform weaponParent;
    public Weapon weapon;

    private Vector3 weaponParentOrigin;
    private Vector3 targetWeaponBobPosition;

    [Header("FOV")]
    private float baseFOV;
    public float sprintFOVModifier = 1.25f;
    public float aimFOV = 0.75f;
    private float movementCounter;
    private float idleCounter;

    void Start()
    {
        baseFOV = normalCam.fieldOfView;
        animator = GetComponent<Animator>();
        weaponParentOrigin = weaponParent.localPosition;
        weapon = GetComponentInChildren<Weapon>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isADS = Input.GetButton("Fire2");

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        //Debug.Log(move.magnitude);  // Testing to see if it works

        // Headbob
        if (x == 0 && z == 0) // idle
        {
            Headbob(idleCounter, 0f, 0f);
            idleCounter += Time.deltaTime;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);
            weapon.spreadFactor = 0.05f;
        }
        else if(!isSprinting) // moving
        {
            Headbob(movementCounter, 0.04f, 0.04f);
            movementCounter += Time.deltaTime * 3f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 6f);
            weapon.spreadFactor = 0.1f;
        }
        else // sprinting
        {
            Headbob(movementCounter, 0.15f, 0.075f);
            movementCounter += Time.deltaTime * 7f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
            weapon.spreadFactor = 0.2f;
        }

        // Weapon SPREAD control
        if(isADS) // if aiming, reduce spread
        {
            weapon.spreadFactor = weapon.spreadFactor * 0.367f;
        }

        if(!controller.isGrounded) // if in air, spread is horrible
        {
            weapon.spreadFactor += 0.4f;
        }

        //Debug.Log(weapon.spreadFactor);

        // Sprinting control
        if(Input.GetKey(KeyCode.LeftShift))
        {
            controller.Move(move * speed * sprintModifier * Time.deltaTime);
            isSprinting = true;
        }
        else
        {
            controller.Move(move * speed * Time.deltaTime);
            isSprinting = false;
        }

        // Jump controls
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Animator
        if((x != 0 || z != 0) || (z != 0 && x != 0))
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        // FOV controller
        if (isSprinting)
        {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f);
        }
        else if(isADS)
        {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * aimFOV, Time.deltaTime * 8f);
        }
        else
        {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
        }
    }

    void Headbob(float p_z, float p_x_intensity, float p_y_intensity)
    {
        targetWeaponBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z * 2) * p_y_intensity, 0);
    }

    void playFootstep()
    {
        // Footsteps
        if (controller.isGrounded && !audio.isPlaying)
        {
            audio.volume = Random.Range(0.4f, 0.8f);
            audio.pitch = Random.Range(0.8f, 1.4f);
            audio.Play();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}

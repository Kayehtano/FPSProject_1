using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSights : MonoBehaviour
{
    private Vector3 originalPosition;
    public Vector3 aimPosition;
    public float adsSpeed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        Weapon weapon = GetComponent<Weapon>();
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2") && !Weapon.isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * adsSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * adsSpeed);
        }
    }
}

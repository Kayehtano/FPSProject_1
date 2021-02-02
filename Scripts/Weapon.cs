using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Animator")]
    private Animator anim;
    private AudioSource _AudioSource;

    [Header("Attributes")]
    public float range = 100f;
    public float damage = 2f;
    public int bulletsPerMag = 30;
    public int bulletsLeft = 90;
    public int currentBullets;
    public float spreadFactor = 0.1f;
    public float fireRate = 0.0f;

    [Header("References")]
    public Camera cam;
    public ParticleSystem muzzleFlash;
    public AudioClip shootSound;
    public GameObject bulletImpact;
    public GameObject[] hitParticles;
    private GameObject hitParticleEffect;
    public LineRenderer bulletTrail;
    public Transform shootPoint;

    float fireTimer;

    static public bool isReloading;

    private bool cheatsOn = false;
    private int originalBulletsPerMag;
    private int originalBulletsLeft;
    private float originalFireRate;

    private DetectHit detect;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();
        currentBullets = bulletsPerMag;
        originalBulletsLeft = bulletsLeft;
        originalBulletsPerMag = bulletsPerMag;
        originalFireRate = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            if (currentBullets > 0)
            {
                Fire();
            }
            else if(bulletsLeft > 0)
            {
                DoReload();
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(currentBullets < bulletsPerMag && bulletsLeft > 0)
                DoReload();
        }

        if(fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }

        // CHEATS

        if(Input.GetKeyDown(KeyCode.C) && !cheatsOn)
        {
            cheatsOn = !cheatsOn;
            bulletsPerMag = 150;
            bulletsLeft = 1000;
            fireRate = 0.05f;
            Debug.Log("Cheats are on!");
        }
        else if(Input.GetKeyDown(KeyCode.C) && cheatsOn)
        {
            cheatsOn = !cheatsOn;
            bulletsPerMag = originalBulletsPerMag;
            bulletsLeft = originalBulletsLeft;
            fireRate = originalFireRate;
            Debug.Log("Cheats are off!");
        }
    }

    void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        isReloading = info.IsName("AK103_reload");
        //if (info.IsName("AK103_fire")) anim.SetBool("Fire", false);   
    }

    private void Fire()
    {
        if (fireTimer < fireRate || currentBullets <= 0 || isReloading) return;

        RaycastHit hit;

        Vector3 accuracy = cam.transform.forward;
        accuracy.x += Random.Range(-spreadFactor, spreadFactor);
        accuracy.y += Random.Range(-spreadFactor, spreadFactor);

        if (Physics.Raycast(cam.transform.position, accuracy, out hit, range))
        {
            Debug.Log(hit.transform.name + " found");

            if (!hit.transform.CompareTag("Flesh")) { GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)); }

            if (hit.transform.CompareTag("Metal"))
            {
                hitParticleEffect = Instantiate(hitParticles[0], hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }
            else if(hit.transform.CompareTag("Wood"))
            {
                hitParticleEffect = Instantiate(hitParticles[1], hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }

            // DESTROY EFFECTS
            Destroy(hitParticleEffect, 1f);

            // DO DAMAGE
            DetectHit detect = hit.collider.transform.GetComponent<DetectHit>(); // detect hitbox
            if(detect != null)
            {
                detect.TakeDamage(damage);
                //Debug.Log(detect.name);
            }

            // Bullet trail
            SpawnBulletTrail(hit.point);
        }

        anim.CrossFadeInFixedTime("AK103_fire", 0.01f);
        muzzleFlash.Play();
        playShootSound();

        currentBullets--;
        fireTimer = 0.0f; // Reset fire timer
    }

    public void Reload()
    {
        if (bulletsLeft <= 0) return;

        int bulletsToLoad = bulletsPerMag - currentBullets;
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;
    }

    private void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject trailEffect = Instantiate(bulletTrail.gameObject, shootPoint.parent.position, Quaternion.identity);

        LineRenderer lineR = trailEffect.GetComponent<LineRenderer>();

        lineR.SetPosition(0, shootPoint.position);
        lineR.SetPosition(1, hitPoint);

        Destroy(trailEffect, 1f);
    }

    private void DoReload()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        if (isReloading) return;

        anim.CrossFadeInFixedTime("AK103_reload", 0.01f);

    }

    private void playShootSound()
    {
        _AudioSource.PlayOneShot(shootSound);
    }
}

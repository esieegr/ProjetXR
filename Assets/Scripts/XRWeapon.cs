using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRWeapon : MonoBehaviour
{

    public OVRInput.RawButton shootingBtn;

    public GameObject projectile;
    public Transform pointeur;
    public float vitesseProj = 20f;
    public bool forcedHeld = false;
    public Grabbable _grabbable;
    public float timedelay = 0.1f;


    public int magazineSize = 1;
    private int remainingAmmo;
    public int bulletPerShot = 1;

    private AudioSource shotSound;
    private OVRCameraRig cameraRig;


    // Start is called before the first frame update
    void Start()
    {

        shotSound = GetComponent<AudioSource>();
        remainingAmmo = magazineSize;
        cameraRig = FindAnyObjectByType<OVRCameraRig>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(shootingBtn) && forcedHeld) tir();
        
        if (_grabbable.SelectingPointsCount > 0)
        {
            OVRInput.RawButton currentTrigger = GetCurrentHandTrigger();
            if (OVRInput.GetDown(currentTrigger)) tir();
        }
        
        if (remainingAmmo == 0) Destroy(gameObject);
    }

    private OVRInput.RawButton GetCurrentHandTrigger()
    {
        if (cameraRig == null)
        {
            return shootingBtn;
        }

        Vector3 leftControllerPos = cameraRig.leftHandAnchor.position;
        Vector3 rightControllerPos = cameraRig.rightHandAnchor.position;
        
        float distanceToLeft = Vector3.Distance(transform.position, leftControllerPos);
        float distanceToRight = Vector3.Distance(transform.position, rightControllerPos);
        
        if (distanceToLeft < distanceToRight)
        {
            return OVRInput.RawButton.LIndexTrigger;
        }
        else
        {
            return OVRInput.RawButton.RIndexTrigger;
        }
    }

    void tir()
    {
        for (int i = 0; i < bulletPerShot; i++)
        {
            Debug.Log(remainingAmmo);
            remainingAmmo -= 1;
            StartCoroutine(projectileSpawn(timedelay * i));
        }
    }

    private IEnumerator projectileSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject nouvProjectile = Instantiate(projectile);
        nouvProjectile.transform.position = pointeur.position;
        nouvProjectile.GetComponent<Rigidbody>()
            .AddForce(pointeur.forward * vitesseProj, ForceMode.Impulse);
        shotSound.Play();
        Destroy(nouvProjectile, 5);
    }
}

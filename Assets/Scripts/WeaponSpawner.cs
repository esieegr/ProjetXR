using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject weaponPrefab;
    public float distanceFromPlayer = 1.5f;
    public float heightOffset = 0f;
    public float followSpeed = 5f;

    private GameObject spawnedWeapon;
    private Rigidbody weaponRigidbody;
    private OVRGrabbable grabbable;
    private bool wasGrabbed = false;
    private Transform playerCamera;
    private OVRCameraRig cameraRig;

    void Start()
    {
        cameraRig = FindAnyObjectByType<OVRCameraRig>();
        if (cameraRig != null)
        {
            playerCamera = cameraRig.centerEyeAnchor;
        }
        else
        {
            Debug.LogError("OVRCameraRig not found in scene");
        }

        SpawnWeapon();
    }

    void SpawnWeapon()
    {
        if (weaponPrefab == null)
        {
            Debug.LogWarning("Weapon prefab not assigned");
            return;
        }

        Vector3 spawnPosition = GetPositionInFrontOfPlayer();
        spawnedWeapon = Instantiate(weaponPrefab, spawnPosition, Quaternion.identity);

        weaponRigidbody = spawnedWeapon.GetComponent<Rigidbody>();
        if (weaponRigidbody != null)
        {
            weaponRigidbody.useGravity = false;
            weaponRigidbody.isKinematic = true;
        }

        grabbable = spawnedWeapon.GetComponent<OVRGrabbable>();
    }

    void Update()
    {
        if (spawnedWeapon != null)
        {
            if (grabbable != null && grabbable.isGrabbed && !wasGrabbed)
            {
                OnWeaponGrabbed();
            }

            if (!wasGrabbed)
            {
                Vector3 targetPosition = GetPositionInFrontOfPlayer();
                spawnedWeapon.transform.position = Vector3.Lerp(spawnedWeapon.transform.position, targetPosition, Time.deltaTime * followSpeed);

                Vector3 directionToPlayer = playerCamera.position - spawnedWeapon.transform.position;
                if (directionToPlayer != Vector3.zero)
                {
                    spawnedWeapon.transform.rotation = Quaternion.Slerp(
                        spawnedWeapon.transform.rotation,
                        Quaternion.LookRotation(directionToPlayer),
                        Time.deltaTime * followSpeed
                    );
                }
            }
        }
    }

    Vector3 GetPositionInFrontOfPlayer()
    {
        Vector3 forward = playerCamera.forward;
        forward.y = 0;
        forward.Normalize();

        return playerCamera.position + forward * distanceFromPlayer + Vector3.up * heightOffset;
    }

    void OnWeaponGrabbed()
    {
        wasGrabbed = true;

        if (weaponRigidbody != null)
        {
            weaponRigidbody.useGravity = true;
        }
    }
}


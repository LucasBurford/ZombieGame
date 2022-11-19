using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shooting : MonoBehaviour
{
    #region References
    [Header("References")]
    public CharacterControl control;
    public Bullet bulletPrefab;
    public GameObject player;
    public GameObject aimDot;
    public LineRenderer lr;
    public Slider ammoSlider;
    public Transform firePoint;
    public LayerMask zombieLayer;
    public TMP_Text reloadingText;
    public Image equippedWeaponImage;
    public Sprite smgIcon, assaultRifleIcon, machineGunIcon, shotGunIcon, axeIcon;
    public Color defaultColour;
    public Color targetColour;
    #endregion

    #region Gameplay
    [Header("Gameplay")]
    public Weapons equippedWeapon;
    public enum Weapons
    {
        SMG,
        AssaultRifle,
        MachineGun,
        ShotGun,
        Axe
    }

    public Vector3 aimOrigin, aimEnd;
    #endregion

    #region Ammo and gameplay values
    [Header("Ammo and gameplay values")]
    public int maxAmmo;
    public int currentAmmo;
    public int reserveAmmo;
    public float timeBetweenShots;
    public float currentDamage;
    public float currentReloadSpeed;
    public bool isAiming;
    RaycastHit aimLineHit;
    #endregion

    #region Fire rate values
    [Header("Fire rate values")]
    public float smgRate;
    public float assaultRifleRate;
    public float machineGunRate;
    public float shotGunRate;
    public float axeRate;
    #endregion

    #region Damage values
    [Header("Damage values")]
    public float smgDamage;
    public float assaultRifleDamage;
    public float machineGunDamage;
    public float shotGunDamage;
    public float axeDamage;
    #endregion

    #region Reload speed values
    [Header("Reload speed values")]
    public float smgReloadSpeed;
    public float assaultRifleReloadSpeed;
    public float machineGunReloadSpeed;
    public float shotGunReloadSpeed;
    public float axeAttackRange;
    #endregion

    #region Weapons acquired
    [Header("Weapons acquired")]
    public bool assaultRifleAcquired;
    public bool machineGunAcquired;
    public bool shotGunAcquired;
    public bool axeAcquired;
    #endregion

    #region Misc
    [Header("Misc")]
    public float aimLineLength;
    private float shotCounter;
    public float worldDepth;

    public string weaponReload;
    public string emptyWeapon;

    public bool isFiring;
    #endregion

    private void Start()
    {
        currentAmmo = maxAmmo;
        ammoSlider.maxValue = maxAmmo;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        firePoint.localRotation = transform.localRotation;
        if (isFiring && isAiming)
        {
            shotCounter -= Time.deltaTime;

            if (shotCounter <= 0 && currentAmmo > 0)
            {
                // Only do bullet and ammo stuff if a gun is equipped
                if (equippedWeapon != Weapons.Axe)
                {
                    Bullet newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity) as Bullet;
                    currentAmmo--;
                }
                else if (equippedWeapon == Weapons.Axe)
                {
                    control.isAxing = true;
                    AxeSwing();
                }

                shotCounter = timeBetweenShots;
                FindObjectOfType<AudioManager>().Play(equippedWeapon.ToString());
            }

            if (currentAmmo <= 0)
            {
                print("Out of ammo! Press R to reload");
                FindObjectOfType<AudioManager>().Play(emptyWeapon);
            }
        }
        else
        {
            shotCounter = 0;
            control.isAxing = false;
        }

        control.isShooting = isAiming;

        GetInput();
        UpdateWeaponSettings();
        DrawAimLine();
        UpdateUI();
    }

    private void DrawAimLine()
    {
        if (isAiming)
        {
            lr.positionCount = 2;

            aimOrigin = firePoint.position;
            aimEnd = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, worldDepth));

            if (DrawAimRay())
            {
                lr.startColor = targetColour;
                lr.endColor = targetColour;
                aimEnd = aimLineHit.point;

                aimDot.gameObject.SetActive(true);
                aimDot.transform.position = aimEnd;
            }
            else
            {
                lr.startColor = defaultColour;
                lr.endColor = defaultColour;
                aimDot.gameObject.SetActive(false);
            }

            Vector3[] line = new Vector3[]
            {
                aimOrigin, aimEnd
            };

            lr.SetPositions(line);
        }
        else
        {
            lr.positionCount = 0;
        }
    }

    private bool DrawAimRay()
    {
        bool lookingAtEnemy = false;

        if (Physics.Linecast(aimOrigin, aimEnd, out aimLineHit, zombieLayer))
        {
            lookingAtEnemy = true;
        }
        else
        {
            lookingAtEnemy = false;
        }

        return lookingAtEnemy;
    }

    private void Reload()
    {
        PlaySound(weaponReload);
        currentAmmo = maxAmmo;
        reloadingText.gameObject.SetActive(false);
    }

    private void GetInput()
    {
        isAiming = Input.GetMouseButton(1);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            equippedWeapon = Weapons.SMG;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && assaultRifleAcquired)
        {
            equippedWeapon = Weapons.AssaultRifle;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && machineGunAcquired)
        {
            equippedWeapon = Weapons.MachineGun;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && shotGunAcquired)
        {
            equippedWeapon = Weapons.ShotGun;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && axeAcquired)
        {
            equippedWeapon = Weapons.Axe;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(WaitToReload());
        }
    }

    private void AxeSwing()
    {
        // Cast sphere and store hits
        Collider[] hitObjects = Physics.OverlapSphere(firePoint.position, axeAttackRange);

        foreach(Collider col in hitObjects)
        {
            // If player hits a zombie with the axe
            if (col.gameObject.GetComponent<Zombie>())
            {
                Zombie z = col.gameObject.GetComponent<Zombie>();
                PlaySound("AxeImpactFlesh");
                z.TakeDamage(axeDamage);
            }
            // Else if they hit anything else
            else
            {
                if (col.gameObject.CompareTag("Concrete"))
                {
                    PlaySound("AxeImpactConcrete");
                }

                if (col.gameObject.CompareTag("Metal"))
                {
                    PlaySound("AxeImpactMetal");
                }

                if (col.gameObject.CompareTag("Wood"))
                {
                    PlaySound("AxeImpactWood");
                }
            }
        }
    }

    private void PlaySound(string name)
    {
        FindObjectOfType<AudioManager>().Play(name);
    }

    private void UpdateWeaponSettings()
    {
        switch (equippedWeapon)
        {
            case Weapons.SMG:
                {
                    timeBetweenShots = smgRate;
                    currentDamage = smgDamage;
                    currentReloadSpeed = smgReloadSpeed;
                    weaponReload = "SMGReload";
                    emptyWeapon = "SMGEmpty";
                    equippedWeaponImage.sprite = smgIcon;
                }
                break;
            case Weapons.AssaultRifle:
                {
                    timeBetweenShots = assaultRifleRate;
                    currentDamage = assaultRifleDamage;
                    currentReloadSpeed = assaultRifleReloadSpeed;
                    weaponReload = "AssaultRifleReload";
                    emptyWeapon = "AssaultRifleEmpty";
                    equippedWeaponImage.sprite = assaultRifleIcon;
                }
                break;
            case Weapons.MachineGun:
                {
                    timeBetweenShots = machineGunRate;
                    currentDamage = machineGunDamage;
                    currentReloadSpeed = machineGunReloadSpeed;
                    weaponReload = "MachineGunReload";
                    emptyWeapon = "MachineGunEmpty";
                    equippedWeaponImage.sprite = machineGunIcon;
                }
                break;
            case Weapons.ShotGun:
                {
                    timeBetweenShots = shotGunRate;
                    currentDamage = shotGunDamage;
                    currentReloadSpeed = shotGunReloadSpeed;
                    weaponReload = "ShotGunReload";
                    emptyWeapon = "ShotGunEmpty";
                    equippedWeaponImage.sprite = shotGunIcon;
                }
                break;
            case Weapons.Axe:
                {
                    timeBetweenShots = axeRate;
                    currentDamage = axeDamage;
                    equippedWeaponImage.sprite = axeIcon;
                }
                break;
        }
    }

    public void ChangeWeapon(int newWeapon)
    {
        equippedWeapon = (Weapons)newWeapon;
    }

    private void UpdateUI()
    {
        ammoSlider.value = currentAmmo;
    }

    IEnumerator WaitToReload()
    {
        reloadingText.gameObject.SetActive(true);
        yield return new WaitForSeconds(currentReloadSpeed);
        Reload();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(firePoint.position, axeAttackRange);
    //}
}

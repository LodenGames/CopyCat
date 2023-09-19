using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour {
    [Header("References")]
    [SerializeField] Transform cam;
    [SerializeField] Transform attackPoint;
    [SerializeField] Transform attackPointAlt;
    //[SerializeField] GameObject objectToThrow;

    [Header("Throwing")]
    KeyCode throwKey = KeyCode.Mouse1;
    [SerializeField] float throwForce;
    [SerializeField] float throwUpwardForce;
    [SerializeField] float throwDelay;

    [SerializeField] Animator throwableAnimator;

    CardCopier cardCopier;
    WeaponsController weaponsController;


    void Start() {
        cardCopier = GetComponent<CardCopier>();
        weaponsController = GetComponent<WeaponsController>();
    }

    void Update() {
        // check for activeWeapon
        if (Input.GetKeyDown(throwKey) && weaponsController.WeaponIsActive()) {
            //throwableAnimator.SetTrigger("Throw");
            Invoke(nameof(Throw), throwDelay);
        } 
    }

    void Throw() {

        // Get Active Weapon and throw that weapon instead of hammer

        SpawnableObjects objectToThrow = cardCopier.GetSpawnableObjects(weaponsController.activeWeapon.spawnable);
        weaponsController.DestroyActiveWeapon();


        GameObject projectile;
        bool addForce = true;

        float distanceToHit = CalculateHitDistance();
        //if (distanceToHit < 1.4f) {
        //    projectile = Instantiate(objectToThrow.spawnable, attackPointAlt.position, attackPointAlt.rotation);
        //    addForce = false;
        //} else if (distanceToHit < 2.25f) {
        //    projectile = Instantiate(objectToThrow.spawnable, attackPointAlt.position, attackPointAlt.rotation);
        //} else {
        //}

        projectile = Instantiate(objectToThrow.spawnable, attackPoint.position, attackPoint.rotation);
        if (addForce) {
            Vector3 forceDirection = CalculateForceDirection();

            Rigidbody projectileRigidBody;
            if (projectile.GetComponent<Rigidbody>() == null) {
                projectile.AddComponent<Rigidbody>();
                projectile.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
                projectile.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

            }
            projectileRigidBody = projectile.GetComponent<Rigidbody>();

            if (projectile.GetComponent<LightBreak>() == null) {
                projectile.AddComponent<LightBreak>();
            }

            Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

            projectileRigidBody.AddForce(forceToAdd, ForceMode.Impulse);
        }

    }


    private float CalculateHitDistance() {

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 100f)) {
            return hit.distance;
        }
        return 1000f;
    }

    private Vector3 CalculateForceDirection() {

        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f)) {
            
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        return forceDirection;
    }

}

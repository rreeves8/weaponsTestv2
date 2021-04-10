using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GunController : MonoBehaviour
{
    public GameObject weapon;
    public GameObject ammo;
    public GameObject fire_point;

    private GunADT gun;

    public double time;

    private List<GameObject> list = new List<GameObject>();

    void Start() {
        BuildGun();
    }

    void Update(){
     
        time += Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0)) {
            
            if (time > gun.getFireRate()) { //hold fire until next round is chambered
                time = 0;
                Transform pos = weapon.transform;
                
                GameObject obj = Instantiate(ammo, fire_point.transform.position, fire_point.transform.rotation) as GameObject;
                
                Rigidbody rb = obj.GetComponent<Rigidbody>();

                rb.velocity = rb.transform.forward * (float) gun.getMuzzleVelocity(); //get the front facing vector and multiply it by the veolocty value.
                
                rb.drag = (float) gun.getBullet().getCoeffeceintOfDrag(); //set drag
                rb.mass = (float) gun.getBullet().getTotalMass(); //set mass

                obj.transform.Rotate(0f, 90.0f, 0.0f, Space.Self); //bullet model comes out sideways and this was the best I got for it
            }
        }
    }

    void BuildGun() {
        gun = new M240(new NATO556());
    }

}

public class M240 : GunADT {
    private BulletADT bullet;

    private AttachmentsADT barrel;

    private double barrelLength = 0.3556; //13inch

    public M240 (BulletADT type) {
        bullet = type;
    }

    public double getMuzzleVelocity() {
        if (barrel != null) {
            barrelLength = barrel.getBarrelLength();
        }

        double ForceOfPropelant = bullet.getAdiabaticFlameConstant() * bullet.getGunPowderEnthalpy(); //force of propelent, see doc
        
        Debug.Log("Propelent force"+": flame constant = "+ bullet.getAdiabaticFlameConstant()+ " * Gun Enthalpy: "+ bullet.getGunPowderEnthalpy() +" = " + ForceOfPropelant);

        double work = ForceOfPropelant * barrelLength; // W= F*D, force being propelent and distance being barrel length.
        Debug.Log("Work Done by propelent through barrel length" + work);

        double velocity = Math.Sqrt(((2 * work) / bullet.getBulletMass())); //convert work energy to kinetic energy as it leaves barrel, should include friction from barrel
        Debug.Log("Work Energy converted to Kinematic energy, V final = " + velocity);

        return velocity;
    }

    public void addAtachment(AttachmentsADT attachment) {

        if (typeof(ExtendedBarrel).IsAssignableFrom(attachment.GetType())) {
            barrel = attachment;
        }
    }

    public double getFireRate() {
        return 1/15.8; //950 rounds per minute
    }

    public BulletADT getBullet() {
        return bullet;
    }
}

/// <summary>
/// barrel Upgrade derived from attachments, not used, it increases the barrel length so when the work calculation is done, distance is higher giving it more kintic energy
/// </summary>

public class ExtendedBarrel : AttachmentsADT {

    public double getIncreasedMass() {
        return 5;
    }

    public double getBarrelLength() {
        return 20; //20 inch
    }
}

/// <summary>
/// nato 5.56 round, data taken from google. 
/// </summary>
public class NATO556 : BulletADT {
    public double totalMass = 12.31; //12.31 grams
    public double grain = 100;
    public double gunPowderEnthalpy = 2931000; //2931 kj/kg

    public double unknownCoeffecient = 0.383229621509869; //very complex calculations that depend on pressure inside cartidge casing and the enthalpy of transformation, I just reversed calculated everything entirely necissary and the missing comonent left as a constant, the constant changes slightly depneding on gun powder quality and different chemcial structure
    
    public double Cd = 1.3; //under normal circumstances

    public double getGunPowderEnthalpy() {
        return gunPowderEnthalpy * ((grain / 15432));
    }

    public double getCoeffeceintOfDrag() {
        
        return Cd;
    }

    public double getTotalMass() {
        return totalMass;
    }

    public double getBulletMass() {
        return ((grain / 15.432) / 1000);
    }

    public double getAdiabaticFlameConstant() {
        Debug.Log("Flame Constant =" + unknownCoeffecient);
        return unknownCoeffecient;
    }
}


/// <summary>
/// bullet types and there properties can be extended from here and different guns can have different bullet types
/// </summary>

public interface BulletADT {
    double getGunPowderEnthalpy();

    double getCoeffeceintOfDrag();

    double getTotalMass();

    double getAdiabaticFlameConstant();

    double getBulletMass();
}

/// <summary>
/// Attachments interface, types of attachments can be made from here and easily added to the gun class
/// </summary>

public interface AttachmentsADT {
    double getIncreasedMass();

    double getBarrelLength();
}

/// <summary>
/// Base interface for gun, different guns cant be derived from here to make player implentation easier
/// </summary>

public interface GunADT {
    double getMuzzleVelocity();
    
    void addAtachment(AttachmentsADT attachment);

    double getFireRate();

    BulletADT getBullet();
}
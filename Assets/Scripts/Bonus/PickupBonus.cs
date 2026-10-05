using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum BonusType { Star, Helmet, Timer, Pistol, Tank, Granade, Shovel }
public class PickupBonus : MonoBehaviour
{
 

   
    [SerializeField] private BonusType bonusType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TankBonusHandler handler = collision.GetComponent<TankBonusHandler>();
        if (handler != null)
        {
            handler.ActivateBonus(bonusType);

            Destroy(gameObject);
        }
    }
}

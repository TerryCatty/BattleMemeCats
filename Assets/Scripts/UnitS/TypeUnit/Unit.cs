using UnityEngine;

public class Unit : MonoBehaviour
{
   
    public int health {get; protected set; }
    public int baseHealth;


    public TeamColor colorTeam;

    public bool isDead;

    public enum TeamColor
    {
        Blue,
        Red,
    }

    protected UnitTower tower;

    public virtual void Init(UnitTower tower)
    {
        health = baseHealth;

        this.tower = tower;
    }

    public virtual void Heal(int value)
    {
        health += value;
    }
    public virtual void GetDamage(int damage)
    {
        health -= damage;
        CheckDeath();
    }

    protected virtual void CheckDeath()
    {
        if (health > 0) return;

        isDead = true;

        if (transform.parent != null)
        {
            tower?.MinusUnit(transform.parent);

            GameObject parent = transform.parent.gameObject;

            transform.SetParent(null);

            Destroy(parent);
        }

        Destroy(gameObject);
        
    }



    
}

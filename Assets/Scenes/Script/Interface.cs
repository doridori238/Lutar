using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IHitAble 
{
    public void Hit(int damage);

}

public interface IAttackAble
{
    public void Attack(IHitAble hitable);

}

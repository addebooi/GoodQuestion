using UnityEngine;

using System.Collections;


public interface IDamageAble {
    void Damage(float damage, Vector3 pos, Constants.ENTITYTEAM team);
}
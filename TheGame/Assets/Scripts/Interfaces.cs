using UnityEngine;

using System.Collections;


public interface IDamageAble {
    void Damage(float damage);
}

public interface IDamage {
    float DamageMultiplier { get; set; }
}

public interface IAOE {
    float Radius { get; set; }
}

public interface IProjectile {
    float Speed { get; set; }
    float MaxDistance { get; set; }
}
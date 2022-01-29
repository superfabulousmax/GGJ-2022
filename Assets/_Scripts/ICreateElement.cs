using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICreateElement
{
    public void Instantiate(ElementProjectile elementProjectile, Vector2 direction);
}

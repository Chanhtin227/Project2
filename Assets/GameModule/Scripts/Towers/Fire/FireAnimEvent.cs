using UnityEngine;
using System.Collections;

public class FireAnimEvent : MonoBehaviour
{
    private FireTower tower;

    void Start()
    {
        tower = GetComponentInParent<FireTower>();
    }

    public void Shoot()
    {
        tower?.shootEvent();
    }
}
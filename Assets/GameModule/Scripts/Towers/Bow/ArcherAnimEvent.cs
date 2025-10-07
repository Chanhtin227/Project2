using UnityEngine;

public class ArcherAnimEvent : MonoBehaviour
{
    private ProjectileTower tower;
    private Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
        tower = GetComponentInParent<ProjectileTower>();
    }

    private void Update()
    {
        detectEnemy();
    }

    private void detectEnemy()
    {
        if (tower == null || tower.enemyLayer == 0) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(tower.transform.position, tower.Range, tower.enemyLayer);
        if (hits.Length > 0)
        {
            if (hits[0].transform.position.x >= this.transform.position.x)
                this.transform.localScale = new Vector2(1, 1);
            else
                this.transform.localScale = new Vector2(-1, 1);
        }
    }

    public void Shoot()
    {
        tower?.shootEvent();
    }
}
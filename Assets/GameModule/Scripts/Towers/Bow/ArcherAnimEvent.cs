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

    // Chỉ flip khi bắn, không theo dõi liên tục
    public void Shoot()
    {
        // Lấy target hiện tại từ tower (giả sử tower có biến currentTarget)
        if (tower != null && tower.enemyLayer != 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(tower.transform.position, tower.Range, tower.enemyLayer);
            if (hits.Length > 0)
            {
                float enemyX = hits[0].transform.position.x;
                float archerX = this.transform.position.x;
                
                // Flip trước khi bắn
                if (enemyX >= archerX)
                    this.transform.localScale = new Vector2(1, 1);
                else
                    this.transform.localScale = new Vector2(-1, 1);
            }
        }
        
        tower?.shootEvent();
    }
}
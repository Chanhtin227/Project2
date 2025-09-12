using UnityEngine;

public class CheckpointsManager : MonoBehaviour
{
    public static CheckpointsManager main;
    public Transform[] checkpoints;
    void Awake()
    {
        main = this;
    }
}

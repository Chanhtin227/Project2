using UnityEngine;

public class MultiPathCheckpointsManager : MonoBehaviour
{
    public static MultiPathCheckpointsManager main; 

    public Transform[] pathA;
    public Transform[] pathB;
    public Transform[] pathC;

    void Awake()
    {
        main = this;
    }

    public Transform[] GetPath(int id)
    {
        switch (id)
        {
            case 0: return pathA;
            case 1: return pathB;
            case 2: return pathC;
        }
        return null;
    }
}

using UnityEngine;

public class MultiPathCheckpointsManager : MonoBehaviour
{
    public static MultiPathCheckpointsManager main; 

    public Transform[] pathA;
    public Transform[] pathB;
    public Transform[] pathC;
    public Transform[] pathD;
    public Transform[] pathE;

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
            case 3: return pathD;
            case 4: return pathE;
        }
        return null;
    }
}

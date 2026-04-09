using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public Transform dropPoint;

    void Awake()
    {
        Instance = this;
    }
}


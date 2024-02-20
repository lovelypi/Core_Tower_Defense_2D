using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] public LevelData levelData;

    
    [SerializeField] public Transform startPoint;
    [SerializeField] public Transform[] pathway;
}

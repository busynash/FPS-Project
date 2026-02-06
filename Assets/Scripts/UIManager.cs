using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static UIManager Instance;

    public GameObject hitUI;

    private void Awake()
    {
        Instance = this;
    }

    public void InstantiateHitUI()
    {
        Instantiate(hitUI,transform);
    }
}

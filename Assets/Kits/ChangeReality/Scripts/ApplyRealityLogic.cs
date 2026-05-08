using Unity.Cinemachine;
using UnityEngine;

public class ApplyRealityLogic : MonoBehaviour
{
    [SerializeField] private Material shadowMaterial;
    [SerializeField] private Material realMaterial;
    [SerializeField] private CinemachineCamera camera;


    private ChangeReality cr;
    private int myReality;

    private void OnEnable()
    {
        myReality = gameObject.layer;
        cr = FindFirstObjectByType<ChangeReality>();
        if (cr == null )
        {
            Debug.LogWarning("Definición de realidad no encontrada");
        }

        if (gameObject.CompareTag("Player"))
        {
            cr.onChangeReality.AddListener(OnChangeRealityPlayer);
        } else
        {
            cr.onChangeReality.AddListener(OnChangeReality);
        }
    }

    private void Awake()
    {
        GetComponent<Collider>().enabled = gameObject.layer == 6;
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().useGravity = gameObject.layer == 6;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChangeReality(int newReality)
    {

        if (myReality == newReality)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().material = realMaterial;
        } else
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().material = shadowMaterial;
        }
    }

    public void OnChangeRealityPlayer(int newReality)
    {
        int childs = transform.GetChild(0).childCount;
        Material newMaterial;
        SkinnedMeshRenderer smr;

        if (myReality == newReality)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = true;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<PlayerBehaviour>().characterActive = true;
            camera.Target.TrackingTarget = gameObject.transform;
            newMaterial = realMaterial;
        }
        else
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<PlayerBehaviour>().characterActive = false;
            newMaterial = shadowMaterial;
        }

        for (int i = 0; i < childs; i++)
        {
            if (smr = transform.GetChild(0).GetChild(i).GetComponent<SkinnedMeshRenderer>())
            {
                smr.material = newMaterial;
            }
        }
    }
}

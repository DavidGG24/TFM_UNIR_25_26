using Unity.Cinemachine;
using UnityEngine;
using static ChangeReality;

public class ApplyRealityLogic : MonoBehaviour
{
    [SerializeField] private Material disabledMaterial;
    [SerializeField] private Material enabledMaterial;
    [SerializeField] private CinemachineCamera camera;
    public KindOfReality myReality;


    private ChangeReality[] crs;

    private void OnEnable()
    {
        crs = FindObjectsByType<ChangeReality>(FindObjectsSortMode.None);
        if (crs.Length == 0 )
        {
            Debug.LogWarning("Definición de realidad no encontrada");
        }

        foreach (ChangeReality cr in crs)
        {
            if (gameObject.CompareTag("Player"))
            {
                cr.onChangeReality.AddListener(OnChangeRealityPlayer);
            }
            else if (gameObject.CompareTag("Mirror"))
            {
                cr.onChangeReality.AddListener(OnChangeRealityMirror);
            }
            else
            {
                cr.onChangeReality.AddListener(OnChangeReality);
            }
        }
    }

    private KindOfReality currentReality;
    private void Awake()
    {
        if (!gameObject.CompareTag("Mirror"))
        {
            GetComponent<Collider>().enabled = myReality == KindOfReality.Real || myReality == KindOfReality.Both;
        }
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().useGravity = myReality == KindOfReality.Real || myReality == KindOfReality.Both;
        }

        currentReality = KindOfReality.Real;
    }

    void FixedUpdate()
    {
        if (myReality != currentReality && myReality != KindOfReality.Both && gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }

    public void OnChangeReality(KindOfReality newReality)
    {

        if (myReality == newReality)
        {
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().material = enabledMaterial;
        } else if (myReality == KindOfReality.Both)
        {
            if (newReality == KindOfReality.Real)
            {
                gameObject.GetComponent<MeshRenderer>().material = enabledMaterial;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material = disabledMaterial;
            }
        } else
        {
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().material = disabledMaterial;
        }
    }

    public void OnChangeRealityPlayer(KindOfReality newReality)
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
            newMaterial = enabledMaterial;
        }
        else if (myReality == KindOfReality.Both)
        {
            if (newReality == KindOfReality.Real)
            {
                newMaterial = enabledMaterial;
            } else
            {
                newMaterial = disabledMaterial;
            }
        } else
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<PlayerBehaviour>().characterActive = false;
            newMaterial = disabledMaterial;
        }

        for (int i = 0; i < childs; i++)
        {
            if (smr = transform.GetChild(0).GetChild(i).GetComponent<SkinnedMeshRenderer>())
            {
                smr.material = newMaterial;
            }
        }

        currentReality = newReality;
    }

    private void OnChangeRealityMirror(KindOfReality newReality)
    {
        if (myReality == newReality || myReality == KindOfReality.Both)
        {
            //GetComponent<ChangeReality>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(1).GetComponent<Camera>().enabled = true;
        }
        else
        {
            //GetComponent<ChangeReality>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<Camera>().enabled = false;
        }
    }
}

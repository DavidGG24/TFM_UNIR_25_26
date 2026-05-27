using Unity.Cinemachine;
using UnityEngine;
using static ChangeReality;

public class ApplyRealityLogic : MonoBehaviour
{
    [SerializeField] private Material disabledMaterial;
    [SerializeField] private Material enabledMaterial;
    [SerializeField] private CinemachineCamera camera;
    public KindOfReality myReality;


    private ChangeReality cr;

    private void OnEnable()
    {
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

    private KindOfReality currentReality;
    private void Awake()
    {
        GetComponent<Collider>().enabled = myReality == KindOfReality.Real || myReality == KindOfReality.Both;
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
            gameObject.GetComponent<BoxCollider>().enabled = true;
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
            gameObject.GetComponent<BoxCollider>().enabled = false;
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
}

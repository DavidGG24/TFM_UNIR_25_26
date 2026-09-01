//using Unity.Cinemachine;
using UnityEngine;
using static ChangeReality;

public class ApplyRealityLogic : MonoBehaviour
{
    [SerializeField] private Material disabledMaterial;
    [SerializeField] private Material enabledMaterial;
    //[SerializeField] private CinemachineCamera camera; 
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

    public KindOfReality currentReality;
    private void Start()
    {
        if (!gameObject.CompareTag("Mirror") && GetComponent<Collider>())
        {
            GetComponent<Collider>().isTrigger = myReality == KindOfReality.Shadow;
            GetComponent<Collider>().enabled = myReality == KindOfReality.Real || myReality == KindOfReality.Both;
        }
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().useGravity = myReality == KindOfReality.Real || myReality == KindOfReality.Both;
        }

        currentReality = FindFirstObjectByType<PlayerDataManager>().playerReality;
        Debug.Log("He empezado y ahora tengo: " + currentReality);
    }

    void FixedUpdate()
    {
        if (myReality != currentReality && myReality != KindOfReality.Both && GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }

    public void OnChangeReality(KindOfReality newReality)
    {

        if (myReality == newReality)
        {
            GetComponent<Collider>().enabled = true;
            GetComponent<Collider>().isTrigger = false;
            GetComponent<MeshRenderer>().material = enabledMaterial;
            if (transform.childCount == 1)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            } else if (transform.childCount >= 1)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<MeshRenderer>().material = enabledMaterial;
                }
            }
        } else if (myReality == KindOfReality.Both)
        {
            if (newReality == KindOfReality.Real)
            {
                GetComponent<MeshRenderer>().material = enabledMaterial;
                if(transform.childCount > 0)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).GetComponent<MeshRenderer>().material = enabledMaterial;
                    }
                }
            }
            else
            {
                GetComponent<MeshRenderer>().material = disabledMaterial;
                if (transform.childCount > 0)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).GetComponent<MeshRenderer>().material = disabledMaterial;
                    }
                }
            }
        } else
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<Collider>().isTrigger = true;
            GetComponent<MeshRenderer>().material = disabledMaterial;
            if (transform.childCount == 1)
            {
                transform.GetChild(0).gameObject.SetActive(false);
            } else if (transform.childCount >= 1)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<MeshRenderer>().material = disabledMaterial;
                }
            }
        }

        currentReality = newReality;
    }

    public void OnChangeRealityPlayer(KindOfReality newReality)
    {
        int childs = transform.GetChild(0).childCount;
        Material newMaterial;
        SkinnedMeshRenderer smr;

        if (myReality == newReality)
        {
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<PlayerBehaviour>().characterActive = true;
            //camera.Target.TrackingTarget = gameObject.transform;
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
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<PlayerBehaviour>().characterActive = false;
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
            GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(1).GetComponent<Camera>().enabled = true;
        }
        else
        {
            //GetComponent<ChangeReality>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<Camera>().enabled = false;
        }

        currentReality = newReality;
    }
}

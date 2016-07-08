using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class PlayerBase : MonoBehaviour {

    public int PlayerId;

    public PlayerLinearMovement Movement { get; private set; }
    public PlayerAttack Attack { get; private set; }
    public PlayerStats Stats { get; private set; }

    void Awake()
    {
        Movement = GetComponent<PlayerLinearMovement>();
        Attack = GetComponent<PlayerAttack>();
        Stats = GetComponent<PlayerStats>();
    }

    void Start()
    {
        try
        {
            InjectGearVrStuff();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    private void InjectGearVrStuff()
    {
        UnityEngine.VR.VRSettings.renderScale = 1.8f;

        var oldCamera = FindOldCamera(gameObject);
        Debug.Log("oldCamera: " + (oldCamera != null));
        var gearCamera = (GameObject)Instantiate(Resources.Load("GearVrCamera"));
        Debug.Log("gearCamera: " + (gearCamera != null));
        gearCamera.transform.parent = oldCamera.transform.parent;
        gearCamera.transform.position = oldCamera.transform.position;
        gearCamera.transform.rotation = oldCamera.transform.rotation;
        gearCamera.transform.localScale = oldCamera.transform.localScale;
        gearCamera.SetActive(true);
        oldCamera.gameObject.SetActive(false);
        Debug.Log("Done Injecting GearVR Stuff");
    }

    private GameObject FindOldCamera(GameObject player)
    {
        GameObject cam = null;
        var camTransform = player.transform.FindChild("Camera/Head/Main Camera").gameObject;
        if (camTransform)
        {
            cam = camTransform.gameObject;
        }
        if (!cam)
        {
            var cams = GameObject.FindGameObjectsWithTag("MainCamera")
                .Where(x => GetComponent<Camera>());
            if (cams.Count() == 1)
            {
                cam = cams.First();
            }
            else
            {
                Debug.LogWarning(string.Format("Found {0} old cameras before swap", cams.Count()));
            }
        }

        return cam;
    }

}

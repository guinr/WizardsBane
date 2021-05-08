using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RoomLightController : MonoBehaviour
{
    public float lightOnIntensity = 3f;
    public int lightSteps = 6;
    
    private GameObject _lights;
    private bool _toggledOn;
    private Hashtable _lightSources;

    private void Start()
    {
        _lightSources = new Hashtable();
        _lights = transform.Find("Lights").gameObject;
        StartCoroutine(ChangeIntensity(false));
    }

    private void OnTriggerStay2D(Collider2D cldr)
    {
        if (_toggledOn || !cldr.gameObject.CompareTag("Player")) return;
        
        StartCoroutine(ChangeIntensity(true));
    }

    private void OnTriggerExit2D(Collider2D cldr)
    {
        if (!cldr.gameObject.CompareTag("Player")) return;
        
        StartCoroutine(ChangeIntensity(false));
    }
    
    private IEnumerator ChangeIntensity(bool toggleOn)
    {
        _toggledOn = toggleOn;
        
        var i = 0;
        while (i <= lightSteps)
        {
            i++;

            ChangeChildrenIntensity(toggleOn);

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ChangeChildrenIntensity(bool toggleOn)
    {
        var lights = _lights.transform;
        if (lights.childCount == 0) return;

        var childCount = lights.childCount;

        for (var i = 0; i < childCount; i++)
        {
            var child = lights.GetChild(i).gameObject;
            
            try
            {
                var childLight = (Light2D) child.GetComponentInChildren(typeof(Light2D));

                var maxIntensity = lightOnIntensity;

                try
                {
                    if (_lightSources[child.GetInstanceID()] == null)
                    {
                        _lightSources.Add(child.GetInstanceID(), childLight.intensity);
                    }

                    maxIntensity = (float) _lightSources[child.GetInstanceID()];
                }
                catch (Exception e)
                {
                    Debug.Log("Problem: " + child.GetInstanceID() + " " + e.Message);
                }
                
                var intensitySteps = maxIntensity / lightSteps;

                if (toggleOn && childLight.intensity >= maxIntensity) return;

                childLight.intensity += toggleOn ? intensitySteps : -intensitySteps;
            }
            catch (Exception e)
            {
                Debug.Log("Light not found in: " + child + " " + e.Message);
            }
        }
    }
}

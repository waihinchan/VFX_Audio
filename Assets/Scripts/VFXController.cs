using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class VFXController : MonoBehaviour
{   
    public VisualEffect VFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // VFX.SetFloat("_Amp",SoundInManager.Amplitude);
        VFX.SetTexture("_ArtibuteMap",VFXAttributeMapGenerator.ArtibuteMap);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
[RequireComponent(typeof(AudioSource))]
public class SoundInManager : MonoBehaviour
{   
    AudioSource _AudioSource;
    public bool UseMicrophone = true;
    public GameObject DebugObject;
    public int sampleDataLength = 1024;
    public static float Amplitude;
    private float[] AudioSample;
    public int timelength = 10;
    public static Texture2D ArtibuteMap;
    private float[] AudioBufferData;
    public RawImage myimage;
    // Start is called before the first frame update
    void Awake(){

    }
    void Start()
    {   
        AudioSample  = new float[sampleDataLength];
        _AudioSource = gameObject.GetComponent<AudioSource>();
        _AudioSource.loop = true;
        if(UseMicrophone){
            if(Microphone.devices.Length==0){
                Debug.LogError("please assign a microphone!");
                return;
            }
            _AudioSource.clip = Microphone.Start(Microphone.devices[0], true, sampleDataLength, 44100);
            while(!(Microphone.GetPosition(Microphone.devices[0])>0)){
                _AudioSource.Play();
            }
        }
        else{
            if(_AudioSource.clip!=null){
                _AudioSource.Play();
            }
            else{
                Debug.LogError("please assign a audio clip!");
                return;
            }
        }
        
        ArtibuteMap = new Texture2D(timelength,sampleDataLength);
        AudioBufferData = new float[timelength * sampleDataLength];
        for(int i = 0; i < AudioBufferData.Length; i++){
            AudioBufferData[i] = 0;
        }

    }


    // Update is called once per frame
    void Update()
    {   
        MakeArtibuteMap();

    }
    void GetAmp(){
        _AudioSource.clip.GetData(AudioSample,_AudioSource.timeSamples);
        Amplitude = 0;
        foreach (var sample in AudioSample)
        {
            Amplitude+=Mathf.Abs(sample);
        }
        Amplitude = Amplitude/sampleDataLength;
        if(DebugObject!=null){
            DebugObject.transform.localScale = new Vector3(Amplitude+0.1f,Amplitude+0.1f,Amplitude+0.1f); 
        }

    }
    void MakeArtibuteMap(){
        
        _AudioSource.GetSpectrumData(AudioSample, 0, FFTWindow.Rectangular);

        float[] KeepPart = AudioBufferData.Skip(sampleDataLength).Take(AudioBufferData.Length).ToArray();
        
        for(int i = 0; i< AudioBufferData.Length - sampleDataLength;i++){
                AudioBufferData[i] = KeepPart[i];
        }
        for(int i = AudioBufferData.Length - sampleDataLength; i < AudioBufferData.Length; i++){
            int index = i - AudioBufferData.Length + sampleDataLength;
            AudioBufferData[i] = AudioSample[index];
        }
        Color[] colors = new Color[AudioBufferData.Length];
        for(int i = 0; i < colors.Length;i++){
            colors[i] = new Color(AudioBufferData[i]*255,AudioBufferData[i]*255,AudioBufferData[i]*255);
        }
        ArtibuteMap.SetPixels(colors);
        ArtibuteMap.Apply();
        // myimage.texture = ArtibuteMap;

    }


    
}












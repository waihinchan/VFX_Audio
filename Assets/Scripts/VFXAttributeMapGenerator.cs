using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class VFXAttributeMapGenerator : MonoBehaviour
{   



    private AudioSpectrum _AudioSpectrum;
    public int timelength = 10;
    public static Texture2D ArtibuteMap;
    private float[] AudioBufferData;
    private float[] AudioSample;
    private int sampleDataLength;
    public RawImage image;
    // Start is called before the first frame update
    void Start()
    {   
        
        _AudioSpectrum = gameObject.GetComponent<AudioSpectrum>();
        if(_AudioSpectrum==null){
            return;
        }
        switch (_AudioSpectrum.bandType)
        {
            case AudioSpectrum.BandType.FourBand:
                sampleDataLength = 4;
                break;
            case AudioSpectrum.BandType.FourBandVisual:
                sampleDataLength = 4;
                break;
            case AudioSpectrum.BandType.EightBand:
                sampleDataLength = 8;
                break;
            case AudioSpectrum.BandType.TenBand:
                sampleDataLength = 10;
                break;
            case AudioSpectrum.BandType.TwentySixBand:
                sampleDataLength = 26;
                break;
            case AudioSpectrum.BandType.ThirtyOneBand:
                sampleDataLength = 31;
                break;
        }
        if(sampleDataLength!=null){
            ArtibuteMap = new Texture2D(timelength,sampleDataLength);
            
            AudioBufferData = new float[timelength * sampleDataLength];
            for(int i = 0; i < AudioBufferData.Length; i++){
                AudioBufferData[i] = 0;
            }
        }
        else{
            return;
        }

       
    } 
    void Update()
    {
        // image.texture = ArtibuteMap;
        MakeArtibuteMap();

    }
    void MakeArtibuteMap(){
        

        AudioSample = _AudioSpectrum.PeakLevels; //need check the length

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

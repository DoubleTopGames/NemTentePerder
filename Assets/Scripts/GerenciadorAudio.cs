using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorAudio : MonoBehaviour
{
    public static GerenciadorAudio instancia;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
    }

    [SerializeField] List<AudioClip> musicas = new List<AudioClip>();
    [SerializeField] AudioSource source;
    [SerializeField] Transform sons;

    List<AudioSource> objsSom = new List<AudioSource>();
    bool trocandoMusica;
    float volumeMax;
    Coroutine ultimaTroca = null;

    void Start()
    {
        volumeMax = source.volume;

        TrocarMusica(0);
    }

    public void TrocarMusica(int indexMusica)
    {
        if (trocandoMusica)
        {
            StopCoroutine(ultimaTroca);
            source.volume = volumeMax;
        }

        ultimaTroca = StartCoroutine(Musica(musicas[indexMusica]));
    }

    IEnumerator Musica(AudioClip musica)
    {
        trocandoMusica = true;

        while (source.volume > 0)
        {
            source.volume = Mathf.Clamp(source.volume - 0.05f, 0, 1);
            yield return new WaitForSeconds(0.075f);
        }

        source.volume = volumeMax * 0.5f;
        AlterarMusica(musica);

        while (source.volume < volumeMax)
        {
            source.volume += 0.01f;
            yield return new WaitForSeconds(0.1f);
        }

        source.volume = volumeMax;
        trocandoMusica = false;
    }

    void AlterarMusica(AudioClip musica)
    {
        source.Stop();
        source.clip = musica;
        source.Play();
    }

    public void ReproduzirEfeito(AudioClip clip, float volume, bool pitchAleatorio)
    {
        AudioSource _source = ObjSom();
        _source.clip = clip;
        _source.volume = volume;
        if (pitchAleatorio)
            _source.pitch = Random.Range(0.9f, 1.1f);
        _source.Play();
    }

    public void PausarMusica(float retroceder)
    {
        source.time = Mathf.Clamp(source.time - retroceder, 0f, source.clip.length);
        if(source.isPlaying)
            source.Pause();
        else
            source.UnPause();
    }

    AudioSource ObjSom()
    {
        if (objsSom.Count > 0)
        {
            for (int i = 0; i < objsSom.Count; i++)
            {
                if (!objsSom[i].isPlaying)
                    return objsSom[i];
            }
        }

        GameObject som = new GameObject("Som");
        som.transform.position = sons.position;
        som.transform.parent = sons;
        AudioSource _source = som.AddComponent<AudioSource>();
        objsSom.Add(_source);

        return _source;
    }
}

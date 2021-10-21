using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moeda : MonoBehaviour
{
    [SerializeField] AudioClip somColetar;

    Objetivo objetivo;

    public void DefinirObjetivo(Objetivo obj)
    {
        objetivo = obj;
    }

    void Coletar()
    {
        objetivo.ColetarMoeda();
        gameObject.SetActive(false);
        GerenciadorAudio.instancia.ReproduzirEfeito(somColetar, 0.25f, true);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
            Coletar();
    }
}

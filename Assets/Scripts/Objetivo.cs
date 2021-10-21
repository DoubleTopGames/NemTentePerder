using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Objetivo : MonoBehaviour
{
    public Action coletarTodas;

    [SerializeField] List<Moeda> moedas;

    int moedasColetadas;

    void Start()
    {
        foreach (Moeda m in moedas)
        {
            m.DefinirObjetivo(this);
        }

        GameManager.instancia.morrer += Resetar;
    }

    public void ColetarMoeda()
    {
        moedasColetadas++;
        
        if(moedasColetadas == moedas.Count && coletarTodas != null)
            coletarTodas();
    }

    public void Resetar()
    {
        foreach (Moeda m in moedas)
        {
            m.gameObject.SetActive(true);
        }

        moedasColetadas = 0;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player") && moedasColetadas == moedas.Count)
            GameManager.instancia.SelecionarCena(GameManager.instancia.CenaAtual() + 1);
    }

    void OnDisable()
    {
        GameManager.instancia.morrer -= Resetar;
    }

    void OnDestroy()
    {
        GameManager.instancia.morrer -= Resetar;
    }
}

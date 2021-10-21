using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GerenciadorDialogos : MonoBehaviour
{
    public static GerenciadorDialogos instancia;

    private void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
    }

    [SerializeField] TMP_Text textoDialogo;
    [SerializeField] GameObject objDialogo;
    [SerializeField] float velocidadeDigitacao;
    [SerializeField] Dialogo teste;
    [SerializeField] List<Dialogo> dialogosMortes;

    [SerializeField] List<AudioClip> falaNormal;
    [SerializeField] List<AudioClip> falaIrritada;
    [SerializeField] List<AudioClip> falaDemoniaca;
    [SerializeField] float intervaloFala;

    Coroutine dialogoAtual;
    int indexMorte = 0;
    float tempoFala = 0;

    void Start()
    {
        GameManager.instancia.morrer += ProximaMorte;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            IniciarDialogo(teste);

        if (tempoFala > 0)
            tempoFala -= Time.deltaTime;
    }

    public void IniciarDialogo(Dialogo dialogo)
    {
        if (dialogoAtual != null)
        {
            StopCoroutine(dialogoAtual);
            dialogoAtual = null;
        }

        dialogoAtual = StartCoroutine(ExibirDialogo(dialogo));
    }

    public void ProximaMorte()
    {
        if (indexMorte < dialogosMortes.Count)
            IniciarDialogo(dialogosMortes[indexMorte++]);
    }

    public void RedefinirMortes()
    {
        indexMorte = 0;
    }

    IEnumerator ExibirDialogo(Dialogo dialogo)
    {
        objDialogo.SetActive(true);

        for (int i = 0; i < dialogo.linhas.Count; i++)
        {
            textoDialogo.text = "";

            LinhaTexto linha = dialogo.linhas[i];

            for (int i2 = 0; i2 < linha.frases.Count; i2++)
            {
                Frase frase = linha.frases[i2];
                foreach (char c in frase.texto.ToCharArray())
                {
                    textoDialogo.text += c;
                    ReproduzirSomFala(frase.tom);
                    yield return new WaitForSeconds(1 / (frase.velocidade * velocidadeDigitacao));
                }

                yield return new WaitForSeconds(frase.pausa);
            }
        }

        objDialogo.SetActive(false);
    }

    void ReproduzirSomFala(TomFala tom)
    {
        if (tempoFala <= 0)
        {
            AudioClip somFala = null;
            switch (tom)
            {
                case TomFala.NORMAL:
                    somFala = falaNormal[Random.Range(0, falaNormal.Count)];
                    break;
                case TomFala.IRRITADA:
                    somFala = falaIrritada[Random.Range(0, falaIrritada.Count)];
                    break;
                case TomFala.DEMONIACA:
                    somFala = falaDemoniaca[Random.Range(0, falaDemoniaca.Count)];
                    break;
            }

            GerenciadorAudio.instancia.ReproduzirEfeito(somFala, 0.1f, false);
            tempoFala = intervaloFala;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] List<FaseBoss> fases;

    [SerializeField] Transform player;
    [SerializeField] Transform olhos;
    [SerializeField] GameObject olhosDano;
    [SerializeField] float distMaxOlhos;
    [SerializeField] float velocidadeSeguir;
    [SerializeField] float duracaoDano;
    [SerializeField] AudioClip somDano;
    [SerializeField] Dialogo dialogoFinal;
    [SerializeField] MaoBoss maoDireita;
    [SerializeField] MaoBoss maoEsquerda;
    [SerializeField] GameObject fase;

    int indexFase = 0;
    bool dano;

    void Start()
    {
        AvancarFase();
        GerenciadorAudio.instancia.TrocarMusica(2);
    }

    void Update()
    {
        if (!dano)
        {
            Vector2 posOlhos = (player.position - transform.position).normalized * distMaxOlhos;

            olhos.localPosition = Vector2.Lerp(olhos.localPosition, posOlhos, velocidadeSeguir * Time.deltaTime);
        }
    }

    public void AvancarFase()
    {
        if (indexFase < fases.Count)
        {
            if (indexFase != 0)
            {
                fases[indexFase - 1].objetivo.coletarTodas -= AvancarFase;
                fases[indexFase - 1].objetivo.gameObject.SetActive(false);

                StartCoroutine(PerderVida());
            }

            fases[indexFase].objetivo.coletarTodas += AvancarFase;
            fases[indexFase].objFase.SetActive(true);

            GerenciadorDialogos.instancia.IniciarDialogo(fases[indexFase].dialogo);

            indexFase++;
        }
        else
        {
            StartCoroutine(Morrer());
            fases[indexFase - 1].objetivo.gameObject.SetActive(false);
        }
    }

    IEnumerator PerderVida()
    {
        dano = true;

        CameraShake.instancia.TremerCamera(duracaoDano, 0.25f);
        GerenciadorAudio.instancia.ReproduzirEfeito(somDano, 0.25f, false);

        olhos.localPosition = Vector2.zero;
        TrocarOlhos();

        yield return new WaitForSeconds(duracaoDano);

        TrocarOlhos();

        dano = false;
    }

    void TrocarOlhos()
    {
        bool ativo = olhosDano.activeSelf;

        olhos.gameObject.SetActive(ativo);
        olhosDano.SetActive(!ativo);
    }

    IEnumerator Morrer()
    {
        CameraShake.instancia.TremerCamera(11.5f, 0.1f);
        TrocarOlhos();

        maoDireita.Desativar();
        maoEsquerda.Desativar();
        fase.SetActive(false);

        GerenciadorAudio.instancia.PausarMusica(0f);

        GerenciadorDialogos.instancia.IniciarDialogo(dialogoFinal);

        yield return new WaitForSeconds(11.5f);

        GameManager.instancia.SelecionarCena(GameManager.instancia.CenaAtual() + 1);
    }
}

[System.Serializable]
public class FaseBoss
{
    public GameObject objFase;
    public Objetivo objetivo;
    public Dialogo dialogo;
}
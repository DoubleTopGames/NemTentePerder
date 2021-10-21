using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    private void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
    }

    public Action morrer;

    [SerializeField] TMP_Text textoFases;
    [SerializeField] TMP_Text textoMortes;
    [SerializeField] Animator transicao;
    [SerializeField] GameObject uiJogo;

    [SerializeField] int cenaFinalPadrao;
    [SerializeField] int cenaFinalBom;
    [SerializeField] int cenaDecisao;
    [SerializeField] int mortesNecessarias;

    bool carregando;
    int mortes;

    void Start()
    {
        transicao.SetTrigger("Sair");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            CameraShake.instancia.TremerCamera(1f, 1f);
    }

    public void Morrer()
    {
        if (!carregando)
        {
            AtualizarMortes(++mortes);

            if (morrer != null)
                morrer();
        }
    }

    void AtualizarMortes(int qtd)
    {
        mortes = qtd;
        textoMortes.text = "Mortes: " + mortes;
    }

    public int CenaAtual()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void SelecionarCena(int indexCena)
    {
        if (indexCena > SceneManager.sceneCountInBuildSettings - 1 || indexCena < 0)
            indexCena = 0;

        if (indexCena == cenaDecisao)
        {
            if (mortes < mortesNecessarias)
                indexCena = cenaFinalPadrao;
            else
                GerenciadorAudio.instancia.TrocarMusica(1);
        }


        if (!carregando)
            StartCoroutine(CarregarCena(indexCena));

        if (indexCena == 0)
        {
            AtualizarMortes(0);
            GerenciadorAudio.instancia.TrocarMusica(0);
            GerenciadorDialogos.instancia.RedefinirMortes();
        }
    }

    IEnumerator CarregarCena(int cena)
    {
        carregando = true;
        transicao.SetTrigger("Entrar");

        yield return new WaitForSeconds(0.35f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(cena);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
                yield return new WaitForSeconds(0.25f);
                transicao.SetTrigger("Sair");
                carregando = false;
            }

            yield return null;
        }

        uiJogo.SetActive(cena != 0 && cena != cenaFinalPadrao && cena != cenaFinalBom);
        textoFases.text = CenaAtual() + "/10";
    }
}

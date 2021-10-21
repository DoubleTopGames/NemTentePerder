using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaoBoss : MonoBehaviour
{
    [SerializeField] float xMin;
    [SerializeField] float xMax;
    [SerializeField] Transform player;
    [SerializeField] float velocidade;
    [SerializeField] Transform spriteMao;
    [SerializeField] float alturaMao;
    [SerializeField] float intervaloAtaque;
    [SerializeField] Collider2D colisao;

    [SerializeField] Transform sombra;
    [SerializeField] float tamMinSombra;
    [SerializeField] float tamMaxSombra;
    [SerializeField] AudioClip somImpacto;

    float tempoAtacar;
    bool atacando;
    Vector2 posAlvo;
    bool ativa = true;

    void Start()
    {
        tempoAtacar = intervaloAtaque;
        DefinirAlturaSpriteMao(alturaMao);
        posAlvo = transform.position;
    }

    void Update()
    {
        if (ativa)
        {
            if (!atacando)
            {
                if (player.position.x > xMin && player.position.x < xMax)
                {
                    posAlvo = player.position;

                    if (tempoAtacar > 0)
                        tempoAtacar -= Time.deltaTime;
                    else
                    {
                        tempoAtacar = intervaloAtaque;
                        StartCoroutine(Atacar());
                    }
                }
                else
                    tempoAtacar = intervaloAtaque;

                transform.position = Vector2.Lerp(transform.position, posAlvo, velocidade * Time.deltaTime);
            }

            float tamanho = Mathf.Clamp(alturaMao - spriteMao.localPosition.y, tamMinSombra, tamMaxSombra);
            Vector2 tamSombra = new Vector2(tamanho, tamanho);
            sombra.localScale = Vector2.Lerp(sombra.localScale, tamSombra, Time.deltaTime * 2.5f);
        }
    }

    void DefinirAlturaSpriteMao(float y)
    {
        spriteMao.localPosition = new Vector2(spriteMao.localPosition.x, y);
    }

    public void Desativar()
    {
        ativa = false;
        StopAllCoroutines();
    }

    IEnumerator Atacar()
    {
        atacando = true;

        yield return AlturaMao(alturaMao + 0.5f, 0.5f);

        yield return new WaitForSeconds(0.45f);

        yield return AlturaMao(0, 2f);

        colisao.enabled = true;
        CameraShake.instancia.TremerCamera(0.15f, 0.25f);
        GerenciadorAudio.instancia.ReproduzirEfeito(somImpacto, 0.5f, false);

        yield return new WaitForSeconds(0.5f);

        colisao.enabled = false;

        yield return AlturaMao(alturaMao, 0.75f);

        atacando = false;
    }

    IEnumerator AlturaMao(float altura, float vel)
    {
        if (altura > spriteMao.localPosition.y)
        {
            while (spriteMao.localPosition.y < altura)
            {
                DefinirAlturaSpriteMao(spriteMao.localPosition.y + (0.1f * vel));
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            while (spriteMao.localPosition.y > altura)
            {
                DefinirAlturaSpriteMao(spriteMao.localPosition.y - (0.1f * vel));
                yield return new WaitForSeconds(0.01f);
            }
        }

        DefinirAlturaSpriteMao(altura);
    }
}

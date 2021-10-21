using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolinha : MonoBehaviour
{
    [SerializeField] float distanciaTrocar;
    [SerializeField] List<Vector2> pontos;
    [SerializeField] bool mover;
    [SerializeField] GameObject spriteBolinha;
    [SerializeField] GameObject spriteTriangulo;
    [SerializeField] bool desativar;

    Movimento movimento;
    int pontoAtual = 0;

    void Start()
    {
        movimento = GetComponent<Movimento>();
        movimento.PermitirMovimento(mover);

        AtualizarDirecao();
    }


    void Update()
    {
        if (mover)
        {
            if (Vector2.Distance(movimento.Posicao(), pontos[pontoAtual]) <= distanciaTrocar)
                ProximoPonto();
        }
    }

    void ProximoPonto()
    {
        pontoAtual++;
        if (pontoAtual > pontos.Count - 1)
            pontoAtual = 0;

        AtualizarDirecao();
    }

    void AtualizarDirecao()
    {
        movimento.DefinirDirecao((pontos[pontoAtual] - movimento.Posicao()).normalized);
    }

    public void ColidirPlayer()
    {
        if (desativar)
        {
            spriteTriangulo.SetActive(false);
            spriteBolinha.SetActive(true);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}

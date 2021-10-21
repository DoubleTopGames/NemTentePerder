using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimento : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float velocidade;

    bool mover;
    Vector2 direcaoMovimento;
    

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(mover)
            rb.velocity = direcaoMovimento * velocidade * Time.fixedDeltaTime;
    }

    public void PermitirMovimento(bool mov)
    {
        mover = mov;

        if(!mover)
            rb.velocity = Vector2.zero;
    }

    public void DefinirDirecao(Vector2 dir)
    {
        direcaoMovimento = dir;
    }

    public Vector2 Posicao()
    {
        return rb.position;
    }

    public void DefinirPosicao(Vector2 posicao)
    {
        rb.position = posicao;
    }
}

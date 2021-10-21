using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] AudioClip somMorte;

    Movimento movimento;
    Vector2 posicaoVoltar;
    bool morrendo;

    void Start()
    {
        movimento = GetComponent<Movimento>();
        movimento.PermitirMovimento(true);

        posicaoVoltar = movimento.Posicao();
    }

    void Update()
    {
        Vector2 direcao = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        movimento.DefinirDirecao(direcao.normalized);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!morrendo)
        {
            if (col.CompareTag("Obstaculo"))
            {
                StartCoroutine(Morrer());
                col.GetComponent<Bolinha>().ColidirPlayer();
            }

            if (col.CompareTag("Boss"))
                StartCoroutine(Morrer());
        }


        if (col.CompareTag("Checkpoint"))
            posicaoVoltar = col.transform.position;
    }

    IEnumerator Morrer()
    {
        morrendo = true;

        anim.SetTrigger("Morrer");
        movimento.PermitirMovimento(false);
        GerenciadorAudio.instancia.ReproduzirEfeito(somMorte, 0.5f, false);

        CameraShake.instancia.TremerCamera(0.5f, 0.25f);

        yield return new WaitForSeconds(0.3f);

        movimento.DefinirPosicao(posicaoVoltar);
        GameManager.instancia.Morrer();
        anim.SetTrigger("Voltar");

        yield return new WaitForSeconds(0.3f);

        movimento.PermitirMovimento(true);

        morrendo = false;
    }
}

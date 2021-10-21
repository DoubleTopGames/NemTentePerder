using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivarDialogo : MonoBehaviour
{
    [SerializeField] Dialogo dialogo;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            GerenciadorDialogos.instancia.IniciarDialogo(dialogo);
            gameObject.SetActive(false);
        }
    }
}

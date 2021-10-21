using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Jogar()
    {
        GameManager.instancia.SelecionarCena(1);
    }
}

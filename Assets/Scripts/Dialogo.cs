using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Novo Diálogo", menuName = "Diálogos/Novo Diálogo", order = 1)]
public class Dialogo : ScriptableObject
{
    public List<LinhaTexto> linhas;
}

[System.Serializable]
public class LinhaTexto
{
    public List<Frase> frases;
}

[System.Serializable]
public class Frase
{
    [Multiline]
    public string texto;
    public float pausa;
    public float velocidade = 1f;
    public TomFala tom;
}

[System.Serializable]
public enum TomFala
{
    NORMAL,
    IRRITADA,
    DEMONIACA
}
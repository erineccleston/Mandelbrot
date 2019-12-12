using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlgorithmSelection : MonoBehaviour
{
    Dropdown drop;

    private void Start()
    {
        drop = GetComponentInChildren<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(GetCurrentFractal());
        }
    }

    public fractal GetCurrentFractal()
    {
        string currentSelection = drop.options[drop.value].text;
        if (currentSelection == "Mandelbrot")
            return fractal.mandlebrot;
        if (currentSelection == "Julia Set")
            return fractal.juliaSet;
        else
            return fractal.burningShip;
    }
}

public enum fractal
{
    mandlebrot,
    juliaSet,
    burningShip
}

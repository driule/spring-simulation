using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Obi;

public class FluidController : MonoBehaviour {

    public ObiEmitterMaterialFluid fluidMaterial;
    public ObiEmitter fluidEmmiter;
    public ObiSolver solver;

    public Slider fluidViscositySlider;

    // Use this for initialization
    void Start () {
        this.setFluidValues();
	}
	
	// Update is called once per frame
	void Update () {
        this.setFluidValues();

        if (Input.GetKey(KeyCode.R))
        {
            this.fluidEmmiter.KillAll();
        }
    }

    void setFluidValues()
    {
        if (this.fluidMaterial.viscosity != this.fluidViscositySlider.value)
        {
            this.fluidMaterial.viscosity = this.fluidViscositySlider.value;
            this.solver.UpdateEmitterMaterials();
        }
    }
}

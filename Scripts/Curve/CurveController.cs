using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CurveController : MonoBehaviour {

    public Vector3 Curvature = new Vector3(0, 0, 0);
    //public Vector3 CurvatureHorizontal = new Vector3(0, 0, 0);
    //public float Distance = 0;

    [Space]
    public float CurvatureScaleUnit = 1000f;
    
    int CurvatureID;
    //int CurvatureHorizontalID;
    //int DistanceID;

    

    private void OnEnable()
    {
        CurvatureID = Shader.PropertyToID("_Curvature");
        //CurvatureVerticalID = Shader.PropertyToID("_CurvatureVertical");
        //CurvatureHorizontalID = Shader.PropertyToID("_CurvatureHorizontal");
        
        
        //DistanceID = Shader.PropertyToID("_Distance");
    }

    void Update()
    {
        
        Vector3 curvature = CurvatureScaleUnit == 0 ? Curvature : Curvature / CurvatureScaleUnit;
        //Vector3 curvatureVertical = CurvatureScaleUnit == 0 ? CurvatureVertical : CurvatureVertical / CurvatureScaleUnit;
        //Vector3 curvatureHorizontal = CurvatureScaleUnit == 0 ? CurvatureVertical : CurvatureVertical / CurvatureScaleUnit;
        
        Shader.SetGlobalVector(CurvatureID, curvature);
        
        
        
        //Shader.SetGlobalVector(CurvatureVerticalID, curvatureVertical);
        //Shader.SetGlobalVector(CurvatureHorizontalID, curvatureHorizontal);
        
        
        //Shader.SetGlobalFloat(DistanceID, Distance);
    }
}
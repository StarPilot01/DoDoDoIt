using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    [ExecuteInEditMode]

    public class MaterialApplier : MonoBehaviour
    {
        public Component[] renderers;

        public Material material;

        // Start is called before the first frame update
        void Start()
        {
            renderers = GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer mesh in renderers)
            {
                mesh.material = material;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
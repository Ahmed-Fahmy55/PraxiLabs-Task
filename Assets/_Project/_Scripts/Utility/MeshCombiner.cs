using UnityEngine;

namespace Praxi.Utility
{
    class MeshCombiner : MonoBehaviour
    {
        void Start()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            // Cache parent transform
            Transform parent = transform;

            for (int i = 0; i < meshFilters.Length; i++)
            {
                var mf = meshFilters[i];

                combine[i].mesh = mf.sharedMesh;

                // Convert child local transform to parent local space
                combine[i].transform =
                    parent.worldToLocalMatrix * mf.transform.localToWorldMatrix;

                mf.gameObject.SetActive(false);
            }

            MeshFilter mfCombined = gameObject.AddComponent<MeshFilter>();
            mfCombined.mesh = new Mesh();
            mfCombined.mesh.CombineMeshes(combine, true, true);

            MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
            mr.sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;

            gameObject.SetActive(true);
        }
    }
}

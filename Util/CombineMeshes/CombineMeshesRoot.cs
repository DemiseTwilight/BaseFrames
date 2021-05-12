using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 动态合并网格
/// </summary>
public class CombineMeshesRoot : MonoBehaviour {
    MeshFilter rootMeshFilter;

    // Start is called before the first frame update
    void Start() {
    }
    private void Update() {
        //if(Input.GetKey(KeyCode.P)) {
        //    Combining();
        //}
    }
    /// <summary>
    /// 合并贴图
    /// </summary>
    public void Combining() {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        List<Material> materials = new List<Material>();
        List<Texture2D> textures = new List<Texture2D>();

        foreach(var renderer in meshRenderers) {
            //合并贴图材质：
            materials.Add(renderer.sharedMaterial);
            Texture2D t2d = renderer.sharedMaterial.GetTexture("_MainTex") as Texture2D;
            Texture2D nt2d = new Texture2D(t2d.width, t2d.height, TextureFormat.ARGB32, false);
            nt2d.SetPixels(t2d.GetPixels(0, 0, t2d.width, t2d.height));
            nt2d.Apply();
            textures.Add(nt2d);
        }

        MeshRenderer rootMeshRenderer = GetComponent<MeshRenderer>();
        if(rootMeshRenderer) {

        } else {
            rootMeshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        }
        //创建合并后的材质：
        Material nMaterial = new Material(materials[0].shader);
        nMaterial.CopyPropertiesFromMaterial(materials[0]);
        rootMeshRenderer.material = nMaterial;
        //创建合并后的贴图：
        Texture2D ntex = new Texture2D(1024, 1024);
        nMaterial.SetTexture("_MainTex", ntex);

        //合并网格:
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        List<CombineInstance> combines = new List<CombineInstance>();
        //贴图打包 ，矩形的数组包含每个输入的纹理的UV坐标
        Rect[] rects = ntex.PackTextures(textures.ToArray(), 10, 1024);
        for(int i = 0; i < meshFilters.Length; i++) {
            Rect rect = rects[i];
            Mesh mesh = meshFilters[i].mesh;
            Vector2[] newUVs = new Vector2[mesh.uv.Length];
            for(int j = 0; j < mesh.uv.Length; j++) {
                //uv是一个比值,u = 横向第u个像素/原始贴图的宽度    v = 竖向第v个像素/原始贴图的高度
                //rect.x : 原贴图在合并后的贴图的 x 坐标，  rect.y : 原贴图在合并后的贴图的 y 坐标
                newUVs[j].x = mesh.uv[j].x * rect.width + rect.x;
                newUVs[j].y = mesh.uv[j].y * rect.height + rect.y;
            }
            mesh.uv = newUVs;

            //合并各个子网格：
            CombineInstance combineInfo = new CombineInstance();
            combineInfo.mesh = mesh;
            Matrix4x4 combinMatrix = meshFilters[i].transform.localToWorldMatrix;
            combinMatrix.m03 -= this.transform.position.x;
            combinMatrix.m13 -= this.transform.position.y;
            combinMatrix.m23 -= this.transform.position.z;
            combineInfo.transform = combinMatrix;
            combines.Add(combineInfo);
            Destroy(meshFilters[i]);
            Destroy(meshRenderers[i]);
        }

        this.rootMeshFilter = GetComponent<MeshFilter>();
        if(rootMeshFilter) {
        } else {
            rootMeshFilter = this.gameObject.AddComponent<MeshFilter>();
        }
        rootMeshFilter.mesh = new Mesh();
        rootMeshFilter.mesh.CombineMeshes(combines.ToArray(), true, true);
        this.gameObject.SetActive(true);

    }

    //private void OnDrawGizmos() {
    //    //if(this.rootMeshFilter) {
    //    //    Gizmos.DrawWireMesh(this.rootMeshFilter.mesh);
    //    //}

    //}

}

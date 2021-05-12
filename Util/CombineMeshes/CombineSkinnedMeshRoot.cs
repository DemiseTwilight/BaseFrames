using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 合并骨骼蒙皮,按材质分组
/// </summary>
public class CombineSkinnedMeshRoot : MonoBehaviour {
    SkinnedMeshRenderer[] orginalMeshRenderers;
    void Start() {
        //CombinGroups();
    }

    // Update is called once per frame
    void Update() {
        //if(Input.GetKeyDown(KeyCode.P)) {
        //    CombinGroups();
        //}
    }
    /// <summary>
    /// 分组合并
    /// </summary>
    public void CombinGroups() {
        this.orginalMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        var orGroups = this.orginalMeshRenderers.GroupBy(
            skinRender => skinRender.sharedMaterial.name, skinRender => skinRender).ToList();

        foreach(var materialMap in orGroups) {
            //Debug.Log(materialMap.Key);
            Transform rootTransform = new GameObject("Skin_" + materialMap.Key).transform;
            rootTransform.localPosition = Vector3.zero;
            rootTransform.parent = this.transform;
            Combining(rootTransform, materialMap.ToArray());
        }
    }
    public void Combining(Transform root, SkinnedMeshRenderer[] orginalSkins) {
        List<CombineInstance> combineInstances = new List<CombineInstance>();
        //List<Material> materials = new List<Material>();
        Material material = orginalSkins[0].sharedMaterial;
        List<Transform> boneList = new List<Transform>();
        Transform[] transforms = this.GetComponentsInChildren<Transform>();
        //贴图信息：
        List<Texture2D> textures = new List<Texture2D>();
        int width = 0;
        int height = 0;
        int uvCount = 0;

        List<Vector2[]> uvList = new List<Vector2[]>();
        //收集皮肤网格、贴图、骨骼信息：
        foreach(SkinnedMeshRenderer smr in orginalSkins) {
            ////比较材质信息：
            //if(material.name != smr.sharedMaterial.name) {
            //    continue;
            //}
            //皮肤网格合并信息：
            for(int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++) {
                CombineInstance ci = new CombineInstance();
                ci.mesh = smr.sharedMesh;
                ci.subMeshIndex = sub;
                combineInstances.Add(ci);
            }

            //记录贴图信息：
            if(smr.material.mainTexture != null) {
                Texture2D texture = smr.GetComponent<Renderer>().material.mainTexture as Texture2D;
                textures.Add(texture);
                width += texture.width;
                height += texture.height;
                uvList.Add(smr.sharedMesh.uv);
                uvCount += smr.sharedMesh.uv.Length;
            }
            //记录骨骼信息：
            Transform[] boneInfo = (from node in transforms
                                    from bone in smr.bones
                                    where node.name == bone.name
                                    select node).ToArray();

            boneList.AddRange(boneInfo);

            smr.gameObject.SetActive(false);
        }
        //生成新的网格：
        SkinnedMeshRenderer tempRenderer = root.gameObject.GetComponent<SkinnedMeshRenderer>();
        if(!tempRenderer) {
            tempRenderer = root.gameObject.AddComponent<SkinnedMeshRenderer>();
        }

        tempRenderer.sharedMesh = new Mesh();

        //合并网格，刷新骨骼，附加材质：
        tempRenderer.sharedMesh.CombineMeshes(combineInstances.ToArray(), true, false);
        tempRenderer.bones = boneList.ToArray();
        tempRenderer.material = material;
        //创建贴图：
        Texture2D skinnedMeshAtlas = new Texture2D(Get2Pow(width), Get2Pow(height));
        Rect[] packingResult = skinnedMeshAtlas.PackTextures(textures.ToArray(), 0);
        Vector2[] atlasUVs = new Vector2[uvCount];
        //计算uv：
        for(int i = 0; i < uvList.Count; i++) {
            Rect rect = packingResult[i];
            for(int j = 0; j < uvList[i].Length; j++) {
                atlasUVs[j].x = Mathf.Lerp(rect.xMin, rect.xMax, uvList[i][j].x);
                atlasUVs[j].y = Mathf.Lerp(rect.yMin, rect.yMax, uvList[i][j].y);
            }
        }
        //设置贴图和uv:
        tempRenderer.material.mainTexture = skinnedMeshAtlas;
        tempRenderer.sharedMesh.uv = atlasUVs;
    }

    /// <summary>
    /// 获取最接近输入值的2的N次方的数，最大不会超过1024，例如输入320会得到512
    /// </summary>
    public int Get2Pow(int into) {
        int outo = 1;
        for(int i = 0; i < 10; i++) {
            outo *= 2;
            if(outo > into) {
                break;
            }
        }

        return outo;
    }

    //private void OnDrawGizmos() {
    //    //if(this.rootMeshFilter) {
    //    //    Gizmos.DrawWireMesh(this.rootMeshFilter.mesh);
    //    //}

    //}
}

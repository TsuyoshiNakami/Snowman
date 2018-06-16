#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SpritePrimitive
{
    public class SpritePrimitiveEditor : MonoBehaviour
    {
        // アセットからゲームオブジェクトを生成
        private static void InstantiateGameObject(string path)
        {
			Camera sceneCamera = SceneView.GetAllSceneCameras()[0];                      		// Sceneビューのカメラを取得
            Vector3 centerPos = sceneCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));  // Sceneビューの中心位置をワールド座標で取得

            Object obj = Resources.Load("SpritePrimitives/" + path);                            // Resourcesフォルダからアセットを取得
            GameObject go = Instantiate(obj) as GameObject;                                     // GameObjectとして生成
            Selection.activeGameObject = go;                                                    // 生成したゲームオブジェクトを選択している状態にする
            go.name = obj.name;                                                                 // GameObject名を設定
            go.transform.position = (Vector2)centerPos;                                         // Sceneビューの中心に配置
            go.transform.rotation = Quaternion.identity;                                        // 初期姿勢の設定
        }

        // Createメニューの拡張

        [MenuItem("GameObject/2D Object/Primitives/Box")]
        private static void Box()
        {
            InstantiateGameObject("Box");
        }

        [MenuItem("GameObject/2D Object/Primitives/Circle")]
        private static void Circle()
        {
            InstantiateGameObject("Circle");
        }

        [MenuItem("GameObject/2D Object/Primitives/Triangle")]
        private static void Triangle()
        {
            InstantiateGameObject("Triangle");
        }

        [MenuItem("GameObject/2D Object/Primitives/Pentagon")]
        private static void Pentagon()
        {
            InstantiateGameObject("Pentagon");
        }

        [MenuItem("GameObject/2D Object/Primitives/Hexagon")]
        private static void Hexagon()
        {
            InstantiateGameObject("Hexagon");
        }

        [MenuItem("GameObject/2D Object/Primitives/Star")]
        private static void Star()
        {
            InstantiateGameObject("Star");
        }

    }
}
#endif
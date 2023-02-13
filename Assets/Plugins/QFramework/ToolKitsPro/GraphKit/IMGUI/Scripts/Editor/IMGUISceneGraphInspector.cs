/****************************************************************************
 * Copyright (c) 2017 Thor Brigsted UNDER MIT LICENSE  see licenses.txt 
 * Copyright (c) 2022 liangxiegame UNDER Paid MIT LICENSE  see licenses.txt
 *
 * xNode: https://github.com/Siccity/xNode
 ****************************************************************************/

using System;
using UnityEditor;
using UnityEngine;

namespace QFramework.Pro
{
    [CustomEditor(typeof(IMGUISceneGraph), true)]
    public class IMGUISceneGraphInspector : Editor
    {
        private IMGUISceneGraph sceneGraph;
        private bool removeSafely;
        private Type graphType;

        /// <summary>
        /// 在场景的 Inspector 上渲染
        /// Render in scene obj's inspecotor
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (sceneGraph.graph == null)
            {
                if (GUILayout.Button("New graph", GUILayout.Height(40)))
                {
                    if (graphType == null)
                    {
                        Type[] graphTypes = IMGUIGraphReflection.GetDerivedTypes(typeof(IMGUIGraph));
                        GenericMenu menu = new GenericMenu();
                        for (int i = 0; i < graphTypes.Length; i++)
                        {
                            Type graphType = graphTypes[i];
                            menu.AddItem(new GUIContent(graphType.Name), false, () => CreateGraph(graphType));
                        }

                        menu.ShowAsContext();
                    }
                    else
                    {
                        CreateGraph(graphType);
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Open graph", GUILayout.Height(40)))
                {
                    IMGUIGraphWindow.OpenWithGraph(sceneGraph.graph);
                }

                if (removeSafely)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Really remove graph?");
                    GUI.color = new Color(1, 0.8f, 0.8f);
                    if (GUILayout.Button("Remove"))
                    {
                        removeSafely = false;
                        Undo.RecordObject(sceneGraph, "Removed graph");
                        sceneGraph.graph = null;
                    }

                    GUI.color = Color.white;
                    if (GUILayout.Button("Cancel"))
                    {
                        removeSafely = false;
                    }

                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUI.color = new Color(1, 0.8f, 0.8f);
                    if (GUILayout.Button("Remove graph"))
                    {
                        removeSafely = true;
                    }

                    GUI.color = Color.white;
                }
            }
        }

        private void OnEnable()
        {
            sceneGraph = target as IMGUISceneGraph;
            Type sceneGraphType = sceneGraph.GetType();
            if (sceneGraphType == typeof(IMGUISceneGraph))
            {
                graphType = null;
            }
            else
            {
                Type baseType = sceneGraphType.BaseType;
                if (baseType.IsGenericType)
                {
                    graphType = sceneGraphType = baseType.GetGenericArguments()[0];
                }
            }
        }

        public void CreateGraph(Type type)
        {
            Undo.RecordObject(sceneGraph, "Create graph");
            sceneGraph.graph = ScriptableObject.CreateInstance(type) as IMGUIGraph;
            sceneGraph.graph.name = sceneGraph.name + "-graph";
        }
    }
}
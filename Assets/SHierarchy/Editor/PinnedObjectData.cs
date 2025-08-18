using System.Collections.Generic;
using UnityEngine;

namespace Shadowprofile.SHierarchy
{
    [CreateAssetMenu(fileName = "PinnedObjectsData", menuName = "PinnedObjects/PinnedObjectsData")]
    public class PinnedObjectsData : ScriptableObject
    {
        [System.Serializable]
        public class ScenePinnedObjects
        {
            public string sceneName;
            public List<string> objectIds = new List<string>();
        }

        public List<ScenePinnedObjects> scenesData = new List<ScenePinnedObjects>();

        public List<string> GetPinnedObjectsForScene(string sceneName)
        {
            var sceneData = scenesData.Find(s => s.sceneName == sceneName);
            return sceneData != null ? sceneData.objectIds : new List<string>();
        }

        public void SetPinnedObjectsForScene(string sceneName, List<string> objectIds)
        {
            var sceneData = scenesData.Find(s => s.sceneName == sceneName);
            if (sceneData == null)
            {
                sceneData = new ScenePinnedObjects { sceneName = sceneName };
                scenesData.Add(sceneData);
            }
            sceneData.objectIds = objectIds;
        }
    }
}
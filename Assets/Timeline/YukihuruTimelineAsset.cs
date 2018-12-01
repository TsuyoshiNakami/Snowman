using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class YukihuruTimelineAsset : PlayableAsset
{
    public ExposedReference<OpeningManager> openingManager;
	// Factory method that generates a playable based on this asset
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
        YukihuruTimelineBehavior ytb = new YukihuruTimelineBehavior();
        return ScriptPlayable<YukihuruTimelineBehavior>.Create(graph, ytb);
	}
}

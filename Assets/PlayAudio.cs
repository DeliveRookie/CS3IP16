using UnityEngine;
using System.Collections;
/// <summary>
/// 文字转音效调用类
/// </summary>
public class PlayAudio : MonoBehaviour {
	string word=""; 
	VirtualSpeeker speeker;

	void Start () {
		speeker = new VirtualSpeeker();
	}

	void OnGUI(){
		word = GUI.TextArea(new Rect(100,100,200,200),word,200);
		if(GUI.Button(new Rect(100,20,200,100),"play")){
            speeker.SpecialSpeek(word,1,1);
        }

	}
}

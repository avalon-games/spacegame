using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTasks : MonoBehaviour
{
	[SerializeField] Tasks task;
	[SerializeField] int level;
	[SerializeField] int saveFile;

	UnityEngine.UI.Button button;
	public enum Tasks {
		LoadShip,
		LoadLevel,
		SaveOrLoad,
		NewGame
	}
	

	private void Awake() {
		button = GetComponent<UnityEngine.UI.Button>()	;
	}

	private void Start() {
		button.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick() {

		if (task == Tasks.LoadShip) {
			FindObjectOfType<SaveAndLoad>().LoadLevel(1);
		} else if (task == Tasks.LoadLevel) {
			FindObjectOfType<SaveAndLoad>().LoadLevel(level);
		} else if (task == Tasks.SaveOrLoad) {
			UIMenus uiMenus = FindObjectOfType<UIMenus>();
			if (uiMenus.isSaving)
				FindObjectOfType<SaveAndLoad>().SaveGame(saveFile);
			else
				FindObjectOfType<SaveAndLoad>().LoadGame(saveFile);
		} else if (task == Tasks.NewGame) {
			FindObjectOfType<SaveAndLoad>().NewGame();
		}
	}

}

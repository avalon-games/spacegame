using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Controller for the ingame debugging console
 * Displays an ingame debugging console that allows quick traveling across scenes and checkpoints
 * 
 * Handles the trasfer of data from the text input and defines what the commands do
 * Tutorial link: https://youtu.be/VzOEM-4A2OM
 */
public class DebugController : MonoBehaviour
{
	GUIStyle textStyle;
	int fontSize = Screen.height / 25;
	int consoleHeight = Screen.height/5;

	bool showConsole;
	bool showHelp;
	bool showLevelList;
	string input;
	public List<object> commandList;
	List<string> levelList;

	//list of available commands, listed in alphabetical order
	public static DebugCommand HELP;
	public static DebugCommand INFINITE_GRAPPLE;
	public static DebugCommand LIST_LEVELS;
	public static DebugCommand<int> LOAD_CHECKPOINT;
	public static DebugCommand<int> LOAD_LEVEL;
	public static DebugCommand<int> SET_HP;
	public static DebugCommand TANK;


	//objects to call functions from
	public UIMenus menu;
	public CheckpointManager checkpoints;
	public GrapplingGun grapple;
	SceneChanger sceneChanger;

	public void OnToggleDebug() {
		showConsole = !showConsole;
		input = "";
	}

	public void OnReturn() {
		if (showConsole) {
			HandleInput();
			input = "";
		}
	}

	private void Start() {
		sceneChanger = FindObjectOfType<SceneChanger>();
		textStyle = new GUIStyle();
		textStyle.normal.textColor = Color.white;

		CheckMissingObjects();
		HELP = new DebugCommand("help", "shows the list of commands", "help", () => {
			showHelp = true;
			showLevelList = false;
		});
		INFINITE_GRAPPLE = new DebugCommand("inf", "toggle player infinite grapple charge.", "inf", () => {
			if (grapple != null)
				grapple.ToggleInfiniteCharge();
		});
		#if UNITY_EDITOR
		LIST_LEVELS = new DebugCommand("listl", "list levels: gets the list of levels in build settings (editor only)", "listl", () => {
			levelList = sceneChanger.GetBuildScenes();
			showLevelList = true;
			showHelp = false;
		});
		#endif
		LOAD_CHECKPOINT = new DebugCommand<int>("loadc", "teleports the player to the specified checkpoint. ", "loadc <target>", (x) => {
			if (checkpoints != null && checkpoints.CheckpointExists(x))
				checkpoints.Teleport(x);
		});
		LOAD_LEVEL = new DebugCommand<int>("loadl", "load level: loads the specified scene. ", "loadl <target_scene>", (x) => {
			sceneChanger.GoToLevel(x);
		});
		SET_HP = new DebugCommand<int>("sh", "set hp: sets hp to the specified amount and heal to full oxygen, max is 8. ", "sh <target_hp>", (x) => {
			if (menu != null)
				menu.SetHp(x);
		});
		TANK = new DebugCommand("tank", "sets the player hp super high (1000). ", "tank", () => {
			if (menu != null)
				menu.SetHp(1000);
		});

		commandList = new List<object> {
			HELP,
			INFINITE_GRAPPLE,
			#if UNITY_EDITOR
				LIST_LEVELS,
			#endif
			LOAD_CHECKPOINT,
			LOAD_LEVEL,
			SET_HP,
			TANK
		};


	}

	/**
	 * Logs missing objects that prevent the console from working properly
	 */
	private void CheckMissingObjects() {
		if (menu == null) {
			Debug.LogError("ERROR *** menu object not set");
		}
		if (checkpoints == null) {
			Debug.LogError("ERROR *** checkpoints object not set");
		}
		if (grapple == null) {
			Debug.LogError("ERROR *** grappling gun object not set");
		}
	}

	Vector2 scroll;

	private void OnGUI() {
		textStyle.fontSize = this.fontSize;
		if (!showConsole) { showLevelList = false; showHelp = false; return; }
		float y = 0f;

		if (showHelp) {
			GUI.Box(new Rect(0, y, Screen.width, consoleHeight), "");

			Rect viewport = new Rect(0, 0, Screen.width - 30, (fontSize * 1.5f) * commandList.Count);

			scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, (consoleHeight - 10)), scroll, viewport);

			for (int i = 0; i < commandList.Count; i++) {
				DebugCommandBase command = commandList[i] as DebugCommandBase;

				string label = $"{command.commandFormat} - {command.commandDescription}";

				Rect labelRect = new Rect(10, (fontSize * 1.5f) * i, viewport.width - 90, (fontSize * 1.5f));

				GUI.Label(labelRect, label, textStyle);
			}
		
			GUI.EndScrollView();
			y += consoleHeight;
		}
		else if (showLevelList) {
			GUI.Box(new Rect(0, y, Screen.width, consoleHeight), "");

			Rect viewport = new Rect(0, 0, Screen.width - 30, (fontSize * 1.5f) * levelList.Count);

			scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, (consoleHeight - 10)), scroll, viewport);

			for (int i = 0; i < levelList.Count; i++) {
				string label = $"{i} - {levelList[i]}";

				Rect labelRect = new Rect(5, (fontSize * 1.5f) * i, viewport.width - 90, (fontSize * 1.5f));
				GUI.Label(labelRect, label,textStyle);
			}

			GUI.EndScrollView();
			y += consoleHeight;
		}

		GUI.Box(new Rect(0, y, Screen.width, (fontSize * 2f)), "");
		GUI.backgroundColor = new Color(0, 0, 0, 0);
		GUI.SetNextControlName("InputField");
		input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, (fontSize * 1.5f)), input, textStyle);
		GUI.FocusControl("InputField"); //auto focuses the cursor onto the input field
	}

	private void HandleInput() {
		string[] properties = input.Split(' '); //splits the command on spaces

		for (int i = 0; i < commandList.Count; i++) {
			DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

			if (input.Contains(commandBase.commandId)) {
				if (commandList[i] as DebugCommand != null) { //test if casting will work
					(commandList[i] as DebugCommand).Invoke();
				}
				else if (commandList[i] as DebugCommand<int> != null) {
					(commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
				}
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Base class for DebugCommands
 * 
 * Tutorial link: https://youtu.be/VzOEM-4A2OM
 */
public class DebugCommandBase
{
	string _commandId; //calls the command
	string _commandDescription; //explains what the command does
	string _commandFormat; //tells user how to format the command parameters

	public string commandId { get { return _commandId; } }
	public string commandDescription { get { return _commandDescription; } }
	public string commandFormat { get { return _commandFormat; } }

	public DebugCommandBase(string id, string description, string format) {
		_commandId = id;
		_commandDescription = description;
		_commandFormat = format;
	}
}

public class DebugCommand : DebugCommandBase
{
	private Action command;
	public DebugCommand(string id, string description, string format, Action command) : base (id, description, format)
	{
		this.command = command;
	}

	public void Invoke() {
		command.Invoke();
	}

	
}

public class DebugCommand<T1> : DebugCommandBase
{
	private Action<T1> command;
	public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format) {
		this.command = command;
	}

	public void Invoke(T1 value) {
		command.Invoke(value);
	}


}

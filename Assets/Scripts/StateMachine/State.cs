using UnityEngine;
using System;
using System.Collections;

public class State  {
	protected string mName = "";

	public string name{ get { return mName; } }
    public State() { mName = GetType().Name; }

    public State(string varName){mName = varName;}

	private StateMachine mMachine;
	public StateMachine machine
	{
		get{return mMachine;}
	}
		
	public void SetStateMachine(StateMachine varMachine)
	{
		mMachine = varMachine;
	}
		
	public virtual void OnEnter()
	{
	}
	public virtual void OnUpdate(float deltaTime)
	{
	}
	public virtual void OnExit()
	{
	}

}

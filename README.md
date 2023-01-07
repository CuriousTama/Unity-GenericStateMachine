# Generic State Machine for unity (C#)

A state machine that try to be compatible with every use case possible.  

Tested and working Unity 2021.3.1f1 (LTS)

If you see anything that not working, seems not right or missing, don't esitate to tell me.
<br> </br>
## How is this state machine generic ?

This state machine is generic because of the possibility to drag and drop scripts in the inspector if you want to create a buffer or if you want to limite the possibilities of states the state machine can take.

In code you juste need to give a  type to a function for it to change or add the state of the said type state.

You can have access to variables from states via a componant attatched to the state machine.

<br> </br>
The state machine have currently two modes :  

&emsp; <ins>**Single**</ins> : the state machine can have only one state and change state exit the current one.  

&emsp; <ins>**Stackable**</ins> : Stack-based state machine.   
&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
The state machine can have a stack of states and add on top or go back.
<br> </br>
## How to use this State Machine

Scripts are inside "GenericStateMachine" namespace.

### State Machine
First you will need to add the component "StateMachine" to the gameObject you want.  

<img src="https://github.com/Tama-sama/Unity-GenericStateMachine/blob/main/Docs/StateMachine.PNG" height="250">  

- <ins>**Variables**</ins> : A referance to the StateVariables component ([see here](#state-variables)) 
- <ins>**State Machine Type**</ins> : The state machine type (single or stack-based). ([see here](#how-is-this-state-machine-generic-))
- <ins>**Changing state skip**</ins> : If true, when changing state the state machine skip the end of the current frame.
- <ins>**Can have any states**</ins> : If true, this state machine can have any kind of state (Can't be true at same time as caching).
- <ins>**Caching states**</ins> : If true, states in <ins>**Possible states**</ins> are going to be created at awake of the state machine (not active).
- <ins>**Starting state**</ins> : This state will automatically be set at the awake of the state machine (can be null).
- <ins>**Possible states**</ins> : Used with <ins>**Caching states**</ins> (for knowing wich state need to be cached) and <ins>**Can have any states**</ins> (if false)

### State Machine Public Methods

#### <ins>**ChangeState**</ins>
```C#
public State ChangeState(Type type);
public State ChangeState<T>();
```
Single mode : Change the current state.  
Stackable mode : Same as AddState(type).  
***Return :*** The new state
<br> </br>
#### <ins>**AddState**</ins>
```C#
public State AddState(Type type);
public State AddState<T>();
```
Single mode : Same as ChangeState(type).  
Stackable mode : Set the current state to pause and add a new one of "type".  
If cache activated resume if it already is in the previous states.  
***Return :*** The new state
<br> </br>
#### <ins>**RemoveState**</ins>
```C#
public State RemoveState();
```
Single mode : Set the state to null.  
Stackable mode : Remove the current state and resume the previous one.  
***Return :*** The new running state.
<br> </br>
#### <ins>**Clear**</ins>
```C#
public void Clear();
```
Single mode : Set the state to null.  
Stackable mode : Remove all states and set the state to null.  
<br> </br>
#### <ins>**Pause**</ins>
```C#
public bool Pause();
```
Pause the current running state.  
***Return :*** If the state was playing and well paused.
<br> </br>
#### <ins>**Resume**</ins>
```C#
public bool Resume();
```
Resume the current running state.  
***Return :*** If the state was paused and well resumed.
<br> </br>
#### <ins>**GetStatesCount**</ins>
```C#
public int GetStatesCount();
```
Get total number of states.  
***Return :*** Single mode : If no current state 0 else 1.   
&emsp;&emsp;&emsp;&emsp;Stackable mode : number of states.  
<br> </br>
#### <ins>**GetStatesAsString**</ins>
```C#
public List<string> GetStatesAsString();
```
Get states list.  
***Return :*** A list with all state as string (in order).
<br> </br>
#### <ins>**GetStatesAsType**</ins>
```C#
public List<Type> GetStatesAsType();
```
Get states list.  
***Return :*** A list with all states as System.Type (in order).
<br> </br>
#### <ins>**GetCurrentState**</ins>
```C#
public State GetCurrentState();
```
Get the current running state.  
***Return :*** The current running state.

<br> </br>
### State Variables
This component is automatically added with State machine, it can be removed and place in another Object and re-linked.
State Variables component is here to give access at scripts / objects for states.

<img src="https://github.com/Tama-sama/Unity-GenericStateMachine/blob/main/Docs/StateVariables.PNG" height="130"> <!-- -->
<img src="https://github.com/Tama-sama/Unity-GenericStateMachine/blob/main/Docs/Set_StateVariable.gif" height="250">  


It is basically a list with a name and a reference of any type of object.
- <ins>**Name**</ins> : To be found when using `Get<T>("name")` method 
- <ins>**Value**</ins> : The reference you want, can be anything derived from UnityEngine.Object class (every component and more).
 
### State
State an abstract class that you will need for every state.

```c#
using GenericStateMachine;

public class YourStateName : State
{
    // Add your overrides here.
}

```

The state class have base methods you can override : 
- <ins>**Init**</ins> : Called when a state is created (**not cached** called when you add or change state / **cached** called when a state is created in the cache (awake state machine))
- <ins>**Enter**</ins> : Called when entering a new state (**ChangeState()** and **AddState()**)  
&emsp;&emsp;&ensp;&nbsp;
NOTE : if the state machine is in stack-base mode and caching : when the state is more than one time in the stack it calls **Resume()**.
- <ins>**Exit**</ins> : Called when the state is "removed" from the state machine.  
&emsp;&emsp;&ensp;&nbsp;
NOTE : if the state machine is in stack-base mode and caching : when the state is more than one time in the stack it calls **Pause()**.
- <ins>**Resume**</ins> : Called when Resume() method from state machine is called or when the state is back to the top (stack-base)
- <ins>**Pause**</ins> : Called when Pause() method from state machine is called or when the state is no more at the top (stack-base)
- <ins>**PreUpdate**</ins> : Called every frame just before the state **Update()**.
- <ins>**Update**</ins> : Called every frame (Unity Update).
- <ins>**LateUpdate**</ins> : Called every frame (Unity LateUpdate).
- <ins>**FixedUpdate**</ins> : Called when the physics update (Unity FixedUpdate).
- <ins>**RegisterInput**</ins> : Called inside base.Enter() and base.Resume().
- <ins>**UnregisterInput**</ins> : Called inside base.Exit() and base.Pause().

You can have access to the state machine owner of the state via the public variable `stateMachine`.

## History

The idea of this project pop out after watching some codes and observed people and also myself, alwase create a new state machine script for every behaviour (principaly when caching states).

This project was create by myself after 9 months of practice in C# and unity.

### Goal :
- Make a state machine generic enough to not have to create multiple scripts for each state machine.

### Problem encounters and how I have resolved them : 
- Make the user set a sort of "Type" in the inspector : Use MonoScript.
- MonoScript are not usable outside the editor :  Get the type from string and use reflection.

### What I've learned :
- Reflection in C#.
- Editor scripting in unity.
- Make a decent documentation. 

## License

[MIT License](https://github.com/Tama-sama/Unity-GenericStateMachine/blob/main/LICENSE)

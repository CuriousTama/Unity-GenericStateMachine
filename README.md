# Generic State Machine for unity (C#)

A state machine that try to be compatible with every use case possible.
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

<img src="https://github.com/Tama-sama/Unity-GenericStateMachine/blob/Description/Docs/StateMachine.PNG" height="250">  

- <ins>**Variables**</ins> : A referance to the StateVariables component (see below) 
- <ins>**State Machine Type**</ins> : The state machine type (single or stack-based). (see upper)
- <ins>**Changing state skip**</ins> : If true, when changing state the state machine skip the end of the current frame.
- <ins>**Can have any states**</ins> : If true, this state machine can have any kind of state (Can't be true at same time as caching).
- <ins>**Caching states**</ins> : If true, states in <ins>**Possible states**</ins> are going to be created at awake of the state machine (not active).
- <ins>**Starting state**</ins> : This state will automatically be set at the awake of the state machine (can be null).
- <ins>**Possible states**</ins> : Used with <ins>**Caching states**</ins> (for knowing wich state need to be cached) and <ins>**Can have any states**</ins> (if false)

### State Variables
This component is automatically added with State machine, it can be removed and place in another Object and re-linked.
State Variables component is here to give access at scripts / objects for states.

<img src="https://github.com/Tama-sama/Unity-GenericStateMachine/blob/Description/Docs/StateVariables.PNG" height="130"> <!-- -->
<img src="https://github.com/Tama-sama/Unity-GenericStateMachine/blob/Description/Docs/Set_StateVariable.gif" height="250">  


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

The project was born out after watching some codes and observed people and also myself, alwase create a new state machine script for every behaviour (principaly when caching states).

This project was create by myself after 9 months of practice in c# and unity.

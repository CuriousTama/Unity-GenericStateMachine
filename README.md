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
 
description here

## History

description here.

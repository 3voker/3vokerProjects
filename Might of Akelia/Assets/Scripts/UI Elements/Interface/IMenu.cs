using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenu
{
    void ShowMenu();
    void HideMenu();
    MyEventSystem MyEvent();
    MyButtonScript MyButton();
	
}

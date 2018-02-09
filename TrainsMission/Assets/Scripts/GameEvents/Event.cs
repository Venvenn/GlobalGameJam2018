using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event
{
    //Mean Time To Happen: the chance of an event triggering is 1/MTTH 
    int MTTH = 0;
    string name = "New Event";
    List<Condition> conditions;
    string eventText = "An Event";
    bool fire = false;
    //constructors 
    public Event()
    {

    }

    public Event(string e_name, string e_Text, int e_MTTH)
    {
        name = e_name;
        eventText = e_Text;
        MTTH = e_MTTH;
    }

    void AddCondition()
    {
        Condition newCondition = new Condition();
        conditions.Add(newCondition);
    }

    public bool Check()
    {
        fire = true;
        foreach (Condition c in conditions)
        { 
            if(!c.Check())
            {
                fire = false;
            }
                
        }
        return fire;
    }


}

public class Condition
{
    public bool Check()
    {
        return true;
    }

}

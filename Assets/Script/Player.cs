using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Person
{
    //单例
    public static Player playerInstance;

    public static Player getInstance()
    {
        if (playerInstance == null)
        {
            playerInstance = new Player();
        }

        return playerInstance;
    }

    public Player():base(1,"Player")
    {
        friendList = new List<RelationShip>();
        Person.persons.Add(this);
        
    }

    //方法
    float getCoreSocialRadius()
    {
        return b_social * 2.0f;
    }

    float getBaseSocialRadius()
    {
        return b_social * 0.5f;
    }
}

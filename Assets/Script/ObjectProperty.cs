using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectProperty
{
    None = 0,
    Hard, //坚硬，from 石头
    Soft,
    Flexible,
    Flammable, //可燃， from 树枝
    Liquid,
    //Solid? 水+hard = solid？ 还是说 水+hard = hard
}


using GameSparks;
using GameSparks.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class MyTestClass
{
    public string myString;
    public int myInt;
    public bool myBool;
    public float myFloat;

    public string[] myStringArray;
    public List<string> myStringList;

    public float[] myFloatArray;
    public List<int> myIntList;

    

    public TestItem mySpecialItem;

    public List<TestItem> myInventory = new List<TestItem>();

    public DateTime myDate;

    public MyTestClass() {}

    public static MyTestClass BuildMyTestClass() {
        MyTestClass myTestClass = new MyTestClass();
        myTestClass.myString = "Hello World!";
        myTestClass.myInt = 100;
        myTestClass.myBool = true;
        myTestClass.myFloat = 0.1234567f;

        myTestClass.myStringArray = new string[]{ "ar_one", "ar_two", "ar_three" };
        myTestClass.myStringList = new List<string>(){ "list_one", "list_two", "list_three" };

        myTestClass.myFloatArray = new float[]{ 0.1f, 0.12f, 1.23f };
        myTestClass.myIntList = new List<int>(){ 3, 2, 1 };

        myTestClass.myDate = DateTime.Now;

        // this.myPlayerInfo = new PlayerInfo("Randy", 10, 1234);
        myTestClass.mySpecialItem = new TestItem("Special", 2, 299);

        myTestClass.myInventory.Add(new TestItem("Shield", 1, 100));
        myTestClass.myInventory.Add(new TestItem("Potion", 1, 100));
        myTestClass.myInventory.Add(new TestItem("Sword", 100, 0));

        return myTestClass;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatic{}
public interface ITestA{}
public interface ITestB{}
public class StaticClass:IStatic,ITestA,ITestB{
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Run(){
        StaticClass instance = new StaticClass();
        instance.Test();
    }
}

public static class StaticClassSystem{
    public static void Test(this IStatic instance){
        Debug.Log("I used interface");
    }

    public static void Test(this StaticClass instance){
        Debug.Log("I used class");

    }

    public static void Test(this ITestA testA){
        Debug.Log("I used TestA");
    }

    public static void Test(this ITestB testA){
        Debug.Log("I used TestB");
    }
}
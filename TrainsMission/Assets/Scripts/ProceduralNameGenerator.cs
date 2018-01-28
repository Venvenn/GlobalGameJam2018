using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class ProceduralNameGenerator
{

    protected FileInfo stationNamesFile = null;


    public List<string> stationNames = new List<string>();

    //int nameCount = 0;

    // Use this for initialization
    void Start ()
    {
        Init();
    }

    public void Init()
    {
        stationNamesFile = new FileInfo("StationNames.txt");
        stationNames = ReadFromFile(stationNamesFile);

    }

    List<string> ReadFromFile(FileInfo file )
    {
        List<string> stringList = new List<string>();
        StreamReader reader = null;
        string text = " ";

        try
        {
            reader = file.OpenText();

            using (reader)
            {
                do
                {
                    text = reader.ReadLine();

                    if (text != null)
                    {
                        string[] entries = text.Split('\t', ',');

                        foreach (string s in entries)
                        {
                            stringList.Add(s);
                        }
                    }
                }
                while (text != null);         
                reader.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("{0}\n", e.Message);
        }


        return stringList;
    }


}

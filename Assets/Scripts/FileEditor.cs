using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileEditor
{
    //Instantiating reading and writing objects
    static StreamWriter outFile;
    static StreamReader inFile;

    //Pre: file location and data proerly formatted within string array
    //Post: n/a
    //Desc: Writes data in specific format to a specified file location
    public static void WriteFile(string fileName, string[] data)
    {
        try
        {
            //Attempts to reach file
            outFile = File.CreateText(fileName);

            //Loops through all indexes of the data array, storing them in requested file location
            for(int i = 0; i < data.Length; i++)
            {
                if(i != data.Length - 1)outFile.Write(data[i] + ",");
            }

            //Closes access to file
            outFile.Close();
        }
        catch(FileNotFoundException fnfe)
        {
            Console.WriteLine(fnfe);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    //Pre: file location properly formatted
    //Post: returns string array in specific order as designated by the writing object
    //Desc: Reads data returning it in a string array
    public static string[] ReadFile(String fileName)
    {
        //Creating string array to read in written data
        string[] data = { "" };

        try
        {
            //Attempts to reach file
            inFile = File.OpenText(fileName);

            //Variables used in data harvesting
            string line;
            int counter = 0;

            //Loops through all lines of data
            while(!inFile.EndOfStream)
            {
                line = inFile.ReadLine();
                data = line.Split(',');
                counter++;
            }

            //Closes access to file 
            inFile.Close();
            //Returns properly formatted data
            return data;
        }
        catch (FormatException fe)
        {
            Console.WriteLine(fe.Message);
        }
        catch (FileNotFoundException fnfe)
        {
            Console.WriteLine(fnfe.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return data;
    }
}

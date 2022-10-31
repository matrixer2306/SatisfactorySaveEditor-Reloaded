using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SatisfactorySaveParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Save File name:");
            string fileName = "Another_World_-_U5.sav"; //Console.ReadLine();
            Debug.WriteLine(RuntimeInformation.FrameworkDescription);
            var FSaveHeader = new FSaveHeader();
            FileStream SaveFile = new FileStream($"C:/Users/gerha/AppData/Local/FactoryGame/Saved/SaveGames/90f441043e9440cbb0cf4bf1e44f9d52/{fileName}", FileMode.Open, FileAccess.Read);
            FSaveHeader.Parse(SaveFile);
        }
    }
}
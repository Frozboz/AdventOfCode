using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Program
{
    public static void Main()
    {
        int part = 2;
        char[] signal = new char[14];

        bool found = false;
        char ch;
        int totalChars = 0;

        using (var fileStream = File.OpenRead("../../Files/info.txt"))
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            if (part == 1)
            {
                do
                {
                    totalChars++;
                    ch = (char) streamReader.Read();
                    if (totalChars < 4)
                        signal[totalChars] = ch;
                    else
                    {
                        //shift left
                        signal = signal.Skip(1).Concat(new char[] {ch}).ToArray();
                        string str = new string(signal);
                        found = !str.GroupBy(x => x).Any(g => g.Count() > 1);
                    }

                    foreach (var c in signal)
                    {
                        Console.Write(c);
                    }

                    Console.WriteLine($" {totalChars} {found}");

                } while (!streamReader.EndOfStream && !found);
            }
            else //part 2
            {
                do
                {
                    totalChars++;
                    ch = (char) streamReader.Read();
                    if (totalChars < 14)
                        signal[totalChars] = ch;
                    else
                    {
                        //shift left
                        signal = signal.Skip(1).Concat(new char[] {ch}).ToArray();
                        string str = new string(signal);
                        found = !str.GroupBy(x => x).Any(g => g.Count() > 1);
                    }

                    foreach (var c in signal)
                    {
                        Console.Write(c);
                    }

                    Console.WriteLine($" {totalChars} {found}");

                } while (!streamReader.EndOfStream && !found);
            }
        }

        Console.ReadKey();

    }
}


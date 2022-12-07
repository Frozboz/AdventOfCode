using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Program
{
    public static void Main()
    {
        /*

        Lowercase item types a through z have priorities 1 through 26.
        Uppercase item types A through Z have priorities 27 through 52.

         * PART ONE
         - read file
         - split each line in equal halves
         - loop, check for like characters
         - store those character representations in list<int>: 'a' = 97, 'A' = 65
         *
         * PART TWO
         Find the item type that corresponds to the badges of each three-Elf group. What is the sum of the priorities of those item types?
         */

        const int UPPEROFFSET = 38;
        const int LOWEROFFSET = 96;

        List<string> firsthalf = new List<string>();
        List<string> secondhalf = new List<string>();

        List<string> elves = new List<string>();

        List<int> scores = new List<int>();
        int rounds = 0;
        int count = 0;

        const Int32 BufferSize = 128;
        using (var fileStream = File.OpenRead("../../Files/info.txt"))
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
        {
            
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                elves.Add(line);
                int midway = line.Length / 2;
                firsthalf.Add(line.Substring(0, midway));
                secondhalf.Add(line.Substring(midway, midway));
                string alike = DetermineLikeChar(firsthalf[count], secondhalf[count]);
                int alikeValue = char.IsUpper(alike[0]) ? (int)alike[0] - UPPEROFFSET : (int)alike[0] - LOWEROFFSET;
                //Console.WriteLine($"LINE: {line} | 1:{firsthalf[count]}: length {firsthalf[count].Length} | 2:{secondhalf[count]}: length {secondhalf[count].Length} | LIKE: {alike} value: {alikeValue}");

                scores.Add(alikeValue);
                count++;
            }
        }

        Console.WriteLine($"PART ONE Sum of all rounds: {scores.Sum()}");
        count = 0;
        scores.Clear();

        //----------------------------PART TWO--------------------------------------------------
        List<string> tmpList = new List<string>();
        
        do
        {
            string s = elves[count];
            if (tmpList.Count < 3)
            {
                tmpList.Add(s);
            }

            if (tmpList.Count == 3)
            {
                string alike = DetermineLikeChar(tmpList[0], tmpList[1], tmpList[2]);
                int alikeValue = char.IsUpper(alike[0]) ? (int)alike[0] - UPPEROFFSET : (int)alike[0] - LOWEROFFSET;
                //Console.WriteLine($" LIKE: {alike} value: {alikeValue}");
                scores.Add(alikeValue);
                tmpList.Clear();
            }

            count++;
        } while (count < elves.Count);

        Console.WriteLine($"PART TWO Sum of all rounds: {scores.Sum()}");
        Console.ReadKey();
    }

    public static string DetermineLikeChar(string s1, string s2, string s3 = null)
    {
        string alike = string.Empty;

        int alikeLoc = s1.IndexOfAny(s2.ToCharArray());
        alike = s1.Substring(alikeLoc, 1);

        if (!string.IsNullOrEmpty(s3))
        {
            //Console.WriteLine($"Checking {s1} | {s2} | {s3} - PRELIM: {alike}");

            //check s3 for the same character
            //if not found, this means S1 and S2 have more than one "alike", which is not shared by S3.  remove it and try again
            if (!s3.Contains(alike))
            {
                //Console.WriteLine($"match on {s1} and {s2} [{alike}] but not {s3}.. retrying");
                
                s1 = s1.Replace(alike, string.Empty);
                return DetermineLikeChar(s1, s2, s3);
            }

            //Console.WriteLine($"match on {s1} and {s2} and {s3}.. [{alike}]");
        }

        return alike;
    }
}
/*
match on SnmPBPBnMLnPBsSgSDqRNRRccDfNcNQQRg and lZVWtWVzCjvZzCCGzDwbwRwtqJwJNTtDfD [D] but not zCzHZFFFfdLnBfFf.. retrying
match on SnmPBPBnMLnPBsSgSqRNRRccfNcNQQRg and lZVWtWVzCjvZzCCGzDwbwRwtqJwJNTtDfD [q] but not zCzHZFFFfdLnBfFf.. retrying
match on SnmPBPBnMLnPBsSgSRNRRccfNcNQQRg and lZVWtWVzCjvZzCCGzDwbwRwtqJwJNTtDfD [R] but not zCzHZFFFfdLnBfFf.. retrying
 */

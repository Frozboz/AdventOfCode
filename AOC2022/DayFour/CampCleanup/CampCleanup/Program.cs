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
         - build arrays
         *
         * PART TWO
         * the Elves would like to know the number of pairs that overlap at all
         */

        List<ElfPair> elves = new List<ElfPair>();

        List<string> pairs = new List<string>();
        List<ElfPair> scores = new List<ElfPair>();

        int rounds = 0;
        int count = 0;

        const Int32 BufferSize = 128;
        using (var fileStream = File.OpenRead("../../Files/info.txt"))
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
        {
            
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                pairs.Add(line);
            }
        }
        // 31-70 / 30-71
        foreach (var s in pairs)
        {
            ElfPair pair = new ElfPair(count);

            string[] splitValue = s.Split(',');            //something like 2-4,4-8
            string[] firstRange = splitValue[0].Split('-');  //something like 2-4
            string[] secondRange = splitValue[1].Split('-'); //something like 4-8

            int tmp = 0;
            int.TryParse(firstRange[0], out tmp);
            pair.BeginSection1 = tmp;

            int.TryParse(firstRange[1], out tmp);
            pair.EndSection1 = tmp;

            int.TryParse(secondRange[0], out tmp);
            pair.BeginSection2 = tmp;

            int.TryParse(secondRange[1], out tmp);
            pair.EndSection2 = tmp;

            count++;
            elves.Add(pair);
        }

        //part one: overlap completely
        foreach (var elf in elves)
        {
            if (DetermineCompleteOverlap(elf))
            {
                //Console.WriteLine($"Complete Overlap!  {elf.ToString()}");
                scores.Add(elf);
            }
        }

        Console.WriteLine($"PART ONE Count of all overlapping pairs: {scores.Count}");
        scores.Clear();

        //----------------------------PART TWO--------------------------------------------------
        //overlap at all.
        foreach (var elf in elves)
        {
            if (DetermineCompleteOverlap(elf) ||
               ((elf.BeginSection1 <= elf.EndSection2) && (elf.EndSection1 >= elf.BeginSection2)) ||
                (elf.BeginSection2 <= elf.EndSection1) && (elf.EndSection2 >= elf.BeginSection1))
            {
                //Console.WriteLine($"ANY Overlap!  {elf.ToString()}");
                scores.Add(elf);
            }
        }


        Console.WriteLine($"PART TWO Sum of all rounds: {scores.Count}");
        Console.ReadKey();
    }

    public static bool DetermineCompleteOverlap(ElfPair elf)
    {
        if ((elf.BeginSection1 <= elf.BeginSection2) && (elf.EndSection1 >= elf.EndSection2)
            ||
            (elf.BeginSection2 <= elf.BeginSection1) && (elf.EndSection2 >= elf.EndSection1))
        {
            return true;
        }

        return false;
    }

    public class ElfPair
    {
        public ElfPair(int id) { GroupID = id; }

        public int GroupID { get; set; }
        public int BeginSection1 { get; set; }
        public int EndSection1 { get; set; }
        public int BeginSection2 { get; set; }
        public int EndSection2 { get; set; }

        public string ToString()
        {
            return $"{GroupID}: {BeginSection1}-{EndSection1} / {BeginSection2}-{EndSection2}";
        }
    }
}


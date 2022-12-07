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
         - phase one: read in current stacks configuration - letters will be surrounded by [] and separated by spaces
                        these letters start at column 2 and then go every 4 spaces. so 2, 6, 10, 14...
                        determine length of these "levels" first
                                    [D]    
                                [N] [C]    
                                [Z] [M] [P]
                                 1   2   3 

         - phase two: after a linebreak, we will encounter the commands.  they will look like this:
                            move 1 from 2 to 1
                            move 3 from 1 to 3
                            move 2 from 2 to 1
                            move 1 from 1 to 2            

                        these commands will have 3 parts: Number of crates to move, source, destination
                        commands run one at a time: e.g. "move 3" means "move 3, one at a time"
         *
         * PART TWO
         * "move 1" means the same
         * "move 3" now means "move all 3 at once"
         */

        int rounds = 0;
        int count = 0;
        double numStacks = 0;
        int part = 2;

        LinkedList<string>[] stacks = new LinkedList<string>[0];
        List<StackCommand> commands = new List<StackCommand>();

        #region read data
        const Int32 BufferSize = 128;
        using (var fileStream = File.OpenRead("../../Files/info.txt"))
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
        {

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                //read in stack contents first.
                if (line.Contains("["))
                {
                    //first pass through
                    if (numStacks < 1)
                    {
                        //first determine length of level. this will tell us how many stacks (LinkedLists) we will need.
                        numStacks = Math.Floor((double)(line.Length / 4)) + 1;

                        //next create an array of stacks
                        stacks = new LinkedList<string>[(int)numStacks];

                    }

                    //now fill stacks.  possible boxes are every 4 spaces starting at 2
                    for (int i = 2; i < line.Length; i += 4)
                    {
                        int pass = (int)Math.Floor((double)(i / 4));
                        if (char.IsUpper(line[i - 1]))
                        {
                            if (stacks[pass] == null)
                            {
                                stacks[pass] = new LinkedList<string>();
                            }

                            stacks[pass].AddFirst(line.Substring(i - 1, 1));
                        }
                    }
                }

                //this is the break between the stack creation and the instructions
                if (string.IsNullOrEmpty(line))
                {
                    int countStacks = 0;
                    foreach (var stack in stacks)
                    {
                        Console.WriteLine($"Stack {countStacks}: {PrintAllNodes(stack)}");
                        countStacks++;
                    }

                    Console.WriteLine(
                        "----------------------------------------------------------------------------------");
                    Console.ReadKey();
                }

                //commands
                if (line.Contains("move"))
                {
                    //first parse the command: format "move <numCrates> from <Source> to <Destination>"

                    int cratesFrom = line.IndexOf("move ") + "move ".Length;
                    int cratesTo = line.IndexOf(" from ");
                    int sourceFrom = line.IndexOf(" from ") + " from ".Length;
                    int sourceTo = line.IndexOf(" to ");
                    int destFrom = line.IndexOf(" to ") + " to ".Length;

                    int crates, source, dest;

                    int.TryParse(line.Substring(cratesFrom, cratesTo - cratesFrom), out crates);
                    int.TryParse(line.Substring(sourceFrom, sourceTo - sourceFrom), out source);
                    int.TryParse(line.Substring(destFrom, line.Length - destFrom), out dest);

                    //Console.WriteLine($"line: {line}  ////  crates: {crates}, source: {source}, dest: {dest} ");
                    //Console.ReadKey();

                    StackCommand cmd = new StackCommand
                    { Destination = dest - 1, NumCrates = crates, Source = source - 1 };
                    commands.Add(cmd);
                }
            }
        } 
        #endregion

        StringBuilder sb = new StringBuilder();

        #region part1
        if (part == 1)
        {
            //execute the commands.
            foreach (var cmd in commands)
            {
                for (int i = 0; i < cmd.NumCrates; i++)
                {
                    Console.WriteLine(cmd.ToString());
                    string temp = stacks[cmd.Source].Last.Value;
                    stacks[cmd.Destination].AddLast(temp);
                    stacks[cmd.Source].RemoveLast();
                }

                Console.WriteLine(ShowAllStacks(stacks));
                //Console.ReadKey();
            }

            
            sb.Append("Final result PART ONE: ");
            foreach (var stack in stacks)
            {
                sb.Append(stack.Last.Value);
            }


            Console.WriteLine(sb.ToString());
            Console.ReadKey();
        }

        #endregion

        #region part2
        else
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            //execute the commands.
            foreach (var cmd in commands)
            {
                if (cmd.NumCrates == 1)
                {
                    //Console.WriteLine(cmd.ToString());
                    string temp = stacks[cmd.Source].Last.Value;
                    stacks[cmd.Destination].AddLast(temp);
                    stacks[cmd.Source].RemoveLast();
                }
                else
                {
                    string[] bulkMove = new string[cmd.NumCrates];
                    for (int i = 0; i < cmd.NumCrates; i++)
                    {
                        //Console.WriteLine(cmd.ToString());
                        bulkMove[i] = stacks[cmd.Source].Last.Value;
                        stacks[cmd.Source].RemoveLast();

                        //Console.WriteLine($"grabbing {bulkMove[i]}");
                    }

                    for (int x = cmd.NumCrates - 1; x >= 0; x--)
                    {
                        //Console.WriteLine($"adding {bulkMove[x]}");
                        stacks[cmd.Destination].AddLast(bulkMove[x]);
                    }
                }

                Console.WriteLine(cmd.ToString());
                Console.WriteLine(ShowAllStacks(stacks));
                //Console.ReadKey();
            }

            sb = new StringBuilder();
            sb.Append("Final result PART TWO: ");
            foreach (var stack in stacks)
            {
                sb.Append(stack.Last.Value);
            }

            Console.WriteLine(sb.ToString());
            Console.ReadKey();
        } 
        #endregion
    }

    #region helpers
    public static string PrintAllNodes(LinkedList<string> stack)
    {
        string[] arr = stack.ToArray();
        StringBuilder sb = new StringBuilder();
        foreach (string s in arr)
        {
            sb.Append($"{s} ");
        }

        return sb.ToString();
    }

    public static string ShowAllStacks(LinkedList<string>[] stacks)
    {
        int countStacks = 0;
        StringBuilder sb = new StringBuilder();
        foreach (var stack in stacks)
        {
            sb.AppendLine($"Stack {countStacks}: {PrintAllNodes(stack)}");
            countStacks++;
        }

        return sb.ToString();
    }

    public class StackCommand
    {
        public int NumCrates { get; set; }
        public int Source { get; set; }
        public int Destination { get; set; }

        public string ToString()
        {
            return $"command: moving {NumCrates} crate(s) from {Source} to {Destination}  ";
        }
    } 
    #endregion

}


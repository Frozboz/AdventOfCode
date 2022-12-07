using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RockPaperScissors;

public class Program
{
    public static void Main()
    {
        /*
         * PART ONE
         * A opponent rock
         * B opponent paper
         * C opponent scissors
         *
         * X my rock = 1
         * Y my paper = 2
         * Z my scissors = 3
         *
         * loss = 0
         * tie = 3
         * win = 6
         *
         * PART TWO
         * - as above, but X = intentional loss
         *                 Y means you need to end the round in a draw, and
         *                 Z means you need to win
         */

        List<KeyValuePair<string, string>> alwaysLose = new List<KeyValuePair<string, string>>();
        alwaysLose.Add(new KeyValuePair<string, string>("A", "Z"));
        alwaysLose.Add(new KeyValuePair<string, string>("B", "X"));
        alwaysLose.Add(new KeyValuePair<string, string>("C", "Y"));

        List<KeyValuePair<string, string>> alwaysDraw = new List<KeyValuePair<string, string>>();
        alwaysDraw.Add(new KeyValuePair<string, string>("A", "X"));
        alwaysDraw.Add(new KeyValuePair<string, string>("B", "Y"));
        alwaysDraw.Add(new KeyValuePair<string, string>("C", "Z"));

        List<KeyValuePair<string, string>> alwaysWin = new List<KeyValuePair<string, string>>();
        alwaysWin.Add(new KeyValuePair<string, string>("A", "Y"));
        alwaysWin.Add(new KeyValuePair<string, string>("B", "Z"));
        alwaysWin.Add(new KeyValuePair<string, string>("C", "X"));

        List<int> scores = new List<int>();
        int rounds = 0;

        const Int32 BufferSize = 128;
        using (var fileStream = File.OpenRead("../../Files/info.txt"))
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
        {
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                int roundScore = 0;

                string[] round;
                round = line.Split(' ');

                //part one
                //scores.Add(DetermineWinner(round[0], round[1]));

                //part two
                string loser = alwaysLose.Single(x => x.Key == round[0]).Value;
                string winner = alwaysWin.Single(x => x.Key == round[0]).Value;
                string drawer = alwaysDraw.Single(x => x.Key == round[0]).Value;

                switch (round[1])
                {
                    case "X": //always lose
                        scores.Add(DetermineWinner(round[0], alwaysLose.Single(x => x.Key == round[0]).Value));
                        Console.WriteLine($"{rounds} was {round[0]} vs {loser}, a score of {scores[rounds]}");
                        break;
                    case "Y": //always draw
                        scores.Add(DetermineWinner(round[0], alwaysDraw.Single(x => x.Key == round[0]).Value));
                        Console.WriteLine($"{rounds} was {round[0]} vs {drawer}, a score of {scores[rounds]}");
                        break;
                    case "Z": //always win
                        scores.Add(DetermineWinner(round[0], alwaysWin.Single(x => x.Key == round[0]).Value));
                        Console.WriteLine($"{rounds} was {round[0]} vs {winner}, a score of {scores[rounds]}");
                        break;
                }

                //Console.WriteLine($"{rounds} was {round[0]} vs {round[1]}, a score of {scores[rounds]}");

                rounds++;
            }
        }

        Console.WriteLine($"Sum of all rounds: {scores.Sum()}");
        
        Console.ReadKey();
    }

    public static int DetermineWinner(string them, string me)
    {
        int roundScore = 0;

        switch (me)
        {
            case "X": //my rock
                roundScore = 1;
                switch (them)
                {
                    case "A": //them rock, draw
                        roundScore += (int)RoundResult.Draw;
                        break;
                    case "B": //them paper, loss
                        roundScore += (int)RoundResult.Loss;
                        break;
                    case "C": //them scissors, win
                        roundScore += (int)RoundResult.Win;
                        break;
                }
                break;
            case "Y": //my paper
                roundScore = 2;
                switch (them)
                {
                    case "A": //them rock, loss
                        roundScore += (int)RoundResult.Win;
                        break;
                    case "B": //them paper, draw
                        roundScore += (int)RoundResult.Draw;
                        break;
                    case "C": //them scissors, win
                        roundScore += (int)RoundResult.Loss;
                        break;
                }
                break;
            case "Z": //my scissors
                roundScore = 3;
                switch (them)
                {
                    case "A": //them rock, loss
                        roundScore += (int)RoundResult.Loss;
                        break;
                    case "B": //them paper, win
                        roundScore += (int)RoundResult.Win;
                        break;
                    case "C": //them scissors, draw
                        roundScore += (int)RoundResult.Draw;
                        break;
                }
                break;
        }

        return roundScore;
    }
}
 
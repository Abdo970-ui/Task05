using System;
using System.Collections.Generic;

namespace ExaminationSystem
{
    class Program
    {
        static void Main()
        {
            Tools.Start();
        }
    }

    // ================= TOOLS =================
    class Tools
    {
        static List<Question> questions = new List<Question>();

        public static void Start()
        {
            while (true)
            {
                Console.WriteLine("1- Doctor Mode");
                Console.WriteLine("2- Student Mode");
                Console.WriteLine("3- Exit");
                Console.Write("Choose: ");

                byte mode = Convert.ToByte(Console.ReadLine());

                if (mode == 1) DoctorMode();
                else if (mode == 2) StudentMode();
                else if (mode == 3) break;


            }
        }

        static void DoctorMode()
        {

            Console.Write("Enter number of questions: ");
            int count = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("\n1- True/False");
                Console.WriteLine("2- Choose One");
                Console.WriteLine("3- Multiple Choice");
                Console.Write("Type: ");
                int type = Convert.ToInt32(Console.ReadLine());

                Console.Write("Level (0-Easy, 1-Medium, 2-Hard): ");
                ENLevel level = (ENLevel)Convert.ToInt32(Console.ReadLine());

                Console.Write("Question Header: ");
                string header = Console.ReadLine();

                Console.Write("Marks: ");
                int marks = Convert.ToInt32(Console.ReadLine());

                if (type == 1)
                {
                    TrueFalseQuestion q = new TrueFalseQuestion();
                    q.Header = header;
                    q.Level = level;
                    q.Marks = marks;

                    Console.Write("Correct Answer (true/false): ");
                    q.CorrectAnswer = Convert.ToBoolean(Console.ReadLine());

                    questions.Add(q);
                }
                else if (type == 2)
                {
                    ChooseOneQuestion q = new ChooseOneQuestion();
                    q.Header = header;
                    q.Level = level;
                    q.Marks = marks;

                    q.Choices = new string[4];
                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write($"Choice {j + 1}: ");
                        q.Choices[j] = Console.ReadLine();
                    }

                    Console.Write("Correct choice number (1-4): ");
                    q.CorrectIndex = Convert.ToInt32(Console.ReadLine());

                    questions.Add(q);
                }
                else if (type == 3)
                {
                    MultipleChoiceQuestion q = new MultipleChoiceQuestion();
                    q.Header = header;
                    q.Level = level;
                    q.Marks = marks;

                    q.Choices = new string[4];
                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write($"Choice {j + 1}: ");
                        q.Choices[j] = Console.ReadLine();
                    }

                    Console.Write("How many correct answers? ");
                    int n = Convert.ToInt32(Console.ReadLine());
                    q.CorrectIndexes = new int[n];

                    for (int j = 0; j < n; j++)
                    {
                        Console.Write($"Correct choice {j + 1}: ");
                        q.CorrectIndexes[j] = Convert.ToInt32(Console.ReadLine());
                    }

                    questions.Add(q);
                }
            }
        }

        static void StudentMode()
        {
            Console.Clear();
            Console.WriteLine("1- Practical");
            Console.WriteLine("2- Final");
            Console.Write("Choose exam type: ");
            int examType = Convert.ToInt32(Console.ReadLine());

            Console.Write("Level (0-Easy, 1-Medium, 2-Hard): ");
            ENLevel level = (ENLevel)Convert.ToInt32(Console.ReadLine());

            int total = 0;
            int score = 0;
            int count = 0;

            for (int i = 0; i < questions.Count; i++)
            {
                if (questions[i].Level == level)
                    count++;
            }

            int limit = (examType == 1) ? count / 2 : count;

            for (int i = 0, shown = 0; i < questions.Count && shown < limit; i++)
            {
                if (questions[i].Level == level)
                {
                    questions[i].Display();
                    score += questions[i].CheckAnswer();
                    total += questions[i].Marks;
                    shown++;
                }
            }

            Console.WriteLine($"\nResult: {score} / {total}");
            Console.ReadLine();
        }
    }

    // ================= ENUM =================
    enum ENLevel { Easy, Medium, Hard }

    // ================= QUESTIONS =================
    class Question
    {
        public string Header;
        public ENLevel Level;
        public int Marks;

        public virtual void Display()
        {
            Console.WriteLine("\n" + Header);
        }

        public virtual int CheckAnswer()
        {
            return 0;
        }
    }

    class TrueFalseQuestion : Question
    {
        public bool CorrectAnswer;

        public override void Display()
        {
            base.Display();
            Console.WriteLine("1- True");
            Console.WriteLine("2- False");
        }

        public override int CheckAnswer()
        {
            int ans = Convert.ToInt32(Console.ReadLine());
            bool userAnswer = (ans == 1);
            if (userAnswer == CorrectAnswer)
                return Marks;
            return 0;
        }
    }

    class ChooseOneQuestion : Question
    {
        public string[] Choices;
        public int CorrectIndex;

        public override void Display()
        {
            base.Display();
            for (int i = 0; i < 4; i++)
                Console.WriteLine($"{i + 1}- {Choices[i]}");
        }

        public override int CheckAnswer()
        {
            int ans = Convert.ToInt32(Console.ReadLine());
            if (ans == CorrectIndex)
                return Marks;
            return 0;
        }
    }

    class MultipleChoiceQuestion : Question
    {
        public string[] Choices;
        public int[] CorrectIndexes;

        public override void Display()
        {
            base.Display();
            for (int i = 0; i < 4; i++)
                Console.WriteLine($"{i + 1}- {Choices[i]}");
            Console.WriteLine("Enter answers separated by space:");
        }

        public override int CheckAnswer()
        {
            string[] input = Console.ReadLine().Split(' ');
            int correct = 0;

            for (int i = 0; i < input.Length; i++)
            {
                int ans = Convert.ToInt32(input[i]);

                for (int j = 0; j < CorrectIndexes.Length; j++)
                {
                    if (ans == CorrectIndexes[j])
                        correct++;
                }
            }

            if (correct == CorrectIndexes.Length)
                return Marks;

            return 0;
        }
    }
}

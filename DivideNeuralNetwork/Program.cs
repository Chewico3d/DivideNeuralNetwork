using BasicInteligenceSystem;
using System.Diagnostics;
using System.IO;

static class Program
{
    static NeuralAI neuralNetwork;

    static void Main()
    {
        Help();
        Console.WriteLine("If you want to load type l if not other");
        if(Console.ReadLine() == "l")
        {
            Console.WriteLine("Insert the path to the neural network or the name");
            string Path = Console.ReadLine();
            if (File.Exists(Path))
            {
                neuralNetwork = NeuralSave.LoadNeuralNetwork(Path);
                neuralNetwork.InitializeTrainingValues();

            }
            else
            {

                Console.WriteLine("Could Not load from path");
                return;
            }

        }
        else
        {
            neuralNetwork = new NeuralAI(new int[] { 16, 82, 64, 8 });
            neuralNetwork.RanzomitzeValues();
            neuralNetwork.InitializeTrainingValues();
        }

        while (true)
        {
            Console.Write("\n>>");
            string Input = Console.ReadLine();

            switch (Input)
            {
                case "h":
                    Help();
                    break;
                case "t":
                    Train();
                    break;
                case "p":
                    Test();
                    break;
                case "l":
                    LinealTrain();
                    break;
                case "c":
                    CheckAll();
                    break;
                case "s":
                    Console.WriteLine("Insert the name or the path you want to save");
                    string Path = Console.ReadLine();

                    NeuralSave.SaveNeuralNetworkIn(neuralNetwork, Path);
                    Console.WriteLine("Neural network saved with error : " + NeuralSave.ErrorGave);

                    break;

            }

        }

    }

    static void Help()
    {
        Console.WriteLine("h to get list help");
        Console.WriteLine("t to train the NN");
        Console.WriteLine("p to test the neural network");
        Console.WriteLine("c to check all possible errors");
        Console.WriteLine("s to save the neural network");

    }
    static void Train()
    {
        Console.WriteLine("-Insert the number of itirenations you want to train");

        int Number = GetNumber();
        if (Number == -1)
            return;

        float AverageTime = 0;
        for(int it = 0; it < Number; it++)
        {
            float AverageError = 0;
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            for(int x = 0; x < 1000; x++)
            {
                for(int u = 0; u < 3; u++)
                {
                    float RandomAf = (float)NeuralMath.Random.NextDouble();
                    //RandomAf = MathF.Sqrt(RandomAf);
                    float RandomBf = (float)NeuralMath.Random.NextDouble();
                    //RandomBf *= RandomBf;

                    byte InputA = (byte)(RandomAf * 255);
                    byte InputB = (byte)(RandomBf * 255);
                    if (InputB == 0)
                        continue;

                    int IResult = (int)((float)InputA / (float)InputB);

                    //Console.WriteLine(InputA + " / " + InputB + " = " + IResult);
                    //continue;

                    for (int z = 0; z < 8; z++)
                        neuralNetwork.Inputs[z] = ((InputA & (1 << z)) > 0) ? 1 : 0;

                    for (int z = 0; z < 8; z++)
                        neuralNetwork.Inputs[z + 8] = ((InputB & (1 << z)) > 0) ? 1 : 0;

                    neuralNetwork.FeedFoward();

                    for (int z = 0; z < 8; z++)
                        neuralNetwork.ExpectedOutput[z] = ((IResult & (1 << z)) > 0) ? 1 : 0;

                    AverageError += neuralNetwork.AverageError;

                    neuralNetwork.Train();

                }
                neuralNetwork.ApplyTraining();

            }
            sw.Stop();
            AverageTime += sw.ElapsedMilliseconds / 100f;
            Console.WriteLine("Train number " + (it + 1) * 3000 + " with : " + (int)((float)AverageError / 3f) + " of error; will end in : " + (int)(AverageTime / (float)(it + 1) * (Number - it - 1)));
        }

    }
    static void LinealTrain()
    {
        Console.WriteLine("-Insert the number of itirenations you want to train");

        int Number = GetNumber();
        if (Number == -1)
            return;

        for (int it = 0; it < Number; it++)
        {
            float AverageError = 0;
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            for (int x = 1; x < 255; x++)
            {
                for (int u = 1; u < 255; u++)
                {

                    byte InputA = (byte)x;
                    byte InputB = (byte)u;
                    if (InputB == 0)
                        continue;

                    int IResult = (int)((float)InputA / (float)InputB);

                    //Console.WriteLine(InputA + " / " + InputB + " = " + IResult);
                    //continue;

                    for (int z = 0; z < 8; z++)
                        neuralNetwork.Inputs[z] = ((InputA & (1 << z)) > 0) ? 1 : 0;

                    for (int z = 0; z < 8; z++)
                        neuralNetwork.Inputs[z + 8] = ((InputB & (1 << z)) > 0) ? 1 : 0;

                    neuralNetwork.FeedFoward();

                    for (int z = 0; z < 8; z++)
                        neuralNetwork.ExpectedOutput[z] = ((IResult & (1 << z)) > 0) ? 1 : 0;

                    AverageError += neuralNetwork.AverageError;

                    neuralNetwork.Train();
                    neuralNetwork.ApplyTraining();

                }

            }
            sw.Stop();

            Console.WriteLine("Train number " + (it + 1) * 3000 + " with : " + (float)AverageError / 3000f + " of error; will end in : " + (int)((float)sw.ElapsedMilliseconds / 1000f * (Number - it)));
        }

    }
    static int GetNumber()
    {
        Console.Write(">>");

        int Number = -1;
        if (!int.TryParse(Console.ReadLine(), out Number))
        {
            Console.WriteLine("The system could not recognise the number");
            return -1;
        }
        if (Number < 0)
        {
            Console.WriteLine("The number can't be negative");
            return -1;
        }

        return Number;

    }
    static void Test()
    {
        Console.WriteLine("Insert the first number");

        int NumberA = GetNumber();
        if (NumberA == -1)
            return;

        Console.WriteLine("Insert the second number number");

        int NumberB = GetNumber();
        if (NumberB == -1)
            return;


        byte InputA = (byte)NumberA;
        byte InputB = (byte)NumberB;

        int IResult = (int)((float)InputA / (float)InputB);

        for (int z = 0; z < 8; z++)
            neuralNetwork.Inputs[z] = ((InputA & (1 << z)) > 0) ? 1 : 0;

        for (int z = 0; z < 8; z++)
            neuralNetwork.Inputs[z + 8] = ((InputB & (1 << z)) > 0) ? 1 : 0;

        neuralNetwork.FeedFoward();

        for (int z = 0; z < 8; z++)
            neuralNetwork.ExpectedOutput[z] = ((IResult & (1 << z)) > 0) ? 1 : 0;

        Console.WriteLine("Average Error : " + neuralNetwork.AverageError);
        float NeuralResult = 0;
        for (int x = 0; x < 8; x++)
            NeuralResult += (neuralNetwork.Outputs[x] > .5f)? (float)(1 << x) : 0;
        neuralNetwork.Train();
        neuralNetwork.ApplyTraining();

        Console.WriteLine(NumberA + " / " + NumberB + " = " + NeuralResult);


    }
    static void CheckAll()
    {
        int Errors = 0;
        for(int InputA = 1; InputA < 256; InputA++)
        {
            for(int InputB = 1; InputB < 256; InputB++)
            {

                int IResult = (int)((float)InputA / (float)InputB);

                //Console.WriteLine(InputA + " / " + InputB + " = " + IResult);
                //continue;

                for (int z = 0; z < 8; z++)
                    neuralNetwork.Inputs[z] = ((InputA & (1 << z)) > 0) ? 1 : 0;

                for (int z = 0; z < 8; z++)
                    neuralNetwork.Inputs[z + 8] = ((InputB & (1 << z)) > 0) ? 1 : 0;

                neuralNetwork.FeedFoward();

                for (int z = 0; z < 8; z++)
                    neuralNetwork.ExpectedOutput[z] = ((IResult & (1 << z)) > 0) ? 1 : 0;

                neuralNetwork.Train();
                neuralNetwork.ApplyTraining();


                int Res = 0;
                for (int z = 0; z < 8; z++)
                    Res += (neuralNetwork.Outputs[z] > .5f) ? (1 << z) : 0;

                if (Res != IResult)
                    Errors += Math.Abs(IResult - Res);

            }

        }
        Console.WriteLine("Errors : " + Errors);

    }

}


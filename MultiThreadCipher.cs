using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace CaesarCipher
{
    internal class MultiThreadCipher
    {
        private readonly char[] alphabet;
        private int numberOfThreads = 2, oneBlockSize, lefoverChars;
        static readonly object _lockObject = new object();

        private Thread[] threads; //hold the number of selected threads
        private int[] blockBorderIndexes; //hold the ending index of each block
        private string[] blockText; //chopped main string into cumputable blocks
        public int NumberOfThreads
        {
            get { return numberOfThreads; }

            set {if (value <= Environment.ProcessorCount && value > 1) numberOfThreads = value;}
        }

        public MultiThreadCipher(char[] currentAlphabet)
        {
            alphabet = currentAlphabet;
        }

        //SINGLE THREAD
        private void CreateBlockIndexes(string inputText)
        {
            //Array initialization
            blockBorderIndexes = new int[numberOfThreads+1]; //each block has same size as all others example: 10 chars calculated by 3 threads -> 3chars per thread and the last thread will have one char extra (from leftovers) -> 3,3,4
            blockText = new string[numberOfThreads];

            //set size of one block and get leftover values from division
            oneBlockSize = inputText.Length / numberOfThreads;
            lefoverChars = inputText.Length % numberOfThreads;

            for (int i = 1; i < numberOfThreads; i++) //leave array index 0 empty
                blockBorderIndexes[i] = (i * oneBlockSize - 1);

            //set left over values to last block
            blockBorderIndexes[numberOfThreads] = (numberOfThreads * oneBlockSize + lefoverChars -1);

            ThreadsInitialization(inputText);
        }

        //MULTI THREAD
        private string TextToLower(string inputText)
        {
            try
            {
                return inputText.ToLower();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Chyba!");
                return "Error";
            }
        }

        //MULTI THREAD
        private void SeparateTextToBlocks(string inputText, int arrayIndex, int fromIndex, int toIndex)
        {
            blockText[arrayIndex] = inputText.Substring(fromIndex, toIndex - fromIndex); //separete huge text into smaller blocks
            blockText[arrayIndex] = TextToLower(blockText[arrayIndex]); //set whole block to lowercase letters
            //MessageBox.Show(blockText[arrayIndex], Thread.CurrentThread.Name +"Index: "+ arrayIndex.ToString());
        }

        //SINGLE THREAD
        private void ThreadsInitialization(string inputText)
        {
            if (Thread.CurrentThread.Name == null)
                Thread.CurrentThread.Name = "MainThread";

            threads = new Thread[numberOfThreads-1]; //example: When I want to use 4 threads for cipher I dont need to create new 4 threads, but just 3 because one is alreading running this code.

            for (int i = 0; i < threads.Length; i++)
            {
                int index = i; //Need to save i to separate variable called "index" If NOT then the "threads[index].Start();" will receive different index value because the for loop will already incement it's own value.
                if (i == 0)
                {
                    threads[index] = new Thread(() => SeparateTextToBlocks(inputText, index, blockBorderIndexes[index], blockBorderIndexes[index + 1])); //first block need to start from zero because of block separation by indexes and it prevents from overlapping.
                    threads[index].Name = $"Thread {index}";
                    //Debug.Print("for loop index: "+ index.ToString() + " Name: " + threads[index].Name);
                }
                else
                {
                    threads[index] = new Thread(() => SeparateTextToBlocks(inputText, index, blockBorderIndexes[index] + 1, blockBorderIndexes[index + 1]));
                    threads[index].Name = $"Thread {index}";
                    //Debug.Print(index.ToString() + " " + threads[index].Name);
                }
                threads[index].Start();
            }

            SeparateTextToBlocks(inputText, threads.Length, blockBorderIndexes[threads.Length] + 1, blockBorderIndexes[threads.Length + 1]); //Run last method on default thread thats assigned to this app
        }

        //SINGLE THREAD
        public string MultiThreaded(string userInputText)
        {
            CreateBlockIndexes(userInputText);
            string output = "";

            foreach (Thread item in threads)
                item.Join();

            foreach (var item in blockText)
                output += "|" + item + "\n";

            MessageBox.Show(output);
            return "";
        }
    }
}

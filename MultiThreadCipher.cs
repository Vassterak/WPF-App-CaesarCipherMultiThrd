using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Threading;

namespace CaesarCipher
{
    internal class MultiThreadCipher
    {
        private readonly char[] alphabet;
        private int numberOfThreads = 2;
        public int NumberOfThreads
        {
            get { return numberOfThreads; }
            set
            {
                if (value <= Environment.ProcessorCount && value > 1)
                    numberOfThreads = value;
            }
        }
        private Thread[] threads;
        private List<int> blockSizes;

        public MultiThreadCipher(char[] currentAlphabet)
        {
            alphabet = currentAlphabet;
        }

        private void ChopTextToBlocks(string inputText)
        {
            blockSizes = new List<int>();
            int oneBlockSize = inputText.Length / numberOfThreads;
            int lefoverChars = inputText.Length % numberOfThreads;

            for (int i = 0; i < numberOfThreads-1; i++)
            {
                blockSizes.Add(oneBlockSize);
            }
            blockSizes.Add(oneBlockSize + lefoverChars);

            string output = "";

            foreach (var item in blockSizes)
            {
                output += item.ToString() + "\r\n";
            }
            MessageBox.Show(output);

            MessageBox.Show("Left overs: "+ lefoverChars.ToString());
        }

        private void ThreadsInit()
        {
            threads = new Thread[numberOfThreads];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => ConvertText());
                threads[i].Start();
            }
        }

        private void ConvertText()
        {

        }

        public string MultiThreaded(string userInputText)
        {
            ChopTextToBlocks(userInputText);
            return "";
        }

    }
}

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
        private int numberOfThreads = 2, oneBlockSize, lefoverChars;

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
            oneBlockSize = inputText.Length / numberOfThreads;
            lefoverChars = inputText.Length % numberOfThreads;

            for (int i = 1; i < numberOfThreads; i++)
                blockSizes.Add(i * oneBlockSize - 1);

            blockSizes.Add(numberOfThreads * oneBlockSize + lefoverChars -1);

            string output = "";

            foreach (var item in blockSizes)
                output += item.ToString() + "\r\n";

            MessageBox.Show($"number of chars: {inputText.Length}\n {output}\n Left overs:+ {lefoverChars.ToString()}");
        }

        private void ConvertText(int fromIndex, int toIndex)
        {

        }

        private void ThreadsInit()
        {
            threads = new Thread[numberOfThreads];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => ConvertText( i * oneBlockSize, oneBlockSize));
                threads[i].Start();
            }
        }

        public string MultiThreaded(string userInputText)
        {
            ChopTextToBlocks(userInputText);
            return "";
        }

    }
}

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
        private int[] manipulatedText;

        private Thread[] threads; //hold the number of selected threads
        private List<int> blockSizesIndexes; //hold the ending index of each block, first position is calculated
        private string[] blockTextParts; //chopped main string into cumputable blocks
        public int NumberOfThreads
        {
            get { return numberOfThreads; }
            set
            {
                if (value <= Environment.ProcessorCount && value > 1)
                    numberOfThreads = value;
            }
        }

        public MultiThreadCipher(char[] currentAlphabet)
        {
            alphabet = currentAlphabet;
        }

        //SINGLE THREAD
        private void CreateBlockIndexes(string inputText)
        {
            blockSizesIndexes = new List<int>();
            blockTextParts = new string[numberOfThreads];

            //set sizes
            oneBlockSize = inputText.Length / numberOfThreads;
            lefoverChars = inputText.Length % numberOfThreads;

            for (int i = 1; i < numberOfThreads; i++)
                blockSizesIndexes.Add(i * oneBlockSize - 1);

            blockSizesIndexes.Add(numberOfThreads * oneBlockSize + lefoverChars -1);

            //string output = "";
            //foreach (var item in blockSizesIndexes)
            //    output += item.ToString() + "\r\n";
            //MessageBox.Show($"number of chars: {inputText.Length}\n {output}\n Left overs:+ {lefoverChars.ToString()}");
            ThreadsInitialization(inputText);
        }

        //MULTI THREAD
        private void SoftTextToBlocks(string inputText, int arrayIndex, int fromIndex, int toIndex)
        {
            blockTextParts[arrayIndex] = inputText.Substring(fromIndex, toIndex - fromIndex); //separete huge text into smaller blocks
            blockTextParts[arrayIndex] = TextToLower(blockTextParts[arrayIndex]); //set whole block to lowercase letters
            MessageBox.Show("Show block: " + blockTextParts[arrayIndex]);
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

        private void ThreadsInitialization(string inputText)
        {
            threads = new Thread[numberOfThreads];

            MessageBox.Show($"Zero index: {0} - {blockSizesIndexes[0]}");
            threads[0] = new Thread(() => SoftTextToBlocks(inputText, 0, 0, blockSizesIndexes[0])); //Create first thread
            threads[0].Name = "ZeroThread";
            threads[0].Start();

            for (int i = 1; i < numberOfThreads; i++) //Create the rest of the threads
            {
                MessageBox.Show($"Cyklus jde s indexem: {i}");
                threads[i] = new Thread(() => SoftTextToBlocks(inputText, i, blockSizesIndexes[i-1], blockSizesIndexes[i]));
                threads[i].Name = $"ThreadNumber{i}";
                MessageBox.Show($"Current index: {blockSizesIndexes[i - 1]+1} - {blockSizesIndexes[i]}");
                threads[i].Start();
            }
        }

        //SINGLE THREAD
        public string MultiThreaded(string userInputText)
        {
            CreateBlockIndexes(userInputText);
            return "";
        }

    }
}

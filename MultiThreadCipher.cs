using System;
using System.Threading;
using System.Windows;

namespace CaesarCipher
{
    internal class MultiThreadCipher
    {
        private readonly char[] alphabet;
        private int numberOfThreads = 2, oneBlockSize, lefoverChars;
        private int[] manipulatedText;

        private Thread[] threads; //hold the number of selected threads
        private int[] blockSize; //hold the ending index of each block, first position is calculated and final values is size of each block
        private string[] blockText; //chopped main string into cumputable blocks
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
            //Array initialization
            blockSize = new int[numberOfThreads+1];
            blockText = new string[numberOfThreads];

            //set size of one block and get leftover values from division
            oneBlockSize = inputText.Length / numberOfThreads;
            lefoverChars = inputText.Length % numberOfThreads;

            for (int i = 1; i < numberOfThreads; i++)
                blockSize[i] = (i * oneBlockSize - 1);

            //set left over values to last block
            blockSize[numberOfThreads] = (numberOfThreads * oneBlockSize + lefoverChars -1);

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
        private void SoftTextToBlocks(string inputText, int arrayIndex, int fromIndex, int toIndex)
        {
            blockText[arrayIndex] = inputText.Substring(fromIndex, toIndex - fromIndex); //separete huge text into smaller blocks
            blockText[arrayIndex] = TextToLower(blockText[arrayIndex]); //set whole block to lowercase letters
            MessageBox.Show("Show block: " + blockText[arrayIndex]);
        }

        private void ThreadsInitialization(string inputText)
        {
            threads = new Thread[numberOfThreads];

            for (int i = 0; i < numberOfThreads; i++) //Create the rest of the threads
            {
                MessageBox.Show($"Cyklus jde s indexem: {i}");

                if (i == 0)
                {
                    threads[i] = new Thread(() => SoftTextToBlocks(inputText, i, blockSize[i], blockSize[i + 1]));
                    MessageBox.Show($"Current index: {blockSize[i]} - {blockSize[i + 1]}");
                }

                else
                {
                    threads[i] = new Thread(() => SoftTextToBlocks(inputText, i, blockSize[i]+1, blockSize[i + 1]));
                    MessageBox.Show($"Current index: {blockSize[i]+1} - {blockSize[i + 1]}");
                }
            }
        }

        //SINGLE THREAD
        public string MultiThreaded(string userInputText)
        {
            CreateBlockIndexes(userInputText);
            for (int i = 0; i < numberOfThreads; i++)
            {
                threads[i].Start();
            }
            return "";
        }

    }
}

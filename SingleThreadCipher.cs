using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CaesarCipher
{
    internal class SingleThreadCipher
    {
        private int[] manipulatedText;
        private readonly char[] alphabet;
        private int charShift = 0;
        public int CharShift
        {
            get { return charShift; }
            set
            {
                if (value < alphabet.Length && value > 0)
                    charShift = value;
            }
        }

        public SingleThreadCipher(char[] currentAplhabet)
        {
            alphabet = currentAplhabet;
        }

        private string TextToLower(string inputText)
        {
            try
            {
                return inputText.ToLower();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(),"Chyba!");
                return "Error";
            }
        }

        private void ConvertText(string inpuText)
        {
            manipulatedText = new int[inpuText.Length];
            
            for (int i = 0; i < inpuText.Length; i++)
            {
                for (int j = 0; j < alphabet.Length +1; j++)
                {
                    //Check letter in the text if it is supported char in the alphabet.
                    if (j == alphabet.Length)
                    {
                        MessageBox.Show($"Nepodporovaný znak: {inpuText[i]} Prosím nahraď ho.", "Chyba!");
                        Array.Clear(manipulatedText, 0, inpuText.Length);
                        return;
                    }

                    else if (inpuText[i] == alphabet[j])
                    {
                        manipulatedText[i] = ShiftLetter(j);
                        break;
                    }

                    else if (inpuText[i] == '\r') //check for new line (need to be this way because Windows is using /r/n for line termination)
                    {
                        i += 1; //No need remove chars just skip them (it's faster)
                        break;
                        //Now it's removing new line. (just easier to handle, for future this should handle the newline so it could be implemented into cipher)
                        //if (inpuText.Substring(i, 2) == "\r\n") 
                        //   inpuText = inpuText.Remove(i, 2);
                    }
                }
            }
        }

        private int ShiftLetter(int j)
        {
            if (j + charShift >= alphabet.Length)
                return j + charShift - alphabet.Length;

            else
                return j + charShift;
        }

        private string ConvertBackToChars()
        {
            string output = "";
            for (int i = 0; i < manipulatedText.Length; i++)
                output += alphabet[manipulatedText[i]];

            return output;
        }

        public string SingleThreaded(string userInputText)
        {
            ConvertText(TextToLower(userInputText));
            return ConvertBackToChars();
        }
    }
}

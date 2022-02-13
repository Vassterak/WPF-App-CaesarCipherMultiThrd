using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CaesarCipher
{
    internal class CaesarEncrytion
    {
        private int[] manipulatedText;
        private readonly char[] alphabet;
        private int charShift = 4;
        private int CharShift
        {
            get { return charShift; }
            set
            {
                if (value < alphabet.Length && value > 0)
                    charShift = value;
            }
        }

        public CaesarEncrytion(char[] currentAplhabet)
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
                return "";
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
                        manipulatedText[i] = j;
                        break;
                    }
                }
            }
        }

        private void ShiftLetters()
        {
            for (int i = 0; i < manipulatedText.Length; i++)
            {
                if (manipulatedText[i] + charShift >= alphabet.Length)
                    manipulatedText[i] = manipulatedText[i] + charShift - alphabet.Length;

                else
                    manipulatedText[i] += charShift;
            }
        }

        private string ConvertBackToChars()
        {
            string output = "";
            for (int i = 0; i < manipulatedText.Length; i++)
            {
                output += alphabet[manipulatedText[i]];
            }
            return output;
        }

        public string SingleThreaded(string userInputText)
        {
            ConvertText(TextToLower(userInputText));
            ShiftLetters();

            //string output = "";
            //foreach (var item in manipulatedText)
            //{
            //    output += item.ToString() + " ";
            //}
            //output += "délka: " +  alphabet.Length;
            //output += "znak: -" + alphabet[47];
            //return output;

            return ConvertBackToChars();
            //return TextToLower(userInputText);
        }
    }
}

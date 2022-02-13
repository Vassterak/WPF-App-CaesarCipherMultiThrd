using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CaesarCipher
{
    internal class CaesarEncrytion
    {

        public CaesarEncrytion()
        {

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

        public string SingleThreaded(string userInputText)
        {
            return TextToLower(userInputText);
        }
    }
}

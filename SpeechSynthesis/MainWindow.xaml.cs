using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeechSynthesis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Logic logic = new Logic();
            logic.Text = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd).Text.ToLower();
            
            String a = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";
            Char[] ch = a.ToCharArray();
            String b = null;
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i]!=' ') 
                b += "\"" + ch[i].ToString() + "\", ";
            }

            Paragraph outPutString = new Paragraph();
            outPutString.Inlines.Add(b.ToLower());
            richTextBox1.Document.Blocks.Add(outPutString);

            logic.FillDictionary();

            logic.Dictionary();
            logic.GetSyllables();
            logic.CreateSpeach();
            logic.SpeachText();

            //MessageBox.Show(logic.Text);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}

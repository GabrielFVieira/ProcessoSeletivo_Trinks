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
using System.Text.RegularExpressions;

namespace Questao2_Trinks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int coorX, coorY;
        private char[] direction = {'N', 'S', 'L', 'O'};
        private bool moveStarted;

        public MainWindow()
        {
            InitializeComponent();

            moveStarted = false;
            InitializeDirections();
        }

        private int StartPosX
        {
            get
            {
                return ReturnIntFromText(txtInitialPosX.Text);
            }
        }

        private int StartPosY
        {
            get
            {
                return ReturnIntFromText(txtInitialPosY.Text);
            }
        }

        private int NumSteps
        {
            get
            {
                return ReturnIntFromText(txtNumSteps.Text);
            }
        }

        public void CalcPosition(object sender, RoutedEventArgs e)
        {
            MovePosition();
        }

        private void MovePosition()
        {
            if (!ValidateCalc())
                return;

            if(!moveStarted)
                ChangeState(true);

            switch (cboDirection.SelectedValue)
            {
                case 'N':
                    coorY += NumSteps;
                    break;
                case 'S':
                    coorY -= NumSteps;
                    break;
                case 'L':
                    coorX += NumSteps;
                    break;
                case 'O':
                    coorX -= NumSteps;
                    break;
                default:
                    ShowErrorAlert("Direção invalida");
                    break;
            }

            lvHistoric.Items.Add("(" + cboDirection.SelectedValue + ", " + NumSteps + ") " + MountCoordinates(coorX, coorY));

            UpdateInterfaceText();
        }

        private bool ValidateCalc()
        {
            if (string.IsNullOrEmpty(txtInitialPosX.Text) || string.IsNullOrEmpty(txtInitialPosY.Text))
            {
                ShowErrorAlert("Por favor preencha a posição inicial");
                return false;
            }
            else if (cboDirection.SelectedIndex < 0)
            {
                ShowErrorAlert("Por favor seleciona a direção");
                return false;
            }
            else if (string.IsNullOrEmpty(txtNumSteps.Text))
            {
                ShowErrorAlert("Por favor preencha o número de casas a se andar");
                return false;
            }

            return true;
        }

        private void InitializeDirections()
        {
            foreach (char c in direction)
                cboDirection.Items.Add(c);
        }

        private void ChangeState(bool active)
        {
            if (active)
            {
                coorX = StartPosX;
                coorY = StartPosY;
            }
            else
            {
                coorX = 0;
                coorY = 0;
                lvHistoric.Items.Clear();
            }
            moveStarted = active;
            txtInitialPosX.IsReadOnly = active;
            txtInitialPosY.IsReadOnly = active;
        }

        public void Reset(object sender, RoutedEventArgs e)
        {
            ChangeState(false);
        }

        private string MountCoordinates(int x, int y)
        {
            return "[" + coorX.ToString() + ", " + coorY.ToString() + "]";
        }

        private void UpdateInterfaceText()
        {
            txtActualPos.Text = MountCoordinates(coorX, coorY);
            txtNumSteps.Text = string.Empty;
        }

        private int ReturnIntFromText(string text)
        {
            string textOriginal = text;
            string textChanged = Regex.Replace(textOriginal, " ", string.Empty);
            if (textChanged.ToLower().Contains('-'))
                textChanged = "-" + Regex.Replace(textChanged, "-", string.Empty);
            return Convert.ToInt32(textChanged);
        }

        private void ShowAlert(string message, string title)
        {
            MessageBox.Show(message, title);
        }

        private void ShowErrorAlert(string message)
        {
            MessageBox.Show(message, "Error");
        }

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

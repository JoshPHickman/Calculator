using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private string numKeyPressed;
        private string operatorKeyPressed;
        private string fullNum;         //The entire number being entered rather than the individual digit (clears when operators are pressed)
        private string equationElem;
        List<string> equationList = new List<string>();
        private bool firstOperator = true;

        private void numButton_Click(object sender, EventArgs e) {          //Number Sequence
            numKeyPressed = (sender as Button).Text;

            fullNum += numKeyPressed;
        }

        private void operatorButton_Click(object sender, EventArgs e) {     //Operator Sequence
            operatorKeyPressed = (sender as Button).Text;

            if (operatorKeyPressed == "=") {
                equationElem += fullNum;
                equationList.Add(equationElem);

                //Solver('*', equationList);            //**NB** The storing of components seems to work well but the solver is inconsistent and wont deal with a one element array probably because of a for loop
                //Solver('/', equationList);
                Solver('+', equationList);
                //Solver('-', equationList);

                equationElem = string.Empty;
                fullNum = string.Empty;
                firstOperator = true;
                numKeyPressed = string.Empty;

            } else if (firstOperator) {
                firstOperator = false;
                equationElem = fullNum + operatorKeyPressed;

                fullNum = string.Empty;
            } else {
                equationElem += fullNum;
                equationList.Add(equationElem);

                equationElem = fullNum + operatorKeyPressed;     //Last number & second operator of previous equation must be first of the next ie. {2+3*4-5} == {2+3; 3*4; 4-5}
                fullNum = string.Empty;
            }
        }

        private void Solver(char operation, List<string> equationListParam) {
            string result = "";

            for (int i = 0; i < equationListParam.Count; i++) {
                if (equationListParam[i].Contains(operation)) {
                    string[] equationSplit = equationListParam[i].Split(operation);
                    switch (operation) {
                        case '*':
                            result = (double.Parse(equationSplit[0]) * double.Parse(equationSplit[1])).ToString();  //TODO handle double operators (prolly by replacing old operators with new operators until a new number is entered)
                            break;
                        case '/':
                            result = (double.Parse(equationSplit[0]) / double.Parse(equationSplit[1])).ToString();
                            break;
                        case '+':
                            result = (double.Parse(equationSplit[0]) + double.Parse(equationSplit[1])).ToString();
                            break;
                        case '-':
                            result = (double.Parse(equationSplit[0]) - double.Parse(equationSplit[1])).ToString();
                            break;
                    }

                    if (equationListParam.Count == 1) {
                        equationListParam[0].Replace(equationListParam[0], result);
                    }

                    if (i != equationListParam.Count - 1) {
                        equationListParam[i + 1] = equationListParam[i + 1].Replace(equationSplit[1], result);
                    }

                    if (i != 0) {
                        equationListParam[i - 1] = equationListParam[i - 1].Replace(equationSplit[0], result);
                    }

                    equationListParam.RemoveAt(i);
                }
            }
        }

        private void clearButton_Click(object sender, EventArgs e) {        //Clear
            equationList.Clear();
            //equationList = new List<string>();
            mainDisplayLabel.Text = "0";
            fullNum = string.Empty;
            equationElem = string.Empty;
        }

        private void printClicked(object sender, EventArgs e) {

            numKeyPressed = string.Empty;
            equationElem += fullNum;
            //equationList.Add(equationElem);
            //Test Start
            for (int i = 0; i < equationList.Count; i++) {
                mainDisplayLabel.Text += equationList[i]; //NOTE this test will always display a 0 alongside the first number because we are concatting to display
            }
            //END
            listCountLabel.Text = equationList.Count.ToString();
        }
    }
}

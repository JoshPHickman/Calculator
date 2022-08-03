using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/**
 *Author:           Josh Hickman 
 *Date:             2022-08-02
 *Description:      Accepts user input for numbers and operators which are used to create
 *                  & solve mathematical equations.
**/

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
        List<int> solvedIndeces = new List<int>();
        string result = "";

        private void numButton_Click(object sender, EventArgs e) {          //Number Pressed
            numKeyPressed = (sender as Button).Text;

            fullNum += numKeyPressed;
        }

        private void operatorButton_Click(object sender, EventArgs e) {     //Operator Pressed
            operatorKeyPressed = (sender as Button).Text;

            if (operatorKeyPressed == "=") {
                equationElem += fullNum;
                equationList.Add(equationElem);

                //Call Solver Function
                Solver(equationList, '*', '/');
                for (int i = solvedIndeces.Count - 1; i > -1; i--) {
                    equationList.RemoveAt(solvedIndeces[i]);    //TODO turn this into a cleanup function
                }
                solvedIndeces.Clear();

                Solver(equationList, '+', '-');
                for (int i = solvedIndeces.Count - 1; i > -1; i--) {
                    equationList.RemoveAt(solvedIndeces[i]);    //TODO turn this into a cleanup function
                }
                solvedIndeces.Clear();

                mainDisplayLabel.Text = result;

                //Reset appropriate values for next equation
                numKeyPressed = string.Empty;
                fullNum = string.Empty;
                equationElem = string.Empty;
                firstOperator = true;

            } else if (firstOperator) {
                firstOperator = false;
                equationElem = fullNum + operatorKeyPressed;

                fullNum = string.Empty;

            } else {
                equationElem += fullNum;
                equationList.Add(equationElem);

                equationElem = fullNum + operatorKeyPressed;     //Last number & second operator of previous equation component must be first of the next ie. {2+3*4-5} == {2+3; 3*4; 4-5}
                fullNum = string.Empty;
            }
        }

        private void clearButton_Click(object sender, EventArgs e) {
            equationList.Clear();
            mainDisplayLabel.Text = "0";
            fullNum = string.Empty;
            equationElem = string.Empty;
            operatorKeyPressed= string.Empty; 
            numKeyPressed= string.Empty;
        }

        private void Solver(List<string> equation, char operator1, char operator2) {

            for (int i = 0; i < equation.Count; i++) {

                if (equation[i].Contains(operator1)) {

                    solvedIndeces.Add(i);
                    string[] operands = equation[i].Split(operator1);

                    switch (operator1) {
                        case '*':
                            result = (double.Parse(operands[0]) * double.Parse(operands[1])).ToString();
                            break;
                        case '/':
                            result = (double.Parse(operands[0]) / double.Parse(operands[1])).ToString();
                            break;
                        case '+':
                            result = (double.Parse(operands[0]) + double.Parse(operands[1])).ToString();
                            break;
                        case '-':
                            result = (double.Parse(operands[0]) - double.Parse(operands[1])).ToString();
                            break;
                    }

                    if (i != equation.Count - 1) {
                        equation[i + 1] = replaceFirstOccurance(equation[i + 1], operands[1], result);
                    }

                    if (i != 0) {
                        for (int passerIndex = i - 1; passerIndex >= 0; passerIndex--) { 
                            if (!((equation[passerIndex].Contains(operator1)) || (equation[passerIndex].Contains(operator2)))) {
                                equation[passerIndex] = replaceLastOccurance(equation[passerIndex], operands[0], result);
                                break;
                            }
                        }
                    }

                } else if (equation[i].Contains(operator2)) {

                    solvedIndeces.Add(i);
                    string[] operands = equation[i].Split(operator2);

                    switch (operator2) {
                        case '*':
                            result = (double.Parse(operands[0]) * double.Parse(operands[1])).ToString();
                            break;
                        case '/':
                            result = (double.Parse(operands[0]) / double.Parse(operands[1])).ToString();
                            break;
                        case '+':
                            result = (double.Parse(operands[0]) + double.Parse(operands[1])).ToString();
                            break;
                        case '-':
                            result = (double.Parse(operands[0]) - double.Parse(operands[1])).ToString();
                            break;
                    }

                    if (i != equation.Count - 1) {
                        equation[i + 1] = replaceFirstOccurance(equation[i + 1], operands[1], result);
                    }

                    if (i != 0) {
                        for (int passerIndex = i - 1; passerIndex >= 0; passerIndex--) {
                            if (!((equation[passerIndex].Contains(operator2)) || (equation[passerIndex].Contains(operator1)))) {
                                equation[passerIndex] = replaceLastOccurance(equation[passerIndex], operands[0], result);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private string replaceFirstOccurance(string original, string operand, string result) {
            int loc = original.IndexOf(operand);
            if (loc != -1) {        //checks if it is there first
                Console.WriteLine("firstWorks");
                return original.Remove(loc, operand.Length).Insert(loc, result);
            } else { Console.WriteLine("first"); return original; }
        }

        private string replaceLastOccurance(string original, string operand, string result) {
            int loc = original.LastIndexOf(operand);
            if (loc != -1) {        //checks if it is there first
                Console.WriteLine("lastWorks");
                return original.Remove(loc, operand.Length).Insert(loc, result);
            } else { Console.WriteLine("last"); return original; }
        }
    }
}

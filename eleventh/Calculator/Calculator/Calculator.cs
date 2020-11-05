using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Calculator : Form
    {

        double firstNumber, secondNumber;
        string operand;
        double memory;

        bool clearText = false;
        bool isText = false;
        bool clearError = false;

        readonly ScienceMathLib calLib = new ScienceMathLib();
        readonly MenuStrip menuStrip = new MenuStrip();
        readonly TextBox inputTxtbox = new TextBox();
        readonly Label label = new Label();

        public Calculator()
        {
            InitializeComponent();

            // menuStrip 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(370, 28);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";

            // display menu items
            var menuSettings = ConfigurationManager.GetSection("MenuSettings") as NameValueCollection;
            var subMenuSettings = ConfigurationManager.GetSection("SubMenuSettings") as NameValueCollection;
            int menuSpacing = 0, subMenuSpacing = 0;
            foreach (var key in menuSettings.AllKeys)
            {
                string value = menuSettings[key];
                ToolStripMenuItem item = new ToolStripMenuItem
                {
                    Name = key,
                    Text = value
                };
                if (item.Text == "File")
                {
                    foreach (var subKey in subMenuSettings.AllKeys)
                    {
                        string subValue = subMenuSettings[subKey];
                        ToolStripMenuItem subItem = new ToolStripMenuItem
                        {
                            Name = subKey,
                            Text = subValue
                        };
                        if (subItem.Text == "Standard" || subItem.Text == "Scientific")
                        {
                            subItem.Size = new System.Drawing.Size(151 + subMenuSpacing, 26);
                            item.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { subItem });
                            subMenuSpacing += 50;
                            if (subItem.Text == "Standard")
                            { 
                                subItem.Click += new EventHandler(StandardToolStripMenuItem_Click);
                            }
                            else
                            {
                                subItem.Click += new EventHandler(ScientificToolStripMenuItem_Click);
                            }
                        }
                    }
                }
                else if (item.Text == "Edit")
                {
                    foreach (var subKey in subMenuSettings.AllKeys)
                    {
                        string subValue = subMenuSettings[subKey];
                        ToolStripMenuItem subItem = new ToolStripMenuItem();
                        subItem.Name = subKey;
                        subItem.Text = subValue;
                        if (subItem.Text == "Copy" || subItem.Text == "Paste")
                        {
                            subItem.Size = new System.Drawing.Size(151 + subMenuSpacing, 26);
                            item.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { subItem });
                            subMenuSpacing += 50;
                        }
                    }
                }
                menuStrip.Items.Add(item);
                item.Size = new System.Drawing.Size(44, 24);
                menuSpacing += 3;
            }

            // Display standard calculator button
            int buttonSpacing = 0;
            int count = 1, xcord = 17, ycord = 144;
            foreach (string key in ConfigurationManager.AppSettings)
            {
                Button button = new Button();
                string value = ConfigurationManager.AppSettings[key];
                button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                button.Name = key;
                if (button.Name == "dotBtn" || button.Name == "plusBtn")
                {
                    button.Location = new System.Drawing.Point((xcord + buttonSpacing + 95), ycord);
                }
                else
                {
                    button.Location = new System.Drawing.Point(xcord + buttonSpacing, ycord);
                }
                if (button.Name == "equalBtn")
                {
                    button.Size = new System.Drawing.Size(84, 130);
                }
                else if (button.Name == "zeroBtn")
                {
                    button.Size = new System.Drawing.Size(180, 60);
                }
                else
                {
                    button.Size = new System.Drawing.Size(84, 60);
                }
                button.TabIndex = 2;
                button.Text = value;
                button.UseVisualStyleBackColor = true;
                if (button.Name == "escapeBtn" || button.Name == "cBtn" || button.Name == "CeBtn")
                {
                    button.Click += new System.EventHandler(this.ClearScreen);
                }
                else if (button.Name == "oneBtn" || button.Name == "twoBtn" || button.Name == "threeBtn" || button.Name == "fourBtn" || button.Name == "fiveBtn" || button.Name == "sixBtn" || button.Name == "sevenBtn" || button.Name == "eightBtn" || button.Name == "nineBtn" || button.Name == "zeroBtn" || button.Name == "dotBtn")
                {
                    button.Click += new System.EventHandler(this.NumberBtnClick);
                }
                else
                {
                    button.Click += new System.EventHandler(this.OperatorClicked);
                }
                Controls.Add(button);
                count++;
                if (count % 6 != 0)
                {
                    buttonSpacing += 95;
                }
                else
                {
                    ycord += 70;
                    count = 1;
                    buttonSpacing = 0;
                }
            }

            // Display scientific calculator buttons
            var scientificSettings = ConfigurationManager.GetSection("ScientificSettings") as NameValueCollection;
            buttonSpacing = 0;
            count = 1;
            xcord = 17;
            ycord = 562;
            foreach (var key in scientificSettings.AllKeys)
            {
                Button button = new Button();
                string value = scientificSettings[key];
                button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                button.Name = key;
                button.TabIndex = 2;
                button.Text = value;
                button.UseVisualStyleBackColor = true;
                button.Location = new System.Drawing.Point(xcord + buttonSpacing, ycord);
                button.Size = new System.Drawing.Size(84, 60);
                button.Click += new System.EventHandler(this.OperatorClicked);
                Controls.Add(button);
                count++;
                if (count % 6 != 0)
                {
                    buttonSpacing += 95;
                }
                else
                {
                    ycord += 70;
                    count = 1;
                    buttonSpacing = 0;
                }
            }


            // Input Textbox
            this.inputTxtbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputTxtbox.Location = new System.Drawing.Point(12, 44);
            this.inputTxtbox.Multiline = true;
            this.inputTxtbox.Name = "inputTxtbox";
            this.inputTxtbox.Size = new System.Drawing.Size(349, 57);
            this.inputTxtbox.TabIndex = 1;
            this.inputTxtbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;


            // label
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(12, 47);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(0, 17);
            this.label.TabIndex = 3;


            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 454);
            this.Controls.Add(this.label);
            this.Controls.Add(this.inputTxtbox);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
        }

        public void StandardToolStripMenuItem_Click(Object sender,EventArgs e)
        {
            this.ClientSize = new System.Drawing.Size(370, 454);
        }
        public void ScientificToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            this.ClientSize = new System.Drawing.Size(370, 580);
        }

        //Number Button click
        private void NumberBtnClick(object sender, EventArgs e)
        {
            string input = ((Button)sender).Text;
            if (clearError)
            {
                label.Text = "";
                clearError = false;
            }
            if (clearText)
            {
                inputTxtbox.Text = "";
                clearText = false;
                isText = false;
            }
            //check if input is a decimal
            if (input == ".")
            {
                if (!label.Text.Contains('.') && !label.Text.Contains('E'))
                {
                    label.Text += '.';
                    inputTxtbox.Text += '.';
                }
                else if (label.Text.Contains('.'))
                {
                    inputTxtbox.Text = "Cannot add another decimal";
                    clearError = true;
                }
                else if (label.Text.Contains('E'))
                {
                    inputTxtbox.Text = "Cannot add a decimal to E";
                    clearError = true;
                }
            }
            else
            {
                label.Text += input;
                inputTxtbox.Text += input;
                isText = false;
            }
        }


        //Clear button is clicked
        public void ClearScreen(Object sender, EventArgs e)
        {
            Button clear = (Button)(sender);
            if (clear.Text == "C")
            {
                this.inputTxtbox.Text = "0";
            }
            else if (clear.Text == "CE")
            {
                this.inputTxtbox.Text = "0";
                label.Text = "";
            }
            else
            {
                this.inputTxtbox.Text = this.inputTxtbox.Text.Substring(0, inputTxtbox.Text.Length - 1);
                this.label.Text = this.label.Text.Substring(0, label.Text.Length - 1);
            }
        }

        public void Equals_Click(object sender, EventArgs e)
        {
            //check if label and operand is empty and label is not text
            if (inputTxtbox.Text != null && !isText)
            {
                secondNumber = double.Parse(inputTxtbox.Text);
                clearText = true;
                clearError = true;

                if (operand != null)
                {
                    label.Text = $"{firstNumber} {operand} {secondNumber} = ";
                }

                calLib.FNum = firstNumber;
                calLib.SNum = secondNumber;
               
                switch (operand)
                {
                    case "+":
                        calLib.Add();
                        inputTxtbox.Text = calLib.Result.ToString();
                        break;
                    case "-":
                        calLib.Sub();
                        inputTxtbox.Text = calLib.Result.ToString();
                        break;
                    case "*":
                        calLib.Mul();
                        inputTxtbox.Text = calLib.Result.ToString();
                        break;
                    case "%":
                        calLib.Mod();
                        inputTxtbox.Text = calLib.Result.ToString();
                        break;
                    case "^":
                        calLib.Power();
                        inputTxtbox.Text = calLib.Result.ToString();
                        break;
                    case "/":
                        //check if case is a divide by zero
                        if (secondNumber == 0)
                        {
                            inputTxtbox.Text = "Undefined: divide by Zero";
                            break;
                        }
                        calLib.Div();
                        inputTxtbox.Text = calLib.Result.ToString();
                        break;

                }
                operand = null;
            }
        }

        public void PlusMinus()
        {
            //check if label is empty or text
            if (inputTxtbox.Text != null && !isText)
            {
                secondNumber = double.Parse(inputTxtbox.Text) * -1;
                inputTxtbox.Text = secondNumber.ToString();
                label.Text = secondNumber.ToString();
            }
            else if (inputTxtbox.Text != null)
            {
               int index = inputTxtbox.Text.IndexOf('E');

                if ((index + 1) == inputTxtbox.Text.Length) inputTxtbox.Text += "-";
                else if (label.Text[(index + 1)] != '-')
                {
                    secondNumber = double.Parse(inputTxtbox.Text) * -1;
                    inputTxtbox.Text = secondNumber.ToString();
                }
            }
        }


        private void OperatorClicked(object sender, EventArgs e)
        {
            Button op = (Button)(sender);
            switch (op.Text.ToString())
            {
                case "+":
                    BinaryOperation("+");
                    break;
                case "-":
                    BinaryOperation("-");
                    break;
                case "*":
                    BinaryOperation("*");
                    break;
                case "/":
                    BinaryOperation("/");
                    break;
                case "%":
                    BinaryOperation("%");
                    break;
                case "√":
                    calLib.FNum = double.Parse(inputTxtbox.Text);
                    calLib.Sqrt();
                    label.Text = $"√ {inputTxtbox.Text} = ";
                    inputTxtbox.Text = calLib.Result.ToString();
                    break;
                case "±":
                    PlusMinus();
                    break;
                case "=":
                    Equals_Click(new object(), new EventArgs());
                    break;
                case "1/x":
                    calLib.FNum = double.Parse(inputTxtbox.Text);
                    calLib.OneDivByX();
                    label.Text = $"1 / {inputTxtbox.Text} = ";
                    inputTxtbox.Text = calLib.Result.ToString();
                    break;
                case "∏":
                    label.Text += $"∏";
                    calLib.Pi();
                    inputTxtbox.Text = calLib.Result.ToString();
                    break;
                case "n!":
                    calLib.FNum = double.Parse(inputTxtbox.Text);
                    calLib.Fact();
                    label.Text = $"{inputTxtbox.Text}! = ";
                    inputTxtbox.Text = calLib.Result.ToString();
                    break;
                case "x^2":
                    calLib.FNum = double.Parse(inputTxtbox.Text);
                    calLib.Sqr();
                    label.Text = $"{inputTxtbox.Text}^2 = ";
                    inputTxtbox.Text = calLib.Result.ToString();
                    break;
                case "|n|":
                    calLib.FNum = double.Parse(inputTxtbox.Text);
                    calLib.Abs();
                    label.Text = $"|{inputTxtbox.Text}| = ";
                    inputTxtbox.Text = calLib.Result.ToString();
                    break;
                case "x^y":
                    BinaryOperation("^");
                    break;
                case "Log":
                    calLib.FNum = double.Parse(inputTxtbox.Text);
                    calLib.Log();
                    label.Text = $"log({inputTxtbox.Text}) = ";
                    inputTxtbox.Text = calLib.Result.ToString();
                    break;
                case "Ln":
                    calLib.FNum = double.Parse(inputTxtbox.Text);
                    calLib.Ln();
                    label.Text = $"ln({inputTxtbox.Text}) = ";
                    inputTxtbox.Text = calLib.Result.ToString();
                    break;
                case "sinh":
                    calLib.FNum = double.Parse(inputTxtbox.Text);
                    calLib.Sine();
                    label.Text = $"sin({inputTxtbox.Text}) = ";
                    inputTxtbox.Text = calLib.Result.ToString();
                    break;
                case "cosh":
                    calLib.FNum = double.Parse(inputTxtbox.Text);
                    calLib.Cosine();
                    label.Text = $"cos({inputTxtbox.Text}) = ";
                    inputTxtbox.Text = calLib.Result.ToString();
                    break;
                case "tanh":
                    calLib.FNum = double.Parse(inputTxtbox.Text);
                    calLib.Tan();
                    label.Text = $"tan({inputTxtbox.Text}) = ";
                    inputTxtbox.Text = calLib.Result.ToString();
                    break;
                case "MC":
                    label.Text = "";
                    inputTxtbox.Text = "Memory Cleared";
                    memory = 0;
                    break;
                case "MS":
                    memory = double.Parse(inputTxtbox.Text);
                    inputTxtbox.Text = $"{memory} Stored to Memory";
                    break;
                case "MR":
                    inputTxtbox.Text = memory.ToString();
                    break;
                case "M+":
                    label.Text = $"{inputTxtbox.Text} + {memory} = ";
                    inputTxtbox.Text = (memory + double.Parse(inputTxtbox.Text)).ToString();
                    memory = double.Parse(inputTxtbox.Text);
                    break;
                case "M-":
                    label.Text = $"{inputTxtbox.Text} - {memory} = ";
                    inputTxtbox.Text = (memory - double.Parse(inputTxtbox.Text)).ToString();
                    memory = double.Parse(inputTxtbox.Text);
                    break;
            }
        }


        private void BinaryOperation(string input)
        {
            if (inputTxtbox.Text != null && !isText)
            {
                operand = input;
                firstNumber = double.Parse(inputTxtbox.Text);
                clearText = true;
                label.Text = $"{firstNumber} {operand} ";
                clearError = false;
            }
            else
            {
                inputTxtbox.Text = "Invalid Input";
                clearError = true;
            }
        }
    }
}


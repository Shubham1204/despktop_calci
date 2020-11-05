using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Calculator : Form
    {
        double firstNumber;
        string operand;
        bool clearText = false;
        bool isText = false;
        bool clearError = false;
        readonly MenuStrip menuStrip = new MenuStrip();
        readonly TextBox inputTxtbox = new TextBox();
        readonly Label label = new Label();
        readonly string resxFile = @"../../Resource.resx";

        public Calculator()
        {
            InitializeComponent();

            // menuStrip
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = Resource.menuStripName;
            this.menuStrip.Size = new System.Drawing.Size(370, 28);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = Resource.menuStripName;

            // display menu items
            var menuSettings = ConfigurationManager.GetSection(Resource.menuSettings) as NameValueCollection;
            var subMenuSettings = ConfigurationManager.GetSection(Resource.subMenuSettings) as NameValueCollection;
            string[] fileArray = { Resource.standardItem, Resource.scientificItem };
            string[] editArray = { Resource.copyItem, Resource.pasteItem };
            int menuSpacing = 0, subMenuSpacing = 0;
            using (ResXResourceReader resxReader = new ResXResourceReader(resxFile))
            {
                foreach (var key in menuSettings.AllKeys)
                {
                    string value = menuSettings[key];
                    ToolStripMenuItem item = new ToolStripMenuItem
                    {
                        Name = key,
                        Text = value
                    };
                    if (item.Text.Equals(Resource.fileItem))
                    {
                        foreach (var subKey in subMenuSettings.AllKeys)
                        {
                            string subValue = subMenuSettings[subKey];
                            ToolStripMenuItem subItem = new ToolStripMenuItem
                            {
                                Name = subKey,
                            };
                            foreach (DictionaryEntry entry in resxReader)
                            {
                                foreach (string str in fileArray)
                                {
                                    if (entry.Value.Equals(str) && subValue.Equals(entry.Key))
                                    {
                                        subItem.Text = (string)entry.Value;
                                        subItem.Size = new System.Drawing.Size(151 + subMenuSpacing, 26);
                                        item.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { subItem });
                                        subMenuSpacing += 50;
                                    }
                                }
                            }
                        }
                    }
                    else if (item.Text.Equals(Resource.editItem))
                    {
                        foreach (var subKey in subMenuSettings.AllKeys)
                        {
                            string subValue = subMenuSettings[subKey];
                            ToolStripMenuItem subItem = new ToolStripMenuItem
                            {
                                Name = subKey
                            };
                            foreach (DictionaryEntry entry in resxReader)
                            {
                                foreach (string str in editArray)
                                {
                                    if (entry.Value.Equals(str) && subValue.Equals(entry.Key))
                                    {
                                        subItem.Text = (string)entry.Value;
                                        subItem.Size = new System.Drawing.Size(151 + subMenuSpacing, 26);
                                        item.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { subItem });
                                        subMenuSpacing += 50;
                                    }
                                }
                            }
                        }
                    }
                    menuStrip.Items.Add(item);
                    item.Size = new System.Drawing.Size(44, 24);
                    menuSpacing += 3;
                }

                // inputTxtbox
                this.inputTxtbox.Font = new System.Drawing.Font(Resource.fontName, 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.inputTxtbox.Location = new System.Drawing.Point(12, 44);
                this.inputTxtbox.Multiline = true;
                this.inputTxtbox.Name = Resource.inputTxtboxName;
                this.inputTxtbox.Size = new System.Drawing.Size(349, 57);
                this.inputTxtbox.TabIndex = 1;
                this.inputTxtbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;


                // label
                this.label.AutoSize = true;
                this.label.Location = new System.Drawing.Point(12, 47);
                this.label.Name = Resource.label;
                this.label.Size = new System.Drawing.Size(0, 17);
                this.label.TabIndex = 3;

                // Display standard calculator button
                int buttonSpacing = 0;
                int count = 1, xcord = 17, ycord = 144;
                string[] clearArray = { Resource.escapeBtnName, Resource.cBtnName, Resource.CeBtnName };
                string[] digitArray = { Resource.zeroBtnName,Resource.oneBtnName,Resource.twoBtnName,Resource.threeBtnName,Resource.fourBtnName,
                    Resource.fiveBtnName,Resource.sixBtnName,Resource.sevenBtnName,Resource.eightBtnName,Resource.nineBtnName,Resource.dotBtnName };
                foreach (string key in ConfigurationManager.AppSettings)
                {
                    Button button = new Button();
                    string value = ConfigurationManager.AppSettings[key];
                    button.Font = new System.Drawing.Font(Resource.fontName, 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    button.Name = key;
                    button.Location = button.Name.Equals(Resource.dotBtnName) || button.Name.Equals(Resource.plusBtnName)
                        ? new System.Drawing.Point((xcord + buttonSpacing + 95), ycord)
                        : new System.Drawing.Point(xcord + buttonSpacing, ycord);
                    if (button.Name.Equals(Resource.equalBtnName))
                    {
                        button.Size = new System.Drawing.Size(84, 130);
                    }
                    else if (button.Name.Equals(Resource.zeroBtnName))
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

                    bool clearExist = Array.Exists(clearArray, element => element.Equals(button.Name));
                    bool digitExist = Array.Exists(digitArray, element => element.Equals(button.Name));
                    if (clearExist)
                    {
                        button.Click += new System.EventHandler(this.ClearScreen);
                    }
                    else if (digitExist)
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


                this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(370, 454);
                this.Controls.Add(this.label);
                this.Controls.Add(this.inputTxtbox);
                this.Controls.Add(this.menuStrip);
                this.MainMenuStrip = this.menuStrip;
            }
        }

        //Number button click
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
                label.Text = "";
                clearText = false;
                isText = false;
            }
            if (input.Equals(Resource.dotName))
            {
                if (!label.Text.Contains(Resource.dotName) && !label.Text.Contains(Resource.exponentName))
                {
                    label.Text += Resource.dotName;
                    inputTxtbox.Text += Resource.dotName;
                }
                else if (label.Text.Contains(Resource.dotName))
                {
                    inputTxtbox.Text = Resource.notDecimal;
                    clearError = true;
                }
                else if (label.Text.Contains(Resource.exponentName))
                {
                    inputTxtbox.Text = Resource.notExponent;
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
            if (clear.Text.Equals(Resource.clear))
            {
                this.inputTxtbox.Text = Resource.zero;
            }
            else if (clear.Text.Equals(Resource.clearEntry))
            {
                this.inputTxtbox.Text = Resource.zero;
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
        }


        //Operator button is clicked
        private void OperatorClicked(object sender, EventArgs e)
        {
            Button op = (Button)(sender);
            switch (op.Text.ToString())
            {
                case "+":
                    BinaryOperation(Resource.plus);
                    break;
                case "-":
                    BinaryOperation(Resource.minus);
                    break;
                case "*":
                    BinaryOperation(Resource.multiply);
                    break;
                case "/":
                    BinaryOperation(Resource.divide);
                    break;
                case "%":
                    BinaryOperation(Resource.modulus);
                    break;
                case "^":
                    BinaryOperation(Resource.power);
                    break;
                case "=":
                    Equals_Click(new object(), new EventArgs());
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
                inputTxtbox.Text = Resource.invalidInput;
                clearError = true;
            }
        }
    }
}


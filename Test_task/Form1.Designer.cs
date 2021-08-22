
namespace Test_task
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.CountryTextBox = new System.Windows.Forms.TextBox();
            this.DataBaseButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SortComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AscRadioButton = new System.Windows.Forms.RadioButton();
            this.DescRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.SearchButton);
            this.groupBox1.Controls.Add(this.CountryTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 54);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Поиск страны по названию";
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Location = new System.Drawing.Point(297, 23);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 1;
            this.SearchButton.Text = "Поиск";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // CountryTextBox
            // 
            this.CountryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CountryTextBox.Location = new System.Drawing.Point(7, 23);
            this.CountryTextBox.Name = "CountryTextBox";
            this.CountryTextBox.Size = new System.Drawing.Size(284, 23);
            this.CountryTextBox.TabIndex = 0;
            this.CountryTextBox.PlaceholderText = "Введите название страны на английском языке";
            // 
            // DataBaseButton
            // 
            this.DataBaseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DataBaseButton.Location = new System.Drawing.Point(396, 20);
            this.DataBaseButton.Name = "DataBaseButton";
            this.DataBaseButton.Size = new System.Drawing.Size(112, 47);
            this.DataBaseButton.TabIndex = 1;
            this.DataBaseButton.Text = "Вывод всех стран из базы данных";
            this.DataBaseButton.UseVisualStyleBackColor = true;
            this.DataBaseButton.Click += new System.EventHandler(this.DataBaseButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 73);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(776, 365);
            this.dataGridView1.TabIndex = 2;
            // 
            // SortComboBox
            // 
            this.SortComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SortComboBox.FormattingEnabled = true;
            this.SortComboBox.Items.AddRange(new object[] {
            "По названию страны",
            "По коду страны",
            "По названию столицы",
            "По площади страны",
            "По населению страны",
            "По названию региона"});
            this.SortComboBox.Location = new System.Drawing.Point(514, 43);
            this.SortComboBox.Name = "SortComboBox";
            this.SortComboBox.Size = new System.Drawing.Size(152, 23);
            this.SortComboBox.TabIndex = 3;
            this.SortComboBox.SelectedIndexChanged += new System.EventHandler(this.SortComboBox_SelectedIndexChanged_RadioButton_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(514, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Сортировка записей";
            // 
            // AscRadioButton
            // 
            this.AscRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AscRadioButton.AutoSize = true;
            this.AscRadioButton.Location = new System.Drawing.Point(672, 25);
            this.AscRadioButton.Name = "AscRadioButton";
            this.AscRadioButton.Size = new System.Drawing.Size(116, 19);
            this.AscRadioButton.TabIndex = 5;
            this.AscRadioButton.TabStop = true;
            this.AscRadioButton.Text = "По возрастанию";
            this.AscRadioButton.UseVisualStyleBackColor = true;
            this.AscRadioButton.CheckedChanged += new System.EventHandler(this.SortComboBox_SelectedIndexChanged_RadioButton_CheckedChanged);
            // 
            // DescRadioButton
            // 
            this.DescRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DescRadioButton.AutoSize = true;
            this.DescRadioButton.Location = new System.Drawing.Point(672, 47);
            this.DescRadioButton.Name = "DescRadioButton";
            this.DescRadioButton.Size = new System.Drawing.Size(102, 19);
            this.DescRadioButton.TabIndex = 6;
            this.DescRadioButton.TabStop = true;
            this.DescRadioButton.Text = "По убыванию";
            this.DescRadioButton.UseVisualStyleBackColor = true;
            this.DescRadioButton.CheckedChanged += new System.EventHandler(this.SortComboBox_SelectedIndexChanged_RadioButton_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DescRadioButton);
            this.Controls.Add(this.AscRadioButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SortComboBox);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.DataBaseButton);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(650, 39);
            this.Name = "Form1";
            this.Text = "Тестовое задание Быков А.М.";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox CountryTextBox { get; set; }
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button DataBaseButton;
        public System.Windows.Forms.DataGridView dataGridView1 { get; set; }
        public System.Windows.Forms.ComboBox SortComboBox { get; set; }
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.RadioButton AscRadioButton { get; set; }
        public System.Windows.Forms.RadioButton DescRadioButton { get; set; }
    }
}


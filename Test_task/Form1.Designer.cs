
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
            this.searchButton = new System.Windows.Forms.Button();
            this.countryTextBox = new System.Windows.Forms.TextBox();
            this.databaseButton = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.sortComboBox = new System.Windows.Forms.ComboBox();
            this.label = new System.Windows.Forms.Label();
            this.ascRadioButton = new System.Windows.Forms.RadioButton();
            this.descRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top 
                | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.searchButton);
            this.groupBox1.Controls.Add(this.countryTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 54);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Поиск страны по названию";
            // 
            // SearchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(297, 23);
            this.searchButton.Name = "SearchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "Поиск";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // CountryTextBox
            // 
            this.countryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top 
                | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.countryTextBox.Location = new System.Drawing.Point(7, 23);
            this.countryTextBox.Name = "CountryTextBox";
            this.countryTextBox.Size = new System.Drawing.Size(284, 23);
            this.countryTextBox.TabIndex = 0;
            this.countryTextBox.PlaceholderText = "Введите название страны на английском языке";
            // 
            // DataBaseButton
            // 
            this.databaseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.databaseButton.Location = new System.Drawing.Point(396, 20);
            this.databaseButton.Name = "DataBaseButton";
            this.databaseButton.Size = new System.Drawing.Size(112, 47);
            this.databaseButton.TabIndex = 1;
            this.databaseButton.Text = "Вывод всех стран из базы данных";
            this.databaseButton.UseVisualStyleBackColor = true;
            this.databaseButton.Click += new System.EventHandler(this.DataBaseButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top 
                | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = 
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 73);
            this.dataGridView.Name = "dataGridView1";
            this.dataGridView.RowTemplate.Height = 25;
            this.dataGridView.Size = new System.Drawing.Size(776, 365);
            this.dataGridView.TabIndex = 2;
            // 
            // SortComboBox
            // 
            this.sortComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.sortComboBox.FormattingEnabled = true;
            this.sortComboBox.Items.AddRange(new object[] {
            "По названию страны",
            "По коду страны",
            "По названию столицы",
            "По площади страны",
            "По населению страны",
            "По названию региона"});
            this.sortComboBox.Location = new System.Drawing.Point(514, 43);
            this.sortComboBox.Name = "SortComboBox";
            this.sortComboBox.Size = new System.Drawing.Size(152, 23);
            this.sortComboBox.TabIndex = 3;
            this.sortComboBox.SelectedIndexChanged += new System.EventHandler(this.SortComboBox_RadioButton_Changed);
            // 
            // label1
            // 
            this.label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(514, 25);
            this.label.Name = "label1";
            this.label.Size = new System.Drawing.Size(120, 15);
            this.label.TabIndex = 4;
            this.label.Text = "Сортировка записей";
            // 
            // AscRadioButton
            // 
            this.ascRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.ascRadioButton.AutoSize = true;
            this.ascRadioButton.Location = new System.Drawing.Point(672, 25);
            this.ascRadioButton.Name = "AscRadioButton";
            this.ascRadioButton.Size = new System.Drawing.Size(116, 19);
            this.ascRadioButton.TabIndex = 5;
            this.ascRadioButton.TabStop = true;
            this.ascRadioButton.Text = "По возрастанию";
            this.ascRadioButton.UseVisualStyleBackColor = true;
            this.ascRadioButton.CheckedChanged += new System.EventHandler(this.SortComboBox_RadioButton_Changed);
            // 
            // DescRadioButton
            // 
            this.descRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.descRadioButton.AutoSize = true;
            this.descRadioButton.Location = new System.Drawing.Point(672, 47);
            this.descRadioButton.Name = "DescRadioButton";
            this.descRadioButton.Size = new System.Drawing.Size(102, 19);
            this.descRadioButton.TabIndex = 6;
            this.descRadioButton.TabStop = true;
            this.descRadioButton.Text = "По убыванию";
            this.descRadioButton.UseVisualStyleBackColor = true;
            this.descRadioButton.CheckedChanged += new System.EventHandler(this.SortComboBox_RadioButton_Changed);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.descRadioButton);
            this.Controls.Add(this.ascRadioButton);
            this.Controls.Add(this.label);
            this.Controls.Add(this.sortComboBox);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.databaseButton);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(650, 39);
            this.Name = "Form1";
            this.Text = "Тестовое задание Быков А.М.";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox countryTextBox { get; set; }
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button databaseButton;
        public System.Windows.Forms.DataGridView dataGridView { get; set; }
        public System.Windows.Forms.ComboBox sortComboBox { get; set; }
        private System.Windows.Forms.Label label;
        public System.Windows.Forms.RadioButton ascRadioButton { get; set; }
        public System.Windows.Forms.RadioButton descRadioButton { get; set; }
    }
}


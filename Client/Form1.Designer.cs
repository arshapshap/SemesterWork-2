namespace Client
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
            this.playersList = new System.Windows.Forms.ListBox();
            this.readyButton = new System.Windows.Forms.Button();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.saveNameButton = new System.Windows.Forms.Button();
            this.startScreen = new System.Windows.Forms.GroupBox();
            this.startScreen.SuspendLayout();
            this.SuspendLayout();
            // 
            // playersList
            // 
            this.playersList.FormattingEnabled = true;
            this.playersList.ItemHeight = 20;
            this.playersList.Location = new System.Drawing.Point(0, 0);
            this.playersList.Name = "playersList";
            this.playersList.Size = new System.Drawing.Size(154, 424);
            this.playersList.TabIndex = 0;
            // 
            // readyButton
            // 
            this.readyButton.Location = new System.Drawing.Point(396, 238);
            this.readyButton.Name = "readyButton";
            this.readyButton.Size = new System.Drawing.Size(94, 29);
            this.readyButton.TabIndex = 1;
            this.readyButton.Text = "Готов";
            this.readyButton.UseVisualStyleBackColor = true;
            this.readyButton.Click += new System.EventHandler(this.readyButton_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(381, 205);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(125, 27);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.Text = "Player1";
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.ForeColor = System.Drawing.Color.Brown;
            this.errorLabel.Location = new System.Drawing.Point(277, 219);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(261, 20);
            this.errorLabel.TabIndex = 3;
            this.errorLabel.Text = "Не удалось подключиться к серверу";
            this.errorLabel.Visible = false;
            // 
            // saveNameButton
            // 
            this.saveNameButton.Enabled = false;
            this.saveNameButton.Location = new System.Drawing.Point(512, 204);
            this.saveNameButton.Name = "saveNameButton";
            this.saveNameButton.Size = new System.Drawing.Size(30, 29);
            this.saveNameButton.TabIndex = 4;
            this.saveNameButton.Text = "✓";
            this.saveNameButton.UseVisualStyleBackColor = true;
            this.saveNameButton.Visible = false;
            this.saveNameButton.Click += new System.EventHandler(this.saveNameButton_Click);
            // 
            // startScreen
            // 
            this.startScreen.Controls.Add(this.readyButton);
            this.startScreen.Controls.Add(this.nameTextBox);
            this.startScreen.Controls.Add(this.saveNameButton);
            this.startScreen.Controls.Add(this.playersList);
            this.startScreen.Location = new System.Drawing.Point(12, 4);
            this.startScreen.Name = "startScreen";
            this.startScreen.Size = new System.Drawing.Size(776, 440);
            this.startScreen.TabIndex = 5;
            this.startScreen.TabStop = false;
            this.startScreen.Text = "groupBox1";
            this.startScreen.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.startScreen);
            this.Controls.Add(this.errorLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.startScreen.ResumeLayout(false);
            this.startScreen.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox playersList;
        private Button readyButton;
        private TextBox nameTextBox;
        private Label errorLabel;
        private Button saveNameButton;
        private GroupBox startScreen;
    }
}
namespace Client
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.selectColorBox = new System.Windows.Forms.GroupBox();
            this.blueButton = new System.Windows.Forms.Button();
            this.greenButton = new System.Windows.Forms.Button();
            this.yellowButton = new System.Windows.Forms.Button();
            this.redButton = new System.Windows.Forms.Button();
            this.gameOverBox = new System.Windows.Forms.GroupBox();
            this.winnerLabel = new System.Windows.Forms.Label();
            this.playersList = new System.Windows.Forms.ListBox();
            this.readyButton = new System.Windows.Forms.Button();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.saveNameButton = new System.Windows.Forms.Button();
            this.mainGroupBox = new System.Windows.Forms.GroupBox();
            this.hintLabel = new System.Windows.Forms.Label();
            this.cardOnTablePicture = new System.Windows.Forms.PictureBox();
            this.readyGroupBox = new System.Windows.Forms.GroupBox();
            this.unoButton = new System.Windows.Forms.Button();
            this.selectedColorPicture = new System.Windows.Forms.PictureBox();
            this.yourMoveLabel = new System.Windows.Forms.Label();
            this.deckPicture = new System.Windows.Forms.PictureBox();
            this.cardsListView = new System.Windows.Forms.ListView();
            this.cardsList = new System.Windows.Forms.ImageList(this.components);
            this.rulesLabel = new System.Windows.Forms.Label();
            this.selectColorBox.SuspendLayout();
            this.gameOverBox.SuspendLayout();
            this.mainGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cardOnTablePicture)).BeginInit();
            this.readyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedColorPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deckPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // selectColorBox
            // 
            this.selectColorBox.BackColor = System.Drawing.Color.White;
            this.selectColorBox.Controls.Add(this.blueButton);
            this.selectColorBox.Controls.Add(this.greenButton);
            this.selectColorBox.Controls.Add(this.yellowButton);
            this.selectColorBox.Controls.Add(this.redButton);
            this.selectColorBox.Enabled = false;
            this.selectColorBox.Location = new System.Drawing.Point(267, 226);
            this.selectColorBox.Name = "selectColorBox";
            this.selectColorBox.Size = new System.Drawing.Size(350, 125);
            this.selectColorBox.TabIndex = 10;
            this.selectColorBox.TabStop = false;
            this.selectColorBox.Text = "Выберите цвет";
            this.selectColorBox.Visible = false;
            // 
            // blueButton
            // 
            this.blueButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(255)))));
            this.blueButton.Location = new System.Drawing.Point(264, 26);
            this.blueButton.Name = "blueButton";
            this.blueButton.Size = new System.Drawing.Size(80, 80);
            this.blueButton.TabIndex = 3;
            this.blueButton.UseVisualStyleBackColor = false;
            this.blueButton.Click += new System.EventHandler(this.blueButton_Click);
            // 
            // greenButton
            // 
            this.greenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(170)))), ((int)(((byte)(85)))));
            this.greenButton.Location = new System.Drawing.Point(178, 26);
            this.greenButton.Name = "greenButton";
            this.greenButton.Size = new System.Drawing.Size(80, 80);
            this.greenButton.TabIndex = 2;
            this.greenButton.UseVisualStyleBackColor = false;
            this.greenButton.Click += new System.EventHandler(this.greenButton_Click);
            // 
            // yellowButton
            // 
            this.yellowButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.yellowButton.Location = new System.Drawing.Point(92, 26);
            this.yellowButton.Name = "yellowButton";
            this.yellowButton.Size = new System.Drawing.Size(80, 80);
            this.yellowButton.TabIndex = 1;
            this.yellowButton.UseVisualStyleBackColor = false;
            this.yellowButton.Click += new System.EventHandler(this.yellowButton_Click);
            // 
            // redButton
            // 
            this.redButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.redButton.Location = new System.Drawing.Point(6, 26);
            this.redButton.Name = "redButton";
            this.redButton.Size = new System.Drawing.Size(80, 80);
            this.redButton.TabIndex = 0;
            this.redButton.UseVisualStyleBackColor = false;
            this.redButton.Click += new System.EventHandler(this.redButton_Click);
            // 
            // gameOverBox
            // 
            this.gameOverBox.BackColor = System.Drawing.Color.White;
            this.gameOverBox.Controls.Add(this.winnerLabel);
            this.gameOverBox.Location = new System.Drawing.Point(267, 226);
            this.gameOverBox.Name = "gameOverBox";
            this.gameOverBox.Size = new System.Drawing.Size(350, 125);
            this.gameOverBox.TabIndex = 10;
            this.gameOverBox.TabStop = false;
            this.gameOverBox.Text = "Игра завершена!";
            this.gameOverBox.Visible = false;
            // 
            // winnerLabel
            // 
            this.winnerLabel.AutoSize = true;
            this.winnerLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.winnerLabel.Location = new System.Drawing.Point(34, 49);
            this.winnerLabel.Name = "winnerLabel";
            this.winnerLabel.Size = new System.Drawing.Size(293, 41);
            this.winnerLabel.TabIndex = 11;
            this.winnerLabel.Text = "Победитель: Player0";
            this.winnerLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // playersList
            // 
            this.playersList.FormattingEnabled = true;
            this.playersList.ItemHeight = 20;
            this.playersList.Location = new System.Drawing.Point(0, 20);
            this.playersList.Name = "playersList";
            this.playersList.Size = new System.Drawing.Size(154, 244);
            this.playersList.TabIndex = 0;
            this.playersList.DoubleClick += new System.EventHandler(this.playersList_DoubleClick);
            // 
            // readyButton
            // 
            this.readyButton.Location = new System.Drawing.Point(79, 69);
            this.readyButton.Name = "readyButton";
            this.readyButton.Size = new System.Drawing.Size(94, 29);
            this.readyButton.TabIndex = 1;
            this.readyButton.Text = "Готов";
            this.readyButton.UseVisualStyleBackColor = true;
            this.readyButton.Click += new System.EventHandler(this.readyButton_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(62, 36);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ReadOnly = true;
            this.nameTextBox.Size = new System.Drawing.Size(125, 27);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.Text = "Player";
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.ForeColor = System.Drawing.Color.Brown;
            this.errorLabel.Location = new System.Drawing.Point(264, 205);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(261, 20);
            this.errorLabel.TabIndex = 3;
            this.errorLabel.Text = "Не удалось подключиться к серверу";
            this.errorLabel.Visible = false;
            // 
            // saveNameButton
            // 
            this.saveNameButton.Enabled = false;
            this.saveNameButton.Location = new System.Drawing.Point(193, 35);
            this.saveNameButton.Name = "saveNameButton";
            this.saveNameButton.Size = new System.Drawing.Size(30, 29);
            this.saveNameButton.TabIndex = 4;
            this.saveNameButton.Text = "✓";
            this.saveNameButton.UseVisualStyleBackColor = true;
            this.saveNameButton.Visible = false;
            this.saveNameButton.Click += new System.EventHandler(this.saveNameButton_Click);
            // 
            // mainGroupBox
            // 
            this.mainGroupBox.Controls.Add(this.hintLabel);
            this.mainGroupBox.Controls.Add(this.cardOnTablePicture);
            this.mainGroupBox.Controls.Add(this.readyGroupBox);
            this.mainGroupBox.Controls.Add(this.unoButton);
            this.mainGroupBox.Controls.Add(this.selectedColorPicture);
            this.mainGroupBox.Controls.Add(this.yourMoveLabel);
            this.mainGroupBox.Controls.Add(this.deckPicture);
            this.mainGroupBox.Controls.Add(this.cardsListView);
            this.mainGroupBox.Controls.Add(this.playersList);
            this.mainGroupBox.Controls.Add(this.rulesLabel);
            this.mainGroupBox.Location = new System.Drawing.Point(12, 2);
            this.mainGroupBox.Name = "mainGroupBox";
            this.mainGroupBox.Size = new System.Drawing.Size(750, 549);
            this.mainGroupBox.TabIndex = 5;
            this.mainGroupBox.TabStop = false;
            this.mainGroupBox.Visible = false;
            // 
            // hintLabel
            // 
            this.hintLabel.AutoSize = true;
            this.hintLabel.Location = new System.Drawing.Point(480, 223);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(260, 40);
            this.hintLabel.TabIndex = 13;
            this.hintLabel.Text = "Карта \"Пропуск хода\":\r\nВы взяли 2 карты и пропустили ход.\r\n";
            this.hintLabel.Visible = false;
            // 
            // cardOnTablePicture
            // 
            this.cardOnTablePicture.Location = new System.Drawing.Point(302, 20);
            this.cardOnTablePicture.Name = "cardOnTablePicture";
            this.cardOnTablePicture.Size = new System.Drawing.Size(144, 216);
            this.cardOnTablePicture.TabIndex = 7;
            this.cardOnTablePicture.TabStop = false;
            this.cardOnTablePicture.Visible = false;
            // 
            // readyGroupBox
            // 
            this.readyGroupBox.Controls.Add(this.nameTextBox);
            this.readyGroupBox.Controls.Add(this.readyButton);
            this.readyGroupBox.Controls.Add(this.saveNameButton);
            this.readyGroupBox.Location = new System.Drawing.Point(267, 226);
            this.readyGroupBox.Name = "readyGroupBox";
            this.readyGroupBox.Size = new System.Drawing.Size(250, 125);
            this.readyGroupBox.TabIndex = 5;
            this.readyGroupBox.TabStop = false;
            // 
            // unoButton
            // 
            this.unoButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.unoButton.Location = new System.Drawing.Point(161, 164);
            this.unoButton.Name = "unoButton";
            this.unoButton.Size = new System.Drawing.Size(100, 100);
            this.unoButton.TabIndex = 11;
            this.unoButton.Text = "УНО!";
            this.unoButton.UseVisualStyleBackColor = true;
            this.unoButton.Visible = false;
            this.unoButton.Click += new System.EventHandler(this.unoButton_Click);
            // 
            // selectedColorPicture
            // 
            this.selectedColorPicture.BackColor = System.Drawing.Color.Red;
            this.selectedColorPicture.Location = new System.Drawing.Point(480, 164);
            this.selectedColorPicture.Name = "selectedColorPicture";
            this.selectedColorPicture.Size = new System.Drawing.Size(50, 50);
            this.selectedColorPicture.TabIndex = 10;
            this.selectedColorPicture.TabStop = false;
            this.selectedColorPicture.Visible = false;
            // 
            // yourMoveLabel
            // 
            this.yourMoveLabel.AutoSize = true;
            this.yourMoveLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.yourMoveLabel.Location = new System.Drawing.Point(160, 20);
            this.yourMoveLabel.Name = "yourMoveLabel";
            this.yourMoveLabel.Size = new System.Drawing.Size(139, 41);
            this.yourMoveLabel.TabIndex = 9;
            this.yourMoveLabel.Text = "Ваш ход!";
            this.yourMoveLabel.Visible = false;
            // 
            // deckPicture
            // 
            this.deckPicture.Cursor = System.Windows.Forms.Cursors.No;
            this.deckPicture.Image = ((System.Drawing.Image)(resources.GetObject("deckPicture.Image")));
            this.deckPicture.Location = new System.Drawing.Point(585, 53);
            this.deckPicture.Name = "deckPicture";
            this.deckPicture.Size = new System.Drawing.Size(164, 144);
            this.deckPicture.TabIndex = 8;
            this.deckPicture.TabStop = false;
            this.deckPicture.Visible = false;
            this.deckPicture.Click += new System.EventHandler(this.deckPicture_Click);
            // 
            // cardsListView
            // 
            this.cardsListView.LargeImageList = this.cardsList;
            this.cardsListView.Location = new System.Drawing.Point(0, 270);
            this.cardsListView.Margin = new System.Windows.Forms.Padding(0);
            this.cardsListView.MultiSelect = false;
            this.cardsListView.Name = "cardsListView";
            this.cardsListView.Size = new System.Drawing.Size(749, 279);
            this.cardsListView.TabIndex = 6;
            this.cardsListView.UseCompatibleStateImageBehavior = false;
            this.cardsListView.Visible = false;
            this.cardsListView.DoubleClick += new System.EventHandler(this.cardsListView_DoubleClick);
            // 
            // cardsList
            // 
            this.cardsList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.cardsList.ImageSize = new System.Drawing.Size(64, 96);
            this.cardsList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // rulesLabel
            // 
            this.rulesLabel.AutoSize = true;
            this.rulesLabel.Location = new System.Drawing.Point(160, 20);
            this.rulesLabel.Name = "rulesLabel";
            this.rulesLabel.Size = new System.Drawing.Size(477, 60);
            this.rulesLabel.TabIndex = 12;
            this.rulesLabel.Text = "Подсказка:\r\nЕсли Вы заметили игрока с 1 картой, не сказавшего \"УНО!\",\r\nдважды кли" +
    "кните по его имени в списке, чтобы сообщить об этом.\r\n";
            this.rulesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 563);
            this.Controls.Add(this.mainGroupBox);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.selectColorBox);
            this.Controls.Add(this.gameOverBox);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Click += new System.EventHandler(this.main_Click);
            this.selectColorBox.ResumeLayout(false);
            this.gameOverBox.ResumeLayout(false);
            this.gameOverBox.PerformLayout();
            this.mainGroupBox.ResumeLayout(false);
            this.mainGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cardOnTablePicture)).EndInit();
            this.readyGroupBox.ResumeLayout(false);
            this.readyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedColorPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deckPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private static Color RedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
        private static Color YellowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
        private static Color GreenColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(170)))), ((int)(((byte)(85)))));
        private static Color BlueColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(255)))));
        private ListBox playersList;
        private Button readyButton;
        private TextBox nameTextBox;
        private Label errorLabel;
        private Button saveNameButton;
        private GroupBox mainGroupBox;
        private GroupBox readyGroupBox;
        private ListView cardsListView;
        private ImageList cardsList;
        private PictureBox cardOnTablePicture;
        private PictureBox deckPicture;
        private Label yourMoveLabel;
        private GroupBox selectColorBox;
        private GroupBox gameOverBox;
        private Button yellowButton;
        private Button redButton;
        private Button blueButton;
        private Button greenButton;
        private PictureBox selectedColorPicture;
        private Label winnerLabel;
        private Button unoButton;
        private Label rulesLabel;
        private Label hintLabel;
    }
}
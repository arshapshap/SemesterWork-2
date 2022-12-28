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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.playersList = new System.Windows.Forms.ListBox();
            this.readyButton = new System.Windows.Forms.Button();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.saveNameButton = new System.Windows.Forms.Button();
            this.startScreen = new System.Windows.Forms.GroupBox();
            this.lastCardPicture = new System.Windows.Forms.PictureBox();
            this.cardsListView = new System.Windows.Forms.ListView();
            this.cardsList = new System.Windows.Forms.ImageList(this.components);
            this.readyGroup = new System.Windows.Forms.GroupBox();
            this.deckPicture = new System.Windows.Forms.PictureBox();
            this.startScreen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lastCardPicture)).BeginInit();
            this.readyGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deckPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // playersList
            // 
            this.playersList.FormattingEnabled = true;
            this.playersList.ItemHeight = 20;
            this.playersList.Location = new System.Drawing.Point(0, 20);
            this.playersList.Name = "playersList";
            this.playersList.Size = new System.Drawing.Size(154, 384);
            this.playersList.TabIndex = 0;
            // 
            // readyButton
            // 
            this.readyButton.Location = new System.Drawing.Point(67, 71);
            this.readyButton.Name = "readyButton";
            this.readyButton.Size = new System.Drawing.Size(94, 29);
            this.readyButton.TabIndex = 1;
            this.readyButton.Text = "Готов";
            this.readyButton.UseVisualStyleBackColor = true;
            this.readyButton.Click += new System.EventHandler(this.readyButton_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(50, 38);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(125, 27);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.Text = "Player";
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.ForeColor = System.Drawing.Color.Brown;
            this.errorLabel.Location = new System.Drawing.Point(392, 345);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(261, 20);
            this.errorLabel.TabIndex = 3;
            this.errorLabel.Text = "Не удалось подключиться к серверу";
            this.errorLabel.Visible = false;
            // 
            // saveNameButton
            // 
            this.saveNameButton.Enabled = false;
            this.saveNameButton.Location = new System.Drawing.Point(181, 37);
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
            this.startScreen.Controls.Add(this.deckPicture);
            this.startScreen.Controls.Add(this.lastCardPicture);
            this.startScreen.Controls.Add(this.cardsListView);
            this.startScreen.Controls.Add(this.readyGroup);
            this.startScreen.Controls.Add(this.playersList);
            this.startScreen.Location = new System.Drawing.Point(12, 2);
            this.startScreen.Name = "startScreen";
            this.startScreen.Size = new System.Drawing.Size(982, 707);
            this.startScreen.TabIndex = 5;
            this.startScreen.TabStop = false;
            this.startScreen.Visible = false;
            // 
            // lastCardPicture
            // 
            this.lastCardPicture.Location = new System.Drawing.Point(441, 115);
            this.lastCardPicture.Name = "lastCardPicture";
            this.lastCardPicture.Size = new System.Drawing.Size(144, 216);
            this.lastCardPicture.TabIndex = 7;
            this.lastCardPicture.TabStop = false;
            this.lastCardPicture.Visible = false;
            // 
            // cardsListView
            // 
            this.cardsListView.LargeImageList = this.cardsList;
            this.cardsListView.Location = new System.Drawing.Point(0, 422);
            this.cardsListView.Margin = new System.Windows.Forms.Padding(0);
            this.cardsListView.MultiSelect = false;
            this.cardsListView.Name = "cardsListView";
            this.cardsListView.Size = new System.Drawing.Size(982, 279);
            this.cardsListView.TabIndex = 6;
            this.cardsListView.UseCompatibleStateImageBehavior = false;
            this.cardsListView.Visible = false;
            // 
            // cardsList
            // 
            this.cardsList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.cardsList.ImageSize = new System.Drawing.Size(64, 96);
            this.cardsList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // readyGroup
            // 
            this.readyGroup.Controls.Add(this.nameTextBox);
            this.readyGroup.Controls.Add(this.readyButton);
            this.readyGroup.Controls.Add(this.saveNameButton);
            this.readyGroup.Location = new System.Drawing.Point(391, 279);
            this.readyGroup.Name = "readyGroup";
            this.readyGroup.Size = new System.Drawing.Size(250, 125);
            this.readyGroup.TabIndex = 5;
            this.readyGroup.TabStop = false;
            // 
            // deckPicture
            // 
            this.deckPicture.Image = ((System.Drawing.Image)(resources.GetObject("deckPicture.Image")));
            this.deckPicture.Location = new System.Drawing.Point(800, 115);
            this.deckPicture.Name = "deckPicture";
            this.deckPicture.Size = new System.Drawing.Size(144, 216);
            this.deckPicture.TabIndex = 8;
            this.deckPicture.TabStop = false;
            this.deckPicture.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 721);
            this.Controls.Add(this.startScreen);
            this.Controls.Add(this.errorLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.startScreen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lastCardPicture)).EndInit();
            this.readyGroup.ResumeLayout(false);
            this.readyGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deckPicture)).EndInit();
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
        private GroupBox readyGroup;
        private ListView cardsListView;
        private ImageList cardsList;
        private PictureBox lastCardPicture;
        private PictureBox deckPicture;
    }
}
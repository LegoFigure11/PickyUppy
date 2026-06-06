namespace PickyUppy.WinForms;

    partial class MainWindow
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
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
        GB_Connection = new GroupBox();
        B_CopyToInitial = new Button();
        label1 = new Label();
        TB_CurrentSeed1 = new TextBox();
        TB_AdvancesIncrease = new TextBox();
        TB_Status = new TextBox();
        L_CurrentSeed = new Label();
        L_Status = new Label();
        TB_CurrentSeed0 = new TextBox();
        TB_CurrentAdvances = new TextBox();
        L_CurrentAdvances = new Label();
        B_Disconnect = new Button();
        B_Connect = new Button();
        L_SwitchIP = new Label();
        TB_SwitchIP = new TextBox();
        GB_Seed = new GroupBox();
        L_InitialSeed1 = new Label();
        TB_InitialSeed1 = new TextBox();
        L_InitialSeed0 = new Label();
        TB_InitialSeed0 = new TextBox();
        DGV_Results = new DataGridView();
        BS_Results = new BindingSource(components);
        B_Search = new Button();
        TB_Advances = new TextBox();
        TB_Initial = new TextBox();
        CB_FiltersEnabled = new CheckBox();
        CB_Location = new ComboBox();
        CB_ItemTable = new ComboBox();
        CB_CandyTable = new ComboBox();
        CB_TargetItem = new ComboBox();
        NUD_Quantity = new NumericUpDown();
        L_Location = new Label();
        L_ItemTable = new Label();
        L_TargetItem = new Label();
        L_Quantity = new Label();
        L_CandyTable = new Label();
        L_Initial = new Label();
        label2 = new Label();
        GB_Connection.SuspendLayout();
        GB_Seed.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DGV_Results).BeginInit();
        ((System.ComponentModel.ISupportInitialize)BS_Results).BeginInit();
        ((System.ComponentModel.ISupportInitialize)NUD_Quantity).BeginInit();
        SuspendLayout();
        // 
        // GB_Connection
        // 
        GB_Connection.Controls.Add(B_CopyToInitial);
        GB_Connection.Controls.Add(label1);
        GB_Connection.Controls.Add(TB_CurrentSeed1);
        GB_Connection.Controls.Add(TB_AdvancesIncrease);
        GB_Connection.Controls.Add(TB_Status);
        GB_Connection.Controls.Add(L_CurrentSeed);
        GB_Connection.Controls.Add(L_Status);
        GB_Connection.Controls.Add(TB_CurrentSeed0);
        GB_Connection.Controls.Add(TB_CurrentAdvances);
        GB_Connection.Controls.Add(L_CurrentAdvances);
        GB_Connection.Controls.Add(B_Disconnect);
        GB_Connection.Controls.Add(B_Connect);
        GB_Connection.Controls.Add(L_SwitchIP);
        GB_Connection.Controls.Add(TB_SwitchIP);
        GB_Connection.Location = new Point(0, 40);
        GB_Connection.Margin = new Padding(3, 0, 3, 3);
        GB_Connection.Name = "GB_Connection";
        GB_Connection.RightToLeft = RightToLeft.No;
        GB_Connection.Size = new Size(212, 187);
        GB_Connection.TabIndex = 1;
        GB_Connection.TabStop = false;
        // 
        // B_CopyToInitial
        // 
        B_CopyToInitial.Enabled = false;
        B_CopyToInitial.Location = new Point(11, 154);
        B_CopyToInitial.Name = "B_CopyToInitial";
        B_CopyToInitial.Size = new Size(195, 25);
        B_CopyToInitial.TabIndex = 7;
        B_CopyToInitial.Text = "Update RNG States";
        B_CopyToInitial.UseVisualStyleBackColor = true;
        B_CopyToInitial.Click += B_CopyToInitial_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(11, 132);
        label1.Name = "label1";
        label1.Size = new Size(67, 15);
        label1.TabIndex = 23;
        label1.Text = "Current [1]:";
        // 
        // TB_CurrentSeed1
        // 
        TB_CurrentSeed1.CharacterCasing = CharacterCasing.Upper;
        TB_CurrentSeed1.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TB_CurrentSeed1.Location = new Point(88, 130);
        TB_CurrentSeed1.MaxLength = 16;
        TB_CurrentSeed1.Name = "TB_CurrentSeed1";
        TB_CurrentSeed1.ReadOnly = true;
        TB_CurrentSeed1.Size = new Size(118, 22);
        TB_CurrentSeed1.TabIndex = 6;
        TB_CurrentSeed1.TabStop = false;
        TB_CurrentSeed1.Text = "0123456789ABCDEF";
        // 
        // TB_AdvancesIncrease
        // 
        TB_AdvancesIncrease.CharacterCasing = CharacterCasing.Lower;
        TB_AdvancesIncrease.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TB_AdvancesIncrease.Location = new Point(151, 82);
        TB_AdvancesIncrease.MaxLength = 15;
        TB_AdvancesIncrease.Name = "TB_AdvancesIncrease";
        TB_AdvancesIncrease.ReadOnly = true;
        TB_AdvancesIncrease.Size = new Size(55, 22);
        TB_AdvancesIncrease.TabIndex = 4;
        TB_AdvancesIncrease.TabStop = false;
        TB_AdvancesIncrease.Text = "123,456";
        TB_AdvancesIncrease.TextAlign = HorizontalAlignment.Right;
        // 
        // TB_Status
        // 
        TB_Status.BackColor = SystemColors.Control;
        TB_Status.BorderStyle = BorderStyle.None;
        TB_Status.Location = new Point(74, 64);
        TB_Status.Name = "TB_Status";
        TB_Status.ReadOnly = true;
        TB_Status.RightToLeft = RightToLeft.No;
        TB_Status.Size = new Size(132, 16);
        TB_Status.TabIndex = 18;
        TB_Status.TabStop = false;
        TB_Status.Text = "wwwwwwwwwwwwww";
        TB_Status.TextAlign = HorizontalAlignment.Right;
        // 
        // L_CurrentSeed
        // 
        L_CurrentSeed.AutoSize = true;
        L_CurrentSeed.Location = new Point(11, 108);
        L_CurrentSeed.Name = "L_CurrentSeed";
        L_CurrentSeed.Size = new Size(67, 15);
        L_CurrentSeed.TabIndex = 10;
        L_CurrentSeed.Text = "Current [0]:";
        // 
        // L_Status
        // 
        L_Status.AutoSize = true;
        L_Status.Location = new Point(11, 64);
        L_Status.Name = "L_Status";
        L_Status.Size = new Size(42, 15);
        L_Status.TabIndex = 17;
        L_Status.Text = "Status:";
        // 
        // TB_CurrentSeed0
        // 
        TB_CurrentSeed0.CharacterCasing = CharacterCasing.Upper;
        TB_CurrentSeed0.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TB_CurrentSeed0.Location = new Point(88, 106);
        TB_CurrentSeed0.MaxLength = 16;
        TB_CurrentSeed0.Name = "TB_CurrentSeed0";
        TB_CurrentSeed0.ReadOnly = true;
        TB_CurrentSeed0.Size = new Size(118, 22);
        TB_CurrentSeed0.TabIndex = 5;
        TB_CurrentSeed0.TabStop = false;
        TB_CurrentSeed0.Text = "0123456789ABCDEF";
        // 
        // TB_CurrentAdvances
        // 
        TB_CurrentAdvances.CharacterCasing = CharacterCasing.Lower;
        TB_CurrentAdvances.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TB_CurrentAdvances.Location = new Point(51, 82);
        TB_CurrentAdvances.MaxLength = 13;
        TB_CurrentAdvances.Name = "TB_CurrentAdvances";
        TB_CurrentAdvances.ReadOnly = true;
        TB_CurrentAdvances.Size = new Size(98, 22);
        TB_CurrentAdvances.TabIndex = 3;
        TB_CurrentAdvances.TabStop = false;
        TB_CurrentAdvances.Text = "4,294,967,295";
        TB_CurrentAdvances.TextAlign = HorizontalAlignment.Right;
        // 
        // L_CurrentAdvances
        // 
        L_CurrentAdvances.AutoSize = true;
        L_CurrentAdvances.Location = new Point(11, 87);
        L_CurrentAdvances.Name = "L_CurrentAdvances";
        L_CurrentAdvances.Size = new Size(34, 15);
        L_CurrentAdvances.TabIndex = 15;
        L_CurrentAdvances.Text = "Adv.:";
        // 
        // B_Disconnect
        // 
        B_Disconnect.Enabled = false;
        B_Disconnect.Location = new Point(109, 36);
        B_Disconnect.Name = "B_Disconnect";
        B_Disconnect.Size = new Size(97, 25);
        B_Disconnect.TabIndex = 2;
        B_Disconnect.Text = "Disconnect";
        B_Disconnect.UseVisualStyleBackColor = true;
        B_Disconnect.Click += B_Disconnect_Click;
        // 
        // B_Connect
        // 
        B_Connect.Location = new Point(11, 36);
        B_Connect.Name = "B_Connect";
        B_Connect.Size = new Size(97, 25);
        B_Connect.TabIndex = 1;
        B_Connect.Text = "Connect";
        B_Connect.UseVisualStyleBackColor = true;
        B_Connect.Click += B_Connect_Click;
        // 
        // L_SwitchIP
        // 
        L_SwitchIP.AutoSize = true;
        L_SwitchIP.Location = new Point(11, 17);
        L_SwitchIP.Name = "L_SwitchIP";
        L_SwitchIP.Size = new Size(58, 15);
        L_SwitchIP.TabIndex = 12;
        L_SwitchIP.Text = "Switch IP:";
        // 
        // TB_SwitchIP
        // 
        TB_SwitchIP.CharacterCasing = CharacterCasing.Lower;
        TB_SwitchIP.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TB_SwitchIP.Location = new Point(95, 12);
        TB_SwitchIP.MaxLength = 15;
        TB_SwitchIP.Name = "TB_SwitchIP";
        TB_SwitchIP.Size = new Size(111, 22);
        TB_SwitchIP.TabIndex = 0;
        TB_SwitchIP.Text = "123.123.123.123";
        TB_SwitchIP.TextChanged += TB_SwitchIP_TextChanged;
        TB_SwitchIP.KeyDown += IP_HandlePaste;
        TB_SwitchIP.KeyPress += AllowOnlyIP_KeyPress;
        // 
        // GB_Seed
        // 
        GB_Seed.Controls.Add(L_InitialSeed1);
        GB_Seed.Controls.Add(TB_InitialSeed1);
        GB_Seed.Controls.Add(L_InitialSeed0);
        GB_Seed.Controls.Add(TB_InitialSeed0);
        GB_Seed.Location = new Point(0, -8);
        GB_Seed.Name = "GB_Seed";
        GB_Seed.RightToLeft = RightToLeft.No;
        GB_Seed.Size = new Size(212, 58);
        GB_Seed.TabIndex = 0;
        GB_Seed.TabStop = false;
        // 
        // L_InitialSeed1
        // 
        L_InitialSeed1.AutoSize = true;
        L_InitialSeed1.Location = new Point(11, 35);
        L_InitialSeed1.Name = "L_InitialSeed1";
        L_InitialSeed1.Size = new Size(49, 15);
        L_InitialSeed1.TabIndex = 2;
        L_InitialSeed1.Text = "Seed[1]:";
        // 
        // TB_InitialSeed1
        // 
        TB_InitialSeed1.CharacterCasing = CharacterCasing.Upper;
        TB_InitialSeed1.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TB_InitialSeed1.Location = new Point(88, 33);
        TB_InitialSeed1.MaxLength = 16;
        TB_InitialSeed1.Name = "TB_InitialSeed1";
        TB_InitialSeed1.Size = new Size(118, 22);
        TB_InitialSeed1.TabIndex = 3;
        TB_InitialSeed1.Text = "0123456789ABCDEF";
        TB_InitialSeed1.KeyDown += State_HandlePaste;
        TB_InitialSeed1.KeyPress += AllowOnlyHex_KeyPress;
        // 
        // L_InitialSeed0
        // 
        L_InitialSeed0.AutoSize = true;
        L_InitialSeed0.Location = new Point(11, 11);
        L_InitialSeed0.Name = "L_InitialSeed0";
        L_InitialSeed0.Size = new Size(49, 15);
        L_InitialSeed0.TabIndex = 0;
        L_InitialSeed0.Text = "Seed[0]:";
        // 
        // TB_InitialSeed0
        // 
        TB_InitialSeed0.CharacterCasing = CharacterCasing.Upper;
        TB_InitialSeed0.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TB_InitialSeed0.Location = new Point(88, 9);
        TB_InitialSeed0.MaxLength = 16;
        TB_InitialSeed0.Name = "TB_InitialSeed0";
        TB_InitialSeed0.Size = new Size(118, 22);
        TB_InitialSeed0.TabIndex = 1;
        TB_InitialSeed0.Text = "0123456789ABCDEF";
        TB_InitialSeed0.KeyDown += State_HandlePaste;
        TB_InitialSeed0.KeyPress += AllowOnlyHex_KeyPress;
        // 
        // DGV_Results
        // 
        DGV_Results.AllowUserToAddRows = false;
        DGV_Results.AllowUserToDeleteRows = false;
        dataGridViewCellStyle1.BackColor = Color.WhiteSmoke;
        DGV_Results.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        DGV_Results.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        DGV_Results.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        DGV_Results.Location = new Point(11, 233);
        DGV_Results.Name = "DGV_Results";
        DGV_Results.ReadOnly = true;
        DGV_Results.RowHeadersVisible = false;
        DGV_Results.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DGV_Results.Size = new Size(393, 253);
        DGV_Results.TabIndex = 129;
        DGV_Results.CellFormatting += DGV_Results_CellFormatting;
        // 
        // BS_Results
        // 
        BS_Results.DataSource = typeof(Core.Interfaces.ItemFrame);
        // 
        // B_Search
        // 
        B_Search.Location = new Point(218, 194);
        B_Search.Name = "B_Search";
        B_Search.Size = new Size(186, 25);
        B_Search.TabIndex = 130;
        B_Search.Text = "Generate";
        B_Search.UseVisualStyleBackColor = true;
        B_Search.Click += B_Search_Click;
        // 
        // TB_Advances
        // 
        TB_Advances.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TB_Advances.Location = new Point(313, 169);
        TB_Advances.MaxLength = 10;
        TB_Advances.Name = "TB_Advances";
        TB_Advances.Size = new Size(91, 22);
        TB_Advances.TabIndex = 131;
        TB_Advances.Text = "5000";
        TB_Advances.TextAlign = HorizontalAlignment.Right;
        TB_Advances.KeyDown += Dec_HandlePaste;
        TB_Advances.KeyPress += AllowOnlyNumerical_KeyPress;
        // 
        // TB_Initial
        // 
        TB_Initial.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TB_Initial.Location = new Point(313, 145);
        TB_Initial.MaxLength = 10;
        TB_Initial.Name = "TB_Initial";
        TB_Initial.Size = new Size(91, 22);
        TB_Initial.TabIndex = 132;
        TB_Initial.Text = "0";
        TB_Initial.TextAlign = HorizontalAlignment.Right;
        TB_Initial.KeyDown += Dec_HandlePaste;
        TB_Initial.KeyPress += AllowOnlyNumerical_KeyPress;
        // 
        // CB_FiltersEnabled
        // 
        CB_FiltersEnabled.AutoSize = true;
        CB_FiltersEnabled.Checked = true;
        CB_FiltersEnabled.CheckState = CheckState.Checked;
        CB_FiltersEnabled.Location = new Point(218, 117);
        CB_FiltersEnabled.Name = "CB_FiltersEnabled";
        CB_FiltersEnabled.Size = new Size(102, 19);
        CB_FiltersEnabled.TabIndex = 133;
        CB_FiltersEnabled.Text = "Filters Enabled";
        CB_FiltersEnabled.UseVisualStyleBackColor = true;
        // 
        // CB_Location
        // 
        CB_Location.FormattingEnabled = true;
        CB_Location.Location = new Point(283, 12);
        CB_Location.Name = "CB_Location";
        CB_Location.Size = new Size(121, 23);
        CB_Location.TabIndex = 134;
        CB_Location.SelectedIndexChanged += CB_Location_SelectedIndexChanged;
        // 
        // CB_ItemTable
        // 
        CB_ItemTable.FormattingEnabled = true;
        CB_ItemTable.Location = new Point(283, 37);
        CB_ItemTable.Name = "CB_ItemTable";
        CB_ItemTable.Size = new Size(121, 23);
        CB_ItemTable.TabIndex = 135;
        CB_ItemTable.SelectedIndexChanged += CB_ItemTable_SelectedIndexChanged;
        // 
        // CB_CandyTable
        // 
        CB_CandyTable.FormattingEnabled = true;
        CB_CandyTable.Location = new Point(283, 62);
        CB_CandyTable.Name = "CB_CandyTable";
        CB_CandyTable.Size = new Size(121, 23);
        CB_CandyTable.TabIndex = 136;
        CB_CandyTable.SelectedIndexChanged += CB_CandyTable_SelectedIndexChanged;
        // 
        // CB_TargetItem
        // 
        CB_TargetItem.FormattingEnabled = true;
        CB_TargetItem.Location = new Point(283, 91);
        CB_TargetItem.Name = "CB_TargetItem";
        CB_TargetItem.Size = new Size(121, 23);
        CB_TargetItem.TabIndex = 137;
        CB_TargetItem.SelectedIndexChanged += CB_TargetItem_SelectedIndexChanged;
        // 
        // NUD_Quantity
        // 
        NUD_Quantity.Increment = new decimal(new int[] { 9, 0, 0, 0 });
        NUD_Quantity.Location = new Point(372, 116);
        NUD_Quantity.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
        NUD_Quantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        NUD_Quantity.Name = "NUD_Quantity";
        NUD_Quantity.Size = new Size(32, 23);
        NUD_Quantity.TabIndex = 138;
        NUD_Quantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // L_Location
        // 
        L_Location.AutoSize = true;
        L_Location.Location = new Point(214, 15);
        L_Location.Name = "L_Location";
        L_Location.Size = new Size(56, 15);
        L_Location.TabIndex = 139;
        L_Location.Text = "Location:";
        // 
        // L_ItemTable
        // 
        L_ItemTable.AutoSize = true;
        L_ItemTable.Location = new Point(214, 40);
        L_ItemTable.Name = "L_ItemTable";
        L_ItemTable.Size = new Size(37, 15);
        L_ItemTable.TabIndex = 140;
        L_ItemTable.Text = "Table:";
        // 
        // L_TargetItem
        // 
        L_TargetItem.AutoSize = true;
        L_TargetItem.Location = new Point(214, 94);
        L_TargetItem.Name = "L_TargetItem";
        L_TargetItem.Size = new Size(42, 15);
        L_TargetItem.TabIndex = 141;
        L_TargetItem.Text = "Target:";
        // 
        // L_Quantity
        // 
        L_Quantity.AutoSize = true;
        L_Quantity.Location = new Point(359, 118);
        L_Quantity.Name = "L_Quantity";
        L_Quantity.Size = new Size(13, 15);
        L_Quantity.TabIndex = 142;
        L_Quantity.Text = "x";
        // 
        // L_CandyTable
        // 
        L_CandyTable.AutoSize = true;
        L_CandyTable.Location = new Point(214, 65);
        L_CandyTable.Name = "L_CandyTable";
        L_CandyTable.Size = new Size(56, 15);
        L_CandyTable.TabIndex = 143;
        L_CandyTable.Text = "Subtable:";
        // 
        // L_Initial
        // 
        L_Initial.AutoSize = true;
        L_Initial.Location = new Point(214, 147);
        L_Initial.Name = "L_Initial";
        L_Initial.Size = new Size(93, 15);
        L_Initial.TabIndex = 144;
        L_Initial.Text = "Initial Advances:";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(292, 171);
        label2.Name = "label2";
        label2.Size = new Size(15, 15);
        label2.TabIndex = 145;
        label2.Text = "+";
        // 
        // MainWindow
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(416, 498);
        Controls.Add(label2);
        Controls.Add(L_Initial);
        Controls.Add(L_CandyTable);
        Controls.Add(L_Quantity);
        Controls.Add(L_TargetItem);
        Controls.Add(L_ItemTable);
        Controls.Add(L_Location);
        Controls.Add(NUD_Quantity);
        Controls.Add(CB_TargetItem);
        Controls.Add(CB_CandyTable);
        Controls.Add(CB_ItemTable);
        Controls.Add(CB_Location);
        Controls.Add(CB_FiltersEnabled);
        Controls.Add(TB_Initial);
        Controls.Add(TB_Advances);
        Controls.Add(B_Search);
        Controls.Add(DGV_Results);
        Controls.Add(GB_Seed);
        Controls.Add(GB_Connection);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "MainWindow";
        FormClosing += MainWindow_FormClosing;
        Load += MainWindow_Load;
        GB_Connection.ResumeLayout(false);
        GB_Connection.PerformLayout();
        GB_Seed.ResumeLayout(false);
        GB_Seed.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)DGV_Results).EndInit();
        ((System.ComponentModel.ISupportInitialize)BS_Results).EndInit();
        ((System.ComponentModel.ISupportInitialize)NUD_Quantity).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox GB_Connection;
    private TextBox TB_AdvancesIncrease;
    private TextBox TB_Status;
    private Label L_CurrentSeed;
    private Label L_Status;
    private TextBox TB_CurrentSeed0;
    public TextBox TB_CurrentAdvances;
    private Label L_CurrentAdvances;
    private Button B_Disconnect;
    private Button B_Connect;
    private Label L_SwitchIP;
    private TextBox TB_SwitchIP;
    private GroupBox GB_Seed;
    private Label L_InitialSeed0;
    public TextBox TB_InitialSeed0;
    private DataGridView DGV_Results;
    private Label label1;
    private TextBox TB_CurrentSeed1;
    private Label L_InitialSeed1;
    public TextBox TB_InitialSeed1;
    private Button B_CopyToInitial;
    private BindingSource BS_Results;
    private Button B_Search;
    private TextBox TB_Advances;
    private TextBox TB_Initial;
    private CheckBox CB_FiltersEnabled;
    private ComboBox CB_Location;
    private ComboBox CB_ItemTable;
    private ComboBox CB_CandyTable;
    private ComboBox CB_TargetItem;
    private NumericUpDown NUD_Quantity;
    private Label L_Location;
    private Label L_ItemTable;
    private Label L_TargetItem;
    private Label L_Quantity;
    private Label L_CandyTable;
    private Label L_Initial;
    private Label label2;
}


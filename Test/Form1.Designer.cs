using System;
using System.Drawing;
using System.Windows.Forms;

namespace test
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
      buttonStartServer = new Button();
      buttonStartClient = new Button();
      tableLayoutPanel1 = new TableLayoutPanel();
      buttonStopServer = new Button();
      buttonStopClient = new Button();
      textBoxServerLog = new TextBox();
      pictureBoxClientView = new PictureBox();
      textBoxClientLog = new TextBox();
      tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)pictureBoxClientView).BeginInit();
      SuspendLayout();
      // 
      // buttonStartServer
      // 
      buttonStartServer.Dock = DockStyle.Fill;
      buttonStartServer.Location = new Point(3, 3);
      buttonStartServer.Name = "buttonStartServer";
      buttonStartServer.Size = new Size(190, 23);
      buttonStartServer.TabIndex = 0;
      buttonStartServer.Text = "Start Server";
      buttonStartServer.UseVisualStyleBackColor = true;
      buttonStartServer.Click += buttonStartServer_Click;
      // 
      // buttonStartClient
      // 
      buttonStartClient.Dock = DockStyle.Fill;
      buttonStartClient.Location = new Point(395, 3);
      buttonStartClient.Name = "buttonStartClient";
      buttonStartClient.Size = new Size(190, 23);
      buttonStartClient.TabIndex = 1;
      buttonStartClient.Text = "Connect Client";
      buttonStartClient.UseVisualStyleBackColor = true;
      buttonStartClient.Click += buttonStartClient_Click;
      // 
      // tableLayoutPanel1
      // 
      tableLayoutPanel1.ColumnCount = 4;
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
      tableLayoutPanel1.Controls.Add(buttonStartServer, 0, 0);
      tableLayoutPanel1.Controls.Add(buttonStartClient, 2, 0);
      tableLayoutPanel1.Controls.Add(buttonStopServer, 1, 0);
      tableLayoutPanel1.Controls.Add(buttonStopClient, 3, 0);
      tableLayoutPanel1.Controls.Add(textBoxServerLog, 0, 1);
      tableLayoutPanel1.Controls.Add(pictureBoxClientView, 2, 2);
      tableLayoutPanel1.Controls.Add(textBoxClientLog, 2, 1);
      tableLayoutPanel1.Dock = DockStyle.Fill;
      tableLayoutPanel1.Location = new Point(0, 0);
      tableLayoutPanel1.Name = "tableLayoutPanel1";
      tableLayoutPanel1.RowCount = 3;
      tableLayoutPanel1.RowStyles.Add(new RowStyle());
      tableLayoutPanel1.RowStyles.Add(new RowStyle());
      tableLayoutPanel1.RowStyles.Add(new RowStyle());
      tableLayoutPanel1.Size = new Size(784, 561);
      tableLayoutPanel1.TabIndex = 2;
      // 
      // buttonStopServer
      // 
      buttonStopServer.Dock = DockStyle.Fill;
      buttonStopServer.Location = new Point(199, 3);
      buttonStopServer.Name = "buttonStopServer";
      buttonStopServer.Size = new Size(190, 23);
      buttonStopServer.TabIndex = 2;
      buttonStopServer.Text = "Stop Server";
      buttonStopServer.UseVisualStyleBackColor = true;
      buttonStopServer.Click += buttonStopServer_Click;
      // 
      // buttonStopClient
      // 
      buttonStopClient.Dock = DockStyle.Fill;
      buttonStopClient.Location = new Point(591, 3);
      buttonStopClient.Name = "buttonStopClient";
      buttonStopClient.Size = new Size(190, 23);
      buttonStopClient.TabIndex = 3;
      buttonStopClient.Text = "Disconnect Client";
      buttonStopClient.UseVisualStyleBackColor = true;
      buttonStopClient.Click += buttonStopClient_Click;
      // 
      // textBoxServerLog
      // 
      tableLayoutPanel1.SetColumnSpan(textBoxServerLog, 2);
      textBoxServerLog.Dock = DockStyle.Fill;
      textBoxServerLog.Location = new Point(3, 32);
      textBoxServerLog.Multiline = true;
      textBoxServerLog.Name = "textBoxServerLog";
      textBoxServerLog.ReadOnly = true;
      tableLayoutPanel1.SetRowSpan(textBoxServerLog, 2);
      textBoxServerLog.ScrollBars = ScrollBars.Both;
      textBoxServerLog.Size = new Size(386, 526);
      textBoxServerLog.TabIndex = 4;
      // 
      // pictureBoxClientView
      // 
      tableLayoutPanel1.SetColumnSpan(pictureBoxClientView, 2);
      pictureBoxClientView.Dock = DockStyle.Fill;
      pictureBoxClientView.Location = new Point(395, 278);
      pictureBoxClientView.Name = "pictureBoxClientView";
      pictureBoxClientView.Size = new Size(386, 280);
      pictureBoxClientView.TabIndex = 5;
      pictureBoxClientView.TabStop = false;
      pictureBoxClientView.MouseDown += pictureBoxClientView_MouseDown;
      pictureBoxClientView.MouseEnter += pictureBoxClientView_MouseEnter;
      pictureBoxClientView.MouseLeave += pictureBoxClientView_MouseLeave;
      pictureBoxClientView.MouseMove += pictureBoxClientView_MouseMove;
      pictureBoxClientView.MouseUp += pictureBoxClientView_MouseUp;
      pictureBoxClientView.PreviewKeyDown += pictureBoxClientView_PreviewKeyDown;
      // 
      // textBoxClientLog
      // 
      tableLayoutPanel1.SetColumnSpan(textBoxClientLog, 2);
      textBoxClientLog.Dock = DockStyle.Fill;
      textBoxClientLog.Location = new Point(395, 32);
      textBoxClientLog.Multiline = true;
      textBoxClientLog.Name = "textBoxClientLog";
      textBoxClientLog.ReadOnly = true;
      textBoxClientLog.ScrollBars = ScrollBars.Both;
      textBoxClientLog.Size = new Size(386, 240);
      textBoxClientLog.TabIndex = 6;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(784, 561);
      Controls.Add(tableLayoutPanel1);
      Name = "Form1";
      Text = "VNC Test";
      tableLayoutPanel1.ResumeLayout(false);
      tableLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)pictureBoxClientView).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private Button buttonStartServer;
    private Button buttonStartClient;
    private TableLayoutPanel tableLayoutPanel1;
    private Button buttonStopServer;
    private Button buttonStopClient;
    private TextBox textBoxServerLog;
    private PictureBox pictureBoxClientView;
    private TextBox textBoxClientLog;
  }
}

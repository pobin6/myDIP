﻿namespace MyDIP
{
    partial class DIPForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSF = new System.Windows.Forms.Button();
            this.btnHE = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnGray = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSF
            // 
            this.btnSF.Location = new System.Drawing.Point(278, 117);
            this.btnSF.Name = "btnSF";
            this.btnSF.Size = new System.Drawing.Size(75, 23);
            this.btnSF.TabIndex = 9;
            this.btnSF.Text = "平滑滤波";
            this.btnSF.UseVisualStyleBackColor = true;
            this.btnSF.Click += new System.EventHandler(this.btnSF_Click);
            // 
            // btnHE
            // 
            this.btnHE.Location = new System.Drawing.Point(278, 157);
            this.btnHE.Name = "btnHE";
            this.btnHE.Size = new System.Drawing.Size(75, 23);
            this.btnHE.TabIndex = 8;
            this.btnHE.Text = "均衡直方图";
            this.btnHE.UseVisualStyleBackColor = true;
            this.btnHE.Click += new System.EventHandler(this.btnHE_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(278, 199);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "幂律变换";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnGray
            // 
            this.btnGray.Location = new System.Drawing.Point(278, 250);
            this.btnGray.Name = "btnGray";
            this.btnGray.Size = new System.Drawing.Size(75, 23);
            this.btnGray.TabIndex = 6;
            this.btnGray.Text = "灰度化";
            this.btnGray.UseVisualStyleBackColor = true;
            this.btnGray.Click += new System.EventHandler(this.btnGray_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(278, 296);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 5;
            this.btnOpen.Text = "打开";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // DIPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 486);
            this.Controls.Add(this.btnSF);
            this.Controls.Add(this.btnHE);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGray);
            this.Controls.Add(this.btnOpen);
            this.Name = "DIPForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.DIPForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSF;
        private System.Windows.Forms.Button btnHE;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnGray;
        private System.Windows.Forms.Button btnOpen;
    }
}


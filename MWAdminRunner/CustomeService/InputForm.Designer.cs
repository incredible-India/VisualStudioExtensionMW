using System.Windows.Forms;

namespace MWAdminRunner.CustomeService
{
    partial class InputForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblWelcome = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.ForeColor = System.Drawing.Color.Black;
            this.lblWelcome.Location = new System.Drawing.Point(102, 9);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(198, 16);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Enter Your Postgress password:";
            this.lblWelcome.Click += new System.EventHandler(this.lblWelcome_Click);
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(16, 37);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(373, 22);
            this.txtInput.TabIndex = 1;
            this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.ForeColor = System.Drawing.Color.Black;
            this.btnOK.Location = new System.Drawing.Point(105, 76);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(198, 76);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 110);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.ForeColor = System.Drawing.Color.Coral;
            this.Name = "InputForm";
            this.Text = "Postgres Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblWelcome;
        private TextBox txtInput;
        private Button btnOK;
        private Button btnCancel;
    }
}
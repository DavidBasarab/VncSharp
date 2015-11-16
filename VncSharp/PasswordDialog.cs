using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VncSharp
{
    /// <summary>
    ///     A simple GUI Form for obtaining a user's password.  More elaborate interfaces could be used, but this is the default.
    /// </summary>
    public class PasswordDialog : Form
    {
        private readonly Container components = null;
        private Button btnCancel;
        private Button btnOk;
        private TextBox txtPassword;

        /// <summary>
        ///     Gets the Password entered by the user.
        /// </summary>
        public string Password
        {
            get { return txtPassword.Text; }
        }

        private PasswordDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Creates an instance of PasswordDialog and uses it to obtain the user's password.
        /// </summary>
        /// <returns>Returns the user's password as entered, or null if he/she clicked Cancel.</returns>
        public static string GetPassword()
        {
            using (var dialog = new PasswordDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK) return dialog.Password;
                // If the user clicks Cancel, return null and not the empty string.
                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) if (components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnOk = new Button();
            btnCancel = new Button();
            txtPassword = new TextBox();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(144, 8);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(64, 23);
            btnOk.TabIndex = 1;
            btnOk.Text = "OK";
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(144, 40);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(64, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(16, 16);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(112, 20);
            txtPassword.TabIndex = 0;
            txtPassword.Text = "";
            // 
            // ConnectionPassword
            // 
            AcceptButton = btnOk;
            AutoScaleBaseSize = new Size(5, 13);
            CancelButton = btnCancel;
            ClientSize = new Size(216, 73);
            Controls.AddRange(new Control[]
                              {
                                      txtPassword,
                                      btnCancel,
                                      btnOk
                              });
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConnectionPassword";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Password";
            ResumeLayout(false);
        }
    }
}
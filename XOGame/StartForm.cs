using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            PlayForm form = new PlayForm();
            this.Hide();
            form.ShowDialog();
        }
    }
}

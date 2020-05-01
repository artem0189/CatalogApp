using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PluginInterface;

namespace CatalogApp
{
    public partial class PluginDialog : Form
    {
        public PluginDialog(List<IPlugin> plugins)
        {
            InitializeComponent();

            pluginsComboBox.DataSource = plugins;
            pluginsComboBox.DisplayMember = "Name";
        }

        public int PluginIndex { get; set; }

        private void okButton_Click(object sender, EventArgs e)
        {
            PluginIndex = pluginsComboBox.SelectedIndex;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            PluginIndex = -1;
        }
    }
}

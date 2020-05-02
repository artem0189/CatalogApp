using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using CatalogApp.Serialization;
using PluginInterface;
using System.IO;

namespace CatalogApp
{
    public partial class MainForm : Form
    {
        public BindingList<object> itemsList = new BindingList<object>();
        private Dictionary<string, string> createdElement = new Dictionary<string, string>();
        public Dictionary<string, BindingList<object>> element = new Dictionary<string, BindingList<object>>();
        public List<IPlugin> plugins = new List<IPlugin>();
        public MainForm()
        {
            InitializeComponent();
            LoadPlugin();
            LoadSerialization();

            GetCreatedClassesName(ref createdElement);
            createdElementCheckBox.Items.AddRange(createdElement.Keys.ToArray());
            elementListBox.DataSource = itemsList;
        }

        private void LoadPlugin()
        {
            string folder = System.AppDomain.CurrentDomain.BaseDirectory + "\\Plugins";
            string[] files = Directory.GetFiles(folder, "*.dll");
            foreach (var value in files)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(value);
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.GetInterface(typeof(IPlugin).Name) != null)
                        {
                            IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                            plugins.Add(plugin);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки плагина\n" + ex.Message);
                }
            }
        }

        private void LoadSerialization()
        {
            Type[] serializationType = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetInterface(typeof(ISerialization).Name) != null).ToArray();
            foreach (var value in serializationType)
            {
                if (value.IsDefined(typeof(DisplayNameAttribute)))
                {
                    var attribute = value.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                    loadToolStripMenuItem.DropDownItems.Add(attribute.DisplayName);
                    loadToolStripMenuItem.DropDownItems[loadToolStripMenuItem.DropDownItems.Count - 1].Click += new System.EventHandler((sender, e) => LoadFile(sender, e, value));

                    saveToolStripMenuItem.DropDownItems.Add(attribute.DisplayName);
                    saveToolStripMenuItem.DropDownItems[saveToolStripMenuItem.DropDownItems.Count - 1].Click += new System.EventHandler((sender, e) => SaveFile(sender, e, value));
                }
            }
        }

        private void GetCreatedClassesName(ref Dictionary<string, string> dictionary)
        {
            Type[] type = Assembly.GetExecutingAssembly().GetTypes().Where(obj => obj.Namespace.Equals("CatalogApp.Classes") && !obj.IsAbstract && obj.IsClass).ToArray();
            foreach (var value in type)
            {
                Type classType = Type.GetType(value.FullName, false, true);
                if (classType.IsDefined(typeof(SerializableAttribute)))
                {
                    element.Add(classType.FullName, new BindingList<object> { });
                    if (classType.IsDefined(typeof(DisplayNameAttribute)))
                    {
                        var attribute = classType.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                        dictionary.Add(attribute.DisplayName, classType.FullName);
                    }
                }
            }
        }

        private void ShowInformation(object obj)
        {
            infoPanel.Controls.Clear();
            Type type = obj.GetType();
            foreach (var value in type.GetProperties())
            {
                if (value.IsDefined(typeof(DisplayNameAttribute)))
                {
                    var attribute = value.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                    CreateLabel(obj, value, attribute.DisplayName);
                }
            }
        }

        private void CreateLabel(object obj, PropertyInfo propertyInfo, string name)
        {
            Label elem = new Label();
            elem.Text = name + ":";
            elem.Font = new Font(elem.Font, elem.Font.Style | FontStyle.Bold);
            elem.Size = new Size(infoPanel.Size.Width, 15);
            elem.TextAlign = ContentAlignment.BottomLeft;
            infoPanel.Controls.Add(elem);

            elem = new Label();
            elem.Text = propertyInfo.GetValue(obj).ToString();
            elem.Size = new Size(infoPanel.Size.Width, 15);
            elem.TextAlign = ContentAlignment.BottomLeft;
            infoPanel.Controls.Add(elem);
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            if (createdElementCheckBox.SelectedIndex != -1)
            {
                Type type = Type.GetType(createdElement[createdElementCheckBox.SelectedItem.ToString()], false, true);
                EditForm newWindow = new EditForm(Activator.CreateInstance(type), this, this);
                newWindow.Show();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (elementListBox.SelectedIndex != -1)
            {
                var obj = elementListBox.SelectedItem;
                itemsList.Remove(obj);
                element[obj.GetType().FullName].Remove(obj);
                elementListBox_SelectedIndexChanged(sender, e);
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (elementListBox.SelectedIndex != -1)
            {
                EditForm window = new EditForm(elementListBox.SelectedItem, this, this);
                window.Show();
            }
        }

        private void elementListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (elementListBox.SelectedIndex != -1)
            {
                ShowInformation(elementListBox.SelectedItem);
            }
            else
            {
                infoPanel.Controls.Clear();
            }
        }

        private bool EqualsObject(object first, object second)
        {
            bool result = true;
            foreach (var value in first.GetType().GetProperties())
            {
                if (value.GetValue(first).ToString() != value.GetValue(second).ToString())
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private void SynchroData(Type type)
        {
            var list = element[type.FullName];
            foreach (var value in list)
            {
                PropertyInfo[] objectTypes = value.GetType().GetProperties().Where(obj => element.ContainsKey(obj.PropertyType.FullName)).ToArray();
                foreach (var property in objectTypes)
                {
                    foreach (var obj in element[property.PropertyType.FullName])
                    {
                        if (EqualsObject(obj, property.GetValue(value)))
                        {
                            property.SetValue(value, obj);
                        }
                    }
                }
            }
        }

        private void ListBoxUpdate()
        {
            Type[] type = Assembly.GetExecutingAssembly().GetTypes().Where(obj => obj.Namespace.Equals("CatalogApp.Classes") && !obj.IsAbstract && obj.IsClass).ToArray();
            foreach (var value in type)
            {
                if (value.IsDefined(typeof(DisplayNameAttribute)))
                {
                    SynchroData(value);   
                    foreach (var elem in element[value.FullName])
                    {
                        itemsList.Add(elem);
                    }
                }
            }
        }

        private void LoadFile(object sender, EventArgs e, Type type)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                object serializer = Activator.CreateInstance(type);

                var filePath = openFileDialog.FileName;
                string extension = filePath.Substring(filePath.LastIndexOf('.') != -1 ? filePath.LastIndexOf('.') : filePath.Length);
                using (FileStream file = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    byte[] data = new byte[file.Length];
                    file.Read(data, 0, data.Length);
                    if (!(extension.Equals("") || extension.Equals(".txt")))
                    {
                        IPlugin plugin = plugins.Where(obj => obj.Extension == extension).ToArray()[0];
                        if (plugin != null)
                        {
                            data = plugin.OnLoad(data);
                        }
                        else
                        {
                            MessageBox.Show("Нет подходящего плагина!");
                            return;
                        }
                    }
                    (serializer as ISerialization).Deserialize(data, ref element);
                }
                ListBoxUpdate();
            }
        }

        private void SaveFile(object sender, EventArgs e, Type type)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                object deserializer = Activator.CreateInstance(type);

                var filePath = saveFileDialog.FileName;
                string extension = "";
                using (FileStream file = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    byte[] data = (deserializer as ISerialization).Serialize(element);
                    if (plugins.Count != 0)
                    {
                        using (PluginDialog dialog = new PluginDialog(plugins))
                        {
                            DialogResult result = dialog.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                data = plugins[dialog.PluginIndex].OnSave(data);
                                extension = plugins[dialog.PluginIndex].Extension;
                            }
                        }
                    }
                    file.Write(data, 0, data.Length);
                }
                File.Move(filePath, extension != "" ? Path.ChangeExtension(filePath, extension) : filePath);
            }
        }
    }
}

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

namespace CatalogApp
{
    public partial class EditForm : Form
    {
        private MainForm mainForm;
        private Form callForm;
        private Type objectType;
        public EditForm(object obj, MainForm mainForm, Form callForm)
        {
            InitializeComponent();

            this.mainForm = mainForm;
            this.callForm = callForm;
            objectType = obj.GetType();

            CreateForm(obj);
            FillObjects(objectType.FullName, obj);
            FillData(obj);
        }

        private void FillObjects(string name, object obj)
        {
            allObjectsComboBox.DataSource = mainForm.element[name];
            allObjectsComboBox.SelectedItem = mainForm.element[objectType.FullName].Contains(obj) ? obj : null;
            allObjectsComboBox.SelectedIndexChanged += allObjectsComboBox_SelectedIndexChanged;
        }

        private void CreateForm(object obj)
        {
            Type type = obj.GetType();
            foreach (var value in type.GetProperties())
            {
                if (value.IsDefined(typeof(DisplayNameAttribute)))
                {
                    var attribute = value.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                    CreateLabel(value, attribute.DisplayName);
                    if (value.PropertyType.IsPrimitive || value.PropertyType.Equals(typeof(string)))
                    {
                        CreateTextBox(value.Name);
                    }
                    else if (value.PropertyType.IsEnum)
                    {
                        CreateComboBox(value, true);
                    }
                    else
                    {
                        CreateComboBox(value, false);
                    }
                }
            }
        }

        private void CreateLabel(PropertyInfo propertyInfo, string content)
        {
            Label elem = new Label();
            elem.Name = propertyInfo.Name + "Label";
            elem.Text = content + ":";
            elem.Margin = new Padding(0, 0, 0, 3);
            elem.TextAlign = ContentAlignment.BottomLeft;
            formPanel.Controls.Add(elem);
        }

        private void CreateTextBox(string name)
        {
            TextBox elem = new TextBox();
            elem.Name = name;
            elem.Margin = new Padding(0, 0, 0, 0);
            elem.Size = new Size(formPanel.Size.Width, formPanel.Size.Height);
            formPanel.Controls.Add(elem);
        }

        private void CreateComboBox(PropertyInfo propertyInfo, bool isEnum)
        {
            FlowLayoutPanel panel = CreateFlowLayoutPanel();
            ComboBox elem = new ComboBox();
            elem.Name = propertyInfo.Name;
            elem.Margin = new Padding(0, 0, 0, 0);
            elem.Size = new Size(panel.Size.Width, panel.Size.Height);
            elem.DropDownStyle = ComboBoxStyle.DropDownList;
            panel.Controls.Add(elem);
            if (isEnum)
            {
                elem.DataSource = Enum.GetNames(propertyInfo.PropertyType);
                elem.SelectedItem = null;
            }
            else
            {
                elem.DataSource = mainForm.element[propertyInfo.PropertyType.FullName];
                elem.SelectedItem = null;
                Button btn = CreateButton(propertyInfo, panel);
                elem.Size = new Size(panel.Size.Width - btn.Size.Height - btn.Margin.Left, panel.Size.Height);
            }
        }

        private FlowLayoutPanel CreateFlowLayoutPanel()
        {
            FlowLayoutPanel elem = new FlowLayoutPanel();
            elem.Margin = new Padding(0, 0, 0, 0);
            elem.FlowDirection = FlowDirection.LeftToRight;
            elem.Size = new Size(formPanel.Size.Width, 21);
            formPanel.Controls.Add(elem);

            return elem;
        }

        private Button CreateButton(PropertyInfo propertyInfo, FlowLayoutPanel panel)
        {
            Button elem = new Button();
            elem.Text = "..";
            elem.Name = propertyInfo.PropertyType.FullName;
            elem.Margin = new Padding(2, 0, 0, 0);
            elem.Size = new Size(panel.Size.Height, panel.Size.Height);
            elem.Click += editButton_Click;
            panel.Controls.Add(elem);

            return elem;
        }

        private void FillData(object obj)
        {
            Type type = obj.GetType();
            foreach (var value in type.GetProperties())
            {
                var elem = formPanel.Controls.Find(value.Name, true)[0];
                if (elem.GetType().Equals(typeof(TextBox)))
                {
                    (elem as TextBox).Text = value.GetValue(obj) != null ? value.GetValue(obj).ToString() : "";
                }
                else if (elem.GetType().Equals(typeof(ComboBox)))
                {
                    var res = value.GetValue(obj);
                    if (value.PropertyType.IsEnum)
                    {
                        (elem as ComboBox).SelectedIndex = (elem as ComboBox).Items.IndexOf(res.ToString());
                    }
                    else
                    {
                        (elem as ComboBox).SelectedItem = value.GetValue(obj);
                    }
                }
            }
        }

        private void FillObject(Type type, ref object obj)
        {
            foreach (var value in type.GetProperties())
            {
                var elem = formPanel.Controls.Find(value.Name, true)[0];
                if (elem.GetType().Equals(typeof(TextBox)))
                {
                    var converter = TypeDescriptor.GetConverter(value.PropertyType);
                    value.SetValue(obj, converter.ConvertFrom((elem as TextBox).Text));
                }
                else if (elem.GetType().Equals(typeof(ComboBox)))
                {
                    value.SetValue(obj, value.PropertyType.IsEnum ? Enum.Parse(value.PropertyType, (elem as ComboBox).SelectedItem.ToString()) : (elem as ComboBox).SelectedItem);
                }
            }
        }

        private bool CheckFormValidation(Type type)
        {
            bool result = true;
            foreach (var value in type.GetProperties())
            {
                if (value.IsDefined(typeof(DisplayNameAttribute)))
                {
                    bool isValid = true;
                    var elem = formPanel.Controls.Find(value.Name, true)[0];
                    if (elem.GetType().Equals(typeof(ComboBox)))
                    {
                        isValid = (elem as ComboBox).SelectedIndex != -1 ? true : false; 
                    }
                    else if (elem.GetType().Equals(typeof(TextBox)))
                    {
                        int res;
                        isValid = (elem as TextBox).Text != "" ? true : false;
                        isValid = value.PropertyType.Equals(typeof(int)) ? int.TryParse((elem as TextBox).Text, out res) : isValid;
                    }
                    (formPanel.Controls.Find(value.Name + "Label", true)[0] as Label).ForeColor = isValid ? SystemColors.ControlText : Color.Red;
                    result &= isValid;
                }
            }
            return result;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            Type type = Type.GetType((sender as Button).Name, false, true);
            EditForm newWindow = new EditForm(Activator.CreateInstance(type), mainForm, this);
            newWindow.Show();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            if (CheckFormValidation(objectType))
            {
                object newObject = Activator.CreateInstance(objectType);
                FillObject(objectType, ref newObject);
                mainForm.element[objectType.FullName].Add(newObject);
                if (mainForm == callForm)
                {
                    mainForm.itemsList.Add(newObject);
                }
                allObjectsComboBox.SelectedItem = newObject;
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (allObjectsComboBox.SelectedIndex != -1 && CheckFormValidation(objectType))
            {
                object newObject = Activator.CreateInstance(objectType);
                FillObject(objectType, ref newObject);
                if (mainForm == callForm)
                {
                    mainForm.itemsList[mainForm.itemsList.IndexOf(allObjectsComboBox.SelectedItem)] = newObject;
                }
                int index = mainForm.element[objectType.FullName].IndexOf(allObjectsComboBox.SelectedItem);
                mainForm.element[objectType.FullName][index] = newObject;
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (allObjectsComboBox.SelectedIndex != -1)
            {
                var obj = allObjectsComboBox.SelectedItem;
                mainForm.element[obj.GetType().FullName].Remove(obj);
                if (mainForm == callForm)
                {
                    mainForm.itemsList.Remove(obj);
                }
            }
        }

        private void allObjectsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillData(allObjectsComboBox.SelectedItem ?? Activator.CreateInstance(objectType));
        }

        private void EditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            allObjectsComboBox.SelectedIndexChanged -= allObjectsComboBox_SelectedIndexChanged;
        }
    }
}

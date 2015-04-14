using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using POS.Internals.DialogBuilder.Attributes;

namespace POS.Internals.DialogBuilder
{
    public partial class DialogBuilder<T> : Form
    {
        private List<Control> controls;

        public DialogBuilder(string title, T item)
        {
            this.InitializeComponent();
            this.InitializeForm(title);
            this.InitializeControls(item);
            this.DataItem = item;

            // We need to ensure that the required fields are clearly marked, so that
            // the user can see which controls they must complete in order for the OK
            // button to become enabled.
            this.ValidateControls();
        }

        public T DataItem { get; private set; }

        private void InitializeForm(string title)
        {
            this.Text = title;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            this.controls = new List<Control>();
        }

        private void InitializeControls(object item)
        {
            PropertyInfo[] properties = item.GetType().GetProperties();

            // Create layout table
            this.tableLayoutPanel1.RowCount = properties.Length;

            // For each property
            int rowNumber = 0;
            foreach (PropertyInfo property in properties)
            {
                Control ctrl = ControlFactory.CreateControl(item, property);
                if (ctrl != null)
                {
                    // Get custom attributes
                    object[] attributes = property.GetCustomAttributes(true);
                    ctrl = this.ApplyAttributes(ctrl, attributes);

                    // Disable the control if property read only
                    if (!property.CanWrite)
                    {
                        ctrl.Enabled = false;
                    }

                    // Set the tab index
                    //ctrl.TabIndex = controls.Count + 1;

                    // Build label
                    if (ctrl.Visible)
                    {
                        var tag = (ControlTag)ctrl.Tag;
                        Label label = ControlFactory.CreateLabel(property.Name);
                        if (!string.IsNullOrEmpty(tag.CustomLabel))
                        {
                            label.Text = tag.CustomLabel;
                        }
                        this.tableLayoutPanel1.Controls.Add(label, 0, rowNumber);
                        this.tableLayoutPanel1.Controls.Add(ctrl, 1, rowNumber);
                        this.controls.Add(ctrl);
                    }
                }
                rowNumber++;
            }

            // Resize the form
            this.Width = this.tableLayoutPanel1.Width + 40;
            this.Height = this.tableLayoutPanel1.Height + 90;
        }

        /// <summary>
        /// Applies the settings from the custom attributes to the control.
        /// </summary>
        /// <param name="ctrl">A control bound to property</param>
        /// <param name="attributes">Custom attributes for the property</param>
        /// <returns></returns>
        private Control ApplyAttributes(Control ctrl, object[] attributes)
        {
            var tag = (ControlTag)ctrl.Tag;
            NumericSettingsAttribute attrRange = null;
            DisplaySettingsAttribute attrDisplay = null;
            RequiredFieldAttribute attrRequired = null;
            foreach (object attribute in attributes)
            {
                if (attribute is NumericSettingsAttribute)
                {
                    attrRange = (NumericSettingsAttribute)attribute;
                }
                else if (attribute is DisplaySettingsAttribute)
                {
                    attrDisplay = (DisplaySettingsAttribute)attribute;
                }
                else if (attribute is RequiredFieldAttribute)
                {
                    attrRequired = (RequiredFieldAttribute)attribute;
                }
            }

            // Attach LostFocus handler for input validation
            ctrl.LostFocus += this.ctrl_LostFocus;

            // Range Attribute
            if (attrRange != null)
            {
                // todo
            }

            // Display Attribute
            if (attrDisplay != null)
            {
                tag.CustomLabel = attrDisplay.Label;
                ctrl.Enabled = !attrDisplay.ReadOnly;
                ctrl.Visible = attrDisplay.Visible;
                if (attrDisplay.Width > 0)
                {
                    ctrl.Width = attrDisplay.Width;
                }
            }

            // Required Field Attribute
            if (attrRequired != null)
            {
                if (string.IsNullOrEmpty(attrRequired.Message))
                {
                    tag.ErrorMessage = "Required";
                }
                else
                {
                    tag.ErrorMessage = attrRequired.Message;
                }
            }
            return ctrl;
        }

        private void ctrl_LostFocus(object sender, EventArgs e)
        {
            // TODO: It would be better to validate just this control and update the
            // OK button accordingly, instead of validating every control on the
            // form.
            this.ValidateControls();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.SaveControlValues();
        }

        private bool ValidateControl(Control control)
        {
            bool isValid = true;

            // Validation currently limited to TextBoxes only
            var txt = control as TextBox;
            if (txt != null)
            {
                // If the textbox is empty, show a warning
                var tag = (ControlTag)txt.Tag;
                if (tag.IsRequired && string.IsNullOrEmpty(txt.Text))
                {
                    this.errorProvider.SetError(txt, tag.ErrorMessage);
                    isValid = false;
                }
                else
                {
                    this.errorProvider.SetError(txt, string.Empty);
                }
            }
            return isValid;
        }

        /// <summary>
        /// Returns false if any controls are invalid.
        /// </summary>
        /// <returns></returns>
        private void ValidateControls()
        {
            bool isValid = true;
            foreach (Control control in this.controls)
            {
                if (!this.ValidateControl(control))
                {
                    isValid = false;
                }
            }
            this.btnOk.Enabled = isValid;
        }

        /// <summary>
        /// Saves the controls value back to the data object.
        /// </summary>
        private void SaveControlValues()
        {
            // For each TextBox, Dropdown etc...
            foreach (Control c in this.controls)
            {
                var tag = (ControlTag)c.Tag;
                PropertyInfo property = this.DataItem.GetType().GetProperty(tag.PropertyName);
                Type type = property.PropertyType;
                if (c is TextBox)
                {
                    var textbox = (TextBox)c;
                    if (type == typeof (string))
                    {
                        property.SetValue(this.DataItem, textbox.Text, null);
                    }
                    else if (type == typeof (char))
                    {
                        property.SetValue(this.DataItem, Convert.ToChar(textbox.Text), null);
                    }
                }
                else if (c is NumericUpDown)
                {
                    var numeric = (NumericUpDown)c;
                    if (type == typeof (int))
                    {
                        property.SetValue(this.DataItem, Convert.ToInt32(numeric.Value), null);
                    }
                    else if (type == typeof (decimal))
                    {
                        property.SetValue(this.DataItem, Convert.ToDecimal(numeric.Value), null);
                    }
                }
                else if (c is CheckBox)
                {
                    var checkbox = c as CheckBox;
                    property.SetValue(this.DataItem, checkbox.Checked, null);
                }
                else if (c is ComboBox)
                {
                    var dropdown = c as ComboBox;
                    property.SetValue(this.DataItem, Enum.Parse(tag.PropertyType, Convert.ToString(dropdown.SelectedItem)),
                        null);
                }
            }
        }
    }

    public class DialogBuilder : DialogBuilder<object>
    {
        public DialogBuilder(string title, object item) : base(title, item)
        {
        }
    }
}
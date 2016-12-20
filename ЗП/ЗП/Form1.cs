using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace ЗП
{
    public partial class Form1 : Form
    {
        //Parent
        public abstract class Employee 
        {
            private string _id;
            public string id
            {
                get { return _id; }
                set { _id = value; }
            }

            private string _fullname;
            public string fullname
            {
                get { return _fullname; }
                set { _fullname = value; }
            }

            private string _wage;
            public string wage
            {
                get { return _wage; }
                set { _wage = value; }
            }
            
            public Employee(string id, string fullname, string wage) { string _id = id; string _fullname = fullname; string _wage = wage; }

            public double insum;
            public Employee(double msal)
            {
                insum = msal;
            }
            // abstract method
            public abstract double Salary
            {
                get;
                set;
            }
            public Employee(Employee other)//Deep copy constructor                                 
            {
                this.id = other.id;
                this.fullname = other.fullname;
                this.wage = other.wage;
            }
        }

        public List<Employee> newemp = new List<Employee>();

        //Child1
        public class EmpTime : Employee 
        {
            public EmpTime(double msal) : base(msal)
            {
            }
            public override double Salary
            {
                get
                {
                    return (20.8 * 8 * insum);
                }
                set
                {
                    insum = value;
                }                   
            }
        }

        //Child2
        public class EmpFix : Employee 
        {
            public EmpFix(double msal) : base(msal)
            {
            }
            public override double Salary
            {
                get
                {
                    return insum;
                }
                set
                {
                    insum = value;
                }
            }
        }           

        //Сортировка первым способом
        public List<Employee> Sort1(int crow, int ccol, List<Employee> employeemas ) {                   
            string[] savetab = new string[ccol];
            //выполнение сортировки
            for (int i = 0; i < crow; i++)
            {
                //запомним i-ые элементы
                savetab[0] = employeemas[i].id;
                savetab[1] = employeemas[i].fullname; 
                savetab[2] = employeemas[i].wage;
                int j = i - 1;//будем идти, начиная с i-1 элемента 
                while (j >= 0 && Convert.ToDouble(employeemas[j].wage) < Convert.ToDouble(savetab[2]))
                // пока не достигли начала массива 
                // или не нашли элемент больше i-1-го
                // который храниться в строке 
                {
                    //проталкиваем элемент вверх
                    employeemas[j + 1].id = employeemas[j].id;
                    employeemas[j + 1].fullname = employeemas[j].fullname;
                    employeemas[j + 1].wage = employeemas[j].wage;                                      
                    j--;
                }
                // возвращаем i-1 элемент
                employeemas[j + 1].id = savetab[0];
                employeemas[j + 1].fullname = savetab[1];
                employeemas[j + 1].wage = savetab[2];
            }

            //Сортировка ФИО
            string[,] sortname = new string[crow, ccol];
            for (int i = 0; i < crow; i++)
            {
                sortname[i, 0] = employeemas[i].id;
                sortname[i, 1] = employeemas[i].fullname;
                sortname[i, 2] = employeemas[i].wage;
            }
            string[] savename = new string[ccol];
            for (int i = 0; i < crow - 1; i++)
            {
                //Поиск одинаковых сумм
                if (employeemas[i].wage == employeemas[i+1].wage)
                {
                    int rowind = i;
                    int z = i + 1;
                    while ((z < crow) && (employeemas[i].wage == employeemas[z].wage))
                    {
                        z += 1;
                    }
                    //Копирование сотрудников с одинаковыми суммами 
                    for (int m = rowind; m < z; m++)
                    {
                        sortname[m, 0] = employeemas[m].id;
                        sortname[m, 1] = employeemas[m].fullname;
                        sortname[m, 2] = employeemas[m].wage;                        
                    }
                    //Сортировка по алфавиту
                    bool flag = true;
                    while (flag)
                    {
                        flag = false;
                        for (int y = rowind; y < z - 1; ++y)
                            if (sortname[y, 1].CompareTo(sortname[y + 1, 1]) > 0)
                            {
                                for (int k = 0; k < ccol; k++)
                                {
                                    savename[k] = sortname[y, k];
                                    sortname[y, k] = sortname[y + 1, k];
                                    sortname[y + 1, k] = savename[k];
                                }
                                flag = true;
                            }
                    }
                    //Вставка сортированных строк
                    for (int m = rowind; m < z; m++)
                    {
                        employeemas[m].id = sortname[m, 0];
                        employeemas[m].fullname = sortname[m, 1];
                        employeemas[m].wage = sortname[m, 2];
                    }
                    i = z - 1;
                }

            }
            return employeemas;            
        }

        //Сортировка вторым способом
        public List<Employee> Sort2(int crow, int ccol, List<Employee> employeemas)
        {            
            List<Employee> employeemas5 = new List<Employee>(Sort1(crow, ccol, employeemas));
            employeemas5.RemoveRange(5, crow-5);            
            return employeemas5;
        }

        //Сортировка третьим способом
        public List<Employee> Sort3(int crow, int ccol, List<Employee> employeemas)
        {
            List<Employee> employeemas3 = new List<Employee>(Sort1(crow, ccol, employeemas));
            employeemas3.RemoveRange(0, crow-3);
            return employeemas3;
        }

        public Form1()
        {
            InitializeComponent();
        }       

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label3.Text = "Почасовая ставка, руб.";
            textBox3.Text = "0";
            textBox4.Text = "0";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label3.Text = "Фиксированная месячная оплата, руб.";
            textBox3.Text = "0";
            textBox4.Text = "0";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {            
            if ((textBox1.Text == "") || (textBox2.Text == ""))
            {
                MessageBox.Show("Необходимые входные данные не были заполнены!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (newemp.Count == 0)
                {
                    dataGridView1.Columns.Clear();
                }
                if (radioButton1.Checked) {                    
                    newemp.Add(new EmpTime(0) { id = textBox1.Text, fullname = textBox2.Text, wage = textBox4.Text});
                } else if (radioButton2.Checked)
                {                    
                    newemp.Add(new EmpFix(0) { id = textBox1.Text, fullname = textBox2.Text, wage = textBox4.Text});
                }                
                BindingSource biSour = new BindingSource();
                biSour.DataSource = newemp;
                dataGridView1.DataSource = biSour;
                Controls.Add(dataGridView1);
                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
                button6.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if ((cell.RowIndex <= dataGridView1.RowCount-1) && (newemp[cell.RowIndex].fullname !="") && (newemp[cell.RowIndex].wage != "") && (newemp[cell.RowIndex].id != ""))
                {
                    dataGridView1.Rows.RemoveAt(cell.RowIndex);
                }
                else
                {
                    return;
                }
            }       
        }

        private void button3_Click(object sender, EventArgs e)
        {            
            dataGridView1.Rows.Clear();            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            newemp.Add(new EmpFix(0) { id = "1", fullname = "Иванов Иван Иванович", wage = "15000" });
            newemp.Add(new EmpTime(0) { id = "2", fullname = "Кондратьев Ким Антонович", wage = "15000" });
            newemp.Add(new EmpFix(0) { id = "3", fullname = "Капустин Ефим Германнович", wage = "15000" });
            newemp.Add(new EmpFix(0) { id = "4", fullname = "Бирюков Егор Якунович", wage = "11111" });
            newemp.Add(new EmpTime(0) { id = "5", fullname = "Белов Арсений Улебович", wage = "11300" });
            newemp.Add(new EmpTime(0) { id = "6", fullname = "Агафонов Парфений Евгеньевич", wage = "22010" });
            newemp.Add(new EmpFix(0) { id = "7", fullname = "Фёдоров Созон Артёмович", wage = "11111" });
            newemp.Add(new EmpTime(0) { id = "8", fullname = "Фёдоров Абрам Артёмович", wage = "12222" });
            newemp.Add(new EmpFix(0) { id = "9", fullname = "Симонов Авдей Алексеевич", wage = "12222" });
            newemp.Add(new EmpFix(0) { id = "10", fullname = "Капко Иван Петрович", wage = "15000" });
            BindingSource biSour = new BindingSource();
            biSour.DataSource = newemp;
            dataGridView1.DataSource = biSour;
            Controls.Add(dataGridView1);
            dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            button6.Enabled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {            
            double inmoney;
            try {
                inmoney = Convert.ToDouble(textBox3.Text);
            }
            catch {
                textBox3.Text = "0";
                MessageBox.Show("Должно быть введено числовое значение!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Расчет среднемесячной заработной платы
            if (radioButton1.Checked)
            {
                EmpTime emptime = new EmpTime(inmoney);
                textBox4.Text = Convert.ToString(emptime.Salary);
            }
            else if (radioButton2.Checked)
            {
                EmpFix empfix = new EmpFix(inmoney);
                textBox4.Text = Convert.ToString(empfix.Salary);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "0";
            textBox4.Text = "0";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView2.Columns.Clear();
            List<Employee> newemp53 = new List<Employee>(newemp);
            //Первый способ сортировки
            if (radioButton3.Checked)
            {
                newemp53 = new List<Employee>(Sort1(newemp.Count, 3, newemp));                
                newemp = Sort1(newemp.Count, 3, newemp);
                BindingSource biSour = new BindingSource();
                biSour.DataSource = newemp53;
                dataGridView2.DataSource = biSour;
                Controls.Add(dataGridView2);
                dataGridView2.Columns.RemoveAt(dataGridView1.Columns.Count);
            }
            //Второй способ сортировки
            if (radioButton4.Checked) {
                if (newemp.Count < 5)
                {
                    MessageBox.Show("Введите минимально необходимое сотрудников (5) для выполнения данной сортировки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    newemp53 = new List<Employee>(Sort2(newemp.Count, 3, newemp));
                    BindingSource biSour = new BindingSource();
                    biSour.DataSource = newemp53;
                    dataGridView2.DataSource = biSour;
                    Controls.Add(dataGridView2);
                    dataGridView2.Columns.RemoveAt(dataGridView1.Columns.Count);
                }
            }
            //Третий способ сортировки
            if (radioButton5.Checked)
            {
                if (newemp.Count < 3)
                {
                    MessageBox.Show("Введите минимально необходимое сотрудников (3) для выполнения данной сортировки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    newemp53 = new List<Employee>(Sort3(newemp.Count, 3, newemp));
                    BindingSource biSour = new BindingSource();
                    biSour.DataSource = newemp53;
                    dataGridView2.DataSource = biSour;
                    Controls.Add(dataGridView2);
                    dataGridView2.Columns.RemoveAt(dataGridView1.Columns.Count);
                }
            }
            
        }       

        private void button7_Click(object sender, EventArgs e)
        {
            List<Employee> newe = new List<Employee>(newemp);
            BindingSource biSour = new BindingSource();
            newe.Clear();
            newemp.Clear();
            biSour.DataSource = newemp;
            dataGridView1.DataSource = biSour;
            dataGridView2.DataSource = biSour;
            Controls.Add(dataGridView1);
            Controls.Add(dataGridView2);
            dataGridView1.Columns.Clear();
            dataGridView2.Columns.Clear();
            string str = "";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML-файлы (*.xml)|*.xml";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                str = dialog.FileName;
            }
            else
            {
                return;
            }
            try
            {
                XmlDocument xDoc = new XmlDocument();
                List<string> empelem = new List<string>();
                xDoc.Load(str);
                // получим корневой элемент
                XmlElement xRoot = xDoc.DocumentElement;
                // обход всех узлов в корневом элементе
                foreach (XmlNode xnode in xRoot)
                {
                    // обходим все дочерние узлы элемента user
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        // если узел - id
                        if (childnode.Name == "id")
                        {
                            empelem.Add(childnode.InnerText);
                        }
                        // если узел - fullname
                        if (childnode.Name == "fullname")
                        {
                            empelem.Add(childnode.InnerText);
                        }
                        // если узел - wage
                        if (childnode.Name == "wage")
                        {
                            empelem.Add(childnode.InnerText);
                        }
                    }
                    if (empelem.Count != 3)
                    {
                        MessageBox.Show("Некорректный формат входного файла!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    newe.Add(new EmpFix(0) { id = empelem[0], fullname = empelem[1], wage = empelem[2] });
                    empelem.Clear();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при чтении из файла!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            biSour.DataSource = newe;
            dataGridView1.DataSource = biSour;
            Controls.Add(dataGridView1);
            dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
            dataGridView2.Columns.RemoveAt(dataGridView2.Columns.Count - 1);
            newemp = newe;
            button6.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DataGridView dbg = dataGridView1;
            if (radioButton6.Checked)
            {
                dbg = dataGridView1;
            }
            else if ((radioButton7.Checked))
            {
                dbg = dataGridView2;
            }
            string str = "";
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML-файлы (*.xml)|*.xml";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                str = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }
            XmlTextWriter xwriter = new XmlTextWriter(str, Encoding.UTF8);
            xwriter.Formatting = Formatting.Indented;
            xwriter.WriteStartDocument();
            xwriter.WriteStartElement("employees");
            for (int i = 0; i < dbg.RowCount; i++)
            {
                xwriter.WriteStartElement("employee");
                for (int j = 0; j < dbg.ColumnCount; j++)
                {
                    xwriter.WriteStartElement(dbg.Columns[j].Name);
                    xwriter.WriteString(dbg.Rows[i].Cells[j].Value.ToString());
                    xwriter.WriteEndElement();
                }
                xwriter.WriteEndElement();
            }
            xwriter.WriteEndElement();
            xwriter.WriteEndDocument();
            xwriter.Close();
        }       

        private void Form1_Load(object sender, EventArgs e)
        {
            BindingSource biSour = new BindingSource();
            biSour.DataSource = newemp;
            dataGridView2.DataSource = biSour;
            dataGridView1.DataSource = biSour;
            Controls.Add(dataGridView1);
            Controls.Add(dataGridView2);
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
        }        
    }
}

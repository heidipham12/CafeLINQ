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
namespace CafeLINQ
{
    public partial class Form1 : Form
    {
        static string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        CapheDBDataContext db = new CapheDBDataContext();
        public Form1()
        {
            InitializeComponent();
            capheBindingSource.DataSource = db.Caphes;
            capheBindingSource.EndEdit();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'capheDBDataSet.Caphe' table. You can move, or remove it, as needed.
           this.capheTableAdapter.Fill(this.capheDBDataSet.Caphe);
            picCaphe.Image = Image.FromFile(ProjectPath + "\\Data\\caphe.jpg");
            picCaphe.SizeMode = PictureBoxSizeMode.StretchImage;


        }
        public void DisplayData()
        {
            //dgwCaphe.DataSource = (from c in db.Caphes select c);

        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            //Application.Exit();
        }
        private void dgwCaphe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgwCaphe.Focused == false || dgwCaphe.CurrentCell == null)
                return;
            int RowIndex = dgwCaphe.CurrentRow.Index;
            string SelectedID = dgwCaphe.Rows[RowIndex].Cells[0].Value.ToString();
            string CapheImage = (from c in db.Caphes where c.ID == SelectedID select c.ImageName).FirstOrDefault();
            if (CapheImage == null)
                return;
            picCaphe.Image = Image.FromFile(ProjectPath + "\\Data\\" + CapheImage);
            picCaphe.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int RowIndex = dgwCaphe.CurrentRow.Index;
            string SelectedID = dgwCaphe.Rows[RowIndex].Cells[0].Value.ToString();
            Caphe deleteCP = (from c in db.Caphes where c.ID == SelectedID select c).SingleOrDefault();
            db.Caphes.DeleteOnSubmit(deleteCP);
            db.SubmitChanges();
            DisplayData();
            MessageBox.Show("Coffee: " + deleteCP.ID + "is deleted.");

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int RowIndex = dgwCaphe.CurrentRow.Index;
            string SelectedID = dgwCaphe.Rows[RowIndex].Cells[0].Value.ToString();
            Caphe updateedCP = (from c in db.Caphes where c.ID == SelectedID select c).SingleOrDefault();
            updateedCP.ID = dgwCaphe.Rows[RowIndex].Cells[0].Value.ToString();
            updateedCP.Name = dgwCaphe.Rows[RowIndex].Cells[1].Value.ToString();
            updateedCP.Type = dgwCaphe.Rows[RowIndex].Cells[2].Value.ToString();
            updateedCP.MFG = DateTime.Parse(dgwCaphe.Rows[RowIndex].Cells[3].Value.ToString());
            updateedCP.EXP = DateTime.Parse(dgwCaphe.Rows[RowIndex].Cells[4].Value.ToString());
            updateedCP.Price = Convert.ToInt32(dgwCaphe.Rows[RowIndex].Cells[5].Value.ToString());
            updateedCP.ImageName = "caphe.jpg";

            db.SubmitChanges();
            DisplayData();
            MessageBox.Show("Coffee: " + updateedCP.ID + "is updated.");
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            Caphe checkedCP;
            List<Caphe> CPList = (from c in db.Caphes where c.ID.Contains("NEW") select c).ToList();
            string NewID = "NEW";
            if (CPList.Count == 0)
                NewID = NewID + "001";
            else
            {
                checkedCP = CPList.Last();
                NewID = NewID + NextId(checkedCP.ID.Substring(checkedCP.ID.Length - 3));

            }
            Caphe insertedCP = new Caphe();
            insertedCP.ID = NewID;
            insertedCP.Name = "Not Assigned";
            insertedCP.Type = "Not Assigned";
            insertedCP.MFG = DateTime.Now;
            insertedCP.EXP = DateTime.Now;
            insertedCP.Price = 0;
            insertedCP.ImageName = "caphe.jpg";

            db.Caphes.InsertOnSubmit(insertedCP);
            db.SubmitChanges();
            DisplayData();
            MessageBox.Show("New coffee is inserted! Please update data and press button Update ");
        }
       public string NextId(string currentId)
        {
            int i = 0;
            if(int.TryParse(currentId, out i))
            {
                i++;
                return (i.ToString().PadLeft(3, '0'));
            }
            throw new System.ArgumentException("Non-numeric string passed asargument");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class Main : Form
    {
       
        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);


        public Main()
        {
            InitializeComponent();
            RefreshDiary();
            SetColumnsHeader();
            AddGroupsToMainList("Wszystkie grupy");
            AddGroupsToMainList("1A");
            AddGroupsToMainList("2A");
            AddGroupsToMainList("3A");
            AddGroupsToMainWindowList();
            
 
        }

        private void SetColumnsHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Technologia";
            dgvDiary.Columns[6].HeaderText = "Fizyka";
            dgvDiary.Columns[7].HeaderText = "Język polski";
            dgvDiary.Columns[8].HeaderText = "Język obcy";
            dgvDiary.Columns[9].HeaderText = "Zajęcia dodatkowe";
            dgvDiary.Columns[10].HeaderText = "Grupa";

        }
        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile();
            if (cbSelectGroup.Text == "Wszystkie grupy")
                dgvDiary.DataSource = students;
            else
                dgvDiary.DataSource = students.Where(s => s.GroupID == cbSelectGroup.Text).ToList();
              


        }

       private void AddGroupsToMainList(string groupID)
        {
            Program.ListOfGroups.Add(groupID);
        }

        private void AddGroupsToMainWindowList()
        {
            foreach (var group in Program.ListOfGroups)
            {
                cbSelectGroup.Items.Add(group);
            }
           
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Zaznacz ucznia, któego dane chcesz edytować");
                return;
            }

            var addEditStudent = new AddEditStudent(
                Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();

        }

       
        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Zaznacz ucznia, któego dane chcesz edytować");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];

           var confirmDelete = MessageBox.Show($"Czy na pewno chcesz usunąć ucznia" +
                $" {(selectedStudent.Cells[1].Value.ToString() + "" + selectedStudent.Cells[2].Value.ToString())}",
                "usuwanie studenta", MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }

        }


        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }
    }
}

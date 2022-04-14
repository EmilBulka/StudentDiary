using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {

        private Student _student;
        private int _studentId;
        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);
        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;



            GetStudentData();
            tbFirstName.Select();

           
        }

        private void GetStudentData()
        {

            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";
                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                {
                    throw new Exception("brak uzytkownika o podanym id");
                }
                FillTextBoxes();

            }
        }
        private void FillTextBoxes()
        {
           
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            rtbComments.Text = _student.Comments;
            tbForeignLAng.Text = _student.ForeinLang;
            tbMath.Text = _student.Math;
            tbPhysics.Text = _student.Physics;
            tbPolishLang.Text = _student.PolishLang;
            tbTechnology.Text = _student.Technology;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
            {
                students.RemoveAll(x => x.Id == _studentId);
            }
                  
            else
            {
                AssingnIdToNewStudent(students);
            }
                 

            AddNewUserToList(students);
           
            _fileHelper.SerializeToFile(students);

            Close();
        }

        private void AddNewUserToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtbComments.Text,
                ForeinLang = tbForeignLAng.Text,
                Math = tbMath.Text,
                Physics = tbPhysics.Text,
                PolishLang = tbPolishLang.Text,
                Technology = tbTechnology.Text


            };

            students.Add(student);
        }

        private void AssingnIdToNewStudent(List<Student> students)
        {

            var studentsWithHighestId = students
                .OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentsWithHighestId == null ?
                1 : studentsWithHighestId.Id + 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }



        
    }
}

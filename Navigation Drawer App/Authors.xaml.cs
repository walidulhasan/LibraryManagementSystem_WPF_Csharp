using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Navigation_Drawer_App.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Navigation_Drawer_App.Models;

namespace Navigation_Drawer_App
{
    /// <summary>
    /// Interaction logic for Authors.xaml
    /// </summary>
    

    public partial class Authors : Window
    {
        public string gender { get; set; }
        private void Focus(ComboBox cmdtitle)
        {
            List<string> titles = new List<string>()
            {
                "Mr.",
                "Miss",
                "Md.",
                "Mrs."
            };
            this.comTitle.ItemsSource = titles;
        }
        public Authors()
        {
            InitializeComponent();
            this.Focus(comTitle);         
            
        }
        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(texID.Text);
                if (i != 0)
                {
                    AuthorsProp au = new AuthorsProp();
                    au.Id = Convert.ToInt32(texID.Text);
                    au.Title = comTitle.SelectedItem.ToString();
                    au.FirstName = txtFirstName.Text;
                    au.LastName = txtLastName.Text;
                    au.Email = txtEmail.Text;
                    au.Contact = txtContactNo.Text;
                    au.Address = txtAddress.Text;
                    if (gender=="Male")
                    {
                        gender = rdMale.Content.ToString();
                    }
                    else
                    {
                        gender = rdFemale.Content.ToString();
                    }
                    
                    

                    var newEmployeeMember = "{'Id':'" + au.Id + "','Title':'" + au.Title + "','FirstName':'" + au.FirstName + "','LastName':'" + au.LastName + "','Gender':'" + gender + "','Email':'" + au.Email + "','Contact':'" + au.Contact + "','Address':'" + au.Address + "'}";

                    var json = File.ReadAllText(@"AuthorsProp.json");
                    var jsonObj = JObject.Parse(json);
                    var employeeArray = jsonObj.GetValue("AuthorsProp") as JArray;
                    var newEmployee = JObject.Parse(newEmployeeMember);
                    employeeArray.Add(newEmployee);

                    jsonObj["AuthorsProp"] = employeeArray;
                    string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText(@"AuthorsProp.json", newJsonResult);
                    MessageBox.Show("Data Save Successful !!!!");
                    AllClear();
                    ShowData();
                    comTitle.Text = "Mr.";
                }
                else
                {
                    MessageBox.Show("ID can't accept 0");
                }
                
            }
            catch (OverflowException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnHome_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void CloseBtn_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void Tg_Btn_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            int i = Convert.ToInt32(texID.Text);
            if (i != 0)
            {


                var jsonU = File.ReadAllText(@"AuthorsProp.json");
                var jsonObj = JObject.Parse(jsonU);
                JArray auauthorsArray = (JArray)jsonObj["AuthorsProp"];

                var Id = Convert.ToInt32(texID.Text);
                var Title = comTitle.SelectedItem.ToString();
                var FirstName = txtFirstName.Text;
                var LastName = txtLastName.Text;
                var Email = txtEmail.Text;
                var Contact = txtContactNo.Text;
                var Address = txtAddress.Text;
                var gender = rdFemale.Content.ToString();
                var gende = rdMale.Content.ToString();

                foreach (var auauthors in auauthorsArray.Where(obj => obj["Id"].Value<int>() == Id))
                {
                    auauthors["Title"] = !string.IsNullOrEmpty(Title) ? Title : "";
                    auauthors["FirstName"] = !string.IsNullOrEmpty(FirstName) ? FirstName : "";
                    auauthors["LastName"] = !string.IsNullOrEmpty(LastName) ? LastName : "";
                    auauthors["Email"] = !string.IsNullOrEmpty(Email) ? Email : "";
                    auauthors["Contact"] = !string.IsNullOrEmpty(Contact) ? Contact : "";
                    auauthors["Address"] = !string.IsNullOrEmpty(Address) ? Address : "";
                    auauthors["Gender"] = !string.IsNullOrEmpty(gender) ? gender : "";
                    
                }
                jsonObj["AuthorsProp"] = auauthorsArray;
                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(@"AuthorsProp.json", output);
                MessageBox.Show("Data Update Successfully !!!");
                ShowData();
                AllClear();
                texID.IsEnabled = true;
                Btn_save.IsEnabled = true;
            }
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Btn_Update.Visibility = Visibility.Visible;
            Button b = sender as Button;
            AuthorsProp empBtn = b.CommandParameter as AuthorsProp;
            int empId = empBtn.Id;
            texID.Text = empId.ToString();
            comTitle.SelectedItem = empBtn.Title.ToString();
            txtFirstName.Text = empBtn.FirstName.ToString();
            txtLastName.Text = empBtn.LastName.ToString();
            txtEmail.Text = empBtn.Email.ToString();
            txtContactNo.Text = empBtn.Contact.ToString();
            txtAddress.Text = empBtn.Address.ToString();           
            texID.IsEnabled = false;
            Btn_save.IsEnabled = false;
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var json = File.ReadAllText(@"AuthorsProp.json");
            var jsonObj = JObject.Parse(json);
            JArray empArray = (JArray)jsonObj["AuthorsProp"];

            Button b = sender as Button;
            AuthorsProp empBtn = b.CommandParameter as AuthorsProp;
            int empId = empBtn.Id;

            if (empId > 0)
            {
                var employeeToDeleted = empArray.FirstOrDefault(obj => obj["Id"].Value<int>() == empId);
                empArray.Remove(employeeToDeleted);
                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(@"AuthorsProp.json", output);
                MessageBox.Show("Data deleted successfully!!!");
                ShowData();
            }
            else
            {
                MessageBox.Show("Not deleted....");
            }
        }

        private void Btn_AllData_Click(object sender, RoutedEventArgs e)
        {
            ShowData();
        }
        private void AllClear()
        {
            texID.Text = "";
            txtAddress.Clear();
            txtContactNo.Clear();
            txtEmail.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            rdMale.IsChecked = false;
            rdFemale.IsChecked = false;


        }
        private void ShowData()
        {
            var json = File.ReadAllText(@"AuthorsProp.json");
            var jsonObj = JObject.Parse(json);
            if (jsonObj != null)
            {
                JArray empArray = (JArray)jsonObj["AuthorsProp"];
                List<AuthorsProp> aup = new List<AuthorsProp>();
                foreach (var item in empArray)
                {
                    aup.Add(new AuthorsProp() { Id = Convert.ToInt32(item["Id"]), Title = item["Title"].ToString(), FirstName = item["FirstName"].ToString(), LastName = item["LastName"].ToString(), Email = item["Email"].ToString(), Contact = item["Contact"].ToString(), Address = item["Address"].ToString(), Gender = item["Gender"].ToString() });
                }
                listAuthors.ItemsSource = aup;
            }
        }
        private void dragdrop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void rdMale_Checked(object sender, RoutedEventArgs e)
        {
            
            gender = "Male";
        }

        private void rdFemale_Checked(object sender, RoutedEventArgs e)
        {
            gender = "Female";
        }

        private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            AllClear();
            texID.IsEnabled = true;
            Btn_save.IsEnabled = true;
            this.Focus(comTitle);
            comTitle.Text = "";
        }
    }
}

using HospitalManagementSystem.Core;
using HospitalManagementSystem.Helpers;
using HospitalManagementSystem.Models;
using MaterialSkin;
using MaterialSkin.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalManagementSystem.Forms
{
    public partial class LoginForm : MaterialForm
    {
        public LoginForm()
        {
            InitializeComponent();

            // Attach MaterialSkin to this form
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
        }

        // ── Login Button Click ──────────────────────────────────
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = cmbRole.Text;

            // ── Basic Validation ───────────────────────────────
            if (string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill in all fields and select a role.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Logging in...";

            try
            {
                var formData = new Dictionary<string, string>
                {
                    { "username", username },
                    { "password", password }
                };

                switch (role)
                {
                    case "Admin":
                        await LoginAsAdmin(formData);
                        break;

                    case "Doctor":
                        await LoginAsDoctor(formData);
                        break;

                    case "Patient":
                        await LoginAsPatient(formData);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}\n\nMake sure XAMPP is running.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "LOGIN";
            }
        }

        // ── Admin Login ─────────────────────────────────────────
        private async System.Threading.Tasks.Task LoginAsAdmin(Dictionary<string, string> formData)
        {
            var result = await ApiClient.PostAsync<ApiResponse>(
                "admin/login.php", formData);

            if (result.Success)
            {
                SessionManager.Role = "admin";
                SessionManager.Username = formData["username"];

                this.Hide();
                new Admin.AdminDashboard().Show();
            }
            else
            {
                MessageBox.Show(result.Message, "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Doctor Login ────────────────────────────────────────
        private async System.Threading.Tasks.Task LoginAsDoctor(Dictionary<string, string> formData)
        {
            var result = await ApiClient.PostAsync<ApiResponse>(
                "doctor/login.php", formData);

            if (result.Success)
            {
                SessionManager.Role = "doctor";
                SessionManager.Username = formData["username"];

                this.Hide();
                new Doctor.DoctorDashboard().Show();
            }
            else
            {
                MessageBox.Show(result.Message, "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Patient Login ───────────────────────────────────────
        private async System.Threading.Tasks.Task LoginAsPatient(Dictionary<string, string> formData)
        {
            var result = await ApiClient.PostAsync<ApiResponse>(
                "patient/login.php", formData);

            if (result.Success)
            {
                SessionManager.Role = "patient";
                SessionManager.Username = formData["username"];

                // Store patient ID from response
                if (result.Data != null)
                {
                    var data = result.Data as Newtonsoft.Json.Linq.JObject;
                    SessionManager.PatientId = data?["pid"]?.Value<int>() ?? 0;
                }

                this.Hide();
                new Patient.PatientDashboard().Show();
            }
            else
            {
                MessageBox.Show(result.Message, "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

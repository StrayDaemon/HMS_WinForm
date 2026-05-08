using System;
using System.Windows.Forms;
using HospitalManagementSystem.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace HospitalManagementSystem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ── Bootstrap MaterialSkin2 ──────────────────────
            var skinManager = MaterialSkinManager.Instance;

            skinManager.EnforceBackcolorOnAllComponents = true;
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Hospital theme: Deep Blue primary, Teal accent
            skinManager.ColorScheme = new ColorScheme(
                Primary.Blue700,
                Primary.Blue900,
                Primary.Blue500,
                Accent.Teal200,
                TextShade.WHITE
            );

            // ── Launch Login Form ────────────────────────────
            Application.Run(new LoginForm());
        }
    }
}
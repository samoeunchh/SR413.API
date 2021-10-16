using SR413.DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SR413.PresentationDesktop
{
    public partial class Form1 : Form
    {
        private readonly HttpClient client = new HttpClient();
        public Form1()
        {
            client.BaseAddress = new Uri("http://localhost:38581/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            HttpResponseMessage response = await client.GetAsync("api/brands");
            listView1.Items.Clear();
            if (response.IsSuccessStatusCode)
            {
                var brands = await response.Content.ReadAsAsync<List<Brand>>();
                if (brands == null)
                    MessageBox.Show("No Record");
                else
                {
                    foreach(var item in brands)
                    {
                        var li = listView1.Items.Add(item.BrandId.ToString());
                        li.SubItems.Add(item.BrandName);
                        li.SubItems.Add(item.Description);
                    }
                }
            }
            else
            {
                var msg = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                MessageBox.Show(msg);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Brand Name field is required");
                return;
            }
            var brand = new Brand
            {
                BrandName=textBox1.Text,
                Description=textBox1.Text
            };
            HttpResponseMessage response = 
                await client.PostAsJsonAsync<Brand>("api/brands", brand);
            if (response.IsSuccessStatusCode)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                MessageBox.Show("Record was saved");
                Form1_Load(sender, e);
            }
            else
            {
                var msg = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                MessageBox.Show(msg);
            }
        }
    }
}

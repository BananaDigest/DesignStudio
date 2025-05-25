using System;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DesignStudio.WinFormsClient
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        private readonly HttpClient _http;
        private TabControl tabs;

        // Services tab
        private DataGridView dgvServices;
        private Button btnLoadServices, btnFindService, btnAddService, btnUpdateService, btnDeleteService;
        private TextBox txtSvcId, txtSvcName, txtSvcDesc, txtSvcPrice;

        // Orders tab
        private DataGridView dgvOrders;
        private Button btnLoadOrders, btnFindOrder, btnCreateOrder, btnUpdateOrder, btnCancelOrder, btnCompleteOrder;
        private TextBox txtOrderId, txtCustomer, txtPhone, txtOrderServiceId, txtOrderReq, txtOrderDesc;
        private RadioButton rbServiceOrder, rbTurnkeyOrder;
        private RadioButton rdoService, rdoTurnkey;
        private GroupBox gbOrderType;

        // Portfolio tab
        private DataGridView dgvPortfolio;
        private Button btnLoadPortfolio;

        public MainForm()
        {
            _http = new HttpClient { BaseAddress = new Uri("http://localhost:5219/api/") };
            Text = "Design Studio Client";
            Size = new System.Drawing.Size(1000, 700);

            tabs = new TabControl { Dock = DockStyle.Fill };
            tabs.TabPages.Add("Services");
            tabs.TabPages.Add("Orders");
            tabs.TabPages.Add("Portfolio");
            Controls.Add(tabs);

            InitializeServicesTab(tabs.TabPages[0]);
            InitializeOrdersTab(tabs.TabPages[1]);
            InitializePortfolioTab(tabs.TabPages[2]);
        }

        private void InitializeServicesTab(TabPage tab)
        {
            // Data grid
            dgvServices = new DataGridView { Dock = DockStyle.Top, Height = 250, ReadOnly = true, AutoGenerateColumns = true };
            tab.Controls.Add(dgvServices);

            // Load and Find buttons
            btnLoadServices = new Button { Text = "Load Services", Top = 260, Left = 10 };
            btnLoadServices.Click += async (s, e) =>
                dgvServices.DataSource = await _http.GetFromJsonAsync<List<DesignServiceDto>>("designservices");
            btnFindService = new Button { Text = "Find by ID", Top = 260, Left = 130 };
            btnFindService.Click += async (s, e) =>
            {
                if (!int.TryParse(txtSvcId.Text, out var id)) return;
                var svc = await _http.GetFromJsonAsync<DesignServiceDto>($"designservices/{id}");
                if (svc == null) return;
                txtSvcName.Text = svc.Name;
                txtSvcDesc.Text = svc.Description;
                txtSvcPrice.Text = svc.Price.ToString();
            };
            tab.Controls.Add(btnLoadServices);
            tab.Controls.Add(btnFindService);

            // Input fields
            tab.Controls.Add(new Label { Text = "ID:", Top = 300, Left = 10 });
            txtSvcId = new TextBox { Top = 320, Left = 10, Width = 50 };
            tab.Controls.Add(txtSvcId);

            tab.Controls.Add(new Label { Text = "Name:", Top = 300, Left = 80 });
            txtSvcName = new TextBox { Top = 320, Left = 80, Width = 200 };
            tab.Controls.Add(txtSvcName);

            tab.Controls.Add(new Label { Text = "Description:", Top = 360, Left = 10 });
            txtSvcDesc = new TextBox { Top = 380, Left = 10, Width = 300, Height = 60, Multiline = true };
            tab.Controls.Add(txtSvcDesc);

            tab.Controls.Add(new Label { Text = "Price:", Top = 460, Left = 10 });
            txtSvcPrice = new TextBox { Top = 480, Left = 10, Width = 100 };
            tab.Controls.Add(txtSvcPrice);

            // Actions
            btnAddService = new Button { Text = "Add Service", Top = 520, Left = 10 };
            btnAddService.Click += async (s, e) =>
            {
                var dto = new DesignServiceDto
                {
                    Name = txtSvcName.Text,
                    Description = txtSvcDesc.Text,
                    Price = decimal.Parse(txtSvcPrice.Text)
                };
                await _http.PostAsJsonAsync("designservices", dto);
                btnLoadServices.PerformClick();
            };
            tab.Controls.Add(btnAddService);

            btnUpdateService = new Button { Text = "Update Service", Top = 520, Left = 130 };
            btnUpdateService.Click += async (s, e) =>
            {
                var dto = new DesignServiceDto
                {
                    Id = int.Parse(txtSvcId.Text),
                    Name = txtSvcName.Text,
                    Description = txtSvcDesc.Text,
                    Price = decimal.Parse(txtSvcPrice.Text)
                };
                await _http.PutAsJsonAsync($"designservices/{dto.Id}", dto);
                btnLoadServices.PerformClick();
            };
            tab.Controls.Add(btnUpdateService);

            btnDeleteService = new Button { Text = "Delete Service", Top = 520, Left = 260 };
            btnDeleteService.Click += async (s, e) =>
            {
                await _http.DeleteAsync($"designservices/{txtSvcId.Text}");
                btnLoadServices.PerformClick();
            };
            tab.Controls.Add(btnDeleteService);
        }

        private void InitializeOrdersTab(TabPage tab)
        {
            // DataGridView
            dgvOrders = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 200,
                ReadOnly = true,
                AutoGenerateColumns = true
            };
            tab.Controls.Add(dgvOrders);

            // Load Orders button
            btnLoadOrders = new Button { Text = "Load Orders", Top = 210, Left = 10 };
            btnLoadOrders.Click += async (s, e) =>
            {
                try
                {
                    dgvOrders.DataSource = await _http.GetFromJsonAsync<List<OrderDto>>("orders");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            tab.Controls.Add(btnLoadOrders);

            // Order type radio buttons
            rdoService = new RadioButton { Text = "Service", Top = 250, Left = 10, Checked = true };
            rdoTurnkey = new RadioButton { Text = "Turnkey", Top = 250, Left = 110 };
            rdoService.CheckedChanged += (s, e) => ToggleOrderType();
            rdoTurnkey.CheckedChanged += (s, e) => ToggleOrderType();
            tab.Controls.Add(rdoService);
            tab.Controls.Add(rdoTurnkey);

            // Input fields
            var lblOrderId = new Label { Text = "Order ID:", Top = 300, Left = 10 };
            txtOrderId = new TextBox { Top = 320, Left = 10, Width = 50 };
            txtCustomer = new TextBox { PlaceholderText = "Customer Name", Top = 320, Left = 80, Width = 200 };
            txtPhone = new TextBox { PlaceholderText = "Phone", Top = 350, Left = 10, Width = 200 };
            txtOrderServiceId = new TextBox { PlaceholderText = "Service ID", Top = 380, Left = 10, Width = 100 };
            txtOrderReq = new TextBox { PlaceholderText = "Requirement", Top = 410, Left = 10, Width = 200, Enabled = false };
            txtOrderDesc = new TextBox { PlaceholderText = "Description", Top = 440, Left = 10, Width = 200, Height = 60, Multiline = true, Enabled = false };
            tab.Controls.Add(lblOrderId);
            tab.Controls.Add(txtOrderId);
            tab.Controls.Add(txtCustomer);
            tab.Controls.Add(txtPhone);
            tab.Controls.Add(txtOrderServiceId);
            tab.Controls.Add(txtOrderReq);
            tab.Controls.Add(txtOrderDesc);

            // Action buttons
            btnCreateOrder = new Button { Text = "Create Order", Top = 520, Left = 10 };
            btnCreateOrder.Click += async (s, e) => await ExecuteCreate();
            btnCancelOrder = new Button { Text = "Cancel Order", Top = 520, Left = 260 };
            btnCancelOrder.Click += async (s, e) => await ExecuteCancel();
            btnCompleteOrder = new Button { Text = "Complete Order", Top = 520, Left = 380 };
            btnCompleteOrder.Click += async (s, e) => await ExecuteComplete();
            tab.Controls.Add(btnCreateOrder);
            tab.Controls.Add(btnCancelOrder);
            tab.Controls.Add(btnCompleteOrder);
        }

        // Toggle input field states based on order type
        private void ToggleOrderType()
        {
            bool turnkey = rdoTurnkey.Checked;
            txtOrderReq.Enabled = turnkey;
            txtOrderDesc.Enabled = turnkey;
            txtOrderServiceId.Enabled = !turnkey;
        }

        // Create order logic
        private async Task ExecuteCreate()
        {
            var dto = new OrderDto
            {
                CustomerName = txtCustomer.Text,
                Phone = txtPhone.Text,
                IsTurnkey = rdoTurnkey.Checked,
                DesignRequirement = txtOrderReq.Text,
                DesignDescription = txtOrderDesc.Text,
                OrderDate = DateTime.Now
            };

            if (rdoService.Checked)
            {
                if (!int.TryParse(txtOrderServiceId.Text, out int svcId))
                {
                    MessageBox.Show("Invalid Service ID");
                    return;
                }

                try
                {
                    //  «авантажуЇмо повний серв≥с з бекенду
                    var service = await _http.GetFromJsonAsync<DesignServiceDto>($"designservices/{svcId}");

                    if (service == null)
                    {
                        MessageBox.Show("Service not found");
                        return;
                    }

                    dto.Services = new List<DesignServiceDto> { service };
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading service: " + ex.Message);
                    return;
                }
            }
            else
            {
                // якщо "Turnkey", створюЇмо порожн≥й список
                dto.Services = new List<DesignServiceDto>();
            }

            string url = rdoTurnkey.Checked ? "orders/turnkey" : "orders/service";

            try
            {
                var resp = await _http.PostAsJsonAsync(url, dto);
                if (resp.IsSuccessStatusCode)
                {
                    MessageBox.Show("Order created successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnLoadOrders.PerformClick();
                }
                else
                {
                    var content = await resp.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error {resp.StatusCode}: {resp.ReasonPhrase}\n{content}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cancel order logic
        private async Task ExecuteCancel()
        {
            if (!int.TryParse(txtOrderId.Text, out int id))
            {
                MessageBox.Show("Invalid Order ID");
                return;
            }
            if (MessageBox.Show("Cancel this order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            try
            {
                var resp = await _http.DeleteAsync($"orders/{id}");
                if (resp.IsSuccessStatusCode)
                    btnLoadOrders.PerformClick();
                else
                    MessageBox.Show(resp.ReasonPhrase, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Complete order logic
        private async Task ExecuteComplete()
        {
            if (!int.TryParse(txtOrderId.Text, out int id))
            {
                MessageBox.Show("Invalid Order ID");
                return;
            }
            try
            {
                var resp = await _http.PutAsync($"orders/{id}/complete", null);
                if (resp.IsSuccessStatusCode)
                    btnLoadOrders.PerformClick();
                else
                    MessageBox.Show(resp.ReasonPhrase, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializePortfolioTab(TabPage tab)
        {
            dgvPortfolio = new DataGridView { Dock = DockStyle.Top, Height = 300, ReadOnly = true, AutoGenerateColumns = true };
            tab.Controls.Add(dgvPortfolio);
            btnLoadPortfolio = new Button { Text = "Load", Top = 310, Left = 10 };
            btnLoadPortfolio.Click += async (s, e) => dgvPortfolio.DataSource = await _http.GetFromJsonAsync<List<PortfolioItemDto>>("portfolio");
            tab.Controls.Add(btnLoadPortfolio);
        }


        private void OrderTypeChanged(object sender, EventArgs e)
        {
            txtOrderServiceId.Enabled = rbServiceOrder.Checked;
            txtOrderReq.Enabled = rbTurnkeyOrder.Checked;
            txtOrderDesc.Enabled = rbTurnkeyOrder.Checked;
        }
    }

    // DTOs
    public class DesignServiceDto { public int Id { get; set; } public string Name { get; set; } public string Description { get; set; } public decimal Price { get; set; } }
    public class OrderDto { public int Id { get; set; } public string CustomerName { get; set; } public string Phone { get; set; } public bool IsTurnkey { get; set; } public string DesignRequirement { get; set; } public string DesignDescription { get; set; } public List<DesignServiceDto> Services { get; set; } public DateTime OrderDate { get; set; } }
    public class PortfolioItemDto { public int Id { get; set; } public string Title { get; set; } public string Description { get; set; } }
}

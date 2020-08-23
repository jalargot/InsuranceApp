using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using InsuranceWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InsuranceWebApp.Controllers
{
    public class CustomerController : Controller
    {

        private static string accessToken;
        private static HttpClient Client = new HttpClient();

        public async Task<IActionResult> Index()
        {
			CustomerViewPageModel model = new CustomerViewPageModel();
			await SetupAuthorizationHeader();
			var response = await Client.GetAsync("https://localhost:44383/api/customers");
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();
			IEnumerable<CustomerViewModel> customers = JsonConvert.DeserializeObject<List<CustomerViewModel>>(result);
			model.Customers = customers;
			return View(model);
        }

		private async Task SetupAuthorizationHeader()
		{
			if (string.IsNullOrEmpty(accessToken))
			{
				accessToken = await HttpContext.GetTokenAsync("access_token");
			}

			if (Client.DefaultRequestHeaders.Authorization == null)
			{
				Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			}
		}
	}
}

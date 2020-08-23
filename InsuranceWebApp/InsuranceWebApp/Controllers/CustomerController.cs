using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
		private static CustomerViewPageModel model = new CustomerViewPageModel();
		private static CustomerCreatePageModel editModel = new CustomerCreatePageModel();

		public async Task<IActionResult> Index()
        {
			await SetupAuthorizationHeader();
			var response = await Client.GetAsync("https://localhost:44383/api/customers");
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();
			IEnumerable<CustomerViewModel> customers = JsonConvert.DeserializeObject<List<CustomerViewModel>>(result);
			model.Customers = customers;
			return View(model);
        }

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CustomerCreatePageModel createModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(createModel.Customer), Encoding.UTF8, "application/json");
					await SetupAuthorizationHeader();
					var response = await Client.PostAsync("https://localhost:44383/api/customers", content);
					response.EnsureSuccessStatusCode();
					model.Message = "The customer has been created successfully";
					return RedirectToAction("Index");
				}
				return View();
			}
			catch
			{
				return View();
			}
		}

		public async Task<IActionResult> Edit(int id)
		{
			await SetupAuthorizationHeader();
			var response = await Client.GetAsync("https://localhost:44383/api/customers/"+id);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();
			CustomerViewModel customer = JsonConvert.DeserializeObject<CustomerViewModel>(result);
			editModel.Customer = customer;
			return View(editModel);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, CustomerCreatePageModel editedModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(editedModel.Customer), Encoding.UTF8, "application/json");
					await SetupAuthorizationHeader();
					var response = await Client.PutAsync("https://localhost:44383/api/customers/" + id, content);
					response.EnsureSuccessStatusCode();
					model.Message = "The customer has been edited successfully";
					return RedirectToAction("Index");
				}
				return View();
			}
			catch
			{
				return View();
			}
		}

		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await SetupAuthorizationHeader();
				var response = await Client.DeleteAsync("https://localhost:44383/api/customers/" + id);
				response.EnsureSuccessStatusCode();
				model.Message = "The customer has been deleted successfully";
				return RedirectToAction("Index");
			}
			catch
			{
				return RedirectToAction("Index");
			}
		}

        public IActionResult Back()
        {
            model.Message = null;
            return RedirectToAction("Index");
        }

		public async Task<IActionResult> Details(int id)
		{
			await SetupAuthorizationHeader();
			var response = await Client.GetAsync("https://localhost:44383/api/customers/" + id);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();
			CustomerViewModel customer = JsonConvert.DeserializeObject<CustomerViewModel>(result);
			return View(customer);
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

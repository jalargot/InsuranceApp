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
    public class PolicyController : Controller
    {
		private static string accessToken;
		private static HttpClient Client = new HttpClient();
		private static PolicyViewPageModel model = new PolicyViewPageModel();
		private static PolicyCreatePageModel editModel = new PolicyCreatePageModel();

		public async Task<IActionResult> Index()
		{
			await SetupAuthorizationHeader();
			var response = await Client.GetAsync("https://localhost:44383/api/policies");
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();
			IEnumerable<PolicyViewModel> policies = JsonConvert.DeserializeObject<List<PolicyViewModel>>(result);
			model.Policies = policies;
			return View(model);
		}

		public IActionResult Create()
		{
			PolicyCreatePageModel emptyModel = new PolicyCreatePageModel();
			return View(emptyModel);
		}

		[HttpPost]
        public async Task<IActionResult> Create(PolicyCreatePageModel createModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(createModel.Policy), Encoding.UTF8, "application/json");
                    await SetupAuthorizationHeader();
                    var response = await Client.PostAsync("https://localhost:44383/api/policies", content);
					if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                    {
						createModel.Message = "if Risk Type is High Coverage should be less than 50%";
						return View(createModel);
					}
                    response.EnsureSuccessStatusCode();
                    model.Message = "The policy has been created successfully";
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
			var response = await Client.GetAsync("https://localhost:44383/api/policies/" + id);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();
			PolicyViewModel policy = JsonConvert.DeserializeObject<PolicyViewModel>(result);
			editModel.Policy = policy;
			return View(editModel);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, PolicyCreatePageModel editedModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					StringContent content = new StringContent(JsonConvert.SerializeObject(editedModel.Policy), Encoding.UTF8, "application/json");
					await SetupAuthorizationHeader();
					var response = await Client.PutAsync("https://localhost:44383/api/policies/"+id, content);
					if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
					{
						editedModel.Message = "if Risk Type is High Coverage should be less than 50%";
						return View(editedModel);
					}
					response.EnsureSuccessStatusCode();
					model.Message = "The policy has been edited successfully";
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
				var response = await Client.DeleteAsync("https://localhost:44383/api/policies/" + id);
				response.EnsureSuccessStatusCode();
				model.Message = "The policy has been deleted successfully";
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

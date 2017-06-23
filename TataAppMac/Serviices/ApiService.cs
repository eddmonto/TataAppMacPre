﻿namespace TataAppMac.Serviices
{
    using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using System.Threading.Tasks;
	using Newtonsoft.Json;
	using Plugin.Connectivity;
	using TataAppMac.Models;

	public class ApiService
	{
		public async Task<Response> CheckConnection()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				return new Response
				{
					IsSuccess = false,
					Message = "Please turn on your internet.",
				};
			}

			var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
			if (!isReachable)
			{
				return new Response
				{
					IsSuccess = false,
					Message = "Check you internet connection.",
				};
			}

			return new Response
			{
				IsSuccess = true,
				Message = "Ok",
			};
		}

        public async Task<TokenResponse> GetToken(string urlBase, 
                                                  string username, 
                                                  string password)
		{
			try
			{
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
				var response = await client.PostAsync("Token",
					new StringContent(string.Format(
                    "grant_type=password&username={0}&password={1}",
					username, password),
					Encoding.UTF8, "application/x-www-form-urlencoded"));
				var resultJSON = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<TokenResponse>(resultJSON);
				return result;
			}
			catch
			{
				return null;
			}
		}

		public async Task<Response> GetEmployeeByEmailOrCode(
            string urlBase, 
            string servicePrefix,
			string controller, 
            string tokenType, 
            string accessToken, 
            string emailOrCode)
		{
			try
			{
				var employeeRequest = new EmployeeRequest { EmailOrCode = emailOrCode, };
				var request = JsonConvert.SerializeObject(employeeRequest);
				var content = new StringContent(request, Encoding.UTF8, "application/json");
				var client = new HttpClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
					tokenType, accessToken);
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}", servicePrefix, controller);
				var response = await client.PostAsync(url, content);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var employee = JsonConvert.DeserializeObject<Employee>(result);

				return new Response
				{
					IsSuccess = true,
					Message = "OK",
					Result = employee,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> Get<T>(
            string urlBase, string servicePrefix, string controller,
			string tokenType, string accessToken, int id)
		{
			try
			{
				var client = new HttpClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}/{2}", servicePrefix, controller, id);
				var response = await client.GetAsync(url);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var model = JsonConvert.DeserializeObject<T>(result);
				return new Response
				{
					IsSuccess = true,
					Message = "Ok",
					Result = model,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> GetList<T>(
            string urlBase, string servicePrefix, string controller,
			string tokenType, string accessToken)
		{
			try
			{
				var client = new HttpClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}", servicePrefix, controller);
				var response = await client.GetAsync(url);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var list = JsonConvert.DeserializeObject<List<T>>(result);
				return new Response
				{
					IsSuccess = true,
					Message = "Ok",
					Result = list,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> GetList<T>(
			string urlBase, string servicePrefix, string controller,
			string tokenType, string accessToken, int id)
		{
			try
			{
				var client = new HttpClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
				client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, id);
				var response = await client.GetAsync(url);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var list = JsonConvert.DeserializeObject<List<T>>(result);
				return new Response
				{
					IsSuccess = true,
					Message = "Ok",
					Result = list,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> Post<T>(
            string urlBase, string servicePrefix, string controller,
			string tokenType, string accessToken, T model)
		{
			try
			{
				var request = JsonConvert.SerializeObject(model);
				var content = new StringContent(request, Encoding.UTF8, "application/json");
				var client = new HttpClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}", servicePrefix, controller);
				var response = await client.PostAsync(url, content);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var newRecord = JsonConvert.DeserializeObject<T>(result);

				return new Response
				{
					IsSuccess = true,
					Message = "Record added OK",
					Result = newRecord,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> Post<T>(
            string urlBase, string servicePrefix, string controller, T model)
		{
			try
			{
				var request = JsonConvert.SerializeObject(model);
				var content = new StringContent(request, Encoding.UTF8, "application/json");
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}", servicePrefix, controller);
				var response = await client.PostAsync(url, content);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var newRecord = JsonConvert.DeserializeObject<T>(result);

				return new Response
				{
					IsSuccess = true,
					Message = "Record added OK",
					Result = newRecord,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> Put<T>(
            string urlBase, string servicePrefix, string controller,
			string tokenType, string accessToken, T model)
		{
			try
			{
				var request = JsonConvert.SerializeObject(model);
				var content = new StringContent(request, Encoding.UTF8, "application/json");
				var client = new HttpClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
				client.BaseAddress = new Uri(urlBase);
				var url = string.Format("{0}{1}/{2}", servicePrefix, controller, model.GetHashCode());
				var response = await client.PutAsync(url, content);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				var result = await response.Content.ReadAsStringAsync();
				var newRecord = JsonConvert.DeserializeObject<T>(result);

				return new Response
				{
					IsSuccess = true,
					Message = "Record updated OK",
					Result = newRecord,
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<Response> Delete<T>(
            string urlBase, string servicePrefix, string controller,
			string tokenType, string accessToken, T model)
		{
			try
			{
				var client = new HttpClient();
				client.BaseAddress = new Uri(urlBase);
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
				var url = string.Format("{0}{1}/{2}", servicePrefix, controller, model.GetHashCode());
				var response = await client.DeleteAsync(url);

				if (!response.IsSuccessStatusCode)
				{
					return new Response
					{
						IsSuccess = false,
						Message = response.StatusCode.ToString(),
					};
				}

				return new Response
				{
					IsSuccess = true,
					Message = "Record deleted OK",
				};
			}
			catch (Exception ex)
			{
				return new Response
				{
					IsSuccess = false,
					Message = ex.Message,
				};
			}
		}
	}
}

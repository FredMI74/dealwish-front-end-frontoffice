using dw_frontoffice.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System;

namespace dw_frontoffice.Controllers
{
    [Authorize]
    public class CidadesController : Controller
    {

        public IActionResult ConsultarCidadesAutoCompletar(string nome)
        {
            if (!string.IsNullOrWhiteSpace(nome))
            {
                DwClienteHttp httpClient = DwClienteHttp.Instance;
                httpClient.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies[Constantes.TOKEN_JWT_USUARIO]);

                string responseStr;

                var consulta_cidade = new Dictionary<string, string>
                {
                   { "nome", nome},
                   { "token", Request.Cookies[Constantes.TOKEN_USUARIO] }
                };

                var request = new HttpRequestMessage
                {
                    Content = new FormUrlEncodedContent(consulta_cidade),
                    RequestUri = new Uri("api/consultar_cidade", UriKind.Relative),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage response;

                try
                {
                    response = httpClient.client.SendAsync(request).Result;
                }
                catch
                {
                    return new JsonResult("");
                }

                responseStr = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrWhiteSpace(responseStr))
                {
                    return new JsonResult(JsonConvert.DeserializeObject<Cidade>(responseStr).Conteudo);
                }
                else
                {
                    return new JsonResult("");
                }
            }
            else
            {
                return new JsonResult("");
            }
        }

    }
}
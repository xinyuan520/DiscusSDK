using Discus.User.ServerClient;
using HtttpClientDemo;
using System.Security.Principal;

var str = await BaseClient.NacosApiServerClient.GetServerAdressByNacos();
Console.WriteLine(str);

var resutl = await BaseClient.JwtServerClient.GetServerAdressByNacos(new Discus.User.Application.Contracts.Dtos.LoginRequestDto()
{
    Account = "admin",
    Password = "E10ADC3949BA59ABBE56E057F20F883E",
});
Console.WriteLine(resutl.Data);


Console.WriteLine("--------------------------");
